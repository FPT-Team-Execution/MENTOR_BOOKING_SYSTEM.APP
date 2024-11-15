namespace MBS.Services.Services.Interfaces;

public interface IPointTransactionService
{
    Task<bool> ModifyStudentPoint(string studentId, int amount, string transactionType, string kind);
}