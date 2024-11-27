namespace Assesment2;

public class Job
{
    public string Title { get; set; }
    public string Date { get; set; }
    public decimal Cost { get; set; }
    public bool Completed { get; set; }
    public Contractor ContractorAssigned { get; set; }

    public Job(string title, string date, decimal cost, Contractor contractorAssigned)
    {
        Title = title;
        Date = date;
        Cost = cost;
        Completed = false;
        ContractorAssigned = contractorAssigned;
    }

    public string GetJobInfo()
    {
        string contractor = ContractorAssigned == null ? "None" : ContractorAssigned.FirstName + " " + ContractorAssigned.LastName;
        return "Job: " + Title + ", Date: " + Date + ", Cost: " + Cost + ", Completed: " + Completed + ", Assigned: " + contractor;
    }
}