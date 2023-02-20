using System;
using System.Collections.Generic;
using System.Reflection;

namespace FinManApp
{
    public abstract class Account
    {   
        protected string AccountType { get; set; }
        protected bool IsActive { get; set; } = true;
        protected double AccountSum { get; set; }
        public string AccountName { get; set; }
        public static Dictionary<string, dynamic> TransactionsList { get; set; }

        //Пока не реализовано
        //protected Guid AccountID;
        //protected DateTime AccountCreationDateTime;
        //protected DateTime AccountLastChangeDateTime;

        #region Internal Methods

        private static PropertyInfo[] GetAccountProperties(string AnswerAccountType)
        {
            PropertyInfo[] propertiesOfAccount;
            Type accountType;
            switch (AnswerAccountType)
            {
                case "Cash":
                    accountType = typeof(CashAccount);
                    propertiesOfAccount = accountType.GetProperties();
                    return propertiesOfAccount;
                case "Broker":
                    accountType = typeof(BrokerAccount);
                    propertiesOfAccount = accountType.GetProperties();
                    return propertiesOfAccount;
                default:
                    throw new Exception("Unexpected tag value");
            }
        }

        private static dynamic FillAccountParameters(string inputAccountType)
        {
            PropertyInfo[] PropertiesOfAccount = GetAccountProperties(inputAccountType);

            Dictionary<string, dynamic> AccountParameters = new Dictionary<string, dynamic>();

            for (int i = 0; i < PropertiesOfAccount.Length; i++)
            {
                Console.WriteLine($"Введите значение параметра: " +
                                  $"{PropertiesOfAccount[i].Name} типа " +
                                  $"{PropertiesOfAccount[i].PropertyType}");
                string ParameterAnswer = Console.ReadLine();
                try
                {
                    var propertyValue = Convert.ChangeType(ParameterAnswer, 
                                                           PropertiesOfAccount[i].PropertyType);
                    AccountParameters.TryAdd(PropertiesOfAccount[i].Name, propertyValue);
                    Console.WriteLine($"Значение успешно установлено: {PropertiesOfAccount[i].PropertyType} =" +
                                      $"{propertyValue}");
                }
                catch 
                {
                    i--;
                    Console.WriteLine("Некорректный ввод данных. " + 
                                      "Пожалуйста вводите значение согласно указанных типов данных.");
                }
            }

            return AccountParameters;
        }

        #endregion Internal Methods

        #region Interface

        public static dynamic CreateAccount(string inputAccountType)
        {
            var AccountParameters = FillAccountParameters(inputAccountType);

            switch (inputAccountType)
            {
                case "Cash":
                    CashAccount NewCashAccount = new CashAccount(AccountParameters);
                    return NewCashAccount;
                case "Broker":
                    BrokerAccount NewBrokerAccount = new BrokerAccount(AccountParameters);
                    return NewBrokerAccount;
                default:
                    throw new Exception("Unexpected tag value");
            }

        }

        #endregion Interface


    }
}
