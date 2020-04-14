using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using ApiAiSDK;

namespace MyTelegramBot
{
    
    class Program
    {
        static public ITelegramBotClient botClient;
        static public ApiAi apiAi;

        static void Main(string[] args)
        {
            botClient = new TelegramBotClient("1103121030:AAE9c0BIgGlLYkqRnzh1nIa-9-KztN7ZKf4");
            AIConfiguration config = new AIConfiguration("52cd2139c263414ba3936550d3dd2d49", SupportedLanguage.Russian);
            apiAi = new ApiAi(config);

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id} an user name {e.Message.Chat.Username}.");
                
                switch (e.Message.Text)
                {
                    case "/start":
                        string s = "Привет!";
                        await SendMessage(e.Message.Chat, s);
                        break;
                    default:
                         var response = apiAi.TextRequest(e.Message.Text);
                         string answer = response.Result.Fulfillment.Speech;
                            if (answer == "")
                                answer = "Sorry,я тебя не понял.\nПовтори.";
                          await SendMessage(e.Message.Chat, answer);
                        break;
                }

               // await SendMessage(e.Message.Chat, e.Message.Text);
            }
        }
        static async Task SendMessage(Chat chatId,string message)
        {
            await botClient.SendTextMessageAsync(
                 chatId: chatId,
                 text: message
               );
        }

    }
}
