using System.Collections.Generic;

// Класс Акции
namespace FinManApp
{
    class SharesTransaction : Transaction
    {
        public string Ticker { get; set; } // Тикер (краткое наименование)
        public string Issuer { get; set; } // Эмитент

        protected double currentPrice;
        //public double currentprice { get; set; } // рыночная цена


        public SharesTransaction() { }

        public SharesTransaction(Dictionary<string, dynamic> parameters)
        {
            this.TransactionValue = parameters["TransactionValue"];
            this.TransactionDescription = parameters["TransactionDescription"];
            this.Ticker = parameters["Ticker"];
            this.Issuer = parameters["Issuer"];
            this.currentPrice = 0; // В дальнейшем будет инициализироваться методом, который будет получать значение из стороннего источника (веб-сервер)
        }
    }
}
