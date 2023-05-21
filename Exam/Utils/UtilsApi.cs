using Exam.Models;
using OpenQA.Selenium;
using RestSharp;

namespace Exam.Utils;

public static class UtilsApi
{
    public static RestResponse CallApiMethod(string method, Dictionary<string, string?> parameters)
    {
        var restClient = new RestClient(UtilsConfig.GetData("API")!);
        var request = new RestRequest(method);
        foreach (var (key, value) in parameters) request.AddParameter(key, value);
        var response = restClient.ExecutePost(request);
        return response;
    }

    public static Cookie SendCookie(string? token)
    {
        return new Cookie("token", token, "localhost", "/", null);
    }

    public static string GetToken()
    {
        var parameters = new Dictionary<string, string?>
        {
            {"variant", UtilsGetTestData.Get("VARIANT")}
        };
        var token = CallApiMethod(ApiMethods.GetToken, parameters);
        return token.Content!;
    }
}