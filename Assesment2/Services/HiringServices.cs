using Assesment2.DataModels;

namespace Assesment2.Services
{
    public class HiringServices
    {
        public List<Contractor> Contractors { get; set; }
        public List<Job> Jobs { get; set; }

        public HiringServices()
        {
            Contractors = new List<Contractor>();
            Jobs = new List<Job>();
        }

        public void AddContractor(Contractor contractor)
        {
            if (contractor != null)
            {
                Contractors.Add(contractor);
            }
            else
            {
                Console.WriteLine("Cannot add a null contractor.");
            }
        }

        public void RemoveContractor(string firstName, string lastName)
        {
            var removed = Contractors.RemoveAll(c => c.FirstName == firstName && c.LastName == lastName);
            if (removed == 0)
            {
                Console.WriteLine($"Contractor {firstName} {lastName} not found.");
            }
        }

        public void AddJob(Job job)
        {
            if (job != null)
            {
                Jobs.Add(job);
            }
            else
            {
                Console.WriteLine("Cannot add a null job.");
            }
        }

        public void AssignJob(string jobTitle, string contractorFirstName, string contractorLastName)
        {
            Job job = Jobs.FirstOrDefault(j => j.Title == jobTitle && !j.Completed);
            Contractor contractor = Contractors.FirstOrDefault(c => c.FirstName == contractorFirstName && c.LastName == contractorLastName);

            if (job == null)
            {
                Console.WriteLine($"Job '{jobTitle}' not found or is already completed.");
                return;
            }

            if (contractor == null)
            {
                Console.WriteLine($"Contractor '{contractorFirstName} {contractorLastName}' not found.");
                return;
            }

            // Check if the contractor is already assigned to an incomplete job
            //bool isContractorAssigned = Jobs.Any(j => j.ContractorAssigned == contractor && !j.Completed);
            //if (!isContractorAssigned)
            //{
            //    job.ContractorAssigned = contractor;
            //    Console.WriteLine($"Contractor {contractor.FirstName} {contractor.LastName} assigned to job '{jobTitle}'.");
            //}
            //else
            //{
            //    Console.WriteLine($"Contractor {contractor.FirstName} {contractor.LastName} is already assigned to an incomplete job.");
            //}
        }

        public void CompleteJob(string jobTitle)
        {
            Job job = Jobs.FirstOrDefault(j => j.Title == jobTitle && !j.Completed);
            if (job != null)
            {
                job.Completed = true;
                Console.WriteLine($"Job '{jobTitle}' marked as completed.");
            }
            else
            {
                Console.WriteLine($"Job '{jobTitle}' not found or is already completed.");
            }
        }

        public List<Contractor> GetContractors()
        {
            return Contractors;
        }

        public List<Job> GetJobs()
        {
            return Jobs;
        }

        //public List<Contractor> GetAvailableContractors()
        //{
        //    return Contractors.Where(c => !Jobs.Any(j => j.ContractorAssigned == c && !j.Completed)).ToList();
        //}

        //public List<Job> GetUnassignedJobs()
        //{
        //    return Jobs.Where(j => j.ContractorAssigned == null && !j.Completed).ToList();
        //}

        public List<Job> GetJobsWithinCostRange(decimal minCost, decimal maxCost)
        {
            return Jobs.Where(j => j.Cost >= minCost && j.Cost <= maxCost).ToList();
        }
    }
}
