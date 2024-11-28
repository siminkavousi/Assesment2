using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Assesment2.DataModels;
using Assesment2.Services;

namespace Assesment2;

public partial class MainWindow : Window
{
    // Declare a class-level reference to the shared DataService
    private readonly DataService _dataService;
    public ObservableCollection<Contractor> Contractors { get; set; }
    public ObservableCollection<Job> Jobs { get; set; }
    public ObservableCollection<RequirementSystem> RequirementSystems { get; set; }


    public MainWindow()
    {
        InitializeComponent();
        _dataService = DataService.Instance;

        // Initialize ObservableCollections
        Contractors = new ObservableCollection<Contractor>(_dataService.GetAllContractors());
        Jobs = new ObservableCollection<Job>(_dataService.GetAllJobs());
        RequirementSystems = new ObservableCollection<RequirementSystem>(_dataService.GetAllRequirementSystems());

        // Bind to UI elements
        ContractorsList.ItemsSource = Contractors;
        JobsList.ItemsSource = Jobs;
        RequirementsSystemList.ItemsSource = RequirementSystems;

        // Ensure button is disabled initially
        MixAndAddButton.IsEnabled = false;

        // Initialize RemoveRequirementButton state
        RemoveRequirementButton.IsEnabled = false;

    }

    // Load and bind data to UI components
    private void LoadData()
    {
        JobsList.ItemsSource = _dataService.GetAllJobs();
        ContractorsList.ItemsSource = _dataService.GetAllContractors();
        RequirementsSystemList.ItemsSource = _dataService.GetAllRequirementSystems();
    }

    // Helper methods to refresh specific lists in the UI
    private void RefreshJobs()
    {
        JobsList.ItemsSource = null;
        JobsList.ItemsSource = _dataService.GetAllJobs();
    }

    private void RefreshContractors()
    {
        ContractorsList.ItemsSource = null;
        ContractorsList.ItemsSource = _dataService.GetAllContractors();
    }

