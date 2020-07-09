using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace ParseSites.Site2
{
    class Execute
    {
        private static string Apikey { get; } = "5c22c518-ebcf-4750-8524-872e96677344";
        /// <summary>
        /// Выполняет парсинг сайта KudaGo мск
        /// </summary>
        public static void Do(out List<BarInfo> barinfo_list_list)
        {
            barinfo_list_list = new List<BarInfo>();
            HttpClient client = new HttpClient();

            double PagesForKudaGo = 1;
            for (int j = 0; j < PagesForKudaGo; j++)
            {
                Console.WriteLine("J = " + j + "   Pages = " + PagesForKudaGo);
                int page = j + 1;
                string url = "https://kudago.com/public-api/v1.4/places/?lang=&page=" + page + "&fields=&expand=&order_by=&text_format=&ids=&location=msk&has_showings=&showing_since=&showing_until&categories=bar&lon=&lat=&radius=";
                PagesForKudaGo += 1;
                string stuff = client.GetStringAsync(url).GetAwaiter().GetResult();
                var rootobject = JsonConvert.DeserializeObject<MainJson.Rootobject>(stuff);
                PagesForKudaGo = Math.Ceiling((double)rootobject.count / 20);




                for (int i = 0; i < rootobject.results.Length; i++)
                {

                    if (rootobject.results[i].is_closed == true)
                        continue;

                    BarInfo barinfo = new BarInfo();
                   // Console.WriteLine(rootobject.results[i].title + rootobject.results[i].id + "      " + rootobject.results[i].is_closed + "    " + rootobject.results[i].subway);
                    barinfo.NearSubway = rootobject.results[i].subway.Split(", ");
                    barinfo.BarName = rootobject.results[i].title;
                    if (barinfo.BarName.Contains('ё')) barinfo.BarName.Replace("ё", "е");
                    barinfo.HasMenu = false;
                    barinfo.Phone = rootobject.results[i].phone;
                    url = "https://kudago.com/public-api/v1.4/places/" + rootobject.results[i].id + "/?expand=images";
                    stuff = client.GetStringAsync(url).GetAwaiter().GetResult();
                    var rootobject1 = JsonConvert.DeserializeObject<Details.Rootobject>(stuff);
                    if (rootobject1.address.Contains("«Трёхгорная мануфактура»"))
                        rootobject1.address = rootobject1.address.Replace("«Трёхгорная мануфактура»,", "");
                    else if (rootobject1.address.Contains("башня «Империя»"))
                        rootobject1.address = rootobject1.address.Replace("башня «Империя»","");                    
                    Console.WriteLine(rootobject1.address);
                    List<string> pos = Yandex.Yandex.GetPos(Apikey, rootobject1.address);

                    barinfo.Lat = Convert.ToDouble(pos[0].Split(" ")[1].Replace(".", ","));       //широта
                    barinfo.Lng = Convert.ToDouble(pos[0].Split(" ")[0].Replace(".", ","));       //долгота 

                    //barinfo.Lat = rootobject1.coords.lat;
                    //barinfo.Lng = rootobject1.coords.lon;
                    if (rootobject1.timetable == "")
                        barinfo.WorkTime = "время работы неизвестно";
                    else
                        barinfo.WorkTime = rootobject1.timetable;


                    for (int k = 0; k < rootobject1.images.Length; k++)
                    {
                        barinfo.PictureLinks.Add(rootobject1.images[k].image);
                    }
                    //Barinfo_list.Add(barinfo);
                    barinfo_list_list.Add(barinfo);
                }
                Console.WriteLine();
                Thread.Sleep(2000);
            }
            //PastIntoBD(barinfo_list_list);
        }

        private static void PastIntoBD(List<BarInfo> bar)
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
                foreach (var item_2 in item.NearSubway )
                {
                    subway += item_2 + "|";
                }
                npgSqlCommand.Parameters["Subway"].Value = subway;


                npgSqlCommand.ExecuteNonQuery();
            }
            npgSqlConnection.Close();
            Console.WriteLine("Соединение с БД закрыто");



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
