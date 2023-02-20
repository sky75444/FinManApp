using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FinManApp
{
    class Runner
    {
        static string UserName { get; set; }
        static string Path { get; set; }
        static string FileName { get; set; }
        static string ProfileName { get; set; }

        #region InternalMethods

        static void StartProgrammAndIdentifyUser()
        {
            Console.WriteLine("Financial Manager Application v. 0.2\n");

            FileName = "TransactionBaseJson";//Имя файла базы. Передается в процедуры

            while (true)
            {
                Console.WriteLine("Введите имя пользователя (Nik (n) NikWorker (nw) or John (j) or JohnnyWorker (jw) ): ");
                UserName = Console.ReadLine();

                if (UserName == "Nik" || UserName == "nik" || UserName == "N" || UserName == "n" || UserName == "т" || UserName == "Т") 
                { UserName = "Nik"; Path = @"B:\VSProjects\finmanapp\TransactionsBase"; break; }

                else if (UserName == "NikWorker" || UserName == "nikWorker" || UserName == "NW" || UserName == "nw" || UserName == "тц" || UserName == "ТЦ")
                { UserName = "Nik"; Path = @"C:\Users\nshakirov\Documents\finmanapp\TransactionsBase"; break; }

                else if (UserName == "John" || UserName == "john" || UserName == "J" || UserName == "j" || UserName == "о" || UserName == "О") 
                { UserName = "John"; Path = @"D:\DS\Code\VS_Projects\finmanapp\TransactionsBase"; break; }

                else if (UserName == "JohnnyWorker" || UserName == "johnnyWorker" || UserName == "JW" || UserName == "jw" || UserName == "оц" || UserName == "ОЦ") 
                { UserName = "JohnnyWorker"; Path = @"C:\VSProjects\finmanapp\TransactionsBase"; break; }

                else { Console.WriteLine("Вы ввели не известного пользователя. Вы лох."); continue; }
            }
        }

        static void IdentifyProfile()
        {
            while (true)
            {
                if (BaseWork.DeserializedFileBase.Keys.Count > 0)
                {
                    Console.WriteLine("\nНа устройстве обнаружены следующие профили:");

                    DisplayAllFindingProfiles();

                    Console.WriteLine("\nВведите название одного из найденных профилей, если хотите работать в нем. Или название нового профиля, если хотите его создать.");
                    Console.WriteLine("Осторожно! Ввод чувствителен к регистру: ");
                }
                else { Console.WriteLine("\nВведите имя профиля:"); }

                string UserAnswer = Console.ReadLine();

                if (BaseWork.DeserializedFileBase.ContainsKey(UserAnswer)) 
                { Console.WriteLine($"\nВы выбрали существующий профиль: {UserAnswer}\n"); ProfileName = UserAnswer; break; }

                else
                {
                    Console.WriteLine("Введенный профиль не найден. Вы хотите его создать? \ny - Создать новый профиль \nn - Повторить ввод");
                    string FinalUserAnswer = Console.ReadLine();

                    if (FinalUserAnswer == "y" || FinalUserAnswer == "Y" || FinalUserAnswer == "н" || FinalUserAnswer == "Н") 
                    { ProfileName = UserAnswer; break; }

                    else if (FinalUserAnswer == "n" || FinalUserAnswer == "N" || FinalUserAnswer == "т" || FinalUserAnswer == "Т") 
                    { ProfileName = ""; continue; }
                }
            }
        }

        static void DisplayAllFindingProfiles()
        {
            foreach (string profileName in BaseWork.DeserializedFileBase.Keys)
            {
                Console.WriteLine(profileName);
            }
        }

        static void FindAndLoadBaseFile()
        {
            Console.WriteLine("Finding and loading base file...");

            BaseWork.FindingAndLoadingBaseFile(Path, FileName);
        }

        static void DisplayTheTransactionsList()
        {
            Console.WriteLine("Вывести весь список(all) или только активные транзакции(oa):");
            string AnswerDisplayType = Console.ReadLine();

            if (BaseWork.DeserializedFileBase.ContainsKey(ProfileName))
            {
                if (AnswerDisplayType == "all")
                {
                    Console.WriteLine("\n\n Список транзакций в базе: \n");
                    foreach (string transactionType in BaseWork.DeserializedFileBase[ProfileName].Keys)
                    {
                        Console.WriteLine(transactionType);
                        for (int i = 0; i <= BaseWork.DeserializedFileBase[ProfileName][transactionType].Count - 1; i++)
                        {
                            if (BaseWork.DeserializedFileBase[ProfileName][transactionType][i].TransactionIsActive() == true)
                            {
                                string StringRepresentationOfTransaction = JsonConvert.SerializeObject(BaseWork.DeserializedFileBase[ProfileName][transactionType][i]);
                                Console.WriteLine($"(index = {i}) #{i + 1}. { StringRepresentationOfTransaction } - Active");
                            }
                            else if (BaseWork.DeserializedFileBase[ProfileName][transactionType][i].TransactionIsActive() == false)
                            {
                                string StringRepresentationOfTransaction = JsonConvert.SerializeObject(BaseWork.DeserializedFileBase[ProfileName][transactionType][i]);
                                Console.WriteLine($"(index = {i}) #{i + 1}. { StringRepresentationOfTransaction } - Deleted");
                            }
                        }
                    }
                }
                else if (AnswerDisplayType == "oa")
                {
                    Console.WriteLine("\n\n Список активных транзакций в базе: \n");
                    foreach (string transactionType in BaseWork.DeserializedFileBase[ProfileName].Keys)
                    {
                        Console.WriteLine(transactionType);
                        for (int i = 0; i <= BaseWork.DeserializedFileBase[ProfileName][transactionType].Count - 1; i++)
                        {
                            if (BaseWork.DeserializedFileBase[ProfileName][transactionType][i].TransactionIsActive() == true)
                            {
                                string StringRepresentationOfTransaction = JsonConvert.SerializeObject(BaseWork.DeserializedFileBase[ProfileName][transactionType][i]);
                                Console.WriteLine($"(index = {i}) #{i + 1}. { StringRepresentationOfTransaction } ");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Похоже... Вы не смогли правильно ввести один из вариантов. Очень жаль.");
                }
                Console.WriteLine("\n\n");
            }
            else
            {
                Console.WriteLine("Для данного профиля не найдено записей!");
            }

        }

        static string DetermineTheTransactionType(string inputAnswer)
        {
            string AnswerTransactionType = inputAnswer;
            while (true)
            {
                if (AnswerTransactionType == "Money" || AnswerTransactionType == "money" || AnswerTransactionType == "m") 
                { AnswerTransactionType = "Money"; break; }

                else if (AnswerTransactionType == "Shares" || AnswerTransactionType == "shares" || AnswerTransactionType == "s") 
                { AnswerTransactionType = "Shares"; break; }

                else
                { 
                    Console.WriteLine("Вы ввели не известную транзакцию. Вы лох. (на данный момент доступны только money(m) и shares(s))"); 
                    AnswerTransactionType = Console.ReadLine(); continue; 
                }
            }
            return AnswerTransactionType;
        }

        static void CreateTransaction()
        {
            Console.WriteLine("Введите тип транзакции (на данный момент доступны только money(m) и shares(s)): ");

            string AnswerTransactionType = DetermineTheTransactionType(Console.ReadLine());

            try
            {
                var NewTransaction = Transaction.CreateTransaction(AnswerTransactionType);

                BaseWork.IncludeTransactionOnBase(AnswerTransactionType, NewTransaction, ProfileName);

                Console.WriteLine();
                Console.WriteLine("Создание транзакции окончено успешно");
            }
            catch { Console.WriteLine("Что-то пошло не так. Создать транзакцию и включить ее в базу не удалось."); }
        }

        static void MarkDeleteTransaction()
        {
            Console.WriteLine("Введите тип транзакции, которую хотите удалить (Money(m), Shares(s)):");
            string AnswerTransactionType = DetermineTheTransactionType(Console.ReadLine());

            Console.WriteLine("Введите НОМЕР странзакции, которую хотите удалить (только число типа INT):");

            try
            {
                int AnswerTransactionNumber = Convert.ToInt32(Console.ReadLine());
                BaseWork.MarkDeleteTransaction(AnswerTransactionType, AnswerTransactionNumber, ProfileName);
            }
            catch
            {
                Console.WriteLine("Не корректный ввод. Пожалуйста повторите, введя НОМЕР странзакции, которую хотите удалить (только число типа INT):");
            }
        }

        static void EditTransaction()
        {
            Console.WriteLine("Введите тип транзакции, которую хотите изменить (Money(m), Shares(s)):");
            string AnswerTransactionType = DetermineTheTransactionType(Console.ReadLine());

            Console.WriteLine("Введите НОМЕР странзакции, которую хотите изменить (только число типа INT):");
            int AnswerTransactionNumber = Convert.ToInt32(Console.ReadLine());

            BaseWork.EditTransaction(AnswerTransactionType, AnswerTransactionNumber, ProfileName);
        }

        #endregion InternalMethods


        static void Main(string[] args)
        {
            StartProgrammAndIdentifyUser();
                        
            FindAndLoadBaseFile();

            IdentifyProfile();

            Console.WriteLine("\nPress any key to continue..."); Console.ReadLine(); Console.WriteLine("\n\n\n");


            #region MainProgramLoop
            bool ContinueLoop = true;
            while (ContinueLoop == true)
            {
                Console.WriteLine("\nВыберете действие:" +
                                  "\nnp - Сменить профиль" +
                                  "\ndt - Отобразить транзакции в базе" +
                                  "\nnt - Создать новую транзакцию" +
                                  "\nmt - Пометить транзакцию на удаление" +
                                  "\net - Изменить транзакцию" +
                                  "\nex - Выйти");

                switch (Console.ReadLine())
                {
                    case "nt":
                        CreateTransaction();
                        continue;

                    case "et":
                        EditTransaction();
                        continue;

                    case "dt":
                        DisplayTheTransactionsList();
                        continue;

                    case "mt":
                        MarkDeleteTransaction();
                        continue;

                    case "np":
                        IdentifyProfile();
                        continue;

                    case "ex":
                        FileWork.SaveTransactionJson(Path, FileName, BaseWork.DeserializedFileBase);
                        ContinueLoop = false;
                        break;

                    default:
                        Console.WriteLine("Вы ввели не известное действие. Пожалуйста выберите одно из предложенных. Ожидаемый ввод указан в скобках.");
                        continue;
                }
                
            }

            #endregion MainProgramLoop

        }

    }
}