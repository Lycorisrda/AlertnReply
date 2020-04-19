using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Serialization;
using ff14bot;

namespace AlertnReply
{
    public static class Settings
    {
        public static Profile current = new Profile();
        public static bool save()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Profile));
                TextWriter textWriter = new StreamWriter(Application.StartupPath + @"\Plugins\AlertnReply\Settings.xml");
                serializer.Serialize(textWriter, current);
                textWriter.Close();
            }
            catch (Exception e)
            {
                Log.Bot.print("保存设置失败 :\n" + e.Message);
                return false;
            }
            Log.Bot.print("设置.", Colors.White);
            return true;
        }

        public static bool load()
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Profile));
                TextReader textReader = new StreamReader(Application.StartupPath + @"\Plugins\AlertnReply\Settings.xml");
                current = (Profile)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            catch (Exception e)
            {
                Log.Bot.print("读取设置失败 :\n" + e.Message);
                current = new Profile();
                return false;
            }

            Log.Bot.print("已读取设置", Colors.White);
            return true;

        }

        public class Profile
        {
            //pm and gm ChatChannel only use the enabled attrib atm.
            public ChatChannel Tell;
            public ChatChannel GM;
            public ChatChannel Say;
            public ChatChannel Party;
            public ChatChannel Alliance;
            public ChatChannel Yell;
            public ChatChannel Shout;
            public ChatChannel FC;
            public ChatChannel CWLS;
            public ChatChannel LS;
            public ChatChannel Emote;
            public ChatLog chatLog;
            public bool sound;
            public bool ignoreSelf;
            public double fishCheck;
            
            public Profile()
            {
                Tell = new ChatChannel();
                GM = new ChatChannel();
                Say = new ChatChannel();
                Party = new ChatChannel();
                Alliance = new ChatChannel();
                Yell = new ChatChannel();
                Shout = new ChatChannel();
                FC = new ChatChannel();
                CWLS = new ChatChannel();
                LS = new ChatChannel();
                Emote = new ChatChannel();

                Tell.Enabled = true;
                GM.Enabled = true;
                Emote.keywords = new string[] { Core.Player.Name.ToString() };
                Emote.UseKeywords = true;
                ignoreSelf = true;
                sound = true;
                chatLog = new ChatLog();
            }
        }

        public class ChatChannel
        {
            public ChatChannel()
            {
                Enabled = false;
                UseRegex = false;
                UseKeywords = false;
                Reply = false;
                ReplyText = string.Empty;
                Regex = string.Empty;
                keywords = new string[0];
            }
            public bool Enabled;
            public bool UseRegex;
            public bool UseKeywords;
            public bool Reply;
            public string Regex;
            public string ReplyText;
            public string[] keywords;
        }

        public class ChatLog 
        {
            public bool Enabled = true;
            public bool LogAll = true; //log all messages on monitored channels.
        }
    }
}
