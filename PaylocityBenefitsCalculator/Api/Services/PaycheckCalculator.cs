using Api.Dtos.Paycheck;
using Api.Models;

namespace Api.Services;

public class PaycheckCalculator : IPaycheckCalculator
{
    private const int NUM_PAYCHECKS = 26;
    private const decimal EMP_MONTHLY_BASE_COST = 1000m;
    private const decimal EMP_OVER80K_COST = 0.02m;
    private const decimal DEPENDENT_MONTHLY_COST = 600m;
    private const decimal DEPENDENT_OVER50_COST = 200m;

    public GetPaycheckDto CalculatePaycheck(Employee employee, DateTime checkDate)
    {
        var empYearlyBaseCost = EMP_MONTHLY_BASE_COST * 12;
        var empYearlyOver80kCost = employee.Salary > 80000 ? employee.Salary * EMP_OVER80K_COST : 0;

        // This doesn't account for dependents aging into the over50 category during the current year
        var yearlyDependentCosts = employee.Dependents.Sum(dep => CalculateDependentMonthlyCost(dep, checkDate)) * 12;

        // These calculations don't account for rounding up/down to the nearest cent and would need to add/subtract pennies throughout the year
        var yearlyBenefitsDeductions = empYearlyBaseCost + empYearlyOver80kCost + yearlyDependentCosts;
        var paycheckBenefitsDeductions = Math.Round(yearlyBenefitsDeductions / NUM_PAYCHECKS, 2);
        var paycheckGrossPay = Math.Round(employee.Salary / NUM_PAYCHECKS, 2);
        var paycheckNetPay = paycheckGrossPay - paycheckBenefitsDeductions;

        return new GetPaycheckDto
        {
            EmployeeId = employee.Id,
            EmployeeFirstName = employee.FirstName,
            EmployeeLastName = employee.LastName,
            GrossPay = paycheckGrossPay,
            Deductions = paycheckBenefitsDeductions,
            NetPay = paycheckNetPay,
            CheckDate = checkDate
        };
    }

    private decimal CalculateDependentMonthlyCost(Dependent dependent, DateTime checkDate)
    {
        var dependentCost = DEPENDENT_MONTHLY_COST;
        var age = DateTimeHelper.CalculateAge(dependent.DateOfBirth, checkDate);
        if (age > 50)
        {
            dependentCost += DEPENDENT_OVER50_COST;
        }
        return dependentCost;
    }
}

public interface IPaycheckCalculator
{
    GetPaycheckDto CalculatePaycheck(Employee employee, DateTime checkDate);
}