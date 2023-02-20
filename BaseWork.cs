using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinManApp
{
    static class BaseWork
    {
        public static Dictionary<string, Dictionary<string, List<dynamic>>> DeserializedFileBase { get; private set; }


        public static void IncludeTransactionOnBase(string inputTransactionType, dynamic inputTransaction, string inputProfileName)
        {
            if (DeserializedFileBase.ContainsKey(inputProfileName))
            {
                if (DeserializedFileBase[inputProfileName].ContainsKey(inputTransactionType)) { DeserializedFileBase[inputProfileName][inputTransactionType].Add(inputTransaction); }
                else { DeserializedFileBase[inputProfileName].Add(inputTransactionType, new List<dynamic> { inputTransaction }); }
            }
            else
            {
                DeserializedFileBase.Add(inputProfileName, new Dictionary<string, List<dynamic>>());
                DeserializedFileBase[inputProfileName].Add(inputTransactionType, new List<dynamic> { inputTransaction });
            }
        }

        public static void FindingAndLoadingBaseFile(string inputPath, string inputFileName)
        {
            DeserializedFileBase = FileWork.LoadJsonBase(inputPath, inputFileName);//Загружает файл базы
        }

        public static void MarkDeleteTransaction(string inputTransactionType, int inputNumberOfTransaction, string inputProfileName)
        {
            DeserializedFileBase[inputProfileName][inputTransactionType][inputNumberOfTransaction - 1].IsActive = false;
        }

        public static void EditTransaction(string inputTransactionType, int inputNumberOfTransaction, string inputProfileName)
        {
            var NewTransaction = Transaction.CreateTransaction(inputTransactionType);

            DeserializedFileBase[inputProfileName][inputTransactionType][inputNumberOfTransaction - 1] = NewTransaction;
        }
    }
}
