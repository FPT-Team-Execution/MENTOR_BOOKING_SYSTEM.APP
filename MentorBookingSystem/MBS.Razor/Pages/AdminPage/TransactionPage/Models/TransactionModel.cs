using MBS.Services.Constants.Enums;

namespace MBS.Razor.Pages.AdminPage.TransactionPage.Models
{
    public class TransactionModel
    { 
        public string StudentId { get; set; }
        public int Amout { get; set; }
        public TransactionTypeEnum TransactionTypeEnum { get; set; }
    }
}
