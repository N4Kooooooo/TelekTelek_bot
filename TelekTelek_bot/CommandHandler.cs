using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelekTelek_bot
{
    public static class CommandHandler
    {
        public static bool IsCommand(string message)
        {
            if (message[0]=='/')
            {
                return true;
            }
            return false;
        }
        public static string Handler(string command)
        {
            switch(command)
            {
                case "/start":
                    return "Привет! Здесь ты можешь задать вопрос, а также поделиться историей. \nЕсли хочешь принять участие в конкурсе, то не забудь указать номер выпуска или имя гостя. \nЖдем твоих сообщений 💫";
            }
            return null;
        }
    }
}
