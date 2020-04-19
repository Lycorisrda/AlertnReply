using System;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Helpers;
using ff14bot.Managers;

namespace AlertnReply
{
    public class AlertnReply : BotPlugin
    {

        public override string Author => "Parrot | Lycorisrda";
        public override string Description => "A chat monitor";
        public override Version Version => new Version(2, 0, 0);

        public override string Name => "聊天消息监视 - AlertMe";


        public override void OnEnabled()
        {
            Settings.load();
            Log.Chat.print($"[Date] {DateTime.Now:dd/MM-yy hh:mm}");

            GamelogManager.MessageRecevied += ReceiveMessage;

            if (!init)
            {

                init = true;
                GamelogManager.TellRecevied += TellReceived;
                GamelogManager.GameMasterMessageRecevied += GameMasterMessageReceived;
                GamelogManager.SayRecevied += SayMessageReceived;
                GamelogManager.PartyMessageRecevied += PartyMessageReceived;
                GamelogManager.AllianceMessageRecevied += AllianceMessageReceived;
                GamelogManager.YellRecevied += YellMessageReceived;
                GamelogManager.ShoutRecevied += ShoutMessageReceived;
                GamelogManager.FreeCompanyMessageRecevied += FCMessageReceived;
                GamelogManager.CrossWorldLinkShellMessageRecevied += CWLSMessageReceived;
                GamelogManager.LinkShellMessageRecevied += LSMessageReceived;
                GamelogManager.EmoteRecevied += EmoteMessageReceived;

            }

        }

        public override void OnDisabled()
        {
            GamelogManager.MessageRecevied -= ReceiveMessage;

            if (init)
            {
                init = false;
                GamelogManager.TellRecevied -= TellReceived;
                GamelogManager.GameMasterMessageRecevied -= GameMasterMessageReceived;
                GamelogManager.SayRecevied -= SayMessageReceived;
                GamelogManager.PartyMessageRecevied -= PartyMessageReceived;
                GamelogManager.AllianceMessageRecevied -= AllianceMessageReceived;
                GamelogManager.YellRecevied -= YellMessageReceived;
                GamelogManager.ShoutRecevied -= ShoutMessageReceived;
                GamelogManager.FreeCompanyMessageRecevied -= FCMessageReceived;
                GamelogManager.CrossWorldLinkShellMessageRecevied -= CWLSMessageReceived;
                GamelogManager.LinkShellMessageRecevied -= LSMessageReceived;
                GamelogManager.EmoteRecevied -= EmoteMessageReceived;
            }
        }

        int _count = 0;
        private DateTime _lastMessage;

        protected void ReceiveMessage(object sender, ChatEventArgs e)
        {
            // e.ChatLogEntry.MessageType == (MessageType)2115 && 
            if (e.ChatLogEntry.Contents.Contains("有鱼上钩了") || e.ChatLogEntry.Contents.Contains("收竿停止了钓鱼"))
            {
                Logging.Write(Colors.OrangeRed, "Message - 正常钓鱼中");
                _count = 0;
                Logging.Write("计数清零");
            }
            else  if (e.ChatLogEntry.Contents.Contains("甩出了鱼线开始钓鱼"))
            {
                Logging.Write(Colors.OrangeRed, "Message Received");
                var timeDifference = (DateTime.Now - _lastMessage).TotalSeconds;
                Logging.Write(timeDifference);
                _lastMessage = DateTime.Now;

                if (_count == 0)
                {
                    _count = 1;
                    Logging.Write("计数开始");
                }
                else if (_count != 0 && timeDifference < Settings.current.fishCheck)
                {
                    Logging.Write("上一次甩出鱼线后经过 " + timeDifference + "，小于设定的 " + Settings.current.fishCheck + " 秒");
                    Logging.Write("计数 " + _count);
                    _count++;
                }
                else if (_count == 2)
                {
                    // await Task.Delay(5000);
                    // ChatManager.SendChat("/ac 收竿");
                    _count = 0;
                    Logging.Write("计数清零");
                    SndPlayer.play("pm.wav");
                    TreeRoot.Stop("你可能有暴露的风险，停止RB");
                }
            }
        }

        public override bool WantButton => true;

        public override string ButtonText => "设置";

