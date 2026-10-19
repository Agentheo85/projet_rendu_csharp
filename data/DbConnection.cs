using Npgsql;

namespace CoursSupDeVinci;

public class DbConnection
{
    private readonly SchoolDbContext schoolDbContext;

    public DbConnection(SchoolDbContext schoolDbContext)
    {
        this.schoolDbContext = schoolDbContext;
    }

    #region avec EntityFramework
    // EF génère tout le SQL : classe, personnes, adresses et table de liaison
    public void initDb(Classe maClasse)
    {
        schoolDbContext.Add(maClasse);
        schoolDbContext.SaveChanges();
    }
    #endregion

    #region sans EntityFramework

    public async Task init(Classe maClasse, string connectionString)
    {
        await using var connection = new NpgsqlConnection(connectionString);

        Console.WriteLine($"État avant ouverture : {connection.State}");
        try
        {
            await connection.OpenAsync();
            Console.WriteLine($"État après ouverture : {connection.State}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine($"État après ouverture : ERREUR");
            throw;
        }

        // Commence une transaction pour tout insérer ensemble
        await using var transaction = await connection.BeginTransactionAsync();

        // --- Insert Classe ---
        var insertClasseCmd = new NpgsqlCommand(
            "INSERT INTO classes(name, level, school) VALUES (@name, @level, @school) RETURNING id",
            connection, transaction);
        insertClasseCmd.Parameters.AddWithValue("name", maClasse.Name);
        insertClasseCmd.Parameters.AddWithValue("level", maClasse.Level);
        insertClasseCmd.Parameters.AddWithValue("school", maClasse.School);

        Guid classeId = (Guid)await insertClasseCmd.ExecuteScalarAsync();

        // --- Insert Persons ---
        foreach (var person in maClasse.Persons)
        {
            var insertPersonCmd = new NpgsqlCommand(
                "INSERT INTO person(firstname, lastname, birthdate, size, id_classe) VALUES (@firstname, @lastname, @birthdate, @size, @idClasse) RETURNING id",
                connection, transaction);
            insertPersonCmd.Parameters.AddWithValue("firstname", person.Firstname);
            insertPersonCmd.Parameters.AddWithValue("lastname", person.Lastname);
            insertPersonCmd.Parameters.AddWithValue("birthdate", person.Birthdate);
            insertPersonCmd.Parameters.AddWithValue("size", person.Size);
            insertPersonCmd.Parameters.AddWithValue("idClasse", classeId);

            Guid personId = (Guid)await insertPersonCmd.ExecuteScalarAsync();

            // --- Insert Details ---
            var insertDetailCmd = new NpgsqlCommand(
                "INSERT INTO detail(street, city, zipCode) VALUES (@street, @city, @zipCode) RETURNING id",
                connection, transaction);
            insertDetailCmd.Parameters.AddWithValue("street", person.Details.First().Street);
            insertDetailCmd.Parameters.AddWithValue("city", person.Details.First().City);
            insertDetailCmd.Parameters.AddWithValue("zipCode", person.Details.First().ZipCode);

            Guid detailId = (Guid)await insertDetailCmd.ExecuteScalarAsync();

            // --- Insert table de liaison person_detail ---
            var insertPersonDetailCmd = new NpgsqlCommand(
                "INSERT INTO person_detail(id_person, id_detail) VALUES (@idPerson, @idDetail)",
                connection, transaction);
            insertPersonDetailCmd.Parameters.AddWithValue("idPerson", personId);
            insertPersonDetailCmd.Parameters.AddWithValue("idDetail", detailId);

            await insertPersonDetailCmd.ExecuteNonQueryAsync();
        }

        // Commit
        await transaction.CommitAsync();

        Console.WriteLine("Insertion hiérarchique réussie");
    }

    #endregion
}