using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using System;
using Telegram.Bot.Args;

namespace TestBot.Models.Menu
{
    class NearBars
    {
        private static TelegramBotClient Bot;
        public static string menu { get; } = "Вы в ближлижайших барах";
        public static string Greeting { get; } = "Выберите на сколько вам не лень идти";

        public static string AskLocation { get; } = "Разрешите нам глянуть где вы сейчас";

        private static float LatitudePos;
        private static float LongitudePos;

        public static ReplyKeyboardMarkup ReplyKeyboardGeo { get; } = new ReplyKeyboardMarkup(new[] {
            new[]{
            new KeyboardButton("Показать где я сейчас") { RequestLocation=true}
            },
            new[]
            {
                new KeyboardButton("Назад")
            }
            
        },true);
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

        public static void Execute(TelegramBotClient bot)
        {
            Bot = bot;
            Bot.OnMessage += BotOnMessageRecived;
        }

        private static async void BotOnMessageRecived(object sender, MessageEventArgs e)
        {
            var message = e.Message;

            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location)
            {
                if (message.Location == null)
                {
                    await Bot.SendTextMessageAsync(message.Chat.Id,"Может ты забыл включить геолокацию?");
                    return;
                }
                LatitudePos = message.Location.Latitude;
                LongitudePos = message.Location.Longitude;
                await Bot.SendTextMessageAsync(message.Chat.Id,Greeting,replyMarkup:ReplyKeyboard);

            }

            switch (message.Text)
            {
                
                case "Назад":
                    //в главное меню
                    await Bot.SendTextMessageAsync(message.Chat.Id,StartMenu.menu,replyMarkup:StartMenu.ReplyKeyboard);
                    break;
                case "100m":
                    //тут вывод того, что удалось найти в FindNears
                    break;
                case "500m":
                    //тут вывод того, что удалось найти в FindNears
                    break;
                case "1km":
                    //тут вывод того, что удалось найти в FindNears
                    break;
                case "2km":
                    //тут вывод того, что удалось найти в FindNears
                    break;
                case "3km":
                    //тут вывод того, что удалось найти в FindNears
                    break;
                case "5km":
                    //тут вывод того, что удалось найти в FindNears
                    break;
                case "8km":
                    //тут вывод того, что удалось найти в FindNears
                    break;
                default:
                    break;

            }

        }

        private static /*ModelOfBar*/ void  FindNears(float latitude,float longitude, int range)
        {
            // тут отправлятся координаты в бд и ищутся места на расстоянии не дальше range (100m,500m и тд)

        }




    }
}