    private void RefreshRequirementSystems()
    {
        RequirementsSystemList.ItemsSource = null;
        RequirementsSystemList.ItemsSource = _dataService.GetAllRequirementSystems();
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox != null)
            if (textBox.Text == "Id" || textBox.Text == "First Name" || textBox.Text == "Last Name" || textBox.Text == "Hourly Wage" ||
                textBox.Text == "Job Title" || textBox.Text == "Job Cost")
                textBox.Text = "";
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
        {
            if (textBox.Name == "IDBox") textBox.Text = "Id";
            else if (textBox.Name == "FirstNameBox") textBox.Text = "First Name";
            else if (textBox.Name == "LastNameBox") textBox.Text = "Last Name";
            else if (textBox.Name == "HourlyWageBox") textBox.Text = "Hourly Wage";
            else if (textBox.Name == "JobTitleBox") textBox.Text = "Job Title";
            else if (textBox.Name == "JobCostBox") textBox.Text = "Job Cost";
        }
    }

    private void MixAndAddButton_Click(object sender, RoutedEventArgs e)
    {
        if (ContractorsList.SelectedItem is Contractor selectedContractor &&
            JobsList.SelectedItem is Job selectedJob)
        {
            // Create a new RequirementSystem with selected contractor and job
            var requirement = new RequirementSystem
            {
                Contractors = new List<Contractor> { selectedContractor },
                Jobs = new List<Job> { selectedJob }
            };

            // Add the new requirement system to the DataService and ObservableCollection
            var addedRequirement = DataService.Instance.AddRequirementSystem(requirement);
            RequirementSystems.Add(addedRequirement); // Update the ObservableCollection
        }
        else
        {
            MessageBox.Show("Please select both a Contractor and a Job to create a Requirement.");
        }
    }

    public void AddContractor_Click(object sender, RoutedEventArgs e)
    {
        var firstName = FirstNameBox.Text.Trim();
        var iDBox = IDBox.Text.Trim();
        var lastName = LastNameBox.Text.Trim();
        var hourlyWageText = HourlyWageBox.Text.Trim();
        var startDate = StartDatePicker.SelectedDate;

        // Validate input fields
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(hourlyWageText) ||
            !startDate.HasValue)
        {
            MessageBox.Show("Please fill in all fields for the contractor, including the Start Date.");
            return;
        }

        // Validate that Hourly Wage is a valid decimal number
        if (!decimal.TryParse(hourlyWageText, out var hourlyWage))
        {
            MessageBox.Show("Hourly Wage must be a valid number.");
            return;
        }

        // Format the contractor entry string to include Start Date
        //string contractorEntry = $"{firstName} {lastName} - ${hourlyWage}/hour - Start Date: {startDate.Value.ToShortDateString()}";
        var newContractor = new Contractor()
        {
            Id = Convert.ToInt32(iDBox),
            FirstName = firstName,
            LastName = lastName,
            StartDate = StartDatePicker.SelectedDate.Value.ToShortDateString(),
            HourlyWage = hourlyWage
        };


        //ContractorsList.Items.Add(contractorEntry);
        var addedContractor = DataService.Instance.AddContractor(newContractor);
        Contractors.Add(addedContractor); // Add to ObservableCollection

        // Clear fields after adding contractor
        FirstNameBox.Clear();
        FirstNameBox.Text = "First Name";
        LastNameBox.Clear();
        LastNameBox.Text = "Last Name";
        HourlyWageBox.Clear();
        HourlyWageBox.Text = "Hourly Wage";
        IDBox.Clear();
        IDBox.Text = "Id";
        StartDatePicker.SelectedDate = null;
    }

    public void RemoveContractor_Click(object sender, RoutedEventArgs e)
    {
        if (ContractorsList.SelectedItem is Contractor selectedContractor)
        {
            if (_dataService.DeleteContractor(selectedContractor.Id))
                Contractors.Remove(selectedContractor);
        }
        else
            MessageBox.Show("Please select a contractor to remove.");
    }

    public void AddJob_Click(object sender, RoutedEventArgs e)
    {
        var jobTitle = JobTitleBox.Text.Trim();
        var jobCost = JobCostBox.Text.Trim();

        if (!string.IsNullOrEmpty(jobTitle) && !string.IsNullOrEmpty(jobCost))
        {
            //var jobEntry = $"{jobTitle} - ${jobCost}";

            var newJob = new Job()
            {
                Title = jobTitle,
                Date = JobDatePicker.SelectedDate.Value.ToShortDateString(),
                Cost = decimal.Parse(jobCost),
                Completed = JobCompletedCheckBox.IsChecked.HasValue
            };

            var addedContractor = DataService.Instance.AddJob(newJob);
            Jobs.Add(newJob); // Add to ObservableCollection

            //JobsList.Items.Add(newJob);

            JobTitleBox.Clear();
            JobCostBox.Clear();
            JobCompletedCheckBox.IsChecked = false;
            JobDatePicker.SelectedDate = null;
        }
        else
        {
            MessageBox.Show("Please fill in all fields for the job.");
        }
    }

    public void RemoveJob_Click(object sender, RoutedEventArgs e)
    {
        if (JobsList.SelectedItem is Job selectedJob)
        {
            if (_dataService.DeleteJob(selectedJob.Id))
                Jobs.Remove(selectedJob);
        }
        else
            MessageBox.Show("Please select a job to remove.");
    }



    private void HourlyWageBox_TextChanged(object sender, TextChangedEventArgs e)
    {
    }


    private void JobCompletedCheckBox_Checked(object sender, RoutedEventArgs e)
    {
    }


    private void ContractorsList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
    {
        // Retrieve the ListBox instance
        var contractorsListBox = sender as ListBox;

        if (contractorsListBox != null)
        {
            // Access all selected items
            var selectedItems = contractorsListBox.SelectedItems;

            var item = selectedItems[0];
            if (item is Contractor contractor)
            {
                MessageBox.Show($"Selected Contractor: {contractor.FirstName} {contractor.LastName}");
                FirstNameBox.Text = contractor.FirstName;
                LastNameBox.Text = contractor.LastName;
                HourlyWageBox.Text = contractor.HourlyWage.ToString();
                IDBox.Text = contractor.Id.ToString();
                StartDatePicker.SelectedDate = DateTime.Parse(contractor.StartDate);
            }
        }
        UpdateMixAndAddButtonState();
    }

    // PreviewTextInput handler for IDBox
    private void IDBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Use a regular expression to allow only numbers
        e.Handled = !IsTextNumeric(e.Text);
    }

    // Method to check if the input is numeric
    private bool IsTextNumeric(string text)
    {
        return Regex.IsMatch(text, "^[0-9]+$");
    }

    private void JobsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Retrieve the ListBox instance
        var jobsListBox = sender as ListBox;

        if (jobsListBox != null)
        {
            // Access all selected items
            var selectedItems = jobsListBox.SelectedItems;

            var item = selectedItems[0];
            if (item is Job job)
            {
                MessageBox.Show($"Selected Contractor: {job.Title}");
                JobTitleBox.Text = job.Title;
                JobCostBox.Text = job.Cost.ToString("C");

                JobDatePicker.SelectedDate = DateTime.Parse(job.Date);
                JobCompletedCheckBox.IsChecked = job.Completed;
            }
        }

        UpdateMixAndAddButtonState();
    }
    private void AddRequirementButton_Click(object sender, RoutedEventArgs e)
    {
        // Check if a contractor or job is selected
        var selectedContractor = ContractorsList.SelectedItem;
        var selectedJob = JobsList.SelectedItem;

        if (selectedContractor != null)
        {
            // Add contractor to the RequirementSystemList
            RequirementsSystemList.Items.Add(selectedContractor);
        }
        else if (selectedJob != null)
        {
            // Add job to the RequirementSystemList
            RequirementsSystemList.Items.Add(selectedJob);
        }
        else
        {
            MessageBox.Show("Please select a contractor or job to add to the Requirement System.");
        }
    }

    // Remove Requirement Button Click Event
    private void RemoveRequirementButton_Click(object sender, RoutedEventArgs e)
    {
        if (RequirementsSystemList.SelectedItem is RequirementSystem selectedRequirement)
        {
            if (_dataService.DeleteRequirementSystem(selectedRequirement.Id))
            {
                RequirementSystems.Remove(selectedRequirement); // Update the ObservableCollection
                UpdateRemoveRequirementButtonState(); // Ensure button state is updated
            }
        }
    }

    private void FirstNameBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void UpdateMixAndAddButtonState()
    {
        // Enable the button only if both a job and a contractor are selected
        MixAndAddButton.IsEnabled = JobsList.SelectedItem != null && ContractorsList.SelectedItem != null;
    }

    private void UpdateRemoveRequirementButtonState()
    {
        // Enable the button only if a requirement is selected
        RemoveRequirementButton.IsEnabled = RequirementsSystemList.SelectedItem != null;
    }

}