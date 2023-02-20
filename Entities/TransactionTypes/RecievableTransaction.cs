using System.Collections.Generic;

// Класс Дебиторская задолженность
namespace FinManApp
{
    class ReceivableTransaction : Transaction
    {
        public ReceivableTransaction(Dictionary<string, dynamic> parameters)
        {
            this.TransactionValue = parameters["TransactionValue"];
            this.TransactionDescription = parameters["TransactionDescription"];
        }
    }
}
