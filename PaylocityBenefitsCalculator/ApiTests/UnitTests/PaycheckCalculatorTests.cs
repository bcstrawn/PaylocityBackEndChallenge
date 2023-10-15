using System;
using System.Collections.Generic;
using Api.Dtos.Paycheck;
using Api.Models;
using Api.Services;
using Xunit;

namespace ApiTests.UnitTests;

public class PaycheckCalculatorTests
{
    private readonly PaycheckCalculator _paycheckCalculator = new();

    [Fact]
    public void CalculatePaycheck_ReturnsCorrectEmployeeValues_WhenSingleEmp()
    {
        var employee = new Employee
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        };

        var checkDate = new DateTime(2023, 10, 12);

        var expectedPaycheck = new GetPaycheckDto
        {
            EmployeeId = employee.Id,
            EmployeeFirstName = employee.FirstName,
            EmployeeLastName = employee.LastName,
            CheckDate = checkDate
        };

        var actualPaycheck = _paycheckCalculator.CalculatePaycheck(employee, checkDate);

        Assert.Equal(expectedPaycheck.EmployeeId, actualPaycheck.EmployeeId);
        Assert.Equal(expectedPaycheck.EmployeeFirstName, actualPaycheck.EmployeeFirstName);
        Assert.Equal(expectedPaycheck.EmployeeLastName, actualPaycheck.EmployeeLastName);
        Assert.Equal(expectedPaycheck.CheckDate, actualPaycheck.CheckDate);
    }

    [Fact]
    public void CalculatePaycheck_ReturnsCorrectValues_WhenSingleEmp()
    {
        var employee = new Employee
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        };

        var checkDate = new DateTime(2023, 10, 12);

        var expectedPaycheck = new GetPaycheckDto
        {
            GrossPay = 2900.81m,
            Deductions = 461.54m,
            NetPay = 2439.27m,
        };

        var actualPaycheck = _paycheckCalculator.CalculatePaycheck(employee, checkDate);

        Assert.Equal(expectedPaycheck.GrossPay, actualPaycheck.GrossPay);
        Assert.Equal(expectedPaycheck.Deductions, actualPaycheck.Deductions);
        Assert.Equal(expectedPaycheck.NetPay, actualPaycheck.NetPay);
    }

    [Fact]
    public void CalculatePaycheck_ReturnsCorrectValues_WhenMultipleDependents()
    {
        var employee = new Employee
        {
            Id = 2,
            FirstName = "Ja",
            LastName = "Morant",
            Salary = 92365.22m,
            DateOfBirth = new DateTime(1999, 8, 10),
            Dependents = new List<Dependent>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Spouse",
                    LastName = "Morant",
                    Relationship = Relationship.Spouse,
                    DateOfBirth = new DateTime(1998, 3, 3)
                },
                new()
                {
                    Id = 2,
                    FirstName = "Child1",
                    LastName = "Morant",
                    Relationship = Relationship.Child,
                    DateOfBirth = new DateTime(2020, 6, 23)
                },
                new()
                {
                    Id = 3,
                    FirstName = "Child2",
                    LastName = "Morant",
                    Relationship = Relationship.Child,
                    DateOfBirth = new DateTime(2021, 5, 18)
                }
            }
        };

        var checkDate = new DateTime(2023, 10, 12);

        var expectedPaycheck = new GetPaycheckDto
        {
            GrossPay = 3552.51m,
            Deductions = 1363.36m,
            NetPay = 2189.15m,
        };

        var actualPaycheck = _paycheckCalculator.CalculatePaycheck(employee, checkDate);

        Assert.Equal(expectedPaycheck.GrossPay, actualPaycheck.GrossPay);
        Assert.Equal(expectedPaycheck.Deductions, actualPaycheck.Deductions);
        Assert.Equal(expectedPaycheck.NetPay, actualPaycheck.NetPay);
    }

    [Fact]
    public void CalculatePaycheck_ReturnsCorrectValues_WhenDependentOver50()
    {
        var employee = new Employee
        {
            Id = 3,
            FirstName = "Michael",
            LastName = "Jordan",
            Salary = 143211.12m,
            DateOfBirth = new DateTime(1963, 2, 17),
            Dependents = new List<Dependent>
            {
                new()
                {
                    Id = 4,
                    FirstName = "DP",
                    LastName = "Jordan",
                    Relationship = Relationship.DomesticPartner,
                    DateOfBirth = new DateTime(1972, 1, 2) // deviating from default data to get over 50
                }
            }
        };

        var checkDate = new DateTime(2023, 10, 12);

        var expectedPaycheck = new GetPaycheckDto
        {
            GrossPay = 5508.12m,
            Deductions = 940.93m,
            NetPay = 4567.19m,
        };

        var actualPaycheck = _paycheckCalculator.CalculatePaycheck(employee, checkDate);

        Assert.Equal(expectedPaycheck.GrossPay, actualPaycheck.GrossPay);
        Assert.Equal(expectedPaycheck.Deductions, actualPaycheck.Deductions);
        Assert.Equal(expectedPaycheck.NetPay, actualPaycheck.NetPay);
    }
}