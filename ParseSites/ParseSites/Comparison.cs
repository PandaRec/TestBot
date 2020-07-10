using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ParseSites
{
    class Comparison
    {
        /// <summary>
        /// Сравнение названий
        /// </summary>
        /// <param name="title1"> первое название</param>
        /// <param name="title2">второе название</param>
        /// <param name="IgnoreSpace"> true - сравнение с учетом возможности опущения пробела между словами
        /// false - сравнение строго по словам</param>
        /// <returns></returns>
        public static bool Compare(string title1, string title2, bool IgnoreSpace=false)
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
        private static bool Compare(string title1, string title2)
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
        public static (string, string) DeleteAddresses(string title1, string title2)
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
                title1 = title1.Remove(title1.IndexOf(" на "));

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
        public static (string, string) DeleteExtraWords(string title1, string title2, bool flag = true)
        {
            //удлаяем словосоветание со словами: караоке, бар, ресторан

            string pattern = @"[^\s]*караоке(-)[\S]*|[^\s]*бар(-)[\S]*|[^\s]*ресторан(-)[\S]*|[^\s]*крафтовый(-)[\S]*|[^\s]*кафе(-)[\S]*|[^\s]*музыкальный(-)[\S]*";

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
            pattern = @"\sбар$|^бар\s";
            string pattern2 = @"\sбар\s";
            if (flag == false)
            {

                if (Regex.IsMatch(title1, pattern) && Regex.IsMatch(title2, pattern))
                {
                    Regex regex = new Regex(pattern);
                    title1 = regex.Replace(title1, "").Trim();
                    title2 = regex.Replace(title2, "").Trim();
                }
                else if (Regex.IsMatch(title1, pattern2) && Regex.IsMatch(title2, pattern2))
                {
                    Regex regex = new Regex(pattern2);
                    title1 = regex.Replace(title1, "").Trim();
                    title2 = regex.Replace(title2, "").Trim();
                }
            }
            else if (flag == true)
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

            return (title1, title2);
        }

        /// <summary>
        /// Remove symbols such as: "'","«","»","."
        /// </summary>
        /// <param name="title1">first title</param>
        /// <param name="title2">second title</param>
        /// <returns></returns>
        public static (string, string) DeleteExtraSymbols(string title1, string title2)
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

        
    }
}
