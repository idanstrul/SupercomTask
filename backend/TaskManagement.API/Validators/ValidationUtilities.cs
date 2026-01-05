using System.Text.RegularExpressions;

namespace TaskManagement.API.Validators;

public static class ValidationUtilities
{
    public static bool BeValidDate(DateTime date)
    {
        return date != default(DateTime);
    }

    public static bool BeValidTelephone(string? telephone)
    {
        if (string.IsNullOrWhiteSpace(telephone))
            return false;

        // Basic telephone validation - allows digits, spaces, dashes, parentheses, and plus sign
        var pattern = @"^[\+]?[(]?[0-9]{1,4}[)]?[-\s\.]?[(]?[0-9]{1,4}[)]?[-\s\.]?[0-9]{1,9}$";
        return Regex.IsMatch(telephone, pattern);
    }
}
