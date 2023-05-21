using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace Exam.Pages;

public class ProjectPage : Form
{
    public ProjectPage() : base(By.XPath("//div[@id='pie']//canvas[@class='flot-overlay']"), "project page")
    {
    }

    private ILink LinkToTestPage =>
        ElementFactory.GetLink(By.XPath("//a[contains(@href,'testId')]"), "test duration");

    private ITextBox LogText => 
        ElementFactory.GetTextBox(By.XPath("//td[text()='stub_text']"), "logs text");

    private IComboBox UploadedImage =>
        ElementFactory.GetComboBox(By.XPath("//a[contains(@href,'image')]"), "uploaded image");


    public bool IsTestCreated()
    {
        return LinkToTestPage.State.WaitForDisplayed();
    }

    public bool AreLogsCreated()
    {
        return LogText.State.WaitForDisplayed();
    }

    public bool IsImageUploaded()
    {
        return UploadedImage.State.WaitForDisplayed();
    }

    public void OpenTestInfo()
    {
        LinkToTestPage.ClickAndWait();
    }
}