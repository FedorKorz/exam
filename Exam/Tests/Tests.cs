using Exam.Models;
using Exam.Pages;
using Exam.Utils;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Exam;

public class Tests : Base
{
    private string? _methodName;
    private string? _projectName;
    private string? _sid;
    private string? _testId;
    private string? _testName;
    private readonly int _arrLength = int.Parse(UtilsGetTestData.Get("ARR_LENGTH")!);

    [Test]
    public void TestHasTokenReceived()
    {
        var token = UtilsApi.GetToken();
        Assert.Multiple(() =>
        {
            Assert.That(string.IsNullOrEmpty(token), Is.False, "Token hasn't been created");
            Assert.That(token, Does.Not.Contains("request"), "bad request, token hasn't received");
        });
    }

    [Test]
    public void TestIsTokenCorrect()
    {
        var token = UtilsApi.GetToken();
        var cookie = UtilsApi.SendCookie(token);
        Browser.Driver.Manage().Cookies.AddCookie(cookie);
        Browser.Refresh();
        Assert.That(MainPage.GetVersionText(), Does.Contain(UtilsGetTestData.Get("VARIANT")));
    }


    [Test]
    public void DoesListSorted()
    {
        NexagePage = new();
        MainPage.GoToNexageProject();
        Assert.Multiple(() =>
        {
            Assert.That(NexagePage.State.WaitForDisplayed(), Is.True, "Nexage Page hasn't loaded");
            Assert.That(NexagePage.GetSortedStartDates(), Is.EqualTo(NexagePage.GetStartDates()),
                "test start dates aren't sorted");
        });
    }

    [Test]
    public void TestCorrectList()
    {
        var token = UtilsApi.GetToken();
        NexagePage = new();
        MainPage.GoToNexageProject();
        Assert.That(NexagePage.State.WaitForDisplayed(), Is.True, "Nexage Page hasn't loaded");

        var parameters = new Dictionary<string, string?>
        {
            {"token", token}, {"projectId", "1"}
        };
        var response = UtilsApi.CallApiMethod(ApiMethods.GetJsonData, parameters);
        if (response.ContentType != "application/json")
        {
            Assert.Ignore("Input data is not in json format");
        }
        else
        {
            var allTests = JArray.Parse(response.Content!);
            Assert.That(UtilsArrays.IsArrayHasData(allTests, NexagePage.GetModels()), Is.True,
                "tests from UI aren't equal to tests from qpi request");
        }
    }

    [Test]
    [Order(1)]
    public void TestAddProject()
    {
        _projectName = UtilsStringGen.RandomString(_arrLength);
        Assert.That(MainPage.State.WaitForDisplayed(), Is.True, "Main page hasn't loaded");
        MainPage.AddProject();
        MainPage.SetProjectName(_projectName);
        MainPage.SaveProject();
        Browser.Refresh();

        Assert.Multiple(() =>
        {
            Assert.That(MainPage.State.IsDisplayed, Is.True, "Main page hasn't loaded");
            Assert.That(MainPage.GetProjects(), Does.Contain(_projectName), "project hasn't created");
        });
    }

    [Test]
    [Order(2)]
    public void TestAddTest()
    {
        _testName = UtilsStringGen.RandomString(_arrLength);
        _methodName = UtilsStringGen.RandomString(_arrLength);
        _sid = UtilsStringGen.RandomString(_arrLength);
        var token = UtilsApi.GetToken();

        var parameters = new Dictionary<string, string?>
        {
            {"token", token},
            {"SID", _sid},
            {"projectName", _projectName},
            {"testName", _testName},
            {"methodName", _methodName},
            {"env", "user"}
        };
        _testId = UtilsApi.CallApiMethod(ApiMethods.AddTest, parameters).Content;
        Assert.That(string.IsNullOrEmpty(_testId), Is.False, "project hasn't been created");
        ProjectPage projectPage = new();
        MainPage.GoToCreatedProject(_projectName);
        Assert.That(projectPage.IsTestCreated(), Is.True, "project hasn't been displayed");
    }

    [Test]
    [Order(3)]
    public void AddLogsToTest()
    {
        MainPage.GoToCreatedProject(_projectName);
        var parameters = new Dictionary<string, string?>
        {
            {"testId", _testId},
            {"content", "stub_text"}
        };
        UtilsApi.CallApiMethod(ApiMethods.GetLogs, parameters);
        ProjectPage projectPage = new();
        projectPage.OpenTestInfo();
        Assert.That(projectPage.AreLogsCreated(), Is.True, "logs haven't been displayed");
    }

    [Test]
    [Order(4)]
    public void AddScreenShot()
    {
        var parameters = new Dictionary<string, string?>
        {
            {"testId", _testId},
            {"contentType", "image/png"},
            {"content", UtilsImg.GetArray64(Browser.GetScreenshot())}
        };
        UtilsApi.CallApiMethod(ApiMethods.GetScreenshot, parameters);
        MainPage.GoToCreatedProject(_projectName);
        ProjectPage projectPage = new();
        projectPage.OpenTestInfo();
        Assert.That(projectPage.IsImageUploaded(), Is.True, "screenshot hasn't been displayed");
    }
}