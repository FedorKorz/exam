using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;

namespace Exam.Pages;

public class NexagePage : Form
{
    private readonly JArray _array = new();
    private const int InitialRow = 2;

    public NexagePage() : base(By.XPath("//div[@id='pie']//canvas[@class='flot-overlay']"), "Nexage page")
    {
    }

    private IList<ITextBox> StartDates => ElementFactory.FindElements<ITextBox>(By.XPath("//table/tbody/tr/td[4]"));

    public IList<DateTime> GetStartDates()
    {
        return StartDates.Select(t => DateTime.Parse(t.Text)).ToList();
    }

    public List<DateTime> GetSortedStartDates()
    {
        return StartDates.OrderByDescending(d => DateTime.Parse(d.Text)).Select(t => DateTime.Parse(t.Text)).ToList();
    }

    private static string GetTableRow(int index, int column, string elem)
    {
        return ElementFactory.GetTextBox(By.XPath($"//table[@class='table']//tr[{index}]//td[{column}]{elem}"), "")
            .Text;
    }

    public JArray GetModels()
    {
        for (var i = InitialRow; i < StartDates.Count; i++)
        {
            dynamic td = new JObject();
            td.name = GetTableRow(i, 1, "");
            td.method = GetTableRow(i, 2, "");
            td.startTime = GetTableRow(i, 4, "");
            td.endTime = GetTableRow(i, 5, "");
            td.status = GetTableRow(i, 3, "/span").ToUpper();
            td.duration = GetTableRow(i, 6, "");
            _array.Add(td);
        }

        return _array;
    }
}