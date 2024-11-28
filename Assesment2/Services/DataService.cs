using Assesment2.DataModels;

namespace Assesment2.Services;

public class DataService
{
    private static readonly DataService _instance = new DataService();

    private readonly List<Job> _jobs;
    private readonly List<Contractor> _contractors;
    private readonly List<RequirementSystem> _requirementSystems;

    private int _jobNextId = 1;
    private int _contractorNextId = 1;
    private int _requirementSystemNextId = 1;

    private DataService()
    {
        _jobs = new List<Job>();
        _contractors = new List<Contractor>();
        _requirementSystems = new List<RequirementSystem>();
    }

    public static DataService Instance => _instance;

    // CRUD for Jobs
    public Job AddJob(Job job)
    {
        job.Id = _jobNextId++;
        _jobs.Add(job);
        return job;
    }

    public List<Job> GetAllJobs() => _jobs.ToList();

    public Job GetJobById(int id) => _jobs.FirstOrDefault(j => j.Id == id);

    public bool UpdateJob(Job updatedJob)
    {
        var existingJob = GetJobById(updatedJob.Id);
        if (existingJob != null)
        {
            existingJob.Title = updatedJob.Title;
            existingJob.Date = updatedJob.Date;
            existingJob.Cost = updatedJob.Cost;
            existingJob.Completed = updatedJob.Completed;
            return true;
        }
        return false;
    }

    public bool DeleteJob(int id)
    {
        var job = GetJobById(id);
        if (job != null)
        {
            _jobs.Remove(job);
            return true;
        }
        return false;
    }

    // CRUD for Contractors
    public Contractor AddContractor(Contractor contractor)
    {
        contractor.Id = _contractorNextId++;
        _contractors.Add(contractor);
        return contractor;
    }

    public List<Contractor> GetAllContractors() => _contractors.ToList();

    public Contractor GetContractorById(int id) => _contractors.FirstOrDefault(c => c.Id == id);

    public bool UpdateContractor(Contractor updatedContractor)
    {
        var existingContractor = GetContractorById(updatedContractor.Id);
        if (existingContractor != null)
        {
            existingContractor.FirstName = updatedContractor.FirstName;
            existingContractor.LastName = updatedContractor.LastName;
            existingContractor.StartDate = updatedContractor.StartDate;
            existingContractor.HourlyWage = updatedContractor.HourlyWage;
            return true;
        }
        return false;
    }

    public bool DeleteContractor(int id)
    {
        var contractor = GetContractorById(id);
        if (contractor != null)
        {
            _contractors.Remove(contractor);
            return true;
        }
        return false;
    }

    // CRUD for RequirementSystems
    public RequirementSystem AddRequirementSystem(RequirementSystem requirementSystem)
    {
        requirementSystem.Id = _requirementSystemNextId++;
        requirementSystem.Contractors ??= new List<Contractor>();
        requirementSystem.Jobs ??= new List<Job>();
        _requirementSystems.Add(requirementSystem);
        return requirementSystem;
    }

    public List<RequirementSystem> GetAllRequirementSystems() => _requirementSystems.ToList();

    public RequirementSystem GetRequirementSystemById(int id) => _requirementSystems.FirstOrDefault(rs => rs.Id == id);

    public bool UpdateRequirementSystem(RequirementSystem updatedRequirementSystem)
    {
        var existingRequirementSystem = GetRequirementSystemById(updatedRequirementSystem.Id);
        if (existingRequirementSystem != null)
        {
            existingRequirementSystem.Contractors = updatedRequirementSystem.Contractors;
            existingRequirementSystem.Jobs = updatedRequirementSystem.Jobs;
            return true;
        }
        return false;
    }

    public bool DeleteRequirementSystem(int id)
    {
        var requirementSystem = GetRequirementSystemById(id);
        if (requirementSystem != null)
        {
            _requirementSystems.Remove(requirementSystem);
            return true;
        }
        return false;
    }
}