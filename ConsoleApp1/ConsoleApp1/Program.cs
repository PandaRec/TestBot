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

namespace ConsoleApp1
{
    class A {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding(1251);

            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            System.Console.InputEncoding = enc1251;

            List<MenuItems> menuitems_list = new List<MenuItems>();
            List<BarInfo> barinfo_list = new List<BarInfo>();

            bool EndOfPages = false;
            int CountOfPages = 1;

            while (EndOfPages==false)
            {
                HtmlDocument doc = new HtmlDocument();
                //doc.LoadHtml(getRequest(@"http://gdebar.ru/bars?mainType[0]=3&withFilter=1&p=" + CountOfPages.ToString() + "&fromUrl=/bars"));
                doc.LoadHtml(getRequest(@"http://gdebar.ru/bars?mainType[0]=3&withFilter=1&p="+CountOfPages.ToString()+"&fromUrl=/bars"));
                //http://gdebar.ru/bars?mainType[0]=3&withFilter=1&p=' + str(i) + '&fromUrl=/bars
                //HtmlNodeCollection l = doc.DocumentNode.SelectNodes("//section[@class = 'catalog__list']");
                //Console.WriteLine(doc.DocumentNode.SelectNodes("//div[@class = 'catalog__list']").Count);
                
                if (doc.DocumentNode.SelectNodes("//section[@class = 'catalog__list']")[0].InnerText.Contains("По данному запросу заведений не найдено :("))
                {
                    EndOfPages = true;
                    Console.WriteLine("stop");
                    return;
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
                    if (Menu == null) continue;
                    BarInfo barinfo = new BarInfo();
                    MenuItems menuitems = new MenuItems();

                    foreach (var item_2 in Menu)
                    {

                        HtmlNode subtitle_path_2 = item_2.ParentNode.ParentNode.ParentNode;        //сладкая вода - не обязательная subtitle_2
                        HtmlNode subtitle_path = subtitle_path_2.ParentNode.ParentNode;       //вода
                        HtmlNode title_path = subtitle_path.ParentNode.ParentNode; //бар
                                                                                   //Console.WriteLine(name_path.InnerText);
                                                                                   //Console.WriteLine(subtitle_path.InnerText);
                        menuitems.BarName = item.InnerText;
                        //if (title_path.Name == "parent") Console.WriteLine("1");
                        if (title_path.GetAttributeValue("class", "") == "parent")
                        {
                            menuitems.Title = title_path.ChildNodes[0].InnerText;       //главная принадлежность
                            Console.WriteLine(title_path.ChildNodes[0].InnerText);
                        }
                        menuitems.Subtitle = subtitle_path.ChildNodes[0].InnerText;     //вторичная принадлежность
                        menuitems.Subtitle_2 = subtitle_path_2.ChildNodes[0].ChildNodes[0].InnerText;   // если существует, то третичная

                        Console.WriteLine(subtitle_path.ChildNodes[0].InnerText);
                        Console.WriteLine(subtitle_path_2.ChildNodes[0].ChildNodes[0].InnerText);

                        HtmlNodeCollection childrens = item_2.ChildNodes;
                        //Console.WriteLine(childrens.Count);
                        menuitems.Dish = childrens[0].ChildNodes[0].InnerText;
                        menuitems.Price =Convert.ToInt32(childrens[1].InnerText.Split(" ")[0]);

                        Console.WriteLine("блюдо - " + childrens[0].ChildNodes[0].InnerText);
                        Console.WriteLine("цена - " + childrens[1].InnerText);




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

                    barinfo.WorkTime = doc.DocumentNode.SelectNodes("//a[@class = 'fancybox3']")[0].InnerText.Replace("\r\n", "").Split("работает ")[1].Replace("   ","");
                    barinfo.Phone = doc.DocumentNode.SelectNodes("//a[@class = 'roistat-phone']")[0].InnerText.Replace(" ", "");
                }

                Console.WriteLine(CountOfPages);
                CountOfPages += 1;
                Thread.Sleep(2000);
                
            }
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


    }
}