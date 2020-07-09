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

            Console.WriteLine(IsSame("музыкальный бар «Мумий Тролль»", "бар Tiki-bar"));
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
            foreach (var item in BarlistFromSite1)
            {
                if(item.BarName.Contains("мумий тролль"))
                    Console.WriteLine(item.BarName);
            }
            Console.WriteLine();
            foreach (var item in BarlistFromSite2)
            {
                if (item.BarName.Contains("Мумий Тролль"))
                    Console.WriteLine(item.BarName);
            }
          Console.WriteLine(  );
            foreach (var item in BarlistFromSite1)
            {
                if (item.BarName.Contains("Мумий Тролль"))
                    Console.WriteLine(item.BarName);
            }
            foreach (var item in BarlistFromSite2)
            {
                if (item.BarName.Contains("Мумий Тролль"))
                    Console.WriteLine(item.BarName);
            }
            Console.WriteLine();
            for (int i = 0; i < BarlistFromSite2.Count; i++)
            {
                if (BarlistFromSite2[i].BarName.Contains("Мумий Тролль"))
                    Console.WriteLine(i);
            }
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
            }

            for (int i = 0; i < title2_2.Length; i++)
            {
                if (title2_2[i] >= 97 && title2_2[i] <= 122)
                {
                    title2_2[i] = Transliteration(title2_2[i]);
                }
                
            }
            // убираем апострофы
            string temp1 = new string(title1_1);
            if (temp1.Contains('\''))
                temp1 = temp1.Replace(@"'", "");

            string temp2 = new string(title2_2);
            if (temp2.Contains('\''))
                temp2=temp2.Replace(@"'", "");
            //убираем адрес
            string add1=null;
            string add2=null;
            if (temp1.Contains(" на "))
                add1 = temp1.Split(" на ")[1];
            if (temp2.Contains(" на "))
                add2 = temp2.Split(" на ")[1];

            if(add1 == null && add2 == null)
            {
                // обычная проверка
            }
            else if((add1==null && add2 != null) || (add1!=null && add2==null))
            {
                // у одного есть адрес => удаляем его и сравниваем названия
                if (add1 != null)
                {
                    temp1 = temp1.Remove(temp1.IndexOf(" на "));
                }
                else if (add2 != null)
                {
                    temp2 = temp2.Remove(temp2.IndexOf(" на "));

                }
            }
            else if (!add1.Equals(add2))
            {
                // адреса разные => разные бары
                return false;
            }
            else if (add1.Equals(add2))
            {
                //адрес один => проверка без адреса
                temp1 = temp1.Remove(temp1.IndexOf(" на "));
                temp2 = temp2.Remove(temp2.IndexOf(" на "));


            }
            //удаляем слово не значащее слово "бар"
            string pattern = @"\sбар$|^бар\s";
            string pattern2 = @"\sбар\s";

            if (Regex.IsMatch(temp1, pattern))
            {
                Regex regex = new Regex(pattern);
                temp1 = regex.Replace(temp1, "").Trim();
            }
            else if (Regex.IsMatch(temp1, pattern2))
            {
                Regex regex = new Regex(pattern2);
                temp1 = regex.Replace(temp1, " ");

            }
            if (Regex.IsMatch(temp2, pattern))
            {
                Regex regex = new Regex(pattern);
                temp2= regex.Replace(temp2, "");
            }
            else if (Regex.IsMatch(temp2, pattern2))
            {
                Regex regex = new Regex(pattern2);
                temp2 = regex.Replace(temp2, " ");

            }
            //удаляем ковычки
            if (temp1.Contains("«"))
                temp1 = temp1.Replace("«", "");
            if (temp1.Contains("»"))
                temp1 = temp1.Replace("»", "");

            if (temp2.Contains("«"))
                temp2 = temp2.Replace("«", "");
            if (temp2.Contains("»"))
                temp2 = temp2.Replace("»", "");

            //удаояем точки
            if (temp1.Contains("."))
                temp1 = temp1.Replace(".", "");
            if (temp2.Contains("."))
                temp2 = temp2.Replace(".", "");

            //разбиваем предлажение на слова
            string[] WorsOfTitle1 = temp1.Split(" ");
            string[] WorsOfTitle2 = temp2.Split(" ");

            double part = 0; //чему равна доля совпадения
            double res=0;   //результат

            if (WorsOfTitle1.Length > WorsOfTitle2.Length)
            {
                
                //if(WorsOfTitle2.Length>1)
                  //  part = 100.0 / WorsOfTitle2.Length;
                //else
                    part = 100.0 / WorsOfTitle1.Length;
                    

                for (int i = 0; i < WorsOfTitle1.Length; i++)
                {
                    for (int j = 0; j < WorsOfTitle2.Length; j++)
                    {
                        if (WorsOfTitle1[i].Equals(WorsOfTitle2[j])) res += part;
                    }
                }
            }
            else if (WorsOfTitle2.Length > WorsOfTitle1.Length )
            {
                
                //if (WorsOfTitle1.Length > 1)
                  //  part = 100.0 / WorsOfTitle1.Length;
                //else
                    part = 100.0 / WorsOfTitle2.Length;
                    

                for (int i = 0; i < WorsOfTitle2.Length; i++)
                {
                    for (int j = 0; j < WorsOfTitle1.Length; j++)
                    {
                        if (WorsOfTitle2[i].Equals(WorsOfTitle1[j])) res += part;
                    }
                }
            }
            else
            {
                part = 100.0 / WorsOfTitle1.Length;
                for (int i = 0; i < WorsOfTitle2.Length; i++)
                {
                    for (int j = 0; j < WorsOfTitle1.Length; j++)
                    {
                        if (WorsOfTitle2[i].Equals(WorsOfTitle1[j])) res += part;
                    }
                }
                if (res == 100)
                    return true;
                else
                    return false;
            }
            if (res > 50)
                return true;
            else
                return false;

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
