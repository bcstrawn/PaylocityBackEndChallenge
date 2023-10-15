namespace Api.Services;

public static class DateTimeHelper
{
    // A static helper works fine for this, but it would need to be injected if it ever needed to be mocked
    public static int CalculateAge(DateTime dateOfBirth, DateTime? today = null)
    {
        today ??= DateTime.Today;
        var age = today.Value.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.Value.AddYears(-age)) age--;
        return age;
    }
}