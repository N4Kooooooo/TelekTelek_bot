using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelekTelek_bot
{
    public class Bot
    {
        private static TelegramBotClient bot = new TelegramBotClient("7039845592:AAG4Pv6Schh2lrZqLYZxnnemNJAn-3dzgcE");
        private const int adminChatId = 247389766;
        private ManualResetEvent resetEvent = new ManualResetEvent(false);

        public void Run()
        {
            Console.WriteLine("[BOT]: Бот запущен!\n-----------------------------------");

            bot.StartReceiving(HandlerUpdateAsycn, HandlerErrorAsync);

            resetEvent.WaitOne();

            Console.WriteLine("-----------------------------------\n[BOT]: Бот приостановлен!");
        }

        private Task HandlerErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine($"[EXCEPTION]: {exception.Message}");

            resetEvent.Reset();

            return null;
        }

        private async Task HandlerUpdateAsycn(ITelegramBotClient client, Update update, CancellationToken token)
        {
            Message message = update.Message;

            string username = message.Chat.Username;
            string messagetext = message.Text;
            string answer = null;

            if (messagetext != null)
            {
                Console.WriteLine($"[ID]: {username} - {message.Chat.Id}");
                await Console.Out.WriteLineAsync($"[{username}]: {messagetext}");
                if (message.Chat.Id != adminChatId)
                {
                    await SendTextAsync(client, adminChatId, $"@{username}:\n{messagetext}");
                }
                if (CommandHandler.IsCommand(messagetext))
                {
                    answer = CommandHandler.Handler(messagetext);
                    if (answer != null)
                    {
                        await SendTextAsync(client, message, answer);
                    }
                }
                else
                {
                    await SendTextAsync(client, message, "Информацию получили. Спасибо 🌺");
                }
            }
        }

        private async Task SendTextAsync(ITelegramBotClient client, Message message, string text)
        {
            await client.SendTextMessageAsync(message.Chat.Id, text);
        }

        private async Task SendTextAsync(ITelegramBotClient client, int chatId, string text)
        {
            await client.SendTextMessageAsync(chatId, text);
        }
    }
}
