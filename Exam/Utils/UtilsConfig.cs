using Exam.Models;
using Newtonsoft.Json;

namespace Exam.Utils;

public static class UtilsConfig
{
    public static string? GetData(string key)
    {
        var pathJson = Path.GetFullPath(Paths.Config);
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