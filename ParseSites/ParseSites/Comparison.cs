using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ParseSites
{
    class Comparison
    {

        public enum CompareBy
        {
            Translation,
            Transliteration

        }
        /// <summary>
        /// Сравнение названий
        /// </summary>
        /// <param name="title1"> первое название</param>
        /// <param name="title2">второе название</param>
        /// <param name="IgnoreSpace"> true - сравнение с учетом возможности опущения пробела между словами
        /// false - сравнение строго по словам</param>
        /// <returns></returns>
        private static bool BaseCompare(string title1, string title2, bool IgnoreSpace=false)
        {
            if (IgnoreSpace == false)
                return Compare(title1, title2);

            //разбиваем предложение на слова
            string[] WorsOfTitle1 = title1.Split(" ");
            string[] WorsOfTitle2 = title2.Split(" ");

            double part = 0; //чему равна доля совпадения
            double res = 0;   //результат

            if (WorsOfTitle1.Length > WorsOfTitle2.Length && WorsOfTitle1.Length > 1 && WorsOfTitle2.Length > 1)
            {
                part = 100.0 / WorsOfTitle1.Length;

                for (int i = 0; i < WorsOfTitle2.Length; i++)
                {
                    for (int j = 0; j < WorsOfTitle1.Length; j++)
                    {

                        if (WorsOfTitle2.Length > i + 1 && WorsOfTitle1.Length > j + 1)
                        {
                            if ((WorsOfTitle2[i] + WorsOfTitle2[i + 1]).Equals(WorsOfTitle1[j] + WorsOfTitle1[j + 1]))
                                res += part;
                        }

                    }
                }
            }
            else if (WorsOfTitle2.Length > WorsOfTitle1.Length && WorsOfTitle1.Length > 1 && WorsOfTitle2.Length > 1)
            {
                part = 100.0 / WorsOfTitle2.Length;

                for (int i = 0; i < WorsOfTitle1.Length; i++)
                {
                    for (int j = 0; j < WorsOfTitle2.Length; j++)
                    {
                        if (WorsOfTitle1.Length > i + 1 && WorsOfTitle2.Length > j + 1)
                        {
                            if ((WorsOfTitle1[i] + WorsOfTitle1[i + 1]).Equals(WorsOfTitle2[j] + WorsOfTitle2[j + 1]))
                                res += part;
                        }
                    }
                }
            }
            else if (WorsOfTitle1.Length > 1 && WorsOfTitle2.Length > 1)
            {
                part = 100.0 / WorsOfTitle1.Length;

                for (int i = 0; i < WorsOfTitle2.Length; i++)
                {
                    for (int j = 0; j < WorsOfTitle1.Length; j++)
                    {
                        if (WorsOfTitle1.Length > i + 1 && WorsOfTitle2.Length > j + 1)
                        {
                            if ((WorsOfTitle1[i] + WorsOfTitle1[i + 1]).Equals(WorsOfTitle2[j] + WorsOfTitle2[j + 1]))
                                res += part;
                        }
                    }
                }
                if (res == 100)
                    return true;
                else
                    return false;
            }
            else if (WorsOfTitle1.Length < 2 || WorsOfTitle2.Length < 2)
            {
                part = 100;
                if (WorsOfTitle1.Length > WorsOfTitle2.Length)
                {
                    for (int i = 0; i < WorsOfTitle2.Length; i++)
                    {
                        for (int j = 0; j < WorsOfTitle1.Length; j++)
                        {
                            if (WorsOfTitle1.Length > j + 1)
                            {
                                if (WorsOfTitle2[i].Equals(WorsOfTitle1[j] + WorsOfTitle1[j + 1]))
                                    res += part;
                            }
                        }
                    }
                }
                else if (WorsOfTitle2.Length > WorsOfTitle1.Length)
                {
                    for (int i = 0; i < WorsOfTitle1.Length; i++)
                    {
                        for (int j = 0; j < WorsOfTitle2.Length; j++)
                        {
                            if (WorsOfTitle2.Length > j + 1)
                            {
                                if (WorsOfTitle1[i].Equals(WorsOfTitle2[j] + WorsOfTitle2[j + 1]))
                                    res += part;
                            }
                        }
                    }
                }
            }
            if (res > 50)
                return true;
            else
                return false;

        }

        /// <summary>
        /// сравнение названий
        /// </summary>
        /// <param name="title1">первое название</param>
        /// <param name="title2">второе название</param>
        /// <returns></returns>
        private static bool BaseCompare(string title1, string title2)
        {
            //разбиваем предложение на слова
            string[] WorsOfTitle1 = title1.Split(" ");
            string[] WorsOfTitle2 = title2.Split(" ");

            double part = 0; //чему равна доля совпадения
            double res = 0;   //результат
            //Console.WriteLine(title1 + "    "+ title2);


            if (WorsOfTitle1.Length > WorsOfTitle2.Length)
                part = 100.0 / WorsOfTitle1.Length;
            else if (WorsOfTitle2.Length > WorsOfTitle1.Length)
                part = 100.0 / WorsOfTitle2.Length;
            else
                part = 100.0 / WorsOfTitle1.Length;


            if (WorsOfTitle2.Length != WorsOfTitle1.Length)
            {

                foreach (var item1 in WorsOfTitle1)
                {
                    foreach (var item2 in WorsOfTitle2)
                    {
                        if (item1.Equals(item2))
                            res += part;
                    }
                }
            }
            else
            {
                foreach (var item1 in WorsOfTitle1)
                {
                    foreach (var item2 in WorsOfTitle2)
                    {
                        if (item1.Equals(item2))
                            res += part;
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

        /// <summary>
        /// Remove adresses from titles
        /// </summary>
        /// <param name="title1">first title</param>
        /// <param name="title2">second title</param>
        /// <returns></returns>
        private static (string, string) DeleteAddresses(string title1, string title2)
        {
            string add1 = null;
            string add2 = null;

            if (title1.Contains(" на "))
                add1 = title1.Split(" на ")[1];
            if (title2.Contains(" на "))
                add2 = title2.Split(" на ")[1];

            if (add1 == null && add2 == null)
            {
                // обычная проверка
            }
            else if ((add1 == null && add2 != null) || (add1 != null && add2 == null))
            {
                // у одного есть адрес => удаляем его и сравниваем названия
                if (add1 != null)
                {
                    title1 = title1.Remove(title1.IndexOf(" на "));
                }
                else if (add2 != null)
                {
                    title2 = title2.Remove(title2.IndexOf(" на "));

                }
            }
            else if (!add1.Equals(add2))
            {
                // адреса разные => разные бары
                return (string.Empty, string.Empty);
            }
            else if (add1.Equals(add2))
            {
                //адрес один => проверка без адреса
                title1 = title1.Remove(title1.IndexOf(" на "));
                title2 = title2.Remove(title2.IndexOf(" на "));

            }
            return (title1, title2);

        }

        /// <summary>
        /// Remove words such as: "караоке-"+smth, "бар-"+smth etc
        /// </summary>
        /// <param name="title1">first title</param>
        /// <param name="title2">second title</param>
        /// <param name="flag">true - если удаляется независимое слово "бар" в любом случае,
        /// false - если удаляется только в случае вхождения и в title1 и в title2</param>
        /// <returns></returns>
        private static (string, string) DeleteExtraWords(string title1, string title2, bool flag = true)
        {
            //удлаяем словосоветание со словами: караоке, бар, ресторан

            //string pattern = @"[^\s]*караоке(-)[\S]*|[^\s]*бар(-)[\S]*|[^\s]*ресторан(-)[\S]*|[^\s]*крафтовый(-)[\S]*|[^\s]*кафе(-)[\S]*|[^\s]*музыкальный(-)[\S]*";
            string pattern = @"[^\s]*караоке(-)[\S]*|[^\s]*бар(-)[\S]*|[^\s]*ресторан(-)[\S]*|[^\s]*крафтовый(-)[\S]*|[^\s]*кафе(-)[\S]*|[^\s]*музыкальный(-)[\S]*
[^\s]*крафт(-)[\S]*|[^\s]*гриль(-)[\S]*|[^\s]*лаунж(-)[\S]*|[^\s]*винотека(-)[\S]*|[^\s]*коктейльный(-)[\S]*";


            if (Regex.IsMatch(title1, pattern))
            {
                Regex regex = new Regex(pattern);
                title1 = regex.Replace(title1, "").Trim();
            }
            if (Regex.IsMatch(title2, pattern))
            {
                Regex regex = new Regex(pattern);
                title2 = regex.Replace(title2, "").Trim();
            }

            //удаляем слово не значащее слово "бар"
            //pattern = @"\sбар$|^бар\s";
            pattern = @"\sбар$|^бар\s|\sкафе$|^кафе\s\sкараоке$|^караоке\s|\sресторан$|^ресторан\s|\sкрафтовый$|^крафтовый\s|\sмузыкальный$|^музыкальный\s|\sкрафт$|^крафт\s|\sгриль$|^гриль\s|\sлаунж$|^лаунж\s|
\sвинотека$|^винотека\s|\sкоктейльный$|^коктейльный\s|\sпространство$|^пространство\s|\sпаб$|^паб\s|\sbar$|^bar\s|\scafe$|^cafe\s|\spub$|^pub\s";

            //string pattern2 = @"\sбар\s";
            string pattern2 = @"\sбар\s|\sкафе\s|\sкараоке\s|\ssресторан\s|\sкрафтовый\s|\sмузыкальный\s|\sкрафт\s|\sгриль\s|\sлаунж\s|\sвинотека\s|\sкоктейльный\s|\sпространство\s|\sпаб\s
|\sbar\s|\scafe\s|\spub\s";

            if (flag == false)
            {
                while (Regex.IsMatch(title1, pattern) != false && Regex.IsMatch(title2, pattern) != false) {

                    if (Regex.IsMatch(title1, pattern) && Regex.IsMatch(title2, pattern))
                    {
                        Regex regex = new Regex(pattern);
                        title1 = regex.Replace(title1, "").Trim();
                        title2 = regex.Replace(title2, "").Trim();
                    }
                }
                while (Regex.IsMatch(title1, pattern2) != false && Regex.IsMatch(title2, pattern2) != false)
                {
                    if (Regex.IsMatch(title1, pattern2) && Regex.IsMatch(title2, pattern2))
                    {
                        Regex regex = new Regex(pattern2);
                        title1 = regex.Replace(title1, "").Trim();
                        title2 = regex.Replace(title2, "").Trim();
                    }
                }
            }
            else if (flag == true)
            {
                while (Regex.IsMatch(title1, pattern))
                {
                    if (Regex.IsMatch(title1, pattern))
                    {
                        Regex regex = new Regex(pattern);
                        title1 = regex.Replace(title1, "").Trim();
                    }

                    else if (Regex.IsMatch(title1, pattern2))
                    {
                        Regex regex = new Regex(pattern2);
                        title1 = regex.Replace(title1, " ");

                    }
                }
                while (Regex.IsMatch(title2, pattern) != false)
                {

                    if (Regex.IsMatch(title2, pattern))
                    {
                        Regex regex = new Regex(pattern);
                        title2 = regex.Replace(title2, "");
                    }
                    else if (Regex.IsMatch(title2, pattern2))
                    {
                        Regex regex = new Regex(pattern2);
                        title2 = regex.Replace(title2, " ");

                    }
                }
            }

            return (title1, title2);
        }

        /// <summary>
        /// Remove symbols such as: "'","«","»","."
        /// </summary>
        /// <param name="title1">first title</param>
        /// <param name="title2">second title</param>
        /// <returns></returns>
        private static (string, string) DeleteExtraSymbols(string title1, string title2)
        {


            // убираем апострофы
            if (title1.Contains('\''))
                title1 = title1.Replace(@"'", "");

            if (title2.Contains('\''))
                title2 = title2.Replace(@"'", "");

            //удаляем ковычки
            if (title1.Contains("«"))
                title1 = title1.Replace("«", "");
            if (title1.Contains("»"))
                title1 = title1.Replace("»", "");

            if (title2.Contains("«"))
                title2 = title2.Replace("«", "");
            if (title2.Contains("»"))
                title2 = title2.Replace("»", "");

            //удаояем точки
            if (title1.Contains("."))
                title1 = title1.Replace(".", "");
            if (title2.Contains("."))
                title2 = title2.Replace(".", "");

            return (title1, title2);
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

        public static bool Compare(string title1, string title2,CompareBy compareBy=CompareBy.Transliteration)
        {
            if (compareBy == CompareBy.Transliteration)
                return CompareWithTransliteration(title1, title2);
            else
                return CompareWithTranslation(title1, title2);

        }

        private static bool CompareWithTransliteration(string title1, string title2)
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

            (string, string) middlewords = DeleteExtraSymbols(new string(title1_1), new string(title2_2));
            middlewords = DeleteAddresses(middlewords.Item1, middlewords.Item2);
            if (middlewords.Item1 == "" && middlewords.Item2 == "") // если адреса разные
                return false;
            (string, string) middlewordsExtra = DeleteExtraWords(middlewords.Item1, middlewords.Item2); //удаляется любое вхождение
            //middlewords = DeleteExtraWords(middlewords.Item1, middlewords.Item2);

            if (BaseCompare(middlewordsExtra.Item1, middlewordsExtra.Item2) == false) //строго по словам
            {
                middlewordsExtra = DeleteExtraWords(middlewords.Item1, middlewords.Item2, false); //удаляется только при вхождение в оба

                if (BaseCompare(middlewordsExtra.Item1, middlewordsExtra.Item2) == false) //строго по словам
                {
                    middlewordsExtra = DeleteExtraWords(middlewords.Item1, middlewords.Item2); //удаляется любое вхождение

                    if (BaseCompare(middlewordsExtra.Item1, middlewordsExtra.Item2, true) == false) //с вариацией
                    {
                        middlewordsExtra = DeleteExtraWords(middlewords.Item1, middlewords.Item2, false); //удаляется только при вхождение в оба

                        if (BaseCompare(middlewordsExtra.Item1, middlewordsExtra.Item2, true) == false) // с вариацией
                        {
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

        private static bool CompareWithTranslation(string title1, string title2)
        {
            List<string> TranslatedWord = new List<string>();
            //List<List<string>> TranslatedSentance1 = new List<List<string>>();
            //List<List<string>> TranslatedSentance2 = new List<List<string>>();
            Dictionary<string, List<string>> TranslatedSentance1 = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> TranslatedSentance2 = new Dictionary<string, List<string>>();
            Dictionary<string, int> dictionary = new Dictionary<string, int>();



            char[] title1_1 = title1.ToLower().ToCharArray();
            char[] title2_2 = title2.ToLower().ToCharArray();

            for (int i = 0; i < title1_1.Length; i++)
                if (title1_1[i] == 160) title1_1[i] = ' ';

            for (int i = 0; i < title2_2.Length; i++)
                if (title2_2[i] == 160) title2_2[i] = ' ';

            (string, string) middlewords = DeleteExtraSymbols(new string(title1_1), new string(title2_2));
            middlewords = DeleteAddresses(middlewords.Item1, middlewords.Item2);
            if (middlewords.Item1 == "" && middlewords.Item2 == "") // если адреса разные
                return false;

            string[] WorsOfTitle1 = middlewords.Item1.Split(" ");
            string[] WorsOfTitle2 = middlewords.Item2.Split(" ");
            foreach (var item in WorsOfTitle1)
            {
                TranslatedWord = new List<string>();
                TranslatedWord = Yandex.Yandex.Translate(item,Yandex.Yandex.TargetLanguage.en);

                if (TranslatedWord.Count > 0)
                {
                    TranslatedSentance1.Add(item, TranslatedWord);
                    dictionary.Add(item, 0);
                    //TranslatedWord.Clear();
                }

            }

            (string, string) middlewordsExtra = DeleteExtraWords(middlewords.Item1, middlewords.Item2); //удаляется любое вхождение
            //middlewords = DeleteExtraWords(middlewords.Item1, middlewords.Item2);
            //здесь перевод слов

            //разбиваем предложение на слова
            /*
            string[] WorsOfTitle1 = middlewordsExtra.Item1.Split(" ");
            string[] WorsOfTitle2 = middlewordsExtra.Item2.Split(" ");

            foreach (var item in WorsOfTitle1)
            {
                TranslatedWord = Yandex.Yandex.Translate(item);

                if (TranslatedWord.Count > 0)
                {
                    TranslatedSentance1.Add(item,TranslatedWord);
                    TranslatedWord.Clear();
                }
                
            }
            */
            /*
            foreach (var item in WorsOfTitle2)
            {
                TranslatedWord = Yandex.Yandex.Translate(item);

                if (TranslatedWord.Count > 0)
                {
                    TranslatedSentance2.Add(item,TranslatedWord);
                    TranslatedWord.Clear();
                }

            }
            */
            string Sentance1=string.Empty;
            string Sentance2= middlewordsExtra.Item2;
            int counter=1;
            foreach (var item in TranslatedSentance1)
            {
                counter =counter * item.Value.Count;
            }


            for (int k = 0; k < counter; k++)
            { 

                foreach (var item in TranslatedSentance1)
                {


                    for (int i = 0; i < WorsOfTitle1.Length; i++)
                    {
                        //string KeyToIncrement = string.Empty;
                        if (WorsOfTitle1[i].Equals(item.Key))
                        {
                            WorsOfTitle1[i] = item.Value[dictionary[item.Key]];

                            if (dictionary[item.Key] < item.Value.Count)
                                dictionary[item.Key] += 1;
                            else
                                dictionary[item.Key] = 0;
                            break;
                        }
                    }
                }
                foreach (var item in WorsOfTitle1)
                {
                    Sentance1 += item + " ";
                }
                Sentance1 = Sentance1.Trim();
            }
                
        


            //найти совпадения , вставить переведенные слова скмпоновть предлажение и начать сравнение
            

            if (BaseCompare(middlewordsExtra.Item1, middlewordsExtra.Item2) == false) //строго по словам
            {
                middlewordsExtra = DeleteExtraWords(middlewords.Item1, middlewords.Item2, false); //удаляется только при вхождение в оба

                if (BaseCompare(middlewordsExtra.Item1, middlewordsExtra.Item2) == false) //строго по словам
                {
                    middlewordsExtra = DeleteExtraWords(middlewords.Item1, middlewords.Item2); //удаляется любое вхождение

                    if (BaseCompare(middlewordsExtra.Item1, middlewordsExtra.Item2, true) == false) //с вариацией
                    {
                        middlewordsExtra = DeleteExtraWords(middlewords.Item1, middlewords.Item2, false); //удаляется только при вхождение в оба

                        if (BaseCompare(middlewordsExtra.Item1, middlewordsExtra.Item2, true) == false) // с вариацией
                        {
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



    }
}
