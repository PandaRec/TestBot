using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using System;
using Telegram.Bot.Args;
using System.Collections.Generic;

namespace TestBot.Models.Menu
{
    class NearBars
    {
        private static TelegramBotClient Bot = Program.bot;
        public static string menu { get; } = "Вы в ближлижайших барах";
        public static string Greeting { get; } = "Выберите на сколько вам не лень идти";

        public static string AskLocation { get; } = "Разрешите нам глянуть где вы сейчас";

        public static string BarName { get; private set; }

        private static float LatitudePos;
        private static float LongitudePos;

        private static List<ModelOfBar> NearBarsList = new List<ModelOfBar>();

        private static List<ModelOfMenuItems> MenuOfNearBar = new List<ModelOfMenuItems>();

        private static Dictionary<long, int> counter = new Dictionary<long, int>();

        private static Telegram.Bot.Types.Message answ = null;

        public static List<string> CallBackData { get; private set; } = new List<string>();

        //public static Dictionary<long, bool> Like { get; private set; } = new Dictionary<long, bool>();

        //public static Dictionary<long, bool> Dislike { get; private set; } = new Dictionary<long, bool>();
        //public static int LikeValue { get; private set; }
        //public static int DislikeValue { get; private set; }
        public static ReplyKeyboardMarkup ReplyKeyboardGeo { get; } = new ReplyKeyboardMarkup(new[] {
            new[]{
            new KeyboardButton("Показать где я сейчас") { RequestLocation=true}
            },
            new[]
            {
                new KeyboardButton("Назад")
            }

        }, true);
        public static ReplyKeyboardMarkup ReplyKeyboard { get; } = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("100m"),
                            new KeyboardButton("500m"),
                            new KeyboardButton("1km"),
                            new KeyboardButton("2km")
                         },
                        new[]
                        {
                            new KeyboardButton("3km"),
                            new KeyboardButton("5km"),
                            new KeyboardButton("8km"),
                            new KeyboardButton("Назад")
                        }

        }, true);

        public static bool Contains(Telegram.Bot.Types.Message message)
        {
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location) return true;
            if (message.Text.Equals("\U0001F43E") || message.Text.Equals("Показать где я сейчас")
            || message.Text.Equals("100m") || message.Text.Equals("500m") || message.Text.Equals("1km")
            || message.Text.Equals("2km") || message.Text.Equals("3km") || message.Text.Equals("5km")
            || message.Text.Equals("8km")) return true;
            else return false;
        }
        public static bool ContainsData(Telegram.Bot.Types.CallbackQuery callback)
        {
            if (callback.Data.Equals("Menu") || callback.Data.Equals("Next") || callback.Data.Equals("Photos") 
                || callback.Data.Equals("Back") || callback.Data.Equals("Route") || callback.Data.Equals("like") 
                || callback.Data.Equals("dislike") || callback.Data.Equals("like") || callback.Data.Equals("dilike"))
                return true;
            else return false;
        }
        /// <summary>
        /// Устанвка inline клавиатуры с лайками и дизлайками
        /// </summary>
        /// <param name="barname"></param>
        /// <returns></returns>
        public static InlineKeyboardMarkup SetKeyboard(string barname)
        {
            int temp_likes=0;
            int temp_dislikes=0;
            foreach (var item in Program.UserRate)
            {
                if (item.BarName == barname)
                {
                    temp_likes = item.Likes.Count;
                    temp_dislikes = item.Dislikes.Count;
                    break;
                }
            }
            InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("\U00002764"+temp_likes,"like"),
                InlineKeyboardButton.WithCallbackData("\U0001F494"+temp_dislikes,"dislike")

            }
        });

            return Temp;
        }

            public static InlineKeyboardMarkup SetKeyboard(long ChatId, bool title,bool subtitle,bool subtitle_2,string nameoftitle)
        {
            List<List<InlineKeyboardButton>> rowList = new List<List<InlineKeyboardButton>>();

            if (title == true && subtitle == false&& subtitle_2 == false)
            {
                // вывод кнопок subtitle
                List<string> TempNames = new List<string>();
                foreach (var item in MenuOfNearBar)
                {
                    if (!TempNames.Contains(item.Subtitle) && item.Title==nameoftitle)
                    {
                        InlineKeyboardButton inlineKeyboardButton = new InlineKeyboardButton();
                        inlineKeyboardButton.Text = item.Subtitle;
                        inlineKeyboardButton.CallbackData = item.Subtitle;
                        List<InlineKeyboardButton> keyboardButtonsRow1 = new List<InlineKeyboardButton>();
                        keyboardButtonsRow1.Add(inlineKeyboardButton);
                        rowList.Add(keyboardButtonsRow1);
                        TempNames.Add(item.Subtitle);
                        CallBackData.Add(item.Subtitle);

                    }
                }
                return new InlineKeyboardMarkup(rowList);

            }
            else if (title == true && subtitle == true && subtitle_2 == false)
            {
                // вывод кнопок subtitle & subtitle_2
                List<string> TempNames = new List<string>();
                foreach (var item in MenuOfNearBar)
                {
                    if (!TempNames.Contains(item.Subtitle) && item.Subtitle!=nameoftitle && item.Title==nameoftitle)
                    {
                        InlineKeyboardButton inlineKeyboardButton = new InlineKeyboardButton();
                        inlineKeyboardButton.Text = item.Subtitle;
                        inlineKeyboardButton.CallbackData = item.Subtitle;
                        List<InlineKeyboardButton> keyboardButtonsRow1 = new List<InlineKeyboardButton>();
                        keyboardButtonsRow1.Add(inlineKeyboardButton);
                        rowList.Add(keyboardButtonsRow1);
                        TempNames.Add(item.Subtitle);
                        CallBackData.Add(item.Subtitle);

                    }
                }
                foreach (var item in MenuOfNearBar)
                {
                    if (!TempNames.Contains(item.Subtitle_2) && item.Subtitle==nameoftitle)
                    {
                        InlineKeyboardButton inlineKeyboardButton = new InlineKeyboardButton();
                        inlineKeyboardButton.Text = item.Subtitle_2;
                        inlineKeyboardButton.CallbackData = item.Subtitle_2;
                        List<InlineKeyboardButton> keyboardButtonsRow1 = new List<InlineKeyboardButton>();
                        keyboardButtonsRow1.Add(inlineKeyboardButton);
                        rowList.Add(keyboardButtonsRow1);
                        TempNames.Add(item.Subtitle_2);
                        CallBackData.Add(item.Subtitle_2);

                    }
                }

                return new InlineKeyboardMarkup(rowList);
            }
            else if(title == false && subtitle==true && subtitle_2==false)
            {
                // вывод кнопок subtitle_2
                List<string> TempNames = new List<string>();
                foreach (var item in MenuOfNearBar)
                {
                    if (!TempNames.Contains(item.Subtitle_2) && item.Subtitle==nameoftitle)
                    {
                        InlineKeyboardButton inlineKeyboardButton = new InlineKeyboardButton();
                        inlineKeyboardButton.Text = item.Subtitle_2;
                        inlineKeyboardButton.CallbackData = item.Subtitle_2;
                        List<InlineKeyboardButton> keyboardButtonsRow1 = new List<InlineKeyboardButton>();
                        keyboardButtonsRow1.Add(inlineKeyboardButton);
                        rowList.Add(keyboardButtonsRow1);
                        TempNames.Add(item.Subtitle_2);
                        CallBackData.Add(item.Subtitle_2);
                        //Console.WriteLine(item.Subtitle_2);
                    }
                }
                return new InlineKeyboardMarkup(rowList);

            }
            else if(title == false && subtitle == false && subtitle_2 == false)
            {
                Console.WriteLine("не возможно создать клаву тк нету совпадений в меню");
                return null;
            }
            else
            {
                Console.WriteLine("нет удовлетворяющих условий для создания клавиатуры");
                return null;

            }
        }
        public static InlineKeyboardMarkup SetKeyboard(long ChatId)
        {
            CallBackData.Clear();
            MenuOfNearBar.Clear();
            List<string> titles = new List<string>();
            List<List<InlineKeyboardButton>> rowList = new List<List<InlineKeyboardButton>>();

            foreach (var item in Program.MenuItems)
            {
                if (item.BarName == NearBarsList[counter[ChatId]].BarName)
                {
                    if (item.Title == "" && item.Subtitle != "" && !titles.Contains(item.Subtitle))
                    {
                        titles.Add(item.Subtitle);
                        Console.WriteLine("sub - "+item.Subtitle);

                    }
                    if (item.Subtitle != "" && item.Title != "" && !titles.Contains(item.Title))
                    {
                        titles.Add(item.Title);
                        Console.WriteLine("tit - "+ item.Title);
                    }
                    MenuOfNearBar.Add(item);
                }
            }
            Console.WriteLine(titles.Count);
            foreach (var item in titles)
            {
                Console.WriteLine(item);
            }
            foreach (var item in titles)
            {
                InlineKeyboardButton inlineKeyboardButton = new InlineKeyboardButton();
                inlineKeyboardButton.Text = item;
                inlineKeyboardButton.CallbackData = item;
                List<InlineKeyboardButton> keyboardButtonsRow1 = new List<InlineKeyboardButton>();
                keyboardButtonsRow1.Add(inlineKeyboardButton);
                rowList.Add(keyboardButtonsRow1);
                CallBackData.Add(item);
            }
            return new InlineKeyboardMarkup(rowList);
        }
        public static InlineKeyboardMarkup SetKeyboard(bool hasmenu, bool hasphotos, bool first)
        {
            if (hasmenu == true && hasphotos == true && first == true)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Меню","Menu"),
                InlineKeyboardButton.WithCallbackData("Дальше","Next"),
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Фото","Photos"),
                InlineKeyboardButton.WithCallbackData("Маршрут","Route"),

            }
        });
                return Temp;

            }
            else if (hasmenu == false && hasphotos == true && first == true)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Фото","Photos"),
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Маршрут","Route"),
                InlineKeyboardButton.WithCallbackData("Дальше","Next")
            }
        });

                return Temp;

            }
            else if (hasmenu == true && hasphotos == false && first == true)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Меню","Menu"),
                InlineKeyboardButton.WithCallbackData("Дальше","Next")

            }

        });

                return Temp;
            }
            else if (hasmenu == false && hasphotos == false && first == true)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Маршрут","Route"),
                InlineKeyboardButton.WithCallbackData("Дальше","Next")
            }
        });

                return Temp;
            }

            else if (hasmenu == true && hasphotos == true && first == false)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Назад","Back"),
                InlineKeyboardButton.WithCallbackData("Меню","Menu"),
                InlineKeyboardButton.WithCallbackData("Дальше","Next"),
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Фото","Photos"),
                InlineKeyboardButton.WithCallbackData("Маршрут","Route"),
            }
        });
                return Temp;

            }
            else if (hasmenu == false && hasphotos == true && first == false)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Фото","Photos"),
                InlineKeyboardButton.WithCallbackData("Маршрут","Route"),
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Назад","Back"),
                InlineKeyboardButton.WithCallbackData("Дальше","Next")
            }
        });

                return Temp;

            }
            else if (hasmenu == true && hasphotos == false && first == false)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Назад","Back"),
                InlineKeyboardButton.WithCallbackData("Меню","Menu"),
                InlineKeyboardButton.WithCallbackData("Дальше","Next")

            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Маршрут","Route"),
            }

        });

                return Temp;
            }
            else if (hasmenu == false && hasphotos == false && first == false)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Назад","Back"),
                InlineKeyboardButton.WithCallbackData("Дальше","Next")
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Маршрут","Route"),
            }
        });

                return Temp;
            }
            else return null;


        }


        public static async void MessageRecived(object sender, MessageEventArgs e)
        {
            var message = e.Message;

            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location)
            {
                Console.WriteLine("ffff");
                if (message.Location == null)
                {
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Может ты забыл включить геолокацию?");
                    return;
                }
                LatitudePos = message.Location.Latitude;
                LongitudePos = message.Location.Longitude;
                await Bot.SendTextMessageAsync(message.Chat.Id, Greeting, replyMarkup: ReplyKeyboard);

            }
            if (!counter.ContainsKey(message.Chat.Id))
            {
                counter.Add(message.Chat.Id, 0);
            }
            else
            {
                counter[message.Chat.Id] = 0;
            }

            

            switch (message.Text)
            {

                case "100m":
                    CallBackData.Clear();
                    NearBarsList = FindNears(100);
                    if (NearBarsList.Count == 0)
                        await Bot.SendTextMessageAsync(message.Chat.Id, "На расстоянии 100m ничего не найдено");
                    else
                    {
                        if (NearBarsList[counter[message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, true, true));
                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, false, true));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, true, true));

                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, false, true));

                            }
                        }
                        await Bot.SendTextMessageAsync(message.Chat.Id, "Оценка от юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[message.Chat.Id]].BarName));
                    }
                    break;
                case "500m":
                    CallBackData.Clear();
                    NearBarsList = FindNears(500);
                    if (NearBarsList.Count == 0)
                        await Bot.SendTextMessageAsync(message.Chat.Id, "На расстоянии 500m ничего не найдено");
                    else
                    {
                        if (NearBarsList[counter[message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, true, true));
                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, false, true));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, true, true));

                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, false, true));

                            }
                        }
                        await Bot.SendTextMessageAsync(message.Chat.Id, "Оценка от юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[message.Chat.Id]].BarName));
                    }
                    break;
                case "1km":
                    CallBackData.Clear();
                    NearBarsList = FindNears(1000);
                    if (NearBarsList.Count == 0)
                        await Bot.SendTextMessageAsync(message.Chat.Id, "На расстоянии 1km ничего не найдено");
                    else
                    {
                        if (NearBarsList[counter[message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, true, true));
                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, false, true));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, true, true));

                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, false, true));

                            }
                        }
                        await Bot.SendTextMessageAsync(message.Chat.Id, "Оценка от юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[message.Chat.Id]].BarName));
                    }
                    break;
                case "2km":
                    CallBackData.Clear();
                    NearBarsList = FindNears(2000);
                    if (NearBarsList.Count == 0)
                        await Bot.SendTextMessageAsync(message.Chat.Id, "На расстоянии 2km ничего не найдено");
                    else
                    {
                        if (NearBarsList[counter[message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, true, true));
                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, false, true));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, true, true));

                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, false, true));

                            }
                        }
                        await Bot.SendTextMessageAsync(message.Chat.Id, "Оценка от юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[message.Chat.Id]].BarName));
                    }
                    break;
                case "3km":
                    CallBackData.Clear();
                    NearBarsList = FindNears(3000);
                    if (NearBarsList.Count == 0)
                        await Bot.SendTextMessageAsync(message.Chat.Id, "На расстоянии 3km ничего не найдено");
                    else
                    {
                        if (NearBarsList[counter[message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, true, true));
                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, false, true));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, true, true));

                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, false, true));

                            }
                        }
                        await Bot.SendTextMessageAsync(message.Chat.Id, "Оценка от юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[message.Chat.Id]].BarName));
                    }
                    break;
                case "5km":
                    CallBackData.Clear();
                    NearBarsList = FindNears(5000);
                    if (NearBarsList.Count == 0)
                        await Bot.SendTextMessageAsync(message.Chat.Id, "На расстоянии 5km ничего не найдено");
                    else
                    {
                        if (NearBarsList[counter[message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, true, true));
                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, false, true));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, true, true));

                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, false, true));

                            }
                        }
                        await Bot.SendTextMessageAsync(message.Chat.Id, "Оценка от юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[message.Chat.Id]].BarName));
                    }
                    break;
                case "8km":
                    CallBackData.Clear();
                    NearBarsList = FindNears(8000);
                    if (NearBarsList.Count == 0)
                        await Bot.SendTextMessageAsync(message.Chat.Id, "На расстоянии 8km ничего не найдено");
                    else
                    {
                        if (NearBarsList[counter[message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, true, true));
                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, false, true));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, true, true));

                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, false, true));

                            }
                        }
                        await Bot.SendTextMessageAsync(message.Chat.Id, "Оценка от юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[message.Chat.Id]].BarName));
                    }
                    break;

                default:
                    break;

            }

        }

        public static async void CallBackQRecived(object sender, CallbackQueryEventArgs e)
        {
            var callback = e.CallbackQuery.Data;
            switch (callback)
            {
                case "Menu":
                    await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Категории меню:", replyMarkup: SetKeyboard(e.CallbackQuery.Message.Chat.Id));
                    break;
                case "Next":
                    CallBackData.Clear();
                    Console.WriteLine("Next");
                    if (NearBarsList.Count > counter[e.CallbackQuery.Message.Chat.Id] + 1)
                    {
                        Console.WriteLine(NearBarsList.Count);
                        Console.WriteLine(counter[e.CallbackQuery.Message.Chat.Id] + "   -   " + e.CallbackQuery.Message.Chat.Id);
                        counter[e.CallbackQuery.Message.Chat.Id] += 1;

                        if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(true, true, false));
                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(true, false, false));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(false, true, false));

                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(false, false, false));

                            }
                        }
                        //CallBackData.Clear();
                        await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Оценка бара юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));

                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Бары закончились");
                    }

                    break;
                case "Back":
                    CallBackData.Clear();
                    if (counter[e.CallbackQuery.Message.Chat.Id] > 0)
                    {
                        Console.WriteLine(counter[e.CallbackQuery.Message.Chat.Id] + "   -   " + e.CallbackQuery.Message.Chat.Id);
                        counter[e.CallbackQuery.Message.Chat.Id] -= 1;

                        if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(true, true, false));

                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(true, false, false));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(false, true, false));

                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(false, false, false));

                            }

                        }
                        //CallBackData.Clear();
                        await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Оценка бара от юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));

                    }
                    break;
                case "Photos":
                    Telegram.Bot.Types.InputMediaPhoto[] f;

                    if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count>=10)
                        f= new Telegram.Bot.Types.InputMediaPhoto [10];
                    else
                        f = new Telegram.Bot.Types.InputMediaPhoto[NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count];

                    int temp_counter = 0;
                        for (int i = 1; i < NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count; i++)
                        {
                        if (i % 10 == 0)
                        {
                            await Bot.SendMediaGroupAsync(e.CallbackQuery.Message.Chat.Id, f);
                            if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count - i >= 10)
                                f = new Telegram.Bot.Types.InputMediaPhoto[10];
                            else f = new Telegram.Bot.Types.InputMediaPhoto[NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count - i];
                            temp_counter = 0;
                        }
                        f[temp_counter] = new Telegram.Bot.Types.InputMediaPhoto(new Telegram.Bot.Types.InputMedia(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[i]));
                        temp_counter++;
                        }
                        await Bot.SendMediaGroupAsync(e.CallbackQuery.Message.Chat.Id, f);
                    
                    break;
                case "Route":
                    await Bot.SendVenueAsync(e.CallbackQuery.Message.Chat.Id, (float)NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].Lat, (float)NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].Lng,"Тыкните на карту", NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName);
                    break;
                case "like":
                    //like
                    LikeButtonClicked(sender, e);
                    break;
                case "dislike":
                    //dislike
                    DislikeButtonClicked(sender, e);
                    break;
                default:
                    break;
            }
        }


        private static List<ModelOfBar> FindNears(int range)//float latitude,float longitude,
        {
            List<ModelOfBar> bars = new List<ModelOfBar>();
            foreach (var item in Program.BarInfo)
            {
                Console.WriteLine("lat - " + item.Lat);
                Console.WriteLine("lng - " + item.Lng);
                Console.WriteLine("me lat - " + LatitudePos);
                Console.WriteLine("me lng - " + LongitudePos);

                double lat1 = LatitudePos * Math.PI / 180;
                double lat2 = item.Lat * Math.PI / 180;
                double lng1 = LongitudePos * Math.PI / 180;
                double lng2 = item.Lng * Math.PI / 180;

                double delta_lat = (lat2 - lat1);
                double delta_lng = (lng2 - lng1);

                double dist = (6378137 * Math.Acos(Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(lng1 - lng2) + Math.Sin(lat1) * Math.Sin(lat2)));
                Console.WriteLine(Math.Round(dist));

                if (Math.Round(dist) < range) bars.Add(item);




            }
            if (bars.Count > 0)
            {
                Console.WriteLine("Что то есть");
                Console.WriteLine(bars.Count);
            }
            return bars;

        }

        private static string SetCaption(long ChatId)
        {
            if (NearBarsList[counter[ChatId]].WorkTime.Contains("отсутствует"))
                return NearBarsList[counter[ChatId]].BarName + "\n"
                + NearBarsList[counter[ChatId]].Phone + "\n"
                + "Режим работы :" + NearBarsList[counter[ChatId]].WorkTime;
            else return NearBarsList[counter[ChatId]].BarName + "\n"
            + NearBarsList[counter[ChatId]].Phone + "\n"
            + NearBarsList[counter[ChatId]].WorkTime;
        }
        public static void GetMenuItems(string NamOfTitle,long ChatId)
        {
            
            string MenuItems = string.Empty;
            bool title = false;
            bool subtitle = false;
            bool subtitle_2 = false;
            foreach (var item in MenuOfNearBar)
            {
                if (item.Title == NamOfTitle) title = true;
                if (item.Subtitle == NamOfTitle) subtitle = true;
                if (item.Subtitle_2 == NamOfTitle) subtitle_2 = true;
            }
            Console.WriteLine(NamOfTitle);
            Console.WriteLine(title +"" +subtitle +"" + subtitle_2);

            if ((title == true || subtitle == true) && ( subtitle_2==false))
            {
                // нужны кнопки
                Bot.SendTextMessageAsync(ChatId,"Выберите категорию",replyMarkup: SetKeyboard(ChatId, title, subtitle, subtitle_2,NamOfTitle));
            }
            else if (subtitle_2 == true)
            {
                // вывод сообщения из меню текстом 
                foreach (var item in MenuOfNearBar)
                {
                    if (item.Subtitle_2 == NamOfTitle) MenuItems += item.Dish + "  -  " + item.Price + " руб" + "\n";
                }
                Bot.SendTextMessageAsync(ChatId, MenuItems);
            }

        }

        private static async void LikeButtonClicked(object sender, CallbackQueryEventArgs e)
        {
            Console.WriteLine("like button cliced");
            foreach (var item in Program.UserRate)
            {
                if (item.BarName == NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName)
                {
                    if (item.Likes.Contains(e.CallbackQuery.Message.Chat.Id) && !item.Dislikes.Contains(e.CallbackQuery.Message.Chat.Id))
                    {
                        await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Вы уже голосовали", false);
                        break;

                    }
                    else if (!item.Likes.Contains(e.CallbackQuery.Message.Chat.Id) && item.Dislikes.Contains(e.CallbackQuery.Message.Chat.Id))
                    {
                        item.Dislikes.Remove(e.CallbackQuery.Message.Chat.Id);
                        item.Likes.Add(e.CallbackQuery.Message.Chat.Id);
                        await Bot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, "Спасибо за оценку", replyMarkup: SetKeyboard(item.BarName));
                        break;
                    }
                    else if(!item.Likes.Contains(e.CallbackQuery.Message.Chat.Id) && !item.Dislikes.Contains(e.CallbackQuery.Message.Chat.Id))
                    {
                        item.Likes.Add(e.CallbackQuery.Message.Chat.Id);
                        await Bot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, "Спасибо за оценку", replyMarkup: SetKeyboard(item.BarName));
                        break;
                    }

                    
                }
            }
                    
                    
        }

        private static async void DislikeButtonClicked(object sender, CallbackQueryEventArgs e)
        {
            foreach (var item in Program.UserRate)
            {
                if (item.BarName == NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName)
                {
                    if (item.Dislikes.Contains(e.CallbackQuery.Message.Chat.Id) && !item.Likes.Contains(e.CallbackQuery.Message.Chat.Id))
                    {
                        await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Вы уже голосовали", false);
                        break;

                    }
                    else if (!item.Dislikes.Contains(e.CallbackQuery.Message.Chat.Id) && item.Likes.Contains(e.CallbackQuery.Message.Chat.Id))
                    {
                        item.Likes.Remove(e.CallbackQuery.Message.Chat.Id);
                        item.Dislikes.Add(e.CallbackQuery.Message.Chat.Id);
                        await Bot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, "Спасибо за оценку", replyMarkup: SetKeyboard(item.BarName));
                        break;
                    }
                    else if (!item.Dislikes.Contains(e.CallbackQuery.Message.Chat.Id) && !item.Likes.Contains(e.CallbackQuery.Message.Chat.Id))
                    {
                        item.Dislikes.Add(e.CallbackQuery.Message.Chat.Id);
                        await Bot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, "Спасибо за оценку", replyMarkup: SetKeyboard(item.BarName));
                        break;
                    }


                }
            }
        }









    }
}
