using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Assesment2;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // List of contractors to add sample for testing 
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox != null)
            if (textBox.Text == "ID" || textBox.Text == "First Name" || textBox.Text == "Last Name" || textBox.Text == "Hourly Wage" ||
                textBox.Text == "Job Title" || textBox.Text == "Job Cost")
                textBox.Text = "";
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
        {
            if (textBox.Name == "IDBox") textBox.Text = "ID";
            else if (textBox.Name == "FirstNameBox") textBox.Text = "First Name";
            else if (textBox.Name == "LastNameBox") textBox.Text = "Last Name";
            else if (textBox.Name == "HourlyWageBox") textBox.Text = "Hourly Wage";
            else if (textBox.Name == "JobTitleBox") textBox.Text = "Job Title";
            else if (textBox.Name == "JobCostBox") textBox.Text = "Job Cost";
        }
    }

    private void MixAndAddButton_Click(object sender, RoutedEventArgs e)
    {
        // Validate selection
        if (ContractorsList.SelectedItem is Contractor selectedContractor && JobsList.SelectedItem is Job selectedJob)
        {
            // Create a new RequirementSystem and add to the RequirementsSystemList
            var requirement = new RequirementSystem
            {
                //ContractorName = $"{selectedContractor.FirstName} {selectedContractor.LastName}",
                //JobTitle = selectedJob.Title,
                //Cost = selectedJob.Cost
                Contractors = new List<Contractor>()
                {

                },
                Jobs = new List<Job>()
                {

                }
            };

            RequirementsSystemList.Items.Add(requirement);
            MessageBox.Show("Requirement added successfully.");
        }
        else
        {
            MessageBox.Show("Please select a contractor and a job to mix.");
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
        var contractorEntry = new Contractor(Convert.ToInt32(iDBox),
            firstName,
            lastName,
            StartDatePicker.SelectedDate.Value.ToShortDateString(),
            hourlyWage);

        ;
        ContractorsList.Items.Add(contractorEntry);

        // Clear fields after adding contractor
        FirstNameBox.Clear();
        FirstNameBox.Text = "First Name";
        LastNameBox.Clear();
        LastNameBox.Text = "Last Name";
        HourlyWageBox.Clear();
        HourlyWageBox.Text = "Hourly Wage";
        IDBox.Clear();
        IDBox.Text = "ID";
        StartDatePicker.SelectedDate = null;
    }

    public void RemoveContractor_Click(object sender, RoutedEventArgs e)
    {
        if (ContractorsList.SelectedItem != null)
            ContractorsList.Items.Remove(ContractorsList.SelectedItem);
        else
            MessageBox.Show("Please select a contractor to remove.");
    }

    public void AddJob_Click(object sender, RoutedEventArgs e)
    {
        var jobTitle = JobTitleBox.Text.Trim();
        var jobCost = JobCostBox.Text.Trim();
        var selectedContractor = ContractorsList.SelectedItem as Contractor;
        if (selectedContractor == null)
        {
            MessageBox.Show("Please select a relevent contractor to add a new Job.");
            return;
        }
        if (!string.IsNullOrEmpty(jobTitle) && !string.IsNullOrEmpty(jobCost))
        {
            //var jobEntry = $"{jobTitle} - ${jobCost}";

            if (selectedContractor != null)
            {
                var jobEntry = new Job(jobTitle,
                    JobDatePicker.SelectedDate.Value.ToShortDateString(),
                    decimal.Parse(jobCost), selectedContractor);
                JobsList.Items.Add(jobEntry);
            }

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
        if (JobsList.SelectedItem != null)
            JobsList.Items.Remove(JobsList.SelectedItem);
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
        var listBox = sender as ListBox;

        if (listBox != null)
        {
            // Access all selected items
            var selectedItems = listBox.SelectedItems;

            var item = selectedItems[0];
            if (item is Contractor contractor)
            {
                MessageBox.Show($"Selected Contractor: {contractor.FirstName} {contractor.LastName}");
                FirstNameBox.Text = contractor.FirstName;
                LastNameBox.Text = contractor.LastName;
                HourlyWageBox.Text = contractor.HourlyWage.ToString();
                IDBox.Text = contractor.ID.ToString();
                StartDatePicker.SelectedDate = DateTime.Parse(contractor.StartDate);
            }
        }
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
        var listBox = sender as ListBox;

        if (listBox != null)
        {
            // Access all selected items
            var selectedItems = listBox.SelectedItems;

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
        // Check if an item is selected in the RequirementSystemList
        if (RequirementsSystemList.SelectedItem != null)
        {
            // Remove the selected item
            RequirementsSystemList.Items.Remove(RequirementsSystemList.SelectedItem);
        }
        else
        {
            MessageBox.Show("Please select an item to remove from the Requirement System.");
        }
    }

    private void FirstNameBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
}