using System.Collections.Generic;

// Класс Деньги
namespace FinManApp
{
    public class MoneyTransaction : Transaction
    {

        public MoneyTransaction() { }

        public MoneyTransaction(Dictionary<string, dynamic> parameters)
        {
            this.TransactionValue = parameters["TransactionValue"];
            this.TransactionDescription = parameters["TransactionDescription"];
        }
    }
}
