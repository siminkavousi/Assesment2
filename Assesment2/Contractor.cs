namespace Assesment2;

public class Contractor
{
    public int ID { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StartDate { get; set; }
    public decimal HourlyWage { get; set; }

    public Contractor(int id, string firstName, string lastName, string startDate, decimal hourlyWage)
    {
        ID = id; 
        FirstName = firstName;
        LastName = lastName;
        StartDate = startDate;
        HourlyWage = hourlyWage;
    }

  

    public string GetDetails()
    {
        return $"ID: {ID}, {FirstName} {LastName}, Start Date: {StartDate}, Wage: {HourlyWage}";
    }
}