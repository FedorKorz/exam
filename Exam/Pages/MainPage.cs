using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace Exam.Pages;

public class MainPage : Form
{
    public MainPage() : base(By.XPath("//ol[@class='breadcrumb']"), "main page")
    {
    }

    private ILink NexageLink => 
        ElementFactory.GetLink(By.XPath("//a[contains(text(),'Nexage')]"), "Nexage link");

    private IButton AddProjectBtn =>
        ElementFactory.GetButton(By.CssSelector(".btn.btn-xs.btn-primary.pull-right"), "add project");

    private ITextBox ProjectName => 
        ElementFactory.GetTextBox(By.XPath("//input[@id='projectName']"), "project name");
    private IButton SubmitButton => 
        ElementFactory.GetButton(By.XPath("//button[@type='submit']"), "submit button");

    private ITextBox VersionText =>
        ElementFactory.GetTextBox(By.XPath("//p[contains(@class,'footer-text')]/span"), "project created");

    private IList<ITextBox> Projects =>
        ElementFactory.FindElements<ITextBox>(By.XPath("//a[contains(@class,'list')]"));

    public void SetProjectName(string? projectName)
    {
        ProjectName.State.WaitForDisplayed();
        ProjectName.SendKeys(projectName);
    }

    public string GetVersionText()
    {
        VersionText.State.WaitForDisplayed();
        return VersionText.Text;
    }

    public void GoToCreatedProject(string? projectName)
    {
        foreach (var project in Projects)
        {
            if (project.Text != projectName) continue;
            project.ClickAndWait();
            break;
        }
    }


    public List<string> GetProjects()
    {
        return Projects.Select(project => project.Text).ToList();
    }

    public void GoToNexageProject()
    {
        NexageLink.State.WaitForDisplayed();
        NexageLink.ClickAndWait();
    }

    public void SaveProject()
    {
        SubmitButton.Click();
    }

    public void AddProject()
    {
        AddProjectBtn.State.WaitForDisplayed();
        AddProjectBtn.Click();
    }
}