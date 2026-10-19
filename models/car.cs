using System.ComponentModel.DataAnnotations;

public class Car
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public int Year { get; set; }
    public List<Customer> Owners { get; set; } = [];
}
