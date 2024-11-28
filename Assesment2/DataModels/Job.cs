using System.ComponentModel.DataAnnotations;

namespace Assesment2.DataModels;

public class Job
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Date { get; set; }
    public decimal Cost { get; set; }
    public bool Completed { get; set; }
}