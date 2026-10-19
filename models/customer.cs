using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("customer")]
public class Customer
{
    #region Attributs
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("firstname")]
    public string FirstName { get; set; }

    [Column("lastname")]
    public string LastName { get; set; }

    [Column("birthdate")]
    public DateTime BirthDate { get; set; }


    [Column("phone")]
    public string Phone { get; set; }

    [Column("email")]
    public string Email { get; set; }

    public List<Car> Cars { get; set; } = [];

    #endregion


    #region publicMethods

    public int GetAge()
    {
        DateTime birth = BirthDate;
        DateTime now = DateTime.Now;
        int age = now.Year - birth.Year;
        if (now.Month < birth.Month || (now.Month == birth.Month && now.Day < birth.Day))
            age--;
        return age;
    }

    public String GetFullName()
    {
        return FirstName + " " + LastName?.ToUpper();
    }
    #endregion
}
