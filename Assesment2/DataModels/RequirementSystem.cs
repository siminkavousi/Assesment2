using System.ComponentModel.DataAnnotations;

namespace Assesment2.DataModels
{
    public class RequirementSystem
    {
        [Key]
        public int Id { get; set; }
        public List<Contractor> Contractors { get; set; }
        public List<Job> Jobs { get; set; }

    }
}
