using Telegram.Bot.Types.ReplyMarkups;

namespace TestBot.Models.Menu
{
    class NearBars
    {
        public static string menu { get; } = "Вы в ближлижайших барах";
        public static string Greeting { get; } = "Выберите на сколько вам не лень идти";

        

        public static ReplyKeyboardMarkup ReplyKeyboard { get; } = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("100m"){ RequestLocation=true},
                            new KeyboardButton("500m"){ RequestLocation=true},
                            new KeyboardButton("1km"){ RequestLocation=true},
                            new KeyboardButton("2km"){ RequestLocation=true}
                         },
                        new[]
                        {
                            new KeyboardButton("3km"){ RequestLocation=true},
                            new KeyboardButton("5km"){ RequestLocation=true},
                            new KeyboardButton("8km"){ RequestLocation=true},
                            new KeyboardButton("Назад")
                        }

        }, true);

        


    }
}
