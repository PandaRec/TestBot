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

        private static Dictionary<long, int> counter = new Dictionary<long, int>();

        private static Telegram.Bot.Types.Message answ = null;

        private static List<string> CallBackData = new List<string>();


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
            if (callback.Data.Equals("Menu") || callback.Data.Equals("Next") || callback.Data.Equals("Photos") || callback.Data.Equals("Back")) return true;
            else return false;
        }
        public static InlineKeyboardMarkup SetKeyboard(long ChatId)
        {
            CallBackData.Clear();
            List<string> titles = new List<string>();
            List<List<InlineKeyboardButton>> rowList = new List<List<InlineKeyboardButton>>();

            foreach (var item in Program.MenuItems)
            {
                if (item.Title != "" && !titles.Contains(item.Title) && item.BarName==NearBarsList[counter[ChatId]].BarName)
                    titles.Add(item.Title);
                if (item.Subtitle != "" && !titles.Contains(item.Subtitle) && item.BarName == NearBarsList[counter[ChatId]].BarName)
                    titles.Add(item.Subtitle);
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
            //InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(rowList);
            return new InlineKeyboardMarkup(rowList);
        }
        public static InlineKeyboardMarkup SetKeyboard(bool hasmenu,bool hasphotos,bool first)
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
                InlineKeyboardButton.WithCallbackData("Фотогаллерея","Photos"),
            }
        });
                return Temp;

            }
            else if (hasmenu == false && hasphotos == true && first == true)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Фотогаллерея","Photos"),
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
                InlineKeyboardButton.WithCallbackData("Фотогаллерея","Photos"),
            }
        });
                return Temp;

            }
            else if (hasmenu == false && hasphotos == true && first == false)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Фотогаллерея","Photos"),
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
                    await Bot.SendTextMessageAsync(message.Chat.Id,"Может ты забыл включить геолокацию?");
                    return;
                }
                LatitudePos = message.Location.Latitude;
                LongitudePos = message.Location.Longitude;
                await Bot.SendTextMessageAsync(message.Chat.Id,Greeting,replyMarkup:ReplyKeyboard);

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
                    }
                    break;
                case "500m":
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
                    }                    
                    break;
                case "1km":
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
                    }
                    break;
                case "2km":
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
                    }
                    break;
                case "3km":
                    NearBarsList = FindNears(3000);
                    if (NearBarsList.Count == 0)
                        await Bot.SendTextMessageAsync(message.Chat.Id, "На расстоянии 3km ничего не найдено");
                    else
                    {
                        if (NearBarsList[counter[message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true,true,true));
                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(true,false,true));

                            }

                        }
                        else
                        {
                            if (NearBarsList[counter[message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                answ = await Bot.SendPhotoAsync(message.Chat.Id, NearBarsList[counter[message.Chat.Id]].PictureLinks[0], SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false,true,true));

                            }
                            else
                            {
                                answ = await Bot.SendTextMessageAsync(message.Chat.Id, SetCaption(message.Chat.Id), replyMarkup: SetKeyboard(false,false,true));

                            }
                        }
                    }
                    //тут вывод того, что удалось найти в FindNears
                    break;
                case "5km":
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
                    }
                    break;
                case "8km":
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
                    await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,"Категории меню:",replyMarkup: SetKeyboard(e.CallbackQuery.Message.Chat.Id));
                    break;
                case "Next":
                    Console.WriteLine("Next");
                    if (NearBarsList.Count > counter[e.CallbackQuery.Message.Chat.Id]+1)
                    {
                        Console.WriteLine(NearBarsList.Count);
                        Console.WriteLine(counter[e.CallbackQuery.Message.Chat.Id]+ "   -   "+e.CallbackQuery.Message.Chat.Id);
                        counter[e.CallbackQuery.Message.Chat.Id] += 1;

                        if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].HasMenu == true)
                        {
                            if (NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks.Count > 0)
                            {
                                if(answ!=null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ=await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(true, true, false));
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
                                answ =await Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, NearBarsList[counter[e.CallbackQuery.Message.Chat.Id]].PictureLinks[0], SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(false, true, false));

                            }
                            else
                            {
                                if (answ != null)
                                    await Bot.DeleteMessageAsync(answ.Chat.Id, answ.MessageId);
                                answ = await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, SetCaption(e.CallbackQuery.Message.Chat.Id), replyMarkup: SetKeyboard(false, false, false));

                            }
                        }
                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Бары закончились");
                    }

                    break;
                case "Back":
                    if (counter[e.CallbackQuery.Message.Chat.Id]>0)
                    {
                        Console.WriteLine(counter[e.CallbackQuery.Message.Chat.Id] + "   -   " + e.CallbackQuery.Message.Chat.Id);
                        counter[e.CallbackQuery.Message.Chat.Id]-=1;

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
                    }
                    break;
                case "Photos":
                    break;
                default:
                    break;
            }
        }


        private static List<ModelOfBar>  FindNears( int range)//float latitude,float longitude,
        {
            List<ModelOfBar> bars = new List<ModelOfBar>();
            foreach (var item in Program.BarInfo)
            {
                Console.WriteLine("lat - "+item.Lat);
                Console.WriteLine("lng - "+item.Lng);
                Console.WriteLine("me lat - "+LatitudePos);
                Console.WriteLine( "me lng - "+LongitudePos);

                double lat1 = LatitudePos * Math.PI / 180;
                double lat2 = item.Lat * Math.PI / 180;
                double lng1 = LongitudePos * Math.PI / 180;
                double lng2 = item.Lng * Math.PI / 180;

                double delta_lat = (lat2 - lat1);
                double delta_lng = (lng2 - lng1);

                double dist =  (6378137 * Math.Acos(Math.Cos( lat1) * Math.Cos( lat2) * Math.Cos( lng1 - lng2) + Math.Sin( lat1) * Math.Sin( lat2)));
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
            if(NearBarsList[counter[ChatId]].WorkTime.Contains("отсутствует"))
            return NearBarsList[counter[ChatId]].BarName + "\n"
            + NearBarsList[counter[ChatId]].Phone + "\n"
            +"Режим работы :"+ NearBarsList[counter[ChatId]].WorkTime;
            else return NearBarsList[counter[ChatId]].BarName + "\n"
            + NearBarsList[counter[ChatId]].Phone + "\n"
            + NearBarsList[counter[ChatId]].WorkTime;
       }





    }
}
