using System;
using System.Globalization;
using System.Threading;
using System.Windows.Media;
using ff14bot.Helpers;

namespace AlertnReply
{
    static class Log
    {
        public static class Bot
        {
            public static void print(string input, Color col)
            {
                Logging.Write(col, string.Format("[AlertnReply] {0}", input));
            }

            public static void print(string input)
            {
                print(input, Colors.Red);
            }
        }

        public static class Chat
        {
            public enum Channels { Tell, GM, Say, Party, Alliance, Yell, Shout, FC, CWLS, LS, Emote}
            public static readonly string Filepath = (System.Windows.Forms.Application.StartupPath + @"\plugins\AlertnReply\ChatLog.txt");
            public static void print(String input)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Filepath, true))
                {
                    file.WriteLine(input);
                }

            }
            public static void printMsg(Channels chn, string msg, string auth)
            {                
                Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
                var timestamp = DateTime.Now.ToString("hh:mm:ss");

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Filepath, true))
                {
                    file.WriteLine(string.Format("[{0,-9}]{1,-7} From: {2}", timestamp,'['+chn.ToString()+']', auth));
                    file.WriteLine(string.Format("{0,27}{1}\r\n", "",msg));
                }

            }

            public static void clear()
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Filepath))
                {
                    file.Write("");
                }
            }
        }
    }
}