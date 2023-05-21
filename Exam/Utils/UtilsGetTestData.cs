using Exam.Models;
using Newtonsoft.Json;

namespace Exam.Utils;

public class UtilsGetTestData
{
    public static string? Get(string key)
    {
        var pathJson = Path.GetFullPath(Paths.TestData);
        try
        {
            dynamic file = JsonConvert.DeserializeObject(File.ReadAllText(pathJson))!;
            return Convert.ToString(file[$"{key}"]);
        }
        catch
        {
            return null;
        }
    }
}