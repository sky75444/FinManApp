using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
//using System.Text.Json;
//using System.Text.Json.Serialization;


namespace FinManApp
{


    public abstract class FileWork
    {

        #region Interface

        #region SaveMethods

        public static void SaveTransactionJson(string inputPath, string fileName, dynamic input)
        {
            string path = $@"{inputPath}\{fileName}.json";

            //Сериализуем в строку
            string jsonString = JsonConvert.SerializeObject(input);

            //Чекаем и бэкапаем файл
            CheckTheFileExistsAndBackup(inputPath, fileName);

            //Перезаписываем файл
            StreamWriter stWriter = new StreamWriter(path, false, System.Text.Encoding.Default);
            stWriter.Write(jsonString);
            stWriter.Close();

            Console.WriteLine($"Файл базы сохранен по пути: {path}");
        }//Для сохранения транзакции. Если нужно поменять сохраненные транзакции, то сначала получить их, изменить а потом сохранить используя этот метод

        #endregion SaveMethods


        #region LoadMethods

        public static dynamic LoadJsonBase(string inputPath, string fileName) //Загружает из файла. Требует подключения Newtonsoft.Json
        {
            string path = $@"{inputPath}\{fileName}";
            FileInfo Actualfile = new FileInfo($"{path}.json");
            FileInfo Buckupfile = new FileInfo($@"{inputPath}\{fileName}_buckup.json");

            if ((!Actualfile.Exists || Actualfile.Length <= 2) && (!Buckupfile.Exists || Buckupfile.Length == 0))
            {
                Console.WriteLine("Актуальный файл не найден, либо пуст!");
                CreateTheFileJson(inputPath, fileName);
            }
            else if ((!Actualfile.Exists || Actualfile.Length <= 2) && (Buckupfile.Exists || Buckupfile.Length != 0))
            {
                File.Delete($@"{path}");
                File.Copy($@"{path}_buckup.json", $@"{path}");
                Console.WriteLine("Файл восстановлен из бэкапа! Нужно проверить актуальность данных");
            }

            #region Loading

            Dictionary<string, Dictionary<string, List<dynamic>>> FinalResult = new Dictionary<string, Dictionary<string, List<dynamic>>>();

            Dictionary<string, Dictionary<string, List<dynamic>>> TotalDeserializedFileBase = 
                JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<dynamic>>>>(File.ReadAllText($@"{path}.json"));

            foreach (KeyValuePair<string, Dictionary<string, List<dynamic>>> ProfileBase in TotalDeserializedFileBase)
            {
                FinalResult.Add(ProfileBase.Key, ProfileBase.Value);

                Dictionary<string, List<dynamic>> InterimResult = new Dictionary<string, List<dynamic>>();

                foreach (KeyValuePair<string, List<dynamic>> externalKeyValue in TotalDeserializedFileBase[ProfileBase.Key])
                {
                    InterimResult.Add(externalKeyValue.Key, externalKeyValue.Value);
                    if (externalKeyValue.Key == "Money" || externalKeyValue.Key == "money")
                    {
                        for (int i = 0; i <= TotalDeserializedFileBase[ProfileBase.Key][externalKeyValue.Key].Count - 1; i++)
                        {
                            string SerializedObjectTransaction = JsonConvert.SerializeObject(TotalDeserializedFileBase[ProfileBase.Key][externalKeyValue.Key][i]);

                            MoneyTransaction DeserializedMoneyTransaction = JsonConvert.DeserializeObject<MoneyTransaction>(SerializedObjectTransaction);

                            InterimResult[externalKeyValue.Key][i] = DeserializedMoneyTransaction;
                        }
                    }
                    else if (externalKeyValue.Key == "Shares" || externalKeyValue.Key == "shares")
                    {
                        for (int i = 0; i <= TotalDeserializedFileBase[ProfileBase.Key][externalKeyValue.Key].Count - 1; i++)
                        {
                            string SerializedObjectTransaction = JsonConvert.SerializeObject(TotalDeserializedFileBase[ProfileBase.Key][externalKeyValue.Key][i]);

                            SharesTransaction DeserializedMoneyTransaction = JsonConvert.DeserializeObject<SharesTransaction>(SerializedObjectTransaction);

                            InterimResult[externalKeyValue.Key][i] = DeserializedMoneyTransaction;
                        }
                    }
                }

                FinalResult[ProfileBase.Key] = InterimResult;

            }
            return FinalResult;

            #endregion Loading

        }

        #endregion LoadMethods

        #endregion Interface



        #region InternalLogic

        private static void CreateTheFileJson(string inputPath, string fileName)
        {
            string path = $@"{inputPath}\{fileName}.json";

            Dictionary<string, List<dynamic>> newFile = new Dictionary<string, List<dynamic>>();

            string jsonString = JsonConvert.SerializeObject(newFile);
            StreamWriter stWriter = new StreamWriter(path, false, System.Text.Encoding.Default);
            stWriter.Write(jsonString);
            stWriter.Close();
        }

        private static void CheckTheFileExistsAndBackup(string inputPath, string fileName)
        {
            string path = $@"{inputPath}\{fileName}";
            FileInfo Actualfile = new FileInfo($@"{path}.json");
            FileInfo Buckupfile = new FileInfo($@"{path}_buckup.json");

            if (!Actualfile.Exists || Actualfile.Length <= 2)
            {
                if (!Buckupfile.Exists || Buckupfile.Length <= 2)
                {
                    Console.WriteLine("Актуальный файл базы и бэкап не найдены, либо пусты!");
                    CreateTheFileJson(inputPath, fileName);
                }
                else
                {
                    File.Delete($@"{path}.json");
                    File.Copy($@"{path}_buckup.json", $@"{path}.json");
                    Console.WriteLine("База восстановлена из бэкапа! Нужно проверить актуальность данных");
                }
            }
            else
            {
                File.Delete($@"{path}_buckup.json");
                File.Copy($@"{path}.json", $@"{path}_buckup.json");
                //Console.WriteLine($"Создан бэкап файла по пути: {path}_buckup.json");
            }
        }

        #endregion InternalLogic

    }
}