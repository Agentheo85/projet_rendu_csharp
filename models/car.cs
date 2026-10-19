using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Dealership.Models;

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

    [Column("sold")]
    public bool Sold { get; set; }
    public List<Customer> Owners { get; set; } = [];
}
