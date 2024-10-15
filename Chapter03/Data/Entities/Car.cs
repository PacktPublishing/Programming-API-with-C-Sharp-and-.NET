namespace Cars.Data.Entities;

public class Car
{
    public int id { get; set; }
    public string name { get; set; } 
    public string mpg { get; set; } 
    public string cylinders { get; set; } 
    public string displacement { get; set; } 
    public string horsepower { get; set; } 
    public string weight { get; set; } 
    public string acceleration { get; set; } 
    public string model_year { get; set; } 
    public string origin { get; set; } 
    public string? is_deleted { get; set;}
}