﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Course.Task8
{
    class Subtask1
    {
        public int quarterInfo;
        public int apartmenCount;
        public DateTime[] receivingInfo;
        public List<ClientInfo>[] clientsInfo;

        #region Constructors
        public Subtask1()
        {
            quarterInfo = default;
            apartmenCount = default;
            receivingInfo = Array.Empty<DateTime>();
            clientsInfo = Array.Empty<List<ClientInfo>>();
        }

        public Subtask1(int quarterInfo, int apartmenCount, DateTime[] receivingInfo)
        {
            this.quarterInfo = quarterInfo;
            this.apartmenCount = apartmenCount;
            this.receivingInfo = new DateTime[receivingInfo.Length];
            Array.Copy(receivingInfo, this.receivingInfo, receivingInfo.Length);
            clientsInfo = new List<ClientInfo>[3];
            for(int i = 0; i < 3; i++)
            {
                this.clientsInfo[i] = new List<ClientInfo>();
            }
        }

        public Subtask1(int quarterInfo, int apartmenCount, DateTime[] receivingInfo , List<ClientInfo>[] clientsInfo)
        {
            this.quarterInfo = quarterInfo;
            this.apartmenCount = apartmenCount;
            this.receivingInfo = new DateTime[receivingInfo.Length];
            Array.Copy(receivingInfo, this.receivingInfo, receivingInfo.Length);
            this.clientsInfo = new List<ClientInfo>[clientsInfo.Length];
            Array.Copy(clientsInfo, this.clientsInfo, clientsInfo.Length);
        }

        public Subtask1(string filePath)
        {
            if (File.Exists(filePath))
            {
                clientsInfo = new List<ClientInfo>[3];
                for(int i = 0; i < clientsInfo.Length; i++)
                {
                    clientsInfo[i] = new List<ClientInfo>();
                }
                receivingInfo = new DateTime[3];
                using (StreamReader reader = new StreamReader(filePath))
                {
                    try
                    {
                        string line = reader.ReadLine();
                        string[] str = line.Split();
                        string exceptions = "";

                        while (true)
                        {
                            if (str.Length == 2)
                            {
                                if (!int.TryParse(str[0], out quarterInfo)) exceptions += "Incorect quarterInfo\n";
                                if (!int.TryParse(str[1], out apartmenCount)) exceptions += "Incorect apartmenCount\n";
                                if (exceptions.Length != 0) throw new ArgumentException(exceptions);
                                break;
                            }
                            else
                            {
                                str = UserInterface.GetSplitedStringFromConsole(2, "необхiдний квартал та кiлькiсть квартир");
                            }
                        }

                        int monthForm = (quarterInfo - 1) * 3 + 1;
                        for (int i = 0; i < 3; i++)
                        {
                            line = reader.ReadLine();
                            str = line.Split();

                            while (str.Length != 2)
                            {
                                str = UserInterface.GetSplitedStringFromConsole(2, "день та рiк подання показникiв за " +
                                    $"{CultureInfo.GetCultureInfo("ua-UA").DateTimeFormat.GetMonthName(i + monthForm)}");
                            }
                            int day = 0, year = 0;

                            if (!int.TryParse(str[1], out year))
                            {
                                do
                                {
                                    year = UserInterface.GetIntFromConsole("правильний рiк");
                                } while (year > 0);
                            }
                            if (!int.TryParse(str[0], out day))
                            {
                                do
                                {
                                    day = UserInterface.GetIntFromConsole("правильний день подання показникiв у " +
                                        $"{CultureInfo.GetCultureInfo("ua - UA").DateTimeFormat.GetMonthName(i + monthForm)} {year} року");
                                } while (day > 0 && day < DateTime.DaysInMonth(year, i + monthForm));
                            }

                            receivingInfo[i] = new DateTime(year, i + monthForm, day);

                            while (true)
                            {
                                line = reader.ReadLine();
                                if (line == "" || line == null) break;
                                clientsInfo[i].Add(new ClientInfo(line));
                            }
                        }
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
            else throw new FileNotFoundException();
        }
        #endregion

        #region WriteToFileMethods
        public void WriteToFile(string filePath)
        {
            FileInteract.WriteToFile(filePath, this.ToString());
        }

        public void WriteToFileOneApartment(string filePath, int apartmentNumber)
        {
            string result = $"Квартал: { quarterInfo}. Кiлькiсть квартир: { apartmenCount}\n\n";
            for (int i = 0; i < receivingInfo.Length; i++)
            {
                result += String.Format("{0,-15} {1,-2} {2,-8} {3,-4}\n", "Дати подання показникiв:",
                    receivingInfo[i].Day, CultureInfo.GetCultureInfo("ua-UA").DateTimeFormat.GetMonthName(receivingInfo[i].Month), receivingInfo[i].Year);
                result += String.Format("{0,-15} {1,-20} {2,-30} {3,-30}\n", "Номер квартири", "Прiзвище власника", "Вихiднi показники лiчильника", "Вхiднi показники лiчильника");

                foreach (ClientInfo j in clientsInfo[i]) 
                    if(j.apartmenNumber == apartmentNumber) result += j + "\n";

                result += "\n";
            }

            FileInteract.WriteToFile(filePath, result);
        }
        #endregion

        #region FindMethods
        public string FindSurnameWhoUseMaxEnergy()
        {
            int maxEnergy = 0;
            int indexOfClient = 0;

            for(int i = 0; i < clientsInfo[0].Count; i++)
            {
                int temp = 0;
                for(int j = 0; j < 3; j++)
                {
                    temp += clientsInfo[j][i].GetTheNumberOfUsedEnergy();
                }
                if (temp > maxEnergy)
                {
                    indexOfClient = i;
                    maxEnergy = temp;
                }
            }

            string result = clientsInfo[0][indexOfClient].surname;

            return result;
        }

        public int FindApartmentWhereEnergyDontUse()
        {
            int indexOfClient = 0;

            for (int i = 0; i < clientsInfo[0].Count; i++)
            {
                int temp = 0;
                for (int j = 0; j < 3; j++)
                {
                    temp += clientsInfo[j][i].GetTheNumberOfUsedEnergy();
                }
                if (temp == 0)
                {
                    indexOfClient = i;
                }
            }

            return clientsInfo[0][indexOfClient].apartmenNumber;
        }

        public List<(int, int)> FindPriceOfEnergyForAllApartments(int energyPrice)
        {
            List<(int, int)> result = new List<(int, int)>();
            for (int i = 0; i < clientsInfo[0].Count; i++)
            {
                int temp = 0;
                for (int j = 0; j < 3; j++)
                {
                    temp += clientsInfo[j][i].GetTheNumberOfUsedEnergy();
                }
                result.Add((i + 1, temp * energyPrice));
            }

            return result;
        }

        public int FindNumbersOfDaysFromLastRemoval()
        {
            TimeSpan result = DateTime.Now.Subtract(receivingInfo[2]);
            return result.Days;
        }
        #endregion

        public void PrintInfo()
        {
            Console.WriteLine(FindNumbersOfDaysFromLastRemoval());
        }

        public static Subtask1 operator +(Subtask1 a, Subtask1 b)
        {
            int quarterInfo = a.quarterInfo + b.quarterInfo;
            int apartmentCount = a.apartmenCount + b.apartmenCount;

            DateTime[] receivingInfo = new DateTime[a.receivingInfo.Length + b.receivingInfo.Length];
            Array.Copy(a.receivingInfo, receivingInfo, a.receivingInfo.Length);
            Array.Copy(b.receivingInfo, 0, receivingInfo, a.receivingInfo.Length, b.receivingInfo.Length);

            List<ClientInfo>[] clientsInfo = new List<ClientInfo>[a.clientsInfo.Length + b.clientsInfo.Length];
            Array.Copy(a.clientsInfo, clientsInfo, a.clientsInfo.Length);
            Array.Copy(b.clientsInfo, 0, clientsInfo, a.clientsInfo.Length, b.clientsInfo.Length);

            Subtask1 result = new Subtask1(quarterInfo, apartmentCount, receivingInfo, clientsInfo);
            return result;
        }

        public static Subtask1 operator -(Subtask1 a, Subtask1 b)
        {
            Subtask1 result = new Subtask1(a.quarterInfo, a.apartmenCount, a.receivingInfo);
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < a.clientsInfo[i].Count; j++)
                {
                    foreach (ClientInfo c in b.clientsInfo[i])
                    {
                        if (a.clientsInfo[i][j].Equals(c)) continue;
                        else result.clientsInfo[i].Add(a.clientsInfo[i][j]);
                    }
                }
            }
            return result;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder($"Квартал: {quarterInfo}. Кiлькiсть квартир: {apartmenCount}\n\n");

            for(int i = 0; i < receivingInfo.Length; i++)
            {
                result.AppendFormat("{0,-15} {1,-2} {2,-8} {3,-4}\n", "Дати подання показникiв:",
                    receivingInfo[i].Day, CultureInfo.GetCultureInfo("ua-UA").DateTimeFormat.GetMonthName(receivingInfo[i].Month), receivingInfo[i].Year);
                result.AppendFormat("{0,-15} {1,-20} {2,-30} {3,-30}\n", "Номер квартири", "Прiзвище власника", "Вихiднi показники лiчильника", "Вхiднi показники лiчильника");
                foreach (ClientInfo j in clientsInfo[i]) result.Append(j + "\n");
                result.Append("\n");
            }

            return result.ToString();
        }
    }
}