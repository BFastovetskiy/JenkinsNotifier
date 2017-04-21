using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsNotifier
{
    using System.IO;
    using System.Net;

    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                WriteHelpMessage();
                return 0;
            }

            if (args.Length < 3)
            {
                WriteErrorMessage("Error: Required parameters are not specified");
                WriteHelpMessage();
                return 1;
            }

            var m_botId = args.SingleOrDefault(s => s.ToLower().Contains("-bid"));
            if (string.IsNullOrEmpty(m_botId))
            {
                WriteErrorMessage("Error: Missing parameter Bot Id");
                WriteHelpMessage();
                return 1;
            }
            m_botId = m_botId.Split('=')[1];

            var m_chatId = args.SingleOrDefault(s => s.ToLower().Contains("-cid"));
            if (string.IsNullOrEmpty(m_chatId))
            {
                WriteErrorMessage("Error: Missing parameter Chat Id");
                WriteHelpMessage();
                return 1;
            }
            m_chatId = m_chatId.Split('=')[1];

            var m_message = args.SingleOrDefault(s => s.ToLower().Contains("-m"));
            if (string.IsNullOrEmpty(m_message))
            {
                WriteErrorMessage("Error: Missing parameter Message text");
                WriteHelpMessage();
                return 1;
            }
            m_message = m_message.Split('=')[1];

            string telegramURI = string.Format("https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}",
                m_botId,
                m_chatId,
                m_message);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(telegramURI);
            request.Method = "GET";
            request.Accept = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            StringBuilder output = new StringBuilder();
            WriteInfoMessage(reader.ReadToEnd());
            return 0;
        }

        static void WriteErrorMessage(string message)
        {
            var def = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = def;

        }
        static void WriteWarningsMessage(string message)
        {
            var def = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = def;
        }
        static void WriteInfoMessage(string message)
        {
            var def = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = def;

        }
        static void WriteHelpMessage()
        {
            System.Console.WriteLine("The utility for sending messages in a Telegram Messenger by a bot. Options for sending a message.");
            System.Console.WriteLine("The text of the message is automatically encoded in the html format.");
            System.Console.WriteLine("Bot Id:\t\t-bid=<123456789:abcdf...>");
            System.Console.WriteLine("Chat Id:\t-cid=<123456789>");
            System.Console.WriteLine("Message text:\t-m=message_text");
            System.Console.WriteLine();
            WriteInfoMessage("Example:");
            System.Console.WriteLine("JenkinsNotifier -bid=123456789:AAHY4V2Mz1WsP_MRrvIeyOQn7Zu5yhikChk -cid=-123456789 -m=message_text");
        }
    }
}
