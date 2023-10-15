namespace Api.Dtos.Paycheck;

public class GetPaycheckDto
{
    public int EmployeeId { get; set; }
    public string? EmployeeFirstName { get; set; }
    public string? EmployeeLastName { get; set; }
    public decimal GrossPay { get; set; }
    public decimal Deductions { get; set; }
    public decimal NetPay { get; set; }
    public DateTime CheckDate { get; set; }
}