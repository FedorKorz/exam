using Exam.Models;

namespace Exam.Utils;

public static class UtilsImg
{
    public static string GetArray64(byte[] imageBytes)
    {
        return Convert.ToBase64String(imageBytes);
    }
}