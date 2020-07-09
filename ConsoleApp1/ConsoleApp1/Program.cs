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
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class A
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding(1251);
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            System.Console.InputEncoding = enc1251;

            List<BarInfo> barinfo_list = new List<BarInfo>();
            string CountOfPages = "";
            int cc = 0;

            // тут начинается kuda go


            //HtmlDocument kudago = new HtmlDocument();
            //kudago.LoadHtml(getRequest("https://kudago.com/public-api/v1.4/places/?lang=&page=10&fields=&expand=&order_by=&text_format=&ids=&location=msk&has_showings=&showing_since=&showing_until&categories=bar&lon=&lat=&radius="));

            HttpClient client = new HttpClient();

            double PagesForKudaGo = 1;
            for (int j = 0; j < PagesForKudaGo; j++)
            {
                Console.WriteLine("J = "+j+"   Pages = "+PagesForKudaGo);
                int page = j + 1;
                string url = "https://kudago.com/public-api/v1.4/places/?lang=&page="+page+"&fields=&expand=&order_by=&text_format=&ids=&location=msk&has_showings=&showing_since=&showing_until&categories=bar&lon=&lat=&radius=";
                PagesForKudaGo += 1;
                string stuff = client.GetStringAsync(url).GetAwaiter().GetResult();
                var rootobject = JsonConvert.DeserializeObject<Rootobject>(stuff);
                PagesForKudaGo = Math.Ceiling((double)rootobject.count / 20);




                for (int i = 0; i < rootobject.results.Length; i++)
                {
                    
                    cc += 1;
                    if (rootobject.results[i].is_closed == true)
                        continue;

                    BarInfo barinfo = new BarInfo();
                    Console.WriteLine(rootobject.results[i].title + rootobject.results[i].id + "      " + rootobject.results[i].is_closed + "    " + rootobject.results[i].subway);
                    barinfo.Near = rootobject.results[i].subway.Split(", ");
                    barinfo.BarName = rootobject.results[i].title;
                    barinfo.HasMenu = false;
                    barinfo.Phone = rootobject.results[i].phone;
                    url = "https://kudago.com/public-api/v1.4/places/" + rootobject.results[i].id + "/?expand=images";
                    stuff = client.GetStringAsync(url).GetAwaiter().GetResult();
                    var rootobject1 = JsonConvert.DeserializeObject<Rootobject1>(stuff);

                    barinfo.Lat = rootobject1.coords.lat;
                    barinfo.Lng = rootobject1.coords.lon;
                    barinfo.WorkTime = rootobject1.timetable;


                    for (int k = 0; k < rootobject1.images.Length; k++)
                    {
                        barinfo.PictureLinks.Add(rootobject1.images[k].image);
                    }
                    barinfo_list.Add(barinfo);
                }
                Console.WriteLine();
                Thread.Sleep(2000);
            }
            foreach (var item in barinfo_list)
            {
                Console.WriteLine("barname - "+item.BarName);
                Console.WriteLine("hasmenu - "+item.HasMenu);
                Console.WriteLine("Lat - "+item.Lat);
                Console.WriteLine("lng - "+ item.Lng);
                Console.WriteLine("phone - "+item.Phone);
                Console.WriteLine("worktime - "+item.WorkTime);
                Console.WriteLine("pic links:");
                foreach (var item2 in item.PictureLinks)
                {
                    Console.WriteLine(item2);
                }
                Console.WriteLine();
                Console.WriteLine("near subway:");
                foreach (var item2 in item.Near)
                {
                    Console.WriteLine(item2);
                }

            }
            Console.WriteLine(cc);
            Console.WriteLine();







            // тут начинает трип эдвайзер
            HtmlDocument doc = new HtmlDocument();


    doc.LoadHtml(getRequest("https://www.tripadvisor.ru/Attraction_Review-g298484-d9858900-Reviews-Kot_Shrodingera-Moscow_Central_Russia.html#photos;aggregationId=&albumid=101&filter=7"));
            HtmlNodeCollection Barss = doc.DocumentNode.SelectNodes("//div[@class = 'photoGridWrapper']");
    string lol = doc.ParsedText;
    Console.WriteLine();





            doc.LoadHtml(getRequest(@"https://www.tripadvisor.ru/Attractions-g298484-Activities-c20-t99-Moscow_Central_Russia.html"));
            HtmlNodeCollection BarCollection = doc.DocumentNode.SelectNodes("//div[@class = '_6sUF3jUd']/a");
            //barcollections может быть null завернуть в try catch с переподключением
            for (int i = 0; i<BarCollection.Count; i++)
            {
                var item = BarCollection[i];
                if (item.Attributes["href"].Value.IndexOf("#REVIEWS") != -1) BarCollection.Remove(item);

            }
Console.WriteLine(BarCollection.Count);
            foreach (var item in BarCollection)
            {
                Console.WriteLine(item.Attributes["href"].Value);
            }

            foreach (var item in BarCollection)
            {
                HtmlDocument doc_2 = new HtmlDocument();
doc_2.LoadHtml(getRequest("https://www.tripadvisor.ru" + item.Attributes["href"].Value));
                HtmlNode Bar = doc_2.DocumentNode.SelectNodes("//div[@class = 'attractions-contact-card-ContactCard__contactRow--3Ih6v']")[0];
BarInfo barInfo = new BarInfo();
string address = Bar.InnerText;
                //HtmlNodeCollection Barss = doc_2.DocumentNode.SelectNodes("//div[@class = 'attractions-contact-card-ContactCard__linkWrapper--m0ETF']");
                foreach (var item_2 in Barss)
                {
                    if (item_2.InnerText.IndexOf("Веб") == -1)
                    {
                        barInfo.Phone = item_2.InnerText;
                        break;
                    }

                }
                barInfo.BarName= doc_2.DocumentNode.SelectNodes("//h1")[1].InnerText;
                barInfo.HasMenu = false;
                barInfo.WorkTime = "отсутсвует";

                doc_2.LoadHtml(getRequest("https://www.tripadvisor.ru" + item.Attributes["href"].Value));
                Barss = doc_2.DocumentNode.SelectNodes("//div[@class = 'class=attractions-attraction-review-atf-AttractionReviewATFLayout__atf_component--2Qflo attractions-attraction-review- -AttractionReviewATFLayout__media_carousel_container--2HeRK']");

                /*
                List<string> poss = Yandex.Yandex.GetPos(apikey, address);
                info.Lat = Convert.ToDouble(poss[0].Split(" ")[1].Replace(".", ","));       //широта
                info.Lng = Convert.ToDouble(poss[0].Split(" ")[0].Replace(".", ","));       //долгота 
                */
            }
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