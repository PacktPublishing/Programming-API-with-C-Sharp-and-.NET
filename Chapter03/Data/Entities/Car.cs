namespace Cars.Data.Entities;

public class Car
{
    public int Id { get; set; }
    public string name { get; set; } = null!;
    public string mpg { get; set; } = null!;
    public string cylinders { get; set; } = null!;
    public string displacement { get; set; } = null!;
    public string horsepower { get; set; } = null!;
    public string weight { get; set; } = null!;
    public string acceleration { get; set; } = null!;
    public string model_year { get; set; } = null!;
    public string origin { get; set; } = null!;
    public string? is_deleted { get; set;}
}