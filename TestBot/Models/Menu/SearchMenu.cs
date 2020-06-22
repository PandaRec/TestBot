using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace TestBot.Models.Menu
{
    class SearchMenu
    {
        private static TelegramBotClient Bot = Program.bot;

        public static ReplyKeyboardMarkup ReplyKeyboard { get; } = new ReplyKeyboardMarkup(new[] { new KeyboardButton("Назад") }, true);
        public static bool FlagToWriteName = false;
        public static string Menu { get; } = "Введите название бара";

        public static ReplyKeyboardMarkup ReplyKeyboarBack { get; } = new ReplyKeyboardMarkup(new[] {
            new[]
            {
                new KeyboardButton("Назад")
            }

        }, true);
        public static bool Contains(Telegram.Bot.Types.Message message)
        {
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location) return false;
                if (message.Text.Equals("\U0001F50D")) return true;
            else return false;
        }
        public static async void UserNameRecived(object sender, MessageEventArgs e)
        {
            char[] text_message = e.Message.Text.ToLower().ToCharArray();
            
            Console.WriteLine(text_message);

            List<ModelOfBar> Bars = new List<ModelOfBar>();


            for (int i = 0; i < text_message.Length; i++)
            {
                if (text_message[i].Equals('ё'))
                    text_message[i] = 'е';
            }
            foreach (var item in Program.BarInfo)
            {
                if (item.BarName.ToLower().IndexOf(new string(text_message))!=-1)
                {
                    //записываются бары которые подошли
                    Bars.Add(item);
                }
            }
            if (Bars.Count == 0)
            {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id,"не удалось найти ни одного бара с таким именем");
                return;
            }
            NearBars.NearBarsList = Bars;
            if (!NearBars.counter.ContainsKey(e.Message.Chat.Id))
            {
                NearBars.counter.Add(e.Message.Chat.Id, 0);
            }
            else
            {
                NearBars.counter[e.Message.Chat.Id] = 0;
            }
            if (NearBars.NearBarsList[NearBars.counter[e.Message.Chat.Id]].HasMenu == true)
            {
                if (NearBars.NearBarsList[NearBars.counter[e.Message.Chat.Id]].PictureLinks.Count > 0)
                {
                    if (NearBars.answ != null)
                        await Bot.DeleteMessageAsync(NearBars.answ.Chat.Id, NearBars.answ.MessageId);
                    NearBars.answ = await Bot.SendPhotoAsync(e.Message.Chat.Id, NearBars.NearBarsList[NearBars.counter[e.Message.Chat.Id]].PictureLinks[0], NearBars.SetCaption(e.Message.Chat.Id), replyMarkup: NearBars.SetKeyboard(true, true, true, NearBars.NearBarsList[NearBars.counter[e.Message.Chat.Id]].BarName));
                }
                else
                {
                    if (NearBars.answ != null)
                        await Bot.DeleteMessageAsync(NearBars.answ.Chat.Id, NearBars.answ.MessageId);
                    NearBars.answ = await Bot.SendTextMessageAsync(e.Message.Chat.Id, NearBars.SetCaption(e.Message.Chat.Id), replyMarkup: NearBars.SetKeyboard(true, false, true, NearBars.NearBarsList[NearBars.counter[e.Message.Chat.Id]].BarName));

                }

            }
            else
            {
                if (NearBars.NearBarsList[NearBars.counter[e.Message.Chat.Id]].PictureLinks.Count > 0)
                {
                    if (NearBars.answ != null)
                        await Bot.DeleteMessageAsync(NearBars.answ.Chat.Id, NearBars.answ.MessageId);
                    NearBars.answ = await Bot.SendPhotoAsync(e.Message.Chat.Id, NearBars.NearBarsList[NearBars.counter[e.Message.Chat.Id]].PictureLinks[0], NearBars.SetCaption(e.Message.Chat.Id), replyMarkup: NearBars.SetKeyboard(false, true, true, NearBars.NearBarsList[NearBars.counter[e.Message.Chat.Id]].BarName));

                }
                else
                {
                    if (NearBars.answ != null)
                        await Bot.DeleteMessageAsync(NearBars.answ.Chat.Id, NearBars.answ.MessageId);
                    NearBars.answ = await Bot.SendTextMessageAsync(e.Message.Chat.Id, NearBars.SetCaption(e.Message.Chat.Id), replyMarkup: NearBars.SetKeyboard(false, false, true, NearBars.NearBarsList[NearBars.counter[e.Message.Chat.Id]].BarName));

                }
            }

        }
    }
}
