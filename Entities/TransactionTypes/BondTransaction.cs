using System.Collections.Generic;

// Класс Облигации
namespace FinManApp
{
    class BondTransaction : Transaction
    {
        public string Ticker { get; set; } // Тикер (краткое наименование)
        public string Issuer { get; set; } // Эмитент
        protected double CurrentPrice { get; set; } // Рыночная цена - поле, значение которого должно приходить из стороннего источника (например, с веб-сервера)
        public BondTransaction(Dictionary<string, dynamic> parameters)
        {
            this.TransactionValue = parameters["TransactionValue"];
            this.TransactionDescription = parameters["TransactionDescription"];
            this.Ticker = parameters["Ticker"];
            this.Issuer = parameters["Issuer"];
        }
    }
}
