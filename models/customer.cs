using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Customer
{
    #region Attributs
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public int Height { get; set; }

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
