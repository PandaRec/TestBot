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
        private const string APIKeyGeocoder = "";
        private const string APIKeyTranslate = "dict.1.1.20200709T203350Z.2d54362daf5a4286.b00f04cc915ca73d435e1c7402562e6e18c7ecc9";

        /// <summary>
        /// In which language you want translate your text
        /// </summary>
        public enum TargetLanguage
        {
            en,
            ru
        }
        

        public static List<string> GetPos(string APIKey,string Address)
        {
            //YandexGeocoder geocoder = new YandexGeocoder();
            //geocoder.Apikey = "5c22c518-ebcf-4750-8524-872e96677344";
            HttpClient client = new HttpClient();
            List<string> pos = new List<string>();
            
            string url = "https://geocode-maps.yandex.ru/1.x/?format=json&apikey="+APIKey+ "&geocode=" + Address;

            string stuff = client.GetStringAsync(url).GetAwaiter().GetResult();
            var rootobject = JsonConvert.DeserializeObject<Geocoder.Rootobject>(stuff);
            //Console.WriteLine(rootobject.response.GeoObjectCollection.featureMember[0].GeoObject.Point.pos);

            //37.587614 55.753083
            
            for(int i=0;i< rootobject.response.GeoObjectCollection.featureMember.Length; i++)
            {
                pos.Add(rootobject.response.GeoObjectCollection.featureMember[i].GeoObject.Point.pos);
            }
            
            return pos;
        }
        public static List<string> Translate(string text,TargetLanguage language=TargetLanguage.ru, string APIKey = APIKeyTranslate)
        {
            
            
            List<string> res = new List<string>();
            HttpClient client = new HttpClient();
            string url = String.Empty;
            if (language== TargetLanguage.ru)
                url = "https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key=" + APIKey + "&lang=en-ru&text=" + text;
            else if (language == TargetLanguage.en)
                url = "https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key=" + APIKey + "&lang=ru-en&text=" + text;
            else
                return res;


            string stuff = client.GetStringAsync(url).GetAwaiter().GetResult();
            var rootobject = JsonConvert.DeserializeObject<Translate.Rootobject>(stuff);

            for (int i = 0; i < rootobject.def.Length; i++)
            {
                for (int j = 0; j < rootobject.def[i].tr.Length; j++)
                {
                    res.Add(rootobject.def[i].tr[j].text.ToLower());
                    Console.WriteLine(rootobject.def[i].tr[j].text.ToLower());
                    if (rootobject.def[i].tr[j].syn != null)
                    {
                        for (int k = 0; k < rootobject.def[i].tr[j].syn.Length; k++)
                        {
                            res.Add(rootobject.def[i].tr[j].syn[k].text.ToLower());
                            Console.WriteLine(rootobject.def[i].tr[j].syn[k].text.ToLower());
                        }
                    }
                }
                
            }
            return res;
        }
    }
}
