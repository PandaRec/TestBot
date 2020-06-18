using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Yandex.Geocoder;

namespace ParseSites.Yandex
{
    class Yandex
    {

        public static List<string> GetPos(string APIKey,string Address)
        {
            //YandexGeocoder geocoder = new YandexGeocoder();
            //geocoder.Apikey = "5c22c518-ebcf-4750-8524-872e96677344";
            HttpClient client = new HttpClient();
            List<string> pos = new List<string>();

            string url = "https://geocode-maps.yandex.ru/1.x/?format=json&apikey="+APIKey+ "&geocode=" + Address;

            string stuff = client.GetStringAsync(url).GetAwaiter().GetResult();
            var rootobject = JsonConvert.DeserializeObject<Rootobject>(stuff);
            //Console.WriteLine(rootobject.response.GeoObjectCollection.featureMember[0].GeoObject.Point.pos);

            //37.587614 55.753083
            
            for(int i=0;i< rootobject.response.GeoObjectCollection.featureMember.Length; i++)
            {
                pos.Add(rootobject.response.GeoObjectCollection.featureMember[i].GeoObject.Point.pos);
            }
            
            return pos;
        }
    }
}
