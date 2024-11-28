using System.ComponentModel.DataAnnotations;

namespace Assesment2.DataModels
{
    public class RequirementSystem
    {
        [Key]
        public int Id { get; set; }
        public List<Contractor> Contractors { get; set; }
        public List<Job> Jobs { get; set; }

        // Computed property for ContractorName
        public string ContractorName => Contractors != null && Contractors.Any()
            ? string.Join(", ", Contractors.Select(c => $"{c.FirstName} {c.LastName}"))
            : "No Contractor";

        // Computed property for JobTitle
        public string JobTitle => Jobs != null && Jobs.Any()
            ? string.Join(", ", Jobs.Select(j => j.Title))
            : "No Job";

        // Computed property for total cost (sum of job costs)
        public decimal Cost => Jobs != null && Jobs.Any()
            ? Jobs.Sum(j => j.Cost)
            : 0;

    }
}
