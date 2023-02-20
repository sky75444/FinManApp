using System;
using System.Collections.Generic;
using System.Reflection;


namespace FinManApp
{
    public abstract class Transaction
    {
        protected string TransactionType { get; set; }
        protected bool IsActive { get; set; } = true;
        public double TransactionValue { get; set; }
        public string TransactionDescription { get; set; }

        //Пока не реализовано
        // protected  Guid TransactionID { get; set; }
        // protected DateTime TransactionCreationDateTime { get; set; }
        // public DateTime TransactionDateTime { get; set; }


        #region Internal Methods

        private static PropertyInfo[] GetTransactionProperties(string AnswerTransactionType)
        {
            PropertyInfo[] propertiesOfTransaction;
            Type transactionType;
            switch (AnswerTransactionType)
            {
                case "Money":
                    transactionType = typeof(MoneyTransaction);
                    propertiesOfTransaction = transactionType.GetProperties();
                    return propertiesOfTransaction;
                case "Shares":
                    transactionType = typeof(SharesTransaction);
                    propertiesOfTransaction = transactionType.GetProperties();
                    return propertiesOfTransaction;
                case "Bond":
                    transactionType = typeof(BondTransaction);
                    propertiesOfTransaction = transactionType.GetProperties();
                    return propertiesOfTransaction;
                case "Receivable":
                    transactionType = typeof(ReceivableTransaction);
                    propertiesOfTransaction = transactionType.GetProperties();
                    return propertiesOfTransaction;
                default:
                    throw new Exception("Unexpected tag value");
            }
        }

        private static dynamic FillTransactionParameters(string inputTransactionType)
        {
            PropertyInfo[] PropertiesOfTransaction = GetTransactionProperties(inputTransactionType);

            Dictionary<string, dynamic> TransactionParameters = new Dictionary<string, dynamic>();
            for (int i = 0; i < PropertiesOfTransaction.Length; i++)
            {
                Console.WriteLine($"Введите значение параметра: " +
                                  $"{PropertiesOfTransaction[i].Name} типа " +
                                  $"{PropertiesOfTransaction[i].PropertyType}");
                string ParameterAnswer = Console.ReadLine();
                try
                {
                    var propertyValue = Convert.ChangeType(ParameterAnswer, 
                                                           PropertiesOfTransaction[i].PropertyType);
                    TransactionParameters.TryAdd(PropertiesOfTransaction[i].Name, propertyValue);
                    Console.WriteLine($"Значение успешно установлено: {PropertiesOfTransaction[i].PropertyType} = " +
                                                                       $"{propertyValue}");

                }
                catch 
                { 
                    i--; 
                    Console.WriteLine("Некорректный ввод данных. " + 
                                      "Пожалуйста вводите значения согласно указанным типам данных"); 
                }
            }

            return TransactionParameters;
        }

        #endregion Internal Methods

        #region Interface

        public static dynamic CreateTransaction(string inputTransactionType)
        {
            var TransactionParameters = FillTransactionParameters(inputTransactionType);

            switch (inputTransactionType)
            {
                case "Money":
                    MoneyTransaction NewMoneyTransaction = new MoneyTransaction(TransactionParameters);
                    return NewMoneyTransaction;
                case "Shares":
                    SharesTransaction NewStocksTransaction = new SharesTransaction(TransactionParameters);
                    return NewStocksTransaction;
                case "Bond":
                    BondTransaction NewBondTransaction = new BondTransaction(TransactionParameters);
                    return NewBondTransaction;
                case "Receivable":
                    ReceivableTransaction NewReceivableTransaction = new ReceivableTransaction(TransactionParameters);
                    return NewReceivableTransaction;
                default:
                    throw new Exception("Unexpected tag value");
            }
        }

        public bool TransactionIsActive()
        {
            return this.IsActive; 
        }

        #endregion Interface

    }
}
