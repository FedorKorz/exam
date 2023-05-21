using Aquality.Selenium.Browsers;
using Exam.Pages;
using Exam.Utils;
using NUnit.Framework;

namespace Exam;

public abstract class Base
{
    protected Browser Browser;
    protected MainPage MainPage;
    protected NexagePage NexagePage;
    
    [SetUp]
    public void Setup()
    {
        Browser = AqualityServices.Browser;
        Browser.GoTo(UtilsConfig.GetData("URL"));
        Browser.Maximize();
        MainPage = new MainPage();
    }

    [TearDown]
    public void TearDown()
    {
        Browser.Quit();
    }
}