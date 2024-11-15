using System.Transactions;
using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Enums;
using MBS.Repositories.Interfaces;
using MBS.Services.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace MBS.Services.Services.Implements;

public class PointTransactionService : IPointTransactionService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IPointTransactionRepository _pointTransactionRepository;

    public PointTransactionService(
        IStudentRepository studentRepository,
        IPointTransactionRepository pointTransactionRepository,
        ILogger<PointTransactionService> logger)
    {
        _studentRepository = studentRepository;
        _pointTransactionRepository = pointTransactionRepository;
    }

    public async Task<bool> ModifyStudentPoint(string studentId, int amount, string transactionType, string kind)
    {
        try
        {
            var student = await _studentRepository.GetByIdAsync(studentId, "UserId");

            var pointTransaction = new PointTransaction
            {
                Amount = amount,
                UserId = studentId,
                Kind = TransactionKindEnum.Personal
            };

            switch (transactionType.ToUpper())
            {
                case var type when type == nameof(TransactionTypeEnum.Credit).ToUpper():
                {
                    student.WalletPoint += amount;
                    pointTransaction.TransactionType = TransactionTypeEnum.Credit;
                    pointTransaction.RemainBalance = student.WalletPoint;
                    pointTransaction.Status = TransactionStatusEnum.Success;
                    pointTransaction.Kind = Enum.Parse<TransactionKindEnum>(kind);
                    pointTransaction.CreatedOn = DateTime.UtcNow;
                    break;
                }
                case var type when type == nameof(TransactionTypeEnum.Debit).ToUpper():
                {
                    student.WalletPoint -= amount;
                    pointTransaction.TransactionType = TransactionTypeEnum.Debit;
                    pointTransaction.RemainBalance = student.WalletPoint;
                    pointTransaction.Status = TransactionStatusEnum.Success;
                    pointTransaction.Kind = Enum.Parse<TransactionKindEnum>(kind);
                    pointTransaction.CreatedOn = DateTime.UtcNow;
                    break;
                }
            }

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var updateRs = _studentRepository.Update(student);
                if (!updateRs)
                    return false;
                var pointInsertRs = await _pointTransactionRepository.CreateAsync(pointTransaction);
                if (!pointInsertRs)
                    return false;
                transactionScope.Complete();
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}