        public override void OnButtonPress()
        {
            Settings.load();
            try
            {
                var sf = new MainForm();
                sf.Show();
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        private bool init = false;
        private bool Replied = false;
        private DateTime LastReplyTime = new DateTime(2016, 01, 01);
        private DateTime GMLastReplyTime = new DateTime(2016, 01, 01);

        private async void TellReceived(object sender, ChatEventArgs e)
        {
            if (Settings.current.Tell.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                double timeDifference = (DateTime.Now - LastReplyTime).TotalSeconds;

                SndPlayer.play("pm.wav");
                Log.Bot.print("收到了新的私聊消息");
                Log.Chat.printMsg(Log.Chat.Channels.Tell, msg, author);

                if(timeDifference < 300)
                {
                    Log.Bot.print("距离上次消息回复不足5分钟");
                    ChatManager.SendChat("/echo 距离上次消息回复不足5分钟");
                }

                if (Settings.current.Tell.Reply && (timeDifference > 300))
                {
                    ChatManager.SendChat("/echo 5秒后自动回复消息");
                    await Task.Delay(5000);
                    ChatManager.SendChat("/r " + Settings.current.Tell.ReplyText);
                    LastReplyTime = DateTime.Now;
                }
            }
        }

        private async void GameMasterMessageReceived(object sender, ChatEventArgs e)
        {
            if (Settings.current.GM.Enabled)
            {
                double timeDifference = (DateTime.Now - GMLastReplyTime).TotalSeconds;

                SndPlayer.play("gm.wav");
                Log.Bot.print("警告！110查房了！");
                Log.Chat.printMsg(Log.Chat.Channels.GM, e.ChatLogEntry.Contents, e.ChatLogEntry.SenderDisplayName);

                if (timeDifference < 300)
                {
                    Log.Bot.print("距离上次消息回复不足5分钟");
                    ChatManager.SendChat("/echo 距离上次消息回复不足5分钟");
                }

                if (Settings.current.GM.Reply && (timeDifference > 300))
                {
                    ChatManager.SendChat("/echo 5秒后自动回复消息");
                    await Task.Delay(5000);
                    ChatManager.SendChat("/r " + Settings.current.GM.ReplyText);
                    GMLastReplyTime = DateTime.Now;
                }
            }
        }

        private async void SayMessageReceived(object sender, ff14bot.Managers.ChatEventArgs e)
        {
            if (Settings.current.Say.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.current.Say, msg);
                    if (match)
                    {
                        SndPlayer.play("chat.wav");
                        Log.Bot.print("收到来自[说话]的消息");

                        if (Settings.current.Say.Reply && !Replied)
                        {
                            await Task.Delay(5000);
                            ChatManager.SendChat("/p " + Settings.current.Say.ReplyText);
                            Replied = true;
                            await Task.Delay(300000);
                            Replied = false;
                        }
                    }
                }
                if (match || Settings.current.chatLog.LogAll)
                {
                    Log.Chat.printMsg(Log.Chat.Channels.Say, msg, author);
                }
            }
        }

