using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static AlertnReply.Utils;

namespace AlertnReply
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            FillForm();
        }

        private void FillForm()
        {
            try
            {
                txtLog.Text = File.ReadAllText(Log.Chat.Filepath);
                cbIgnoreSelf.Checked = Settings.current.ignoreSelf;
                cbSound.Checked = Settings.current.sound;
                cbLogAllChats.Checked = Settings.current.chatLog.LogAll;

                // Channels
                cbTell.Checked = Settings.current.Tell.Enabled;
                cbKeyTell.Checked = Settings.current.Tell.UseKeywords;
                if (Settings.current.Tell.keywords.Any()) tbKeyTell.Text = string.Join(",", Settings.current.Tell.keywords);
                cbRegexTell.Checked = Settings.current.Tell.UseRegex;
                if (!string.IsNullOrEmpty(Settings.current.Tell.Regex)) tbRegexTell.Text = Settings.current.Tell.Regex;

                cbGM.Checked = Settings.current.GM.Enabled;
                cbKeyGM.Checked = Settings.current.GM.UseKeywords;
                if (Settings.current.GM.keywords.Any()) tbKeyGM.Text = string.Join(",", Settings.current.GM.keywords);
                cbRegexGM.Checked = Settings.current.GM.UseRegex;
                if (!string.IsNullOrEmpty(Settings.current.GM.Regex)) tbRegexGM.Text = Settings.current.GM.Regex;

                cbSay.Checked = Settings.current.Say.Enabled;
                cbKeySay.Checked = Settings.current.Say.UseKeywords;
                if (Settings.current.Say.keywords.Any()) tbKeySay.Text = string.Join(",", Settings.current.Say.keywords);
                cbRegexSay.Checked = Settings.current.Say.UseRegex;
                if (!string.IsNullOrEmpty(Settings.current.Say.Regex)) tbRegexSay.Text = Settings.current.Say.Regex;

                cbParty.Checked = Settings.current.Party.Enabled;
                cbKeyParty.Checked = Settings.current.Party.UseKeywords;
                if (Settings.current.Party.keywords.Any()) tbKeyParty.Text = string.Join(",", Settings.current.Party.keywords);
                cbRegexParty.Checked = Settings.current.Party.UseRegex;
                if (!string.IsNullOrEmpty(Settings.current.Party.Regex)) tbRegexParty.Text = Settings.current.Party.Regex;

                cbAlliance.Checked = Settings.current.Alliance.Enabled;
                cbKeyAlliance.Checked = Settings.current.Alliance.UseKeywords;
                if (Settings.current.Alliance.keywords.Any()) tbKeyAlliance.Text = string.Join(",", Settings.current.Alliance.keywords);
                cbRegexAlliance.Checked = Settings.current.Alliance.UseRegex;
                if (!string.IsNullOrEmpty(Settings.current.Alliance.Regex)) tbRegexAlliance.Text = Settings.current.Alliance.Regex;

                cbYell.Checked = Settings.current.Yell.Enabled;
                cbKeyYell.Checked = Settings.current.Yell.UseKeywords;
                if (Settings.current.Yell.keywords.Any()) tbKeyYell.Text = string.Join(",", Settings.current.Yell.keywords);
                cbRegexYell.Checked = Settings.current.Yell.UseRegex;
                if (!string.IsNullOrEmpty(Settings.current.Yell.Regex)) tbRegexYell.Text = Settings.current.Yell.Regex;

                cbShout.Checked = Settings.current.Shout.Enabled;
                cbKeyShout.Checked = Settings.current.Shout.UseKeywords;
                if (Settings.current.Shout.keywords.Any()) tbKeyShout.Text = string.Join(",", Settings.current.Shout.keywords);
                cbRegexShout.Checked = Settings.current.Shout.UseRegex;
                if (!string.IsNullOrEmpty(Settings.current.Shout.Regex)) tbRegexShout.Text = Settings.current.Shout.Regex;

                cbFC.Checked = Settings.current.FC.Enabled;
                cbKeyFC.Checked = Settings.current.FC.UseKeywords;
                if (Settings.current.FC.keywords.Any()) tbKeyFC.Text = string.Join(",", Settings.current.FC.keywords);
                cbRegexFC.Checked = Settings.current.FC.UseRegex;
                if (!string.IsNullOrEmpty(Settings.current.FC.Regex)) tbRegexFC.Text = Settings.current.FC.Regex;

                cbCWLS.Checked = Settings.current.CWLS.Enabled;
                cbKeyCWLS.Checked = Settings.current.CWLS.UseKeywords;
                if (Settings.current.CWLS.keywords.Any()) tbKeyCWLS.Text = string.Join(",", Settings.current.CWLS.keywords);
                cbRegexCWLS.Checked = Settings.current.CWLS.UseRegex;
                if (!string.IsNullOrEmpty(Settings.current.CWLS.Regex)) tbRegexCWLS.Text = Settings.current.CWLS.Regex;

                cbLS.Checked = Settings.current.LS.Enabled;
                cbKeyLS.Checked = Settings.current.LS.UseKeywords;
                if (Settings.current.LS.keywords.Any()) tbKeyLS.Text = string.Join(",", Settings.current.LS.keywords);
                cbRegexLS.Checked = Settings.current.LS.UseRegex;
                if (!string.IsNullOrEmpty(Settings.current.LS.Regex)) tbRegexLS.Text = Settings.current.LS.Regex;

                cbEmote.Checked = Settings.current.Emote.Enabled;
                cbKeyEmote.Checked = Settings.current.Emote.UseKeywords;
                if (Settings.current.Emote.keywords.Any()) tbKeyEmote.Text = string.Join(",", Settings.current.Emote.keywords);
                cbRegexEmote.Checked = Settings.current.Emote.UseRegex;
                if (!string.IsNullOrEmpty(Settings.current.Emote.Regex)) tbRegexEmote.Text = Settings.current.Emote.Regex;

                // Reply

                cbAutoReplyTell.Checked = Settings.current.Tell.Reply;
                cbAutoReplyGM.Checked = Settings.current.GM.Reply;
                cbAutoReplySay.Checked = Settings.current.Say.Reply;
                cbAutoReplyParty.Checked = Settings.current.Party.Reply;
                cbAutoReplyAlliance.Checked = Settings.current.Alliance.Reply;
                cbAutoReplyYell.Checked = Settings.current.Yell.Reply;
                cbAutoReplyShout.Checked = Settings.current.Shout.Reply;
                cbAutoReplyFC.Checked = Settings.current.FC.Reply;
                cbAutoReplyCWLS.Checked = Settings.current.CWLS.Reply;
                cbAutoReplyLS.Checked = Settings.current.LS.Reply;
                cbAutoReplyEmote.Checked = Settings.current.Emote.Reply;

                tbAutoReplyTell.Text = Settings.current.Tell.ReplyText;
                tbAutoReplyGM.Text = Settings.current.GM.ReplyText;
                tbAutoReplySay.Text = Settings.current.Say.ReplyText;
                tbAutoReplyParty.Text = Settings.current.Party.ReplyText;
                tbAutoReplyAlliance.Text = Settings.current.Alliance.ReplyText;
                tbAutoReplyYell.Text = Settings.current.Yell.ReplyText;
                tbAutoReplyShout.Text = Settings.current.Shout.ReplyText;
                tbAutoReplyFC.Text = Settings.current.FC.ReplyText;
                tbAutoReplyCWLS.Text = Settings.current.CWLS.ReplyText;
                tbAutoReplyLS.Text = Settings.current.LS.ReplyText;
                tbAutoReplyEmote.Text = Settings.current.Emote.ReplyText;


                fishCheck.Value = Convert.ToDecimal(Settings.current.fishCheck);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ClearLog(object sender, EventArgs e)
        {
            Log.Chat.clear();
            txtLog.Text = File.ReadAllText(Log.Chat.Filepath);
        }

        private Settings.ChatChannel buildChatChannel(bool enabled, bool useKeywords, bool useRegex, string keywords,
            string regex, bool Reply, string ReplyTxt)
        {
            var cc = new Settings.ChatChannel();
            cc.Enabled = enabled;

            if (!string.IsNullOrEmpty(keywords))
                cc.keywords = keywords.Split(',');


            if (useKeywords)
                cc.UseKeywords = true;
            else
                cc.UseKeywords = false;

            if (useRegex)
            {
                var pattern = regex;
                if (isValidRegex(pattern))
                {
                    cc.UseRegex = true;
                    cc.Regex = pattern;
                }
                else
                {
                    MessageBox.Show("Regex is invalid: \r\n" + pattern);
                    cc.Regex = pattern; //Save pattern anyway.
                    cc.UseRegex = false;
                }
            }
            else
            {
                cc.Regex = regex;
                cc.UseRegex = false;
            }

            if (!string.IsNullOrEmpty(ReplyTxt))
                cc.ReplyText = ReplyTxt;

            cc.Reply = Reply;
            return cc;
        }

        private void ReloadLog (object sender, EventArgs e)
        {
            txtLog.Text = File.ReadAllText(Log.Chat.Filepath);
        }

        private void SaveConfig(object sender, EventArgs e)
        {

            var profile = new Settings.Profile();
            profile.sound = cbSound.Checked;
            profile.ignoreSelf = cbIgnoreSelf.Checked;
            profile.chatLog.LogAll = cbLogAllChats.Checked;
            profile.Tell = buildChatChannel(cbTell.Checked, cbKeyTell.Checked, cbRegexTell.Checked, tbKeyTell.Text, tbRegexTell.Text, cbAutoReplyTell.Checked, tbAutoReplyTell.Text);
            profile.GM = buildChatChannel(cbGM.Checked, cbKeyGM.Checked, cbRegexGM.Checked, tbKeyGM.Text, tbRegexGM.Text, cbAutoReplyGM.Checked, tbAutoReplyGM.Text);
            profile.Say = buildChatChannel(cbSay.Checked, cbKeySay.Checked, cbRegexSay.Checked, tbKeySay.Text, tbRegexSay.Text, cbAutoReplySay.Checked, tbAutoReplySay.Text);
            profile.Party = buildChatChannel(cbParty.Checked, cbKeyParty.Checked, cbRegexParty.Checked, tbKeyParty.Text, tbRegexParty.Text, cbAutoReplyParty.Checked, tbAutoReplyParty.Text);
            profile.Alliance = buildChatChannel(cbAlliance.Checked, cbKeyAlliance.Checked, cbRegexAlliance.Checked, tbKeyAlliance.Text, tbRegexAlliance.Text, cbAutoReplyAlliance.Checked, tbAutoReplyAlliance.Text);
            profile.Yell = buildChatChannel(cbYell.Checked, cbKeyYell.Checked, cbRegexYell.Checked, tbKeyYell.Text, tbRegexYell.Text, cbAutoReplyYell.Checked, tbAutoReplyYell.Text);
            profile.Shout = buildChatChannel(cbShout.Checked, cbKeyShout.Checked, cbRegexShout.Checked, tbKeyShout.Text, tbRegexShout.Text, cbAutoReplyShout.Checked, tbAutoReplyShout.Text);
            profile.FC = buildChatChannel(cbFC.Checked, cbKeyFC.Checked, cbRegexFC.Checked, tbKeyFC.Text, tbRegexFC.Text, cbAutoReplyFC.Checked, tbAutoReplyFC.Text);
            profile.CWLS = buildChatChannel(cbCWLS.Checked, cbKeyCWLS.Checked, cbRegexCWLS.Checked, tbKeyCWLS.Text, tbRegexCWLS.Text, cbAutoReplyCWLS.Checked, tbAutoReplyCWLS.Text);
            profile.LS = buildChatChannel(cbLS.Checked, cbKeyLS.Checked, cbRegexLS.Checked, tbKeyLS.Text, tbRegexLS.Text, cbAutoReplyLS.Checked, tbAutoReplyLS.Text);
            profile.Emote = buildChatChannel(cbEmote.Checked, cbKeyEmote.Checked, cbRegexEmote.Checked, tbKeyEmote.Text, tbRegexEmote.Text, cbAutoReplyEmote.Checked, tbAutoReplyEmote.Text);

            profile.fishCheck = Convert.ToDouble(fishCheck.Value);
            Settings.current = profile;
            Settings.save();
            FillForm();
        }
    }
}
