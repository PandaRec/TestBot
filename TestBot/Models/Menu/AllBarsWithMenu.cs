using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TestBot.Models.Menu
{
    class AllBarsWithMenu
    {
        private static TelegramBotClient Bot = Program.bot;

        public static bool Contains(Telegram.Bot.Types.Message message)
        {
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location) return false;
            if (message.Text.Equals("\U0001F52E")) return true;
            else return false;
        }

        public static async void MessageRecived(object sender, MessageEventArgs e)
        {
            List<ModelOfBar> bars = new List<ModelOfBar>();

            foreach (var item in Program.BarInfo)
            {
                if (item.HasMenu == true)
                    bars.Add(item);
            }

            NearBars.NearBarsList = bars;
            if (bars.Count == 0)
            {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "не найдено ни одного бара",replyMarkup: NearBars.ReplyKeyboarBack);
            }
            else if(bars.Count%10==1)
            {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id,"найден "+bars.Count+" бар", replyMarkup: NearBars.ReplyKeyboarBack);

            }
            else if (bars.Count % 10 == 2 || bars.Count % 10 == 3 || bars.Count % 10 == 4 )
            {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "найдено " + bars.Count + " бара", replyMarkup: NearBars.ReplyKeyboarBack);

            }
            else
            {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "найдено " + bars.Count + " баров", replyMarkup: NearBars.ReplyKeyboarBack);

            }
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
