namespace Exam.Utils;

public static class UtilsStringGen
{
    private static readonly Random Random = new();

    public static string? RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}