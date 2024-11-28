using System.ComponentModel.DataAnnotations;

namespace Assesment2.DataModels;

public class Contractor
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StartDate { get; set; }
    public decimal HourlyWage { get; set; }

    public string GetDetails()
    {
        return $"Id: {Id}, {FirstName} {LastName}, Start Date: {StartDate}, Wage: {HourlyWage}";
    }
}