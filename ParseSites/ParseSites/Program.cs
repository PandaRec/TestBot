using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Net.Http;
using Npgsql;
using System.Data.Common;
using System.Threading;
using Npgsql.Logging;

namespace ParseSites.Site1
{
    class Program
    {
        static void Main(string[] args)
        {
            string apikey = "5c22c518-ebcf-4750-8524-872e96677344";
            //string address = "";
            //List<string> pos = Yandex.Yandex.GetPos(apikey, address);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding(1251);

            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            System.Console.InputEncoding = enc1251;
            while (true)
            {
                List<MenuItems> menuitems_list = new List<MenuItems>();
                List<BarInfo> barinfo_list = new List<BarInfo>();

                bool EndOfPages = false;
                int CountOfPages = 1;

                while (EndOfPages == false)
                {
                    HtmlDocument doc = new HtmlDocument();
                    //doc.LoadHtml(getRequest(@"http://gdebar.ru/bars?mainType[0]=3&withFilter=1&p=" + CountOfPages.ToString() + "&fromUrl=/bars"));
                    doc.LoadHtml(getRequest(@"http://gdebar.ru/bars?mainType[0]=3&withFilter=1&p=" + CountOfPages.ToString() + "&fromUrl=/bars"));
                    //http://gdebar.ru/bars?mainType[0]=3&withFilter=1&p=' + str(i) + '&fromUrl=/bars
                    //HtmlNodeCollection l = doc.DocumentNode.SelectNodes("//section[@class = 'catalog__list']");
                    //Console.WriteLine(doc.DocumentNode.SelectNodes("//div[@class = 'catalog__list']").Count);

                    if (doc.DocumentNode.SelectNodes("//section[@class = 'catalog__list']")[0].InnerText.Contains("По данному запросу заведений не найдено :("))
                    {
                        EndOfPages = true;
                        Console.WriteLine("stop");
                        continue;
                    }
                    else { Console.WriteLine("continue"); }

                    //Console.WriteLine(doc.Encoding.EncodingName);
                    //Console.WriteLine(doc);
                    HtmlNodeCollection BarCollection = doc.DocumentNode.SelectNodes("//div[@class = 'place-card__specif']/a");

                    for (int i = 0; i < BarCollection.Count; i++)
                        if (!BarCollection[i].InnerText.Contains("\r\n"))
                            BarCollection.Remove(BarCollection[i]);


                    foreach (var item in BarCollection) //тестовый вывод
                    {
                        Console.WriteLine(item.InnerText);
                        // Console.WriteLine(item.Attributes["href"].Value);

                    }
                    Console.WriteLine(BarCollection.Count);


                    foreach (var item in BarCollection)
                    {
                        HtmlDocument doc_2 = new HtmlDocument();
                        doc_2.LoadHtml(getRequest("http://gdebar.ru" + item.Attributes["href"].Value + "/menu"));
                        HtmlNodeCollection Menu = doc_2.DocumentNode.SelectNodes("//div[@class = 'menu__dish d-flex align-items-center justify-content-between p-2']");
                        //if (Menu == null) continue;
                        if (doc_2.DocumentNode.SelectNodes("//div[@class = 'text-center alert alert-danger h1']")!=null ) continue;
                        if (Menu == null)
                        {
                            BarInfo info = new BarInfo();
                            
                            HtmlNodeCollection ff = doc_2.DocumentNode.SelectNodes("//div[@id = 'bar-gallery-main']/div/a");// отсюда берем ссылки на пикчи
                            if (ff != null)
                            {
                                foreach (var item_3 in ff)
                                {
                                    info.PictureLinks.Add(item_3.Attributes["href"].Value);
                                }
                            }
                            else info.PictureLinks.Add(null);

                            if (doc_2.DocumentNode.SelectNodes("//a[@class = 'fancybox3']")[0].InnerText.ToLower().Contains("работает"))
                                info.WorkTime = doc_2.DocumentNode.SelectNodes("//a[@class = 'fancybox3']")[0].InnerText.Replace("\r\n", "").Split("работает ")[1].Replace("   ", "");
                            else info.WorkTime = "отсутсвует";
                            if (doc_2.DocumentNode.SelectNodes("//a[@class = 'roistat-phone']") != null)
                                info.Phone = doc_2.DocumentNode.SelectNodes("//a[@class = 'roistat-phone']")[0].InnerText.Trim();
                            else
                                info.Phone = doc_2.DocumentNode.SelectNodes("//div[@class = 'phone bar__main--info__line d-flex align-items-center justify-content-start mb-4 w-100 flex-nowrap']")[0].InnerText.Trim();
                            string add = doc_2.DocumentNode.SelectNodes("//span[@class = 'font-weight-light mr-0']")[0].InnerText.Trim();
                            List<string> poss = Yandex.Yandex.GetPos(apikey, add);

                            info.Lat = Convert.ToDouble(poss[0].Split(" ")[1].Replace(".", ","));       //широта
                            info.Lng = Convert.ToDouble(poss[0].Split(" ")[0].Replace(".", ","));       //долгота 

                            info.BarName = item.InnerText.Trim();
                            info.HasMenu = false;
                            barinfo_list.Add(info);

                            continue;

                        }
                        BarInfo barinfo = new BarInfo();
                        MenuItems menuitems = new MenuItems();

                        foreach (var item_2 in Menu)
                        {
                            barinfo = new BarInfo();
                            menuitems = new MenuItems();
                            HtmlNode subtitle_path_2 = item_2.ParentNode.ParentNode.ParentNode;        //сладкая вода - не обязательная subtitle_2
                            HtmlNode subtitle_path = subtitle_path_2.ParentNode.ParentNode;       //вода
                            HtmlNode title_path = subtitle_path.ParentNode.ParentNode; //бар
                                                                                       //Console.WriteLine(name_path.InnerText);
                                                                                       //Console.WriteLine(subtitle_path.InnerText);
                            menuitems.BarName = item.InnerText.Replace("\r", "").Replace("\n", "").Trim();
                            //if (title_path.Name == "parent") Console.WriteLine("1");
                            if (title_path.GetAttributeValue("class", "") == "parent")
                            {
                                if (title_path.ChildNodes[0].InnerText.Contains(" ("))
                                    menuitems.Title = title_path.ChildNodes[0].InnerText.Split(" (")[0];       //главная принадлежность
                                else
                                    menuitems.Title = title_path.ChildNodes[0].InnerText;
                                Console.WriteLine(title_path.ChildNodes[0].InnerText);
                            }
                            if (subtitle_path.ChildNodes[0].InnerText.Contains(" ("))
                                menuitems.Subtitle = subtitle_path.ChildNodes[0].InnerText.Split(" (")[0];     //вторичная принадлежность
                            else
                                menuitems.Subtitle = subtitle_path.ChildNodes[0].InnerText;
                            if (subtitle_path_2.ChildNodes[0].ChildNodes[0].InnerText.Contains(" ("))
                                menuitems.Subtitle_2 = subtitle_path_2.ChildNodes[0].ChildNodes[0].InnerText.Split(" (")[0];   // если существует, то третичная
                            else
                                menuitems.Subtitle_2 = subtitle_path_2.ChildNodes[0].ChildNodes[0].InnerText;

                            Console.WriteLine(subtitle_path.ChildNodes[0].InnerText);
                            Console.WriteLine(subtitle_path_2.ChildNodes[0].ChildNodes[0].InnerText);

                            HtmlNodeCollection childrens = item_2.ChildNodes;
                            //Console.WriteLine(childrens.Count);
                            menuitems.Dish = childrens[0].ChildNodes[0].InnerText;
                            menuitems.Price = Convert.ToInt32(childrens[1].InnerText.Split(" ")[0]);

                            Console.WriteLine("блюдо - " + childrens[0].ChildNodes[0].InnerText);
                            Console.WriteLine("цена - " + childrens[1].InnerText);


                            menuitems_list.Add(menuitems);

                            //Console.WriteLine(subtitle_path.ChildNodes[0].InnerText);
                            //Console.WriteLine(name_path.ChildNodes[0].ChildNodes[0].InnerText);

                            //Console.WriteLine(title_path.ChildNodes[0].InnerText);
                            //Console.WriteLine(name_path.Name);
                            //Console.WriteLine(subtitle_path.Name);
                            //Console.WriteLine(title_path.OriginalName);
                        }


                        Console.WriteLine("-");
                        //Console.WriteLine(item);
                        //doc_2.LoadHtml(getRequest("http://gdebar.ru" + item.Attributes["href"].Value));
                        HtmlNodeCollection f = doc_2.DocumentNode.SelectNodes("//div[@id = 'bar-gallery-main']/div/a");// отсюда берем ссылки на пикчи

                        foreach (var item_3 in f)
                        {
                            barinfo.PictureLinks.Add(item_3.Attributes["href"].Value);
                        }
                        if (doc_2.DocumentNode.SelectNodes("//a[@class = 'fancybox3']")[0].InnerText.ToLower().Contains("работает"))
                            barinfo.WorkTime = doc_2.DocumentNode.SelectNodes("//a[@class = 'fancybox3']")[0].InnerText.Replace("\r\n", "").Split("работает ")[1].Replace("   ", "");
                        else barinfo.WorkTime = "отсутсвует";
                        if (doc_2.DocumentNode.SelectNodes("//a[@class = 'roistat-phone']") != null)
                            barinfo.Phone = doc_2.DocumentNode.SelectNodes("//a[@class = 'roistat-phone']")[0].InnerText.Trim();
                        else
                            barinfo.Phone = doc_2.DocumentNode.SelectNodes("//div[@class = 'phone bar__main--info__line d-flex align-items-center justify-content-start mb-4 w-100 flex-nowrap']")[0].InnerText.Trim();
                        string address = doc_2.DocumentNode.SelectNodes("//span[@class = 'font-weight-light mr-0']")[0].InnerText.Trim();
                        List<string> pos = Yandex.Yandex.GetPos(apikey, address);

                        barinfo.Lat = Convert.ToDouble(pos[0].Split(" ")[1].Replace(".", ","));       //широта
                        barinfo.Lng = Convert.ToDouble(pos[0].Split(" ")[0].Replace(".", ","));       //долгота 

                        barinfo.BarName = item.InnerText.Trim();
                        barinfo.HasMenu = true;
                        barinfo_list.Add(barinfo);
                    }
                    Console.WriteLine(CountOfPages);
                    CountOfPages += 1;
                    Thread.Sleep(2000);

                }
                //запись в бд
                ClearBD("barinfo");
                ClearBD("menuitems");
                PastIntoBD(barinfo_list, menuitems_list);

                Console.WriteLine("--------------------------------------------------------------------");
                Thread.Sleep(50000); //86400000 - это сутки
            }
            //Console.WriteLine("END");
            //выборка данных из тестовой бд
            /*
            string connectionString = "Server=localhost;Port=5432;User ID=postgres;Password=3400430;Database=TestBD;";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            Console.WriteLine("Соединение с БД открыто");
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand("SELECT * FROM example", npgSqlConnection);
            NpgsqlDataReader npgSqlDataReader = npgSqlCommand.ExecuteReader();
            if (npgSqlDataReader.HasRows)
            {
                Console.WriteLine("Таблица: example");
                Console.WriteLine("id value");
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                    Console.WriteLine(dbDataRecord["id"] + "   " + dbDataRecord["value"]);
            }
            else
                Console.WriteLine("Запрос не вернул строк");
                */
            Console.ReadLine();

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

        private static void PastIntoBD(List<BarInfo> bar, List<MenuItems> menu)//List<MenuItems> menu,List<BarInfo>bar
        {

            //запись в BarInfo
            string sqlquery= "INSERT INTO barinfo (BarName, Latitude, Longitude, Phone, WorkTime, HasMenu, Pictures) VALUES (@BarName, @Latitude, @Longitude, @Phone, @WorkTime, @HasMenu, @Pictures)";
            string connectionString = "Server=localhost;Port=5432;User ID=postgres;Password=3400430;Database=Menu;";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            
            //Console.WriteLine("Соединение с БД открыто");
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(sqlquery, npgSqlConnection);

            npgSqlCommand.Parameters.Add("BarName",NpgsqlTypes.NpgsqlDbType.Char);
            npgSqlCommand.Parameters.Add("Latitude", NpgsqlTypes.NpgsqlDbType.Real);
            npgSqlCommand.Parameters.Add("Longitude", NpgsqlTypes.NpgsqlDbType.Real);
            npgSqlCommand.Parameters.Add("Phone", NpgsqlTypes.NpgsqlDbType.Varchar);
            npgSqlCommand.Parameters.Add("WorkTime", NpgsqlTypes.NpgsqlDbType.Varchar);
            npgSqlCommand.Parameters.Add("HasMenu", NpgsqlTypes.NpgsqlDbType.Boolean);
            npgSqlCommand.Parameters.Add("Pictures", NpgsqlTypes.NpgsqlDbType.Text);

            foreach (var item in bar)
            {
                npgSqlCommand.Parameters["BarName"].Value = item.BarName;
                npgSqlCommand.Parameters["Latitude"].Value = item.Lat;
                npgSqlCommand.Parameters["Longitude"].Value = item.Lng;
                npgSqlCommand.Parameters["Phone"].Value = item.Phone;
                npgSqlCommand.Parameters["WorkTime"].Value = item.WorkTime;
                npgSqlCommand.Parameters["HasMenu"].Value = item.HasMenu;

                string pictures = "";
                foreach (var item_2 in item.PictureLinks)
                {
                    pictures += item_2 + "|";
                }
                npgSqlCommand.Parameters["Pictures"].Value = pictures;

                npgSqlCommand.ExecuteNonQuery();
            }

            //запись в MenuItems
            sqlquery = "INSERT INTO menuitems (Title, Subtitle, Subtitle_2, Dish, Price, BarName) VALUES (@Title, @Subtitle, @Subtitle_2, @Dish, @Price, @BarName)";
            npgSqlCommand = new NpgsqlCommand(sqlquery, npgSqlConnection);

            npgSqlCommand.Parameters.Add("Title", NpgsqlTypes.NpgsqlDbType.Char);
            npgSqlCommand.Parameters.Add("Subtitle", NpgsqlTypes.NpgsqlDbType.Char);
            npgSqlCommand.Parameters.Add("Subtitle_2", NpgsqlTypes.NpgsqlDbType.Char);
            npgSqlCommand.Parameters.Add("Dish", NpgsqlTypes.NpgsqlDbType.Varchar);
            npgSqlCommand.Parameters.Add("Price", NpgsqlTypes.NpgsqlDbType.Integer);
            npgSqlCommand.Parameters.Add("BarName", NpgsqlTypes.NpgsqlDbType.Text);

            foreach (var item in menu)
            {
                if (item.Title == null)
                    npgSqlCommand.Parameters["Title"].Value = DBNull.Value;
                else 
                    npgSqlCommand.Parameters["Title"].Value = item.Title; 
                if(item.Subtitle==null)
                    npgSqlCommand.Parameters["Subtitle"].Value = DBNull.Value;
                else
                    npgSqlCommand.Parameters["Subtitle"].Value = item.Subtitle;
                if(item.Subtitle_2==null)
                    npgSqlCommand.Parameters["Subtitle_2"].Value = DBNull.Value;
                else 
                    npgSqlCommand.Parameters["Subtitle_2"].Value = item.Subtitle_2;
                npgSqlCommand.Parameters["Dish"].Value = item.Dish;
                npgSqlCommand.Parameters["Price"].Value = item.Price;
                npgSqlCommand.Parameters["BarName"].Value = item.BarName;

                npgSqlCommand.ExecuteNonQuery();
            }
            npgSqlConnection.Close();

            /*
            string connectionString = "Server=localhost;Port=5432;User ID=postgres;Password=3400430;Database=TestBD;";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            Console.WriteLine("Соединение с БД открыто");
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand("SELECT * FROM bar", npgSqlConnection);
            NpgsqlDataReader npgSqlDataReader = npgSqlCommand.ExecuteReader();
            if (npgSqlDataReader.HasRows)
            {
                Console.WriteLine("Таблица: example");
                Console.WriteLine("id value");
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                    Console.WriteLine(dbDataRecord["id"] + "   " + dbDataRecord["value"]);
            }
            else
                Console.WriteLine("Запрос не вернул строк");
            */

        }

        private static void ClearBD(string name_of_table)
        {
            

                string sqlquery = "TRUNCATE TABLE "+name_of_table;
            string connectionString = "Server=localhost;Port=5432;User ID=postgres;Password=3400430;Database=Menu;";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();

            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(sqlquery, npgSqlConnection);

            npgSqlCommand.ExecuteNonQuery();
            npgSqlConnection.Close();

        }
    }
}
