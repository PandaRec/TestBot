using HtmlAgilityPack;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace ParseSites.Site1
{
    class Execute
    {
        /// <summary>
        /// API key for yandex geocoder
        /// </summary>
        private static string Apikey { get; } = "5c22c518-ebcf-4750-8524-872e96677344";
        private static List<MenuItems> Menuitems_list { get; set; } = new List<MenuItems>();
        //private static List<BarInfo> Barinfo_list { get; set; } = new List<BarInfo>();

        public static void Do(out List<BarInfo> barinfo_list_list)
        {
            
            barinfo_list_list = new List<BarInfo>();
            //while (true)
            //{

                bool EndOfPages = false;
                int CountOfPages = 1;

                while (EndOfPages == false)
                {
                    HtmlDocument doc = new HtmlDocument();
                    //doc.LoadHtml(getRequest(@"http://gdebar.ru/bars?mainType[0]=3&withFilter=1&p=" + CountOfPages.ToString() + "&fromUrl=/bars"));
                    doc.LoadHtml(Program.getRequest(@"http://gdebar.ru/bars?mainType[0]=3&withFilter=1&p=" + CountOfPages.ToString() + "&fromUrl=/bars"));
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
                        doc_2.LoadHtml(Program.getRequest("http://gdebar.ru" + item.Attributes["href"].Value + "/menu"));
                        HtmlNodeCollection Menu = doc_2.DocumentNode.SelectNodes("//div[@class = 'menu__dish d-flex align-items-center justify-content-between p-2']");
                        //if (Menu == null) continue;
                        if (doc_2.DocumentNode.SelectNodes("//div[@class = 'text-center alert alert-danger h1']") != null) continue;
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

                        if (doc_2.DocumentNode.SelectNodes("//div[@class = 'dropdown-menu dropdown-menu--tooltip']/a") != null)
                        {
                            HtmlNodeCollection subwayy = doc_2.DocumentNode.SelectNodes("//div[@class = 'dropdown-menu dropdown-menu--tooltip']/a");

                            List<string> twmm = new List<string>();
                            foreach (var item_2 in subwayy)
                            {
                                twmm.Add(item_2.InnerText.Replace("\n", "").Trim().Split("  ")[0]);
                            }
                            info.NearSubway = twmm.ToArray();
                        }
                        else if (doc_2.DocumentNode.SelectNodes("//div[@class = 'metro d-flex align-items-start pl-4 mt-2 flex-wrap']") != null)
                        {
                            List<string> twm = new List<string>();

                            HtmlNode temp = doc_2.DocumentNode.SelectNodes("//div[@class = 'metro d-flex align-items-start pl-4 mt-2 flex-wrap']")[0];
                            string sub = temp.InnerText.Replace("\r\n", "").Trim().Split("  ")[0];
                            twm.Add(sub);
                            info.NearSubway = twm.ToArray();
                        }
                        else
                        {
                            info.NearSubway = new string[] { "отсутствует" };
                        }

                        if (doc_2.DocumentNode.SelectNodes("//a[@class = 'fancybox3']")[0].InnerText.ToLower().Contains("работает"))
                                info.WorkTime = doc_2.DocumentNode.SelectNodes("//a[@class = 'fancybox3']")[0].InnerText.Replace("\r\n", "").Split("работает ")[1].Replace("   ", "");
                            else info.WorkTime = "время работы неизвестно";
                            if (doc_2.DocumentNode.SelectNodes("//a[@class = 'roistat-phone']") != null)
                                info.Phone = doc_2.DocumentNode.SelectNodes("//a[@class = 'roistat-phone']")[0].InnerText.Trim();
                            else
                                info.Phone = doc_2.DocumentNode.SelectNodes("//div[@class = 'phone bar__main--info__line d-flex align-items-center justify-content-start mb-4 w-100 flex-nowrap']")[0].InnerText.Trim();
                            string add = doc_2.DocumentNode.SelectNodes("//span[@class = 'font-weight-light mr-0']")[0].InnerText.Trim();
                            List<string> poss = Yandex.Yandex.GetPos(Apikey, add);
                        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine(poss[0].Split(" ")[1]);
                        Console.WriteLine("---------------------------------------------------");
                        info.Lat = Convert.ToDouble(poss[0].Split(" ")[1].Replace(".", ","));       //широта
                            info.Lng = Convert.ToDouble(poss[0].Split(" ")[0].Replace(".", ","));       //долгота 

                            info.BarName = item.InnerText.Trim();
                        if (info.BarName.Contains('ё')) info.BarName.Replace("ё", "е");

                        info.HasMenu = false;
                            barinfo_list_list.Add(info);

                            continue;

                        }

                    BarInfo barinfo = new BarInfo();
                        MenuItems menuitems = new MenuItems();
                    //barinfo.NearSubway[0] = doc_2.DocumentNode.SelectNodes("//div[@class = 'metro d-flex align-items-start pl-4 mt-2 flex-wrap']/div");
                    //HtmlNodeCollection temp = doc_2.DocumentNode.SelectNodes("//div[@class = 'metro d-flex align-items-start pl-4 mt-2 flex-wrap']");
                    HtmlNodeCollection subway = doc_2.DocumentNode.SelectNodes("//div[@class = 'dropdown-menu dropdown-menu--tooltip']/a");
                    Console.WriteLine(item.InnerText.Trim());
                    if (doc_2.DocumentNode.SelectNodes("//div[@class = 'dropdown-menu dropdown-menu--tooltip']/a") != null)
                    {
                         subway = doc_2.DocumentNode.SelectNodes("//div[@class = 'dropdown-menu dropdown-menu--tooltip']/a");

                        List<string> twm = new List<string>();
                        foreach (var item_2 in subway)
                        {
                            twm.Add(item_2.InnerText.Replace("\n", "").Trim().Split("  ")[0]);
                        }
                        barinfo.NearSubway = twm.ToArray();

                        
                    }
                    else if (doc_2.DocumentNode.SelectNodes("//div[@class = 'metro d-flex align-items-start pl-4 mt-2 flex-wrap']") != null)
                    {
                        List<string> twm = new List<string>();

                        HtmlNode temp = doc_2.DocumentNode.SelectNodes("//div[@class = 'metro d-flex align-items-start pl-4 mt-2 flex-wrap']")[0];
                        string sub = temp.InnerText.Replace("\r\n", "").Trim().Split("  ")[0];
                        twm.Add(sub);
                        barinfo.NearSubway = twm.ToArray();
                    }
                    else
                    {
                        barinfo.NearSubway = new string[] { "отсутствует" };

                    }


                    //HtmlNode tttt = doc_2.DocumentNode.SelectNodes("//div[@class = 'metro d-flex align-items-start pl-4 mt-2 flex-wrap']")[0];


                    foreach (var item_2 in Menu)
                        {
                            //barinfo = new BarInfo();
                            menuitems = new MenuItems();
                            HtmlNode subtitle_path_2 = item_2.ParentNode.ParentNode.ParentNode;        //сладкая вода - не обязательная subtitle_2
                            HtmlNode subtitle_path = subtitle_path_2.ParentNode.ParentNode;       //вода
                            HtmlNode title_path = subtitle_path.ParentNode.ParentNode; //бар
                                                                                       //Console.WriteLine(name_path.InnerText);
                                                                                       //Console.WriteLine(subtitle_path.InnerText);

                            menuitems.BarName = item.InnerText.Replace("\r", "").Replace("\n", "").Trim();
                        if (menuitems.BarName.Contains('ё')) menuitems.BarName.Replace("ё", "е");

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


                            Menuitems_list.Add(menuitems);

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
                        List<string> pos = Yandex.Yandex.GetPos(Apikey, address);
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine(pos[0].Split(" ")[1]);
                    Console.WriteLine("---------------------------------------------------");
                    barinfo.Lat = Convert.ToDouble(pos[0].Split(" ")[1].Replace(".", ","));       //широта
                        barinfo.Lng = Convert.ToDouble(pos[0].Split(" ")[0].Replace(".", ","));       //долгота 
                    Console.WriteLine("dddddddddddddddd - " + barinfo.Lat);

                    barinfo.BarName = item.InnerText.Trim();
                    if (barinfo.BarName.Contains('ё')) barinfo.BarName.Replace("ё", "е");

                    barinfo.HasMenu = true;
                        barinfo_list_list.Add(barinfo);
                    }
                    Console.WriteLine(CountOfPages);
                    CountOfPages += 1;
                    Thread.Sleep(2000);

                }
                //запись в бд
               // ClearBD("barinfo");
                //ClearBD("menuitems");
                //PastIntoBD(barinfo_list_list, Menuitems_list);

                Console.WriteLine("--------------------------------------------------------------------");
                //Thread.Sleep(50000); //86400000 - это сутки
            //}
        }
        private static void PastIntoBD(List<BarInfo> bar, List<MenuItems> menu)//List<MenuItems> menu,List<BarInfo>bar
        {

            //запись в BarInfo
            string sqlquery = "INSERT INTO barinfo (BarName, Latitude, Longitude, Phone, WorkTime, HasMenu, Pictures, Subway) VALUES (@BarName, @Latitude, @Longitude, @Phone, @WorkTime, @HasMenu, @Pictures, @Subway)";
            string connectionString = "Server=localhost;Port=5432;User ID=postgres;Password=3400430;Database=Menu;";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();

            Console.WriteLine("Соединение с БД открыто");
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(sqlquery, npgSqlConnection);

            npgSqlCommand.Parameters.Add("BarName", NpgsqlTypes.NpgsqlDbType.Char);
            npgSqlCommand.Parameters.Add("Latitude", NpgsqlTypes.NpgsqlDbType.Real);
            npgSqlCommand.Parameters.Add("Longitude", NpgsqlTypes.NpgsqlDbType.Real);
            npgSqlCommand.Parameters.Add("Phone", NpgsqlTypes.NpgsqlDbType.Varchar);
            npgSqlCommand.Parameters.Add("WorkTime", NpgsqlTypes.NpgsqlDbType.Varchar);
            npgSqlCommand.Parameters.Add("HasMenu", NpgsqlTypes.NpgsqlDbType.Boolean);
            npgSqlCommand.Parameters.Add("Pictures", NpgsqlTypes.NpgsqlDbType.Text);
            npgSqlCommand.Parameters.Add("Subway", NpgsqlTypes.NpgsqlDbType.Text);


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

                string subway = "";
                if (item.NearSubway != null)
                {
                    foreach (var item_2 in item.NearSubway)
                    {
                        subway += item_2 + "|";
                    }
                    npgSqlCommand.Parameters["Subway"].Value = subway;
                }
                else
                {
                    npgSqlCommand.Parameters["Subway"].Value = "отсутсвует";
                }

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
                if (item.Subtitle == null)
                    npgSqlCommand.Parameters["Subtitle"].Value = DBNull.Value;
                else
                    npgSqlCommand.Parameters["Subtitle"].Value = item.Subtitle;
                if (item.Subtitle_2 == null)
                    npgSqlCommand.Parameters["Subtitle_2"].Value = DBNull.Value;
                else
                    npgSqlCommand.Parameters["Subtitle_2"].Value = item.Subtitle_2;
                npgSqlCommand.Parameters["Dish"].Value = item.Dish;
                npgSqlCommand.Parameters["Price"].Value = item.Price;
                npgSqlCommand.Parameters["BarName"].Value = item.BarName;

                npgSqlCommand.ExecuteNonQuery();
            }
            npgSqlConnection.Close();


        }
        private static void ClearBD(string name_of_table)
        {


            string sqlquery = "TRUNCATE TABLE " + name_of_table;
            string connectionString = "Server=localhost;Port=5432;User ID=postgres;Password=3400430;Database=Menu;";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();

            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(sqlquery, npgSqlConnection);

            npgSqlCommand.ExecuteNonQuery();
            npgSqlConnection.Close();

        }
    }
}
