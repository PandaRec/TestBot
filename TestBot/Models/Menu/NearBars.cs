using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using System;
using Telegram.Bot.Args;
using System.Collections.Generic;
using Telegram.Bot.Types.Enums;

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

        public static List<ModelOfBar> NearBarsList { get; set; } = new List<ModelOfBar>();

        public static List<ModelOfMenuItems> MenuOfNearBar { get; set; } = new List<ModelOfMenuItems>();

        public static Dictionary<long, int> counter { get; set; } = new Dictionary<long, int>();

        public static Telegram.Bot.Types.Message answ = null;
        private static Telegram.Bot.Types.Message answ2 = null;
        private static Telegram.Bot.Types.Message answ3 = null;
        private static List <Telegram.Bot.Types.Message> answ4 = new List<Telegram.Bot.Types.Message>();


        public static bool FlagToWriteRange { get; set; } = false;

        private static int CounterOfPhotos = 0;


        public static List<string> CallBackData { get; private set; } = new List<string>();

        private static List<Telegram.Bot.Types.Message> MenuItemsToDelete = new List<Telegram.Bot.Types.Message>();

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
        public static ReplyKeyboardMarkup ReplyKeyboarBack { get; } = new ReplyKeyboardMarkup(new[] {
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
                            new KeyboardButton("Свое значение"),
                            //new KeyboardButton("5km"),
                            //new KeyboardButton("8km"),
                            new KeyboardButton("Назад")
                        }

        }, true);

        public static bool Contains(Telegram.Bot.Types.Message message)
        {
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location) return true;
            if (message.Text.Equals("\U0001F43E") || message.Text.Equals("Показать где я сейчас")
            || message.Text.Equals("100m") || message.Text.Equals("500m") || message.Text.Equals("1km")
            || message.Text.Equals("2km") || message.Text.Equals("Свое значение")) return true;
            else return false;
        }
        public static bool ContainsData(Telegram.Bot.Types.CallbackQuery callback)
        {
            if (callback.Data.Equals("Menu") || callback.Data.Equals("Next") || callback.Data.Equals("Photos") 
                || callback.Data.Equals("Back") || callback.Data.Equals("Route") || callback.Data.Equals("like") 
                || callback.Data.Equals("dislike") || callback.Data.Equals("like") || callback.Data.Equals("dilike") 
                || callback.Data.Equals("Next_Photos") || callback.Data.Equals("Back_Photos"))
                return true;
            else return false;
        }
        
      /*  public static InlineKeyboardMarkup SetKeyboard(string barname)
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
        */
        /// <summary>
        /// Утсновка кнопок назад/дальше для фотографий
        /// </summary>
        /// <returns></returns>
        private static InlineKeyboardMarkup SetKeyboard(bool end,bool first)
        {
            if (first == true)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
                    new InlineKeyboardButton[]
                {
                InlineKeyboardButton.WithCallbackData("Дальше","Next_Photos")

                }
                });
                return Temp;
            }
            else if (end == true)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
                    new InlineKeyboardButton[]
                {
                InlineKeyboardButton.WithCallbackData("Назад","Back_Photos")

                }
                });
                return Temp;
            }
            else
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
                    new InlineKeyboardButton[]
                {
                InlineKeyboardButton.WithCallbackData("Назад","Back_Photos"),
                InlineKeyboardButton.WithCallbackData("Дальше","Next_Photos")

                }
                });
                return Temp;
            }
            
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
        public static InlineKeyboardMarkup SetKeyboard(bool hasmenu, bool hasphotos, bool first,string barname)
        {
            int temp_likes = 0;
            int temp_dislikes = 0;
            foreach (var item in Program.UserRate)
            {
                if (item.BarName == barname)
                {
                    temp_likes = item.Likes.Count;
                    temp_dislikes = item.Dislikes.Count;
                    break;
                }
            }

            if (hasmenu == true && hasphotos == true && first == true)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("\U00002764"+temp_likes,"like"),
                InlineKeyboardButton.WithCallbackData("\U0001F494"+temp_dislikes,"dislike")

            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Фото","Photos"),
                InlineKeyboardButton.WithCallbackData("Меню","Menu"),
                InlineKeyboardButton.WithCallbackData("Маршрут","Route")
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Дальше","Next")
            }
        });
                return Temp;

            }
            else if (hasmenu == false && hasphotos == true && first == true)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("\U00002764"+temp_likes,"like"),
                InlineKeyboardButton.WithCallbackData("\U0001F494"+temp_dislikes,"dislike")

            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Фото","Photos"),
                InlineKeyboardButton.WithCallbackData("Маршрут","Route")
            },
            new InlineKeyboardButton[]
            {
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
                InlineKeyboardButton.WithCallbackData("\U00002764"+temp_likes,"like"),
                InlineKeyboardButton.WithCallbackData("\U0001F494"+temp_dislikes,"dislike")

            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Меню","Menu"),
                InlineKeyboardButton.WithCallbackData("Маршрут","Route")
            },
            new InlineKeyboardButton[]
            {
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
                InlineKeyboardButton.WithCallbackData("\U00002764"+temp_likes,"like"),
                InlineKeyboardButton.WithCallbackData("\U0001F494"+temp_dislikes,"dislike")

            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Маршрут","Route"),
            },
            new InlineKeyboardButton[]
            {
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
                InlineKeyboardButton.WithCallbackData("\U00002764"+temp_likes,"like"),
                InlineKeyboardButton.WithCallbackData("\U0001F494"+temp_dislikes,"dislike")

            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Фото","Photos"),
                InlineKeyboardButton.WithCallbackData("Меню","Menu"),
                InlineKeyboardButton.WithCallbackData("Маршрут","Route")

            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Назад","Back"),
                InlineKeyboardButton.WithCallbackData("Дальше","Next")


            }
        });
                return Temp;

            }
            else if (hasmenu == false && hasphotos == true && first == false)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("\U00002764"+temp_likes,"like"),
                InlineKeyboardButton.WithCallbackData("\U0001F494"+temp_dislikes,"dislike")

            },
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
                InlineKeyboardButton.WithCallbackData("\U00002764"+temp_likes,"like"),
                InlineKeyboardButton.WithCallbackData("\U0001F494"+temp_dislikes,"dislike")

            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Меню","Menu"),
                InlineKeyboardButton.WithCallbackData("Маршрут","Route")
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Назад","Back"),
                InlineKeyboardButton.WithCallbackData("Дальше","Next")
            }

        });

                return Temp;
            }
            else if (hasmenu == false && hasphotos == false && first == false)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("\U00002764"+temp_likes,"like"),
                InlineKeyboardButton.WithCallbackData("\U0001F494"+temp_dislikes,"dislike")

            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Маршрут","Route")
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Назад","Back"),
                InlineKeyboardButton.WithCallbackData("Дальше","Next")
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

                    foreach (var item in MenuItemsToDelete)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);
                    }
                    MenuItemsToDelete.Clear();

                    NearBarsList = FindNears(100);
                    if (NearBarsList.Count == 0)
                        await Bot.SendTextMessageAsync(message.Chat.Id, "На расстоянии 100m ничего не найдено");
                    else
                    {
                        if (NearBarsList[counter[message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, true, true, NearBarsList[counter[message.Chat.Id]].BarName));
                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, false, true, NearBarsList[counter[message.Chat.Id]].BarName));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, true, true, NearBarsList[counter[message.Chat.Id]].BarName));

                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, false, true, NearBarsList[counter[message.Chat.Id]].BarName));

                            }
                        }
                        //answ2 = await Bot.SendTextMessageAsync(message.Chat.Id, "Оценка от юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[message.Chat.Id]].BarName));
                    }
                    break;
                case "500m":
                    CallBackData.Clear();

                    foreach (var item in MenuItemsToDelete)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);
                    }
                    MenuItemsToDelete.Clear();

                    NearBarsList = FindNears(500);
                    if (NearBarsList.Count == 0)
                        await Bot.SendTextMessageAsync(message.Chat.Id, "На расстоянии 500m ничего не найдено");
                    else
                    {
                        if (NearBarsList[counter[message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, true, true, NearBarsList[counter[message.Chat.Id]].BarName));
                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, false, true, NearBarsList[counter[message.Chat.Id]].BarName));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, true, true, NearBarsList[counter[message.Chat.Id]].BarName));

                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, false, true, NearBarsList[counter[message.Chat.Id]].BarName));

                            }
                        }
                       // answ2 = await Bot.SendTextMessageAsync(message.Chat.Id, "Оценка от юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[message.Chat.Id]].BarName));
                    }
                    break;
                case "1km":
                    CallBackData.Clear();

                    foreach (var item in MenuItemsToDelete)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);
                    }
                    MenuItemsToDelete.Clear();

                    NearBarsList = FindNears(1000);
                    if (NearBarsList.Count == 0)
                        await Bot.SendTextMessageAsync(message.Chat.Id, "На расстоянии 1km ничего не найдено");
                    else
                    {
                        if (NearBarsList[counter[message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, true, true, NearBarsList[counter[message.Chat.Id]].BarName));
                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, false, true, NearBarsList[counter[message.Chat.Id]].BarName));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, true, true, NearBarsList[counter[message.Chat.Id]].BarName));

                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, false, true, NearBarsList[counter[message.Chat.Id]].BarName));

                            }
                        }
                        //answ2 = await Bot.SendTextMessageAsync(message.Chat.Id, "Оценка от юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[message.Chat.Id]].BarName));
                    }
                    break;
                case "2km":
                    CallBackData.Clear();

                    foreach (var item in MenuItemsToDelete)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);
                    }
                    MenuItemsToDelete.Clear();

                    NearBarsList = FindNears(2000);
                    if (NearBarsList.Count == 0)
                        await Bot.SendTextMessageAsync(message.Chat.Id, "На расстоянии 2km ничего не найдено");
                    else
                    {
                        if (NearBarsList[counter[message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, true, true, NearBarsList[counter[message.Chat.Id]].BarName));
                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true, false, true, NearBarsList[counter[message.Chat.Id]].BarName));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, true, true, NearBarsList[counter[message.Chat.Id]].BarName));

                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false, false, true, NearBarsList[counter[message.Chat.Id]].BarName));

                            }
                        }
                    }
                    break;
                case "Свое значение":

                    FlagToWriteRange = true;
                    await Bot.SendTextMessageAsync(message.Chat.Id,"Введите дистанцию в км:", replyMarkup: ReplyKeyboarBack);
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
                    foreach (var item in answ4)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);

                    }
                    foreach (var item in MenuItemsToDelete)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);

                    }
                    answ4.Clear();
                    MenuItemsToDelete.Clear();
                    answ2 = null;
                    answ3 = null;
                    Telegram.Bot.Types.Message temp = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Категории меню:", replyMarkup: SetKeyboard(e.CallbackQuery.Message.Chat.Id));
                    MenuItemsToDelete.Add(temp);
                    break;
                case "Next":
                    CallBackData.Clear();
                    foreach (var item in MenuItemsToDelete)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);
                    }
                    foreach (var item in answ4)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);

                    }
                    answ4.Clear();
                    MenuItemsToDelete.Clear();
                    answ2 = null;
                    answ3 = null;
                    CounterOfPhotos = 0;
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
                                answ = await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(true, true, false, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));
                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(true, false, false, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(false, true, false, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));

                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(false, false, false, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));

                            }
                        }
                        //CallBackData.Clear();
                        //answ2 = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Оценка бара юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));

                    }
                    else
                    {
                        var j = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Бары закончились");
                        MenuItemsToDelete.Add(j);
                    }

                    break;
                case "Back":
                    CallBackData.Clear();
                    foreach (var item in MenuItemsToDelete)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);
                    }
                    foreach (var item in answ4)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);

                    }
                    answ2 = null;
                    answ3 = null;
                    answ4.Clear();
                    MenuItemsToDelete.Clear();
                    CounterOfPhotos = 0;
                    if (counter[e.CallbackQuery.Message.Chat.Id] > 0)
                    {
                        Console.WriteLine(counter[e.CallbackQuery.Message.Chat.Id] + "   -   " + e.CallbackQuery.Message.Chat.Id);
                        counter[e.CallbackQuery.Message.Chat.Id] -= 1;
                        bool hasphotos = false;

                        if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count > 0)
                            hasphotos = true;

                        if (counter[e.CallbackQuery.Message.Chat.Id] == 0)
                        {
                            if (answ != null)
                                await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                            if(hasphotos==true)
                                answ = await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu,hasphotos, true, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));
                            else
                                answ = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu,hasphotos, true, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));

                        }
                        else
                        {
                            if (answ != null)
                                await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                            if (hasphotos == true)
                                answ = await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu, hasphotos, false, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));
                            else
                                answ = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu, hasphotos, false, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));

                        }
                        //CallBackData.Clear();
                       //answ2 =  await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Оценка бара от юзеров", replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));

                    }
                    Console.WriteLine("------> "+(counter[e.CallbackQuery.Message.Chat.Id]));
                    break;
                case "Photos":
                    CounterOfPhotos = 0;
                    foreach (var item in MenuItemsToDelete)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);

                    }
                    answ2 = null;
                    answ3 = null;
                    MenuItemsToDelete.Clear();
                    PhotoButtonClicked(sender, e);
                    break;
                case "Next_Photos":
                    PhotoButtonClicked(sender,e);
                    break;
                case "Back_Photos":
                    BackPhotosButtonClicked(sender,e);
                    break;
                case "Route":
                    foreach (var item in answ4)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);

                    }
                    foreach (var item in MenuItemsToDelete)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);

                    }
                    answ4.Clear();
                    answ2 = null;
                    answ3 = null;
                    MenuItemsToDelete.Clear();
                    Telegram.Bot.Types.Message t = await Bot.SendVenueAsync(e.CallbackQuery.Message.Chat.Id, (float)NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].Lat, (float)NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].Lng,"Тыкните на карту", NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName);
                    MenuItemsToDelete.Add(t);
                    break;
                case "like":
                    //like
                    foreach (var item in answ4)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);

                    }
                    answ4.Clear();
                    LikeButtonClicked(sender, e);
                    break;
                case "dislike":
                    //dislike
                    foreach (var item in answ4)
                    {
                        await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);

                    }
                    answ4.Clear();
                    DislikeButtonClicked(sender, e);
                    break;
                default:
                    break;
            }
        }


        private static List<ModelOfBar> FindNears(double range)//float latitude,float longitude,
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

                if (Math.Round(dist) < range*1000) bars.Add(item);




            }
            if (bars.Count > 0)
            {
                Console.WriteLine("Что то есть");
                Console.WriteLine(bars.Count);
            }
            return bars;

        }

        public static string SetCaption(long ChatId)
        {
            if (NearBarsList[counter[ChatId]].WorkTime.Contains("отсутствует"))
                return NearBarsList[counter[ChatId]].BarName + "\n"
                + NearBarsList[counter[ChatId]].Phone + "\n"
                + "Режим работы :" + NearBarsList[counter[ChatId]].WorkTime;
            else return NearBarsList[counter[ChatId]].BarName + "\n"
            + NearBarsList[counter[ChatId]].Phone + "\n"
            + NearBarsList[counter[ChatId]].WorkTime;
        }
        public static async void GetMenuItems(string NamOfTitle,long ChatId)
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
                
                if (answ2 != null)
                    await Bot.DeleteMessageAsync(answ2.Chat.Id, answ2.MessageId);
                if (answ3 != null)
                {
                    await Bot.DeleteMessageAsync(answ3.Chat.Id, answ3.MessageId);
                    answ3 = null;
                }

                if (MenuItemsToDelete.Contains(answ2))
                    MenuItemsToDelete.Remove(answ2);
                if(MenuItemsToDelete.Contains(answ3))
                    MenuItemsToDelete.Remove(answ3);

                //else if(answ2 != null)
                // MenuItemsToDelete.Add(answ2);

                answ2 = await Bot.SendTextMessageAsync(ChatId,"Выберите категорию",replyMarkup: SetKeyboard(ChatId, title, subtitle, subtitle_2,NamOfTitle));
                MenuItemsToDelete.Add(answ2);
                //answ2 = null; // ТУТ
            }
            else if (subtitle_2 == true)
            {
                // вывод сообщения из меню текстом 
                foreach (var item in MenuOfNearBar)
                {
                    if (item.Subtitle_2 == NamOfTitle) MenuItems += item.Dish + "  -  " + "<b><i>"+item.Price + " руб" +"</i></b>"+ "\n"+"--------"+"\n";
                }


                /*Telegram.Bot.Types.Message temp*/
                if (answ3 != null)
                {
                    Console.WriteLine("del");
                    await Bot.DeleteMessageAsync(answ3.Chat.Id, answ3.MessageId);

                }
                else Console.WriteLine("ne del");
                if (MenuItemsToDelete.Contains(answ3))
                    MenuItemsToDelete.Remove(answ3);

                answ3 = await Bot.SendTextMessageAsync(ChatId, MenuItems,ParseMode.Html);
                MenuItemsToDelete.Add(answ3);
                //answ3 = null;
            }

        }

        private static async void LikeButtonClicked(object sender, CallbackQueryEventArgs e)
        {
            Console.WriteLine("like button cliced");
            foreach (var item in Program.UserRate)
            {
                if (item.BarName == NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName)
                {
                    Console.WriteLine("++++++++>" + counter[e.CallbackQuery.Message.Chat.Id]);

                    if (item.Likes.Contains(e.CallbackQuery.Message.Chat.Id) && !item.Dislikes.Contains(e.CallbackQuery.Message.Chat.Id))
                    {
                        await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Вы уже голосовали", false);
                        break;

                    }
                    else if (!item.Likes.Contains(e.CallbackQuery.Message.Chat.Id) && item.Dislikes.Contains(e.CallbackQuery.Message.Chat.Id))
                    {
                        item.Dislikes.Remove(e.CallbackQuery.Message.Chat.Id);
                        item.Likes.Add(e.CallbackQuery.Message.Chat.Id);
                        bool hasphotos = false;
                        bool first = false;
                        
                        if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count > 0)
                            hasphotos = true;
                        if (counter[e.CallbackQuery.Message.Chat.Id] == 0) first = true;

                        if (answ != null)
                            await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                        if (hasphotos == false)
                            answ = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu, hasphotos, first, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));
                        else
                           answ = await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu, hasphotos, first, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));

                        break;
                    }
                    else if(!item.Likes.Contains(e.CallbackQuery.Message.Chat.Id) && !item.Dislikes.Contains(e.CallbackQuery.Message.Chat.Id))
                    {
                        item.Likes.Add(e.CallbackQuery.Message.Chat.Id);
                        bool hasphotos = false;
                        bool first = false;

                        if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count > 0)
                            hasphotos = true;
                        if (counter[e.CallbackQuery.Message.Chat.Id] == 0) first = true;

                        if (answ != null)
                            await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                        if(hasphotos==false)
                        answ = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu, hasphotos, first, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));
                        else
                         answ = await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu, hasphotos, first, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));

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
                    Console.WriteLine("++++++++>" + counter[e.CallbackQuery.Message.Chat.Id]);

                    if (item.Dislikes.Contains(e.CallbackQuery.Message.Chat.Id) && !item.Likes.Contains(e.CallbackQuery.Message.Chat.Id))
                    {
                        await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Вы уже голосовали", false);
                        break;

                    }
                    else if (!item.Dislikes.Contains(e.CallbackQuery.Message.Chat.Id) && item.Likes.Contains(e.CallbackQuery.Message.Chat.Id))
                    {
                        item.Likes.Remove(e.CallbackQuery.Message.Chat.Id);
                        item.Dislikes.Add(e.CallbackQuery.Message.Chat.Id);
                        bool hasphotos = false;
                        bool first = false;

                        if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count > 0)
                            hasphotos = true;
                        if (counter[e.CallbackQuery.Message.Chat.Id] == 0) first = true;

                        if (answ != null)
                            await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                        if (hasphotos == false)
                            answ = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu, hasphotos, first, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));
                        else
                            answ = await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu, hasphotos, first, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));
                        break;
                    }
                    else if (!item.Dislikes.Contains(e.CallbackQuery.Message.Chat.Id) && !item.Likes.Contains(e.CallbackQuery.Message.Chat.Id))
                    {
                        item.Dislikes.Add(e.CallbackQuery.Message.Chat.Id);
                        bool hasphotos = false;
                        bool first = false;

                        if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count > 0)
                            hasphotos = true;
                        if (counter[e.CallbackQuery.Message.Chat.Id] == 0) first = true;

                        if (answ != null)
                            await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                        if (hasphotos == false)
                            answ = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu, hasphotos, first, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));
                        else
                            answ = await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu, hasphotos, first, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].BarName));
                        break;
                    }


                }
            }
        }

        private static async void PhotoButtonClicked(object sender, CallbackQueryEventArgs e)
        {
            Telegram.Bot.Types.InputMediaPhoto[] f;

            Console.WriteLine("кол во фоток в этом баре - "+ NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count);
            if (CounterOfPhotos >= NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count)
            {
                return;
            }

            if (answ4.Count!=0)
            {
                foreach (var item in answ4)
                {
                    await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);
                }
                answ4.Clear();

            }
            if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count - CounterOfPhotos >= 10)
            {
                f = new Telegram.Bot.Types.InputMediaPhoto[10];


            }
            else
                f = new Telegram.Bot.Types.InputMediaPhoto[NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count - CounterOfPhotos];
           
            int temp_counter = 0;

            for (int i = CounterOfPhotos; i < f.Length+CounterOfPhotos; i++)
            {
                f[temp_counter] = new Telegram.Bot.Types.InputMediaPhoto(new Telegram.Bot.Types.InputMedia(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[i]));
                temp_counter++;
            }
                var k = await Bot.SendMediaGroupAsync(f, e.CallbackQuery.Message.Chat.Id);
            foreach (var item in k)
            {
                answ4.Add(item);
            }

            if (CounterOfPhotos == 0)
            {
                Telegram.Bot.Types.Message answ_0 = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Дальше", replyMarkup: SetKeyboard(false, true));
                answ4.Add(answ_0);
            }
            else if (CounterOfPhotos + temp_counter == NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count)
            {
                Telegram.Bot.Types.Message answ_0 = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Назад", replyMarkup: SetKeyboard(true, false));
                answ4.Add(answ_0);

            }
            else
            {
                Telegram.Bot.Types.Message answ_0 = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Назад/Дальше", replyMarkup: SetKeyboard(false, false));
                answ4.Add(answ_0);

            }
            CounterOfPhotos += f.Length;
        }

        private static async void BackPhotosButtonClicked(object sender, CallbackQueryEventArgs e)
        {
            Telegram.Bot.Types.InputMediaPhoto[] f;
            int temp = 0;

            if (CounterOfPhotos < 0)
            {
                return;
            }
            if (answ4.Count != 0)
            {
                foreach (var item in answ4)
                {
                    await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);
                }
                answ4.Clear();

            }

            if (CounterOfPhotos % 10 != 0)
            {
                f = new Telegram.Bot.Types.InputMediaPhoto[10];
                temp = CounterOfPhotos%10;
            }
            else
            {
                f = new Telegram.Bot.Types.InputMediaPhoto[10];
                temp = 10;
            }

            int temp_counter = 0;

            for (int i = CounterOfPhotos - f.Length-temp; i <CounterOfPhotos-temp; i++)
            {
                f[temp_counter] = new Telegram.Bot.Types.InputMediaPhoto(new Telegram.Bot.Types.InputMedia(NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[i]));
                temp_counter++;
            }
            var k = await Bot.SendMediaGroupAsync(f, e.CallbackQuery.Message.Chat.Id);

            foreach (var item in k)
            {
                answ4.Add(item);
            }

            if (CounterOfPhotos - temp_counter - temp == 0)
            {
                Telegram.Bot.Types.Message answ_0 = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Дальше", replyMarkup: SetKeyboard(false, true));
                answ4.Add(answ_0);
            }
            else
            {
                Telegram.Bot.Types.Message answ_0 = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Назад/Дальше", replyMarkup: SetKeyboard(false, false));
                answ4.Add(answ_0);

            }
            CounterOfPhotos -= f.Length;

        }


        public static async void UsersRangeRecived(object sender, MessageEventArgs e,double UserRange)
        {
            await Bot.SendTextMessageAsync(e.Message.Chat.Id,"вот что удалось найти",replyMarkup:ReplyKeyboard);
            CallBackData.Clear();
            NearBarsList = FindNears(UserRange);

            foreach (var item in MenuItemsToDelete)
            {
                await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId);
            }
            MenuItemsToDelete.Clear();

            if (NearBarsList.Count == 0)
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "На расстоянии 5km ничего не найдено");
            else
            {
                if (NearBarsList[counter[e.Message.Chat.Id]].HasMenu == true)
                {
                    if (NearBarsList[counter[e.Message.Chat.Id]].PictureLinks.Count > 0)
                    {
                        if (answ != null)
                            await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                        answ = await Bot.SendPhotoAsync(e.Message.Chat.Id, NearBarsList[counter[e.Message.Chat.Id]].PictureLinks[0], SetCaption(e.Message.Chat.Id), replyMarkup: SetKeyboard(true, true, true, NearBarsList[counter[e.Message.Chat.Id]].BarName));
                    }
                    else
                    {
                        if (answ != null)
                            await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                        answ = await Bot.SendTextMessageAsync(e.Message.Chat.Id, SetCaption(e.Message.Chat.Id), replyMarkup: SetKeyboard(true, false, true, NearBarsList[counter[e.Message.Chat.Id]].BarName));

                    }

                }
                else
                {
                    if (NearBarsList[counter[e.Message.Chat.Id]].PictureLinks.Count > 0)
                    {
                        if (answ != null)
                            await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                        answ = await Bot.SendPhotoAsync(e.Message.Chat.Id, NearBarsList[counter[e.Message.Chat.Id]].PictureLinks[0], SetCaption(e.Message.Chat.Id), replyMarkup: SetKeyboard(false, true, true, NearBarsList[counter[e.Message.Chat.Id]].BarName));

                    }
                    else
                    {
                        if (answ != null)
                            await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                        answ = await Bot.SendTextMessageAsync(e.Message.Chat.Id, SetCaption(e.Message.Chat.Id), replyMarkup: SetKeyboard(false, false, true, NearBarsList[counter[e.Message.Chat.Id]].BarName));

                    }
                }
            }
            FlagToWriteRange = false;
        }






    }
}
