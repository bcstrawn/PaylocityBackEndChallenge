using System;
using Api.Services;
using Xunit;

namespace ApiTests.UnitTests;

public class DateTimeHelperTests
{
    [Fact]
    public void GetAge_ReturnsCorrectAge_WhenJanuaryBirthday()
    {
        var dateOfBirth = new DateTime(1990, 1, 1);
        var today = new DateTime(2023, 10, 12);
        var expectedAge = 33;

        var actualAge = DateTimeHelper.CalculateAge(dateOfBirth, today);

        Assert.Equal(expectedAge, actualAge);
    }

    [Fact]
    public void GetAge_ReturnsCorrectAge_WhenDecemberBirthday()
    {
        var dateOfBirth = new DateTime(1990, 12, 31);
        var today = new DateTime(2023, 10, 12);
        var expectedAge = 32;

        var actualAge = DateTimeHelper.CalculateAge(dateOfBirth, today);

        Assert.Equal(expectedAge, actualAge);
    }

    [Fact]
    public void GetAge_ReturnsCorrectAge_WhenBirthdayYesterday()
    {
        var dateOfBirth = new DateTime(1990, 10, 11);
        var today = new DateTime(2023, 10, 12);
        var expectedAge = 33;

        var actualAge = DateTimeHelper.CalculateAge(dateOfBirth, today);

        Assert.Equal(expectedAge, actualAge);
    }

    [Fact]
    public void GetAge_ReturnsCorrectAge_WhenBirthdayToday()
    {
        var dateOfBirth = new DateTime(1990, 10, 12);
        var today = new DateTime(2023, 10, 12);
        var expectedAge = 33;

        var actualAge = DateTimeHelper.CalculateAge(dateOfBirth, today);

        Assert.Equal(expectedAge, actualAge);
    }

    [Fact]
    public void GetAge_ReturnsCorrectAge_WhenBirthdayTomorrow()
    {
        var dateOfBirth = new DateTime(1990, 10, 13);
        var today = new DateTime(2023, 10, 12);
        var expectedAge = 32;

        var actualAge = DateTimeHelper.CalculateAge(dateOfBirth, today);

        Assert.Equal(expectedAge, actualAge);
    }
}