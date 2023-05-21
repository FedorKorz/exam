using Newtonsoft.Json.Linq;

namespace Exam.Utils;

public static class UtilsArrays
{
    public static bool IsArrayHasData(JArray allTest, JArray testFromPage)
    {
        return allTest.All(firstTest => testFromPage.Any(secondTest => JToken.DeepEquals(firstTest, secondTest)));
    }
}