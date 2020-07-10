using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Npgsql;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace ParseSites
{
    class Program
    {
        private static List<BarInfo> BarlistFromSite1 = new List<BarInfo>();
        private static List<BarInfo> BarlistFromSite2 = new List<BarInfo>();
        private static List<BarInfo> BarlistFromAllSites = new List<BarInfo>();


        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding(1251);

            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            System.Console.InputEncoding = enc1251;


            //if("кафе-бар «Дружба 2.0»".Contains(@"&nbsp;"))
              //  Console.WriteLine();
            //string a = "кафе-бар «Дружба 2.0»";

            //string noHTML = Regex.Replace(a, @"<[^>]+>|&nbsp;", "");

            //char[] kk = a.ToCharArray();
            Console.WriteLine(IsSame("Дружба 2.0 на Шаболовке", "кафе-бар «Дружба 2.0»"));
            Console.WriteLine();
            





            /*
            TestMethod();
            
            for (int i = 0; i < 214; i++)
            {
                BarlistFromSite1.Add(BarlistFromAllSites[i]);
            }
            for (int i = 214; i < BarlistFromAllSites.Count; i++)
            {
                BarlistFromSite2.Add(BarlistFromAllSites[i]);
            }
            for (int i = 0; i < BarlistFromSite1.Count; i++)
            {
                for (int j = 0; j < BarlistFromSite2.Count; j++)
                {
                    if(BarlistFromSite1[i].Lat==BarlistFromSite2[j].Lat && BarlistFromSite1[i].Lng==BarlistFromSite2[j].Lng)
                        Console.WriteLine(BarlistFromSite1[i].BarName +"  -  "+BarlistFromSite2[j].BarName);
                }
            }
            */
            Console.WriteLine();
            
            /*
            int t = 0;

            for (int i = 0; i < BarlistFromAllSites.Count; i++)
            {
                for (int j = i+1; j < BarlistFromAllSites.Count; j++)
                {
                    if (IsSame(BarlistFromAllSites[i].BarName.ToLower(), BarlistFromAllSites[j].BarName.ToLower()) == true && !BarlistFromAllSites[i].BarName.ToLower().Equals(BarlistFromAllSites[j].BarName.ToLower()))
                    {
                        Console.WriteLine(BarlistFromAllSites[i].BarName + "  -  " + BarlistFromAllSites[j].BarName);
                    }
                }
            }
            Console.WriteLine();
            */
            
            /*
            foreach (var s1 in BarlistFromAllSites)
            {
                foreach (var s2 in BarlistFromAllSites)
                {
                    if(s1.Lat == s2.Lat && s1.Lng == s2.Lng && IsSame(s1.BarName.ToLower(), s2.BarName.ToLower()) == true && !s1.BarName.ToLower().Equals(s2.BarName.ToLower()))
                    {
                        Console.WriteLine(s1.BarName + "  -  "+s2.BarName);
                        t++;
                    }
                }
            }
            */

            // Console.WriteLine();

            

            Site1.Execute.Do(out BarlistFromSite1);
            Site2.Execute.Do(out BarlistFromSite2);
            Console.WriteLine(BarlistFromSite1.Count);
            Console.WriteLine(BarlistFromSite2.Count);
            ProcessingOfData();
            Console.ReadLine();
            Console.WriteLine("**************************");
            PP();
            Console.WriteLine();

        }

        public static string getRequest(string url)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
                return data;
            }
            else
            {
                //Тут перезапуск службы с ожиданием в несколько минут
                Console.WriteLine("error");
                return "eror";
            }
        }


        
       private static void ProcessingOfData()
       {
            for (int i = 0; i < BarlistFromSite1.Count; i++)
            {
                for (int j = 0; j < BarlistFromSite2.Count; j++)
                {
                    if (BarlistFromSite1[i].Lat == BarlistFromSite2[j].Lat && BarlistFromSite1[i].Lng == BarlistFromSite2[j].Lng)
                        Console.WriteLine(BarlistFromSite1[i].BarName + "  -  " + BarlistFromSite2[j].BarName);
                }
            }

            /*
            int temp_couneter = 0;
            foreach (var s1 in BarlistFromSite1)
                    {
                        foreach (var s2 in BarlistFromSite2)
                        {
                    
                    if(s1.Lat == s2.Lat && s1.Lng == s2.Lng)
                    {
                        Console.WriteLine(s1.BarName + "  -  " + s2.BarName);

                    }
                    //Console.WriteLine("*************************************************");

                    if (s1.Lat == s2.Lat && s1.Lng == s2.Lng && IsSame(s1.BarName.ToLower(), s2.BarName.ToLower()) == true && !s1.BarName.ToLower().Equals(s2.BarName.ToLower()))
                    {
                        //Console.WriteLine("--------------------------------");
                          //      temp_couneter += 1;
                            //    Console.WriteLine(temp_couneter);
                              //  Console.WriteLine(s1.BarName + "  -  "+ s2.BarName);
                                
                                //если есть два одинаковых бара
                                
                                if (s1.HasMenu == true)
                                {
                                    //если у перовго есть меню
                                    List<string> temp_list = new List<string>();
                                    string[] temp_array;
                                    //метро
                                    foreach (var item in s1.NearSubway)
                                    {
                                        //добавляем метро от первого сайт в лист
                                        temp_list.Add(item);
                                    }
                                    foreach (var item in s2.NearSubway)
                                    {
                                        // добавляем метро в лист от второго сайта, если там есть то, чего нет от первого сайта
                                        if (temp_list.IndexOf(item) == -1)
                                            temp_list.Add(item);
                                    }
                                    s1.NearSubway = temp_list.ToArray(); // обновляем данные для первого сайтео метро
                                    temp_list.Clear();
                                    // телефон
                                    temp_array = s1.Phone.Split(", ");
                                    foreach (var item in temp_array)
                                    {
                                        temp_list.Add(item);
                                    }
                                    temp_array = s2.Phone.Split(", ");
                                    //if(temp_list.IndexOf())
                                }
                                
                                
                                else if (s2.HasMenu == true)
                                {
                                    //если у второго есть меню
                                }
                                else
                                {
                                    // если ни у кого нет меню
                                }
                                
                                
                            }
                        }
                    }
*/
        }
        private static void PP()
        {
            for (int i = 0; i < BarlistFromSite1.Count; i++)
            {
                for (int j = 0; j < BarlistFromSite2.Count; j++)
                {
                    if (BarlistFromSite1[i].Lat == BarlistFromSite2[j].Lat && BarlistFromSite1[i].Lng == BarlistFromSite2[j].Lng && IsSame(BarlistFromSite1[i].BarName,BarlistFromSite2[j].BarName))
                        Console.WriteLine(BarlistFromSite1[i].BarName + "  -  " + BarlistFromSite2[j].BarName);
                }
            }
        }

        private static void TestMethod()
        {
            //выборка данных из бд
            BarInfo bar = new BarInfo();
            //Models.ModelOfMenuItems items = new Models.ModelOfMenuItems();
            //Models.ModelOfUserRate rate = new Models.ModelOfUserRate();

            string connectionString = "Server=localhost;Port=5432;User ID=postgres;Password=3400430;Database=Menu;";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            Console.WriteLine("Соединение с БД открыто");
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand("SELECT * FROM barinfo", npgSqlConnection);
            NpgsqlDataReader npgSqlDataReader = npgSqlCommand.ExecuteReader();
            if (npgSqlDataReader.HasRows)
            {
                Console.WriteLine("Таблица: BarInfo");
                Console.WriteLine("");
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                {
                    bar = new BarInfo();
                    //Console.WriteLine(dbDataRecord["BarName"] + "   " + dbDataRecord["Latitude"] + "   " + dbDataRecord["Longitude"] + "   " + dbDataRecord["Phone"] + "   " + dbDataRecord["WorkTime"] + "   " + dbDataRecord["Pictures"]);
                    bar.BarName = dbDataRecord["BarName"].ToString();
                    bar.Lat = Convert.ToDouble(dbDataRecord["Latitude"]);
                    bar.Lng = Convert.ToDouble(dbDataRecord["Longitude"]);
                    bar.Phone = dbDataRecord["Phone"].ToString();
                    bar.WorkTime = dbDataRecord["WorkTime"].ToString();
                    bar.HasMenu = Convert.ToBoolean(dbDataRecord["HasMenu"]);
                    string[] temp = dbDataRecord["Pictures"].ToString().Split("|");
                    foreach (var item in temp)
                        if (item != "")
                            bar.PictureLinks.Add(item);
                    BarlistFromAllSites.Add(bar);
                }
            }
            else
                Console.WriteLine("Запрос не вернул строк в BarInfo");
            npgSqlDataReader.Close();


            /*
            npgSqlCommand = new NpgsqlCommand("SELECT * FROM menuitems", npgSqlConnection);
            npgSqlDataReader = npgSqlCommand.ExecuteReader();
            if (npgSqlDataReader.HasRows)
            {
                Console.WriteLine("Таблица: MenuItems");
                Console.WriteLine("");
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                {
                    items = new Models.ModelOfMenuItems();
                    // Console.WriteLine(dbDataRecord["Title"] + "   " + dbDataRecord["Subtitle"] + "   " + dbDataRecord["Subtitle_2"] + "   " + dbDataRecord["Dish"] + "   " + dbDataRecord["Price"] + "   " + dbDataRecord["BarName"]);
                    items.Title = dbDataRecord["Title"].ToString();
                    items.Subtitle = dbDataRecord["Subtitle"].ToString();
                    items.Subtitle_2 = dbDataRecord["Subtitle_2"].ToString();
                    items.Dish = dbDataRecord["Dish"].ToString();
                    items.Price = Convert.ToInt32(dbDataRecord["Price"]);
                    items.BarName = dbDataRecord["BarName"].ToString();
                    MenuItems.Add(items);
                }
            }
            else
                Console.WriteLine("Запрос не вернул строк в MenuItems");
            npgSqlDataReader.Close();


            npgSqlCommand = new NpgsqlCommand("SELECT * FROM UserRate", npgSqlConnection);
            npgSqlDataReader = npgSqlCommand.ExecuteReader();
            if (npgSqlDataReader.HasRows)
            {
                Console.WriteLine("Таблица: UserRate");
                Console.WriteLine("");
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                {
                    rate = new Models.ModelOfUserRate();
                    // Console.WriteLine(dbDataRecord["Title"] + "   " + dbDataRecord["Subtitle"] + "   " + dbDataRecord["Subtitle_2"] + "   " + dbDataRecord["Dish"] + "   " + dbDataRecord["Price"] + "   " + dbDataRecord["BarName"]);
                    rate.BarName = dbDataRecord["BarName"].ToString();

                    string[] temp = dbDataRecord["Likes"].ToString().Split(",");
                    foreach (var item in temp)
                    {
                        if (item != "")
                            rate.Likes.Add(Convert.ToInt64(item));
                    }

                    temp = dbDataRecord["Dislikes"].ToString().Split(",");
                    foreach (var item in temp)
                    {
                        if (item != "")
                            rate.Dislikes.Add(Convert.ToInt64(item));
                    }


                    UserRate.Add(rate);
                }
            }
            else
                Console.WriteLine("Запрос не вернул строк в MenuItems");

    */
            Console.WriteLine("чтение завершено");
            //npgSqlDataReader.Close();
            npgSqlConnection.Close();
            
        }

        private static bool IsSame(string title1,string title2)
        {
            char[] title1_1 = title1.ToLower().ToCharArray();
            char[] title2_2 = title2.ToLower().ToCharArray();

            //перевод транслитом
            for (int i = 0; i < title1_1.Length; i++)
            {
                if (title1_1[i] >= 97 && title1_1[i] <= 122)
                {
                    title1_1[i] = Transliteration(title1_1[i]);
                }
                if (title1_1[i] == 160) title1_1[i] = ' ';
            }

            for (int i = 0; i < title2_2.Length; i++)
            {
                if (title2_2[i] >= 97 && title2_2[i] <= 122)
                {
                    title2_2[i] = Transliteration(title2_2[i]);
                }
                if (title2_2[i] == 160) title2_2[i] = ' ';


            }
            
            (string,string) middlewords = Comparison.DeleteExtraSymbols(new string(title1_1), new string(title2_2));
            middlewords = Comparison.DeleteAddresses(middlewords.Item1, middlewords.Item2);
            if (middlewords.Item1 == "" && middlewords.Item2 == "") // если адреса разные
                return false;
            (string,string) middlewordsExtra= Comparison.DeleteExtraWords(middlewords.Item1, middlewords.Item2);
            //middlewords = DeleteExtraWords(middlewords.Item1, middlewords.Item2);

            if (Comparison.Compare(middlewordsExtra.Item1, middlewordsExtra.Item2) == false)
            {
                middlewordsExtra= Comparison.DeleteExtraWords(middlewords.Item1, middlewords.Item2,false);

                if(Comparison.Compare(middlewordsExtra.Item1, middlewordsExtra.Item2) == false)
                {
                    middlewordsExtra = Comparison.DeleteExtraWords(middlewords.Item1, middlewords.Item2);

                    if(Comparison.Compare(middlewordsExtra.Item1, middlewordsExtra.Item2, true) == false)
                    {
                        middlewordsExtra = Comparison.DeleteExtraWords(middlewords.Item1, middlewords.Item2, false);

                        if (Comparison.Compare(middlewordsExtra.Item1, middlewordsExtra.Item2, true) == false)
                        {
                            //тут будет перевод
                            return false;
                        }
                        else
                        {
                            return true;
                        }

                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
                
                
            }
            else
                return true;

            
            
            

        }
        private static char Transliteration(char symbol)
        {

            switch (symbol)
            {

                case 'a': return 'а';
                case 'b': return 'б';
                case 'v': return 'в';
                case 'g': return 'г';
                case 'd': return 'д';
                case 'e': return 'е';
                case 'z': return 'з';
                case 'i': return 'и';
                case 'y': return 'й';
                case 'k': return 'к';
                case 'l': return 'л';
                case 'm': return 'м';
                case 'n': return 'н';
                case 'o': return 'о';
                case 'p': return 'п';
                case 'r': return 'р';
                case 's': return 'с';
                case 't': return 'т';
                case 'u': return 'у';
                case 'f': return 'ф';
                case 'c': return 'ц';
                default: return '*';


            }
        }
            
    }
             
}
