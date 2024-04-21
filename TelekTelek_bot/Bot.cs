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
        private static TelegramBotClient bot = new TelegramBotClient("6455289067:AAFtwh7Fvwx-edlJoqWgPr9yyBE8WClAYNU");
        // Мой токен - 6455289067:AAFtwh7Fvwx-edlJoqWgPr9yyBE8WClAYNU
        // Токен Алины - 7039845592:AAG4Pv6Schh2lrZqLYZxnnemNJAn-3dzgcE
        private const long alinaid = 1327824167; // Алина - 247389766, Я - 1327824167, Марк - 5155086541
        private const long markid = 5155086541;
        private const long vladid = 1327824167;

        private List<long> sysadminsid = new List<long>() { markid, vladid };
        private Dictionary<int, string> admins = new Dictionary<int, string>()
        {
            { (int)alinaid, "@AlinaRomashova" },
            //{ (int)markid.ToString(), "@markshapovalov" },
            { (int)vladid, "@N4Koooo" }
        };

        private ManualResetEvent resetEvent = new ManualResetEvent(false);

        public void Run()
        {
            Console.WriteLine("[BOT]: Бот запущен!\n-----------------------------------");

            bot.StartReceiving(HandlerUpdateAsync, HandlerErrorAsync);

            resetEvent.WaitOne();

            Console.WriteLine("-----------------------------------\n[BOT]: Бот приостановлен!");
        }

        private async Task HandlerErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine($"[EXCEPTION]: {exception.Message}");

            await AdminsMailing(client, $"[EXCEPTION]: {exception.Message}");
            
            resetEvent.Reset();
        }

        private async Task HandlerUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            Message message = update.Message;

            if (message!=null)
            {
                string username = message.Chat.Username;
                if (username==null)
                {
                    username = "NAMENULL-"+message.Chat.Id.ToString();
                }

                string messagetext = message.Text;

                if (messagetext != null)
                {
                    string result = $"[{username} - {message.Chat.Id}]: {messagetext}";
                    string answer = "";
                    if (CommandHandler.IsCommand(messagetext))
                    {
                        result = CommandHandler.Handler(messagetext);
                        if (result != null)
                        {
                            if (result == "adminslist")
                            {

                            }
                            else
                            {
                                answer = result;
                                await SendTextAsync(client, (int)message.Chat.Id, answer);
                            }
                        }
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync($"[@{username} - {message.Chat.Id}]: {messagetext}");

                        await SendTextAdminAsync(client, (int)alinaid, $"@{username}:\n{messagetext}",'a');

                        if (message.Chat.Id!=vladid && message.Chat.Id!=markid)
                        {
                            await SendTextAsync(client, (int)message.Chat.Id, "Информацию получили. Спасибо 🌺");
                        }
                    }
                }
            }
        }

        private async Task SendTextAsync(ITelegramBotClient client, int chatId, string text)
        {
            await client.SendTextMessageAsync(chatId, text);
        }

        private async Task SendTextAdminAsync(ITelegramBotClient client, int adminid, string text, char a)
        {
            try
            {
                await SendTextAsync(client, (int)adminid, text);
            }
            catch (Exception)
            {
                await AdminsMailing(client, $"Админ {adminid} не существует!");
                await AdminsMailing(client, text);
            }
        }
        private async Task SendTextAdminAsync(ITelegramBotClient client, int adminid, string text)
        {
            try
            {
                await SendTextAsync(client, (int)adminid, text);
            }
            catch (Exception)
            {
                sysadminsid.Remove(adminid);
                await AdminsMailing(client, $"Админ {adminid} не существует!");
                await AdminsMailing(client, text);
            }
        }
        private async Task AdminsMailing(ITelegramBotClient client, string text)
        {
            if (sysadminsid != null)
            {
                foreach (var adminid in sysadminsid)
                {
                    await SendTextAdminAsync(client, (int)adminid, text);
                }
            }
            else
            {
                await Console.Out.WriteLineAsync($"[BOT]: Список админов пуст!");
            }
        }
    }
}
