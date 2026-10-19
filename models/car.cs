using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("car")]
public class Car
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("brand")]
    public string Brand { get; set; }

    [Column("model")]
    public string Model { get; set; }

    [Column("color")]
    public string Color { get; set; }

    [Column("year")]
    public int Year { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("IsSold")]
    public decimal IsSold { get; set; }


    public List<Customer> Owners { get; set; } = [];
}