        private async void PartyMessageReceived(object sender, ff14bot.Managers.ChatEventArgs e)
        {
            if (Settings.current.Party.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.current.Party, msg);
                    if (match)
                    {
                        SndPlayer.play("chat.wav");
                        Log.Bot.print("收到来自[小队]的消息");

                        if (Settings.current.Party.Reply && !Replied)
                        {
                            await Task.Delay(5000);
                            ChatManager.SendChat("/p " + Settings.current.Party.ReplyText);
                            Replied = true;
                            await Task.Delay(300000);
                            Replied = false;
                        }
                    }
                }
                if (match || Settings.current.chatLog.LogAll)
                {
                    Log.Chat.printMsg(Log.Chat.Channels.Party, msg, author);
                }
            }
        }

        private async void AllianceMessageReceived(object sender, ff14bot.Managers.ChatEventArgs e)
        {
            if (Settings.current.Alliance.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.current.Alliance, msg);
                    if (match)
                    {
                        SndPlayer.play("chat.wav");
                        Log.Bot.print("收到来自[团队]的消息");

                        if (Settings.current.Alliance.Reply && !Replied)
                        {
                            await Task.Delay(5000);
                            ChatManager.SendChat("/p " + Settings.current.Alliance.ReplyText);
                            Replied = true;
                            await Task.Delay(300000);
                            Replied = false;
                        }
                    }
                }
                if (match || Settings.current.chatLog.LogAll)
                {
                    Log.Chat.printMsg(Log.Chat.Channels.Alliance, msg, author);
                }
            }
        }

        private async void YellMessageReceived(object sender, ff14bot.Managers.ChatEventArgs e)
        {
            if (Settings.current.Yell.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.current.Yell, msg);
                    if (match)
                    {
                        SndPlayer.play("chat.wav");
                        Log.Bot.print("收到来自[呼喊]的消息");

                        if (Settings.current.Yell.Reply && !Replied)
                        {
                            await Task.Delay(5000);
                            ChatManager.SendChat("/p " + Settings.current.Yell.ReplyText);
                            Replied = true;
                            await Task.Delay(300000);
                            Replied = false;
                        }
                    }
                }
                if (match || Settings.current.chatLog.LogAll)
                {
                    Log.Chat.printMsg(Log.Chat.Channels.Yell, msg, author);
                }
            }
        }

        private async void ShoutMessageReceived(object sender, ff14bot.Managers.ChatEventArgs e)
        {
            if (Settings.current.Shout.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.current.Shout, msg);
                    if (match)
                    {
                        SndPlayer.play("chat.wav");
                        Log.Bot.print("收到来自[喊话]的消息");

                        if (Settings.current.Shout.Reply && !Replied)
                        {
                            await Task.Delay(5000);
                            ChatManager.SendChat("/p " + Settings.current.Shout.ReplyText);
                            Replied = true;
                            await Task.Delay(300000);
                            Replied = false;
                        }
                    }
                }
                if (match || Settings.current.chatLog.LogAll)
                {
                    Log.Chat.printMsg(Log.Chat.Channels.Shout, msg, author);
                }
            }
        }

        private async void FCMessageReceived(object sender, ff14bot.Managers.ChatEventArgs e)
        {
            if (Settings.current.FC.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.current.FC, msg);
                    if (match)
                    {
                        SndPlayer.play("chat.wav");
                        Log.Bot.print("收到来自[部队]的消息");

                        if (Settings.current.FC.Reply && !Replied)
                        {
                            await Task.Delay(5000);
                            ChatManager.SendChat("/p " + Settings.current.FC.ReplyText);
                            Replied = true;
                            await Task.Delay(300000);
                            Replied = false;
                        }
                    }
                }
                if (match || Settings.current.chatLog.LogAll)
                {
                    Log.Chat.printMsg(Log.Chat.Channels.FC, msg, author);
                }
            }
        }

        private async void CWLSMessageReceived(object sender, ff14bot.Managers.ChatEventArgs e)
        {
            if (Settings.current.CWLS.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.current.CWLS, msg);
                    if (match)
                    {
                        SndPlayer.play("chat.wav");
                        Log.Bot.print("收到来自[跨服贝]的消息");

                        if (Settings.current.CWLS.Reply && !Replied)
                        {
                            await Task.Delay(5000);
                            ChatManager.SendChat("/p " + Settings.current.CWLS.ReplyText);
                            Replied = true;
                            await Task.Delay(300000);
                            Replied = false;
                        }
                    }
                }
                if (match || Settings.current.chatLog.LogAll)
                {
                    Log.Chat.printMsg(Log.Chat.Channels.CWLS, msg, author);
                }
            }
        }

        private async void LSMessageReceived(object sender, ff14bot.Managers.ChatEventArgs e)
        {
            if (Settings.current.LS.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.current.LS, msg);
                    if (match)
                    {
                        SndPlayer.play("chat.wav");
                        Log.Bot.print("收到来自[通讯贝]的消息");

                        if (Settings.current.LS.Reply && !Replied)
                        {
                            await Task.Delay(5000);
                            ChatManager.SendChat("/p " + Settings.current.LS.ReplyText);
                            Replied = true;
                            await Task.Delay(300000);
                            Replied = false;
                        }
                    }
                }
                if (match || Settings.current.chatLog.LogAll)
                {
                    Log.Chat.printMsg(Log.Chat.Channels.LS, msg, author);
                }
            }
        }

        private async void EmoteMessageReceived(object sender, ff14bot.Managers.ChatEventArgs e)
        {
            if (Settings.current.Emote.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.current.Emote, msg);
                    if (match)
                    {
                        SndPlayer.play("emote.wav");
                        Log.Bot.print("收到来自[情感动作]的消息");

                        if (Settings.current.Emote.Reply && !Replied)
                        {
                            await Task.Delay(5000);
                            ChatManager.SendChat("/p " + Settings.current.Emote.ReplyText);
                            Replied = true;
                            await Task.Delay(300000);
                            Replied = false;
                        }
                    }
                }
                if (match || Settings.current.chatLog.LogAll)
                {
                    Log.Chat.printMsg(Log.Chat.Channels.Emote, msg, author);
                }
            }
        }

        private bool msgMatchesCrit(Settings.ChatChannel cc, string msg)
        {
            if (!cc.UseKeywords
                && !cc.UseRegex)
            {
                return true;
            }

            if (cc.UseKeywords)
            {
                if (stringContainsKeywords(msg, cc.keywords))
                    return true;
            }

            if (cc.UseRegex)
            {
                Regex r = new Regex(cc.Regex, RegexOptions.IgnoreCase);
                if (r.Match(msg).Success)
                {
                    return true;
                }
            }
            return false;
        }

        private bool stringContainsKeywords(string msg, string[] keywords)
        {
            if (keywords == null || keywords.Length == 0)
                return false;
            for (int i = 0; i < keywords.Length; ++i)
            {
                if (msg.ToLower().Contains(keywords[i].ToLower()))
                    return true;
            }
            return false;
        }

        private bool authorCheck(string author)
        {
            if (Settings.current.ignoreSelf)
                return !isAuthorMe(author);
            else
                return true;
        }

        private bool isAuthorMe(string author)
        {
            return string.IsNullOrEmpty(author);
        }

        private static class SndPlayer
        {

            public static void play(string fileName)
            {
                if (!Settings.current.sound)
                    return;

                var fullPath = System.Windows.Forms.Application.StartupPath + @"\Plugins\AlertnReply\Sounds\" + fileName;
                try
                {
                    SoundPlayer sp = new SoundPlayer();
                    sp.SoundLocation = fullPath;
                    sp.Play();

                }
                catch (Exception e)
                {
                    Log.Bot.print("Error: " + e.Message);
                    beep();
                }
            }

            public static void beep()
            {
                if (!Settings.current.sound)
                    return;
                try
                {
                    SystemSounds.Beep.Play();
                }
                catch (Exception ee)
                {
                    Log.Bot.print("Error: Could not play system sound \"beep\"\n" + ee.Message);
                }
            }
        }
    }
}
