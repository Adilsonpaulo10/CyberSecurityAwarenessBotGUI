using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Documents;

namespace CyberSecurityAwarenessBotGUI
{
    public partial class MainWindow : Window
    {
        private ChatbotEngine bot;

        public MainWindow()
        {
            InitializeComponent();

            bot = new ChatbotEngine();

            ShowAsciiLogo();
            PlayGreeting();    

            AppendText("Bot: Welcome to the Cybersecurity Awareness Bot!\n\n");
        }
        private void PlayGreeting()

        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try // the wav sound will play
                {
                    SoundPlayer player = new SoundPlayer(@"C:\Users\Student\Documents\CyberSecurityChatbot\CyberSecurityAwarenessBotGUI\CyberSecurityAwarenessBotGUI\greeting.wav");
                    player.PlaySync();
                }
                catch
                {
                    Console.WriteLine("Voice greeting could not be played."); // but if there is an error this message will display
                }
            }
        }
        private void ShowAsciiLogo()
        {
            string logo =
        @"   _____      _                _____            
  / ____|    | |              |  __ \           
 | |     __ _| |__   ___ _ __ | |__) |___  ___  
 | |    / _` | '_ \ / _ \ '_ \|  _  // _ \/ __| 
 | |___| (_| | |_) |  __/ | | | | \ \  __/\__ \ 
  \_____\__,_|_.__/ \___|_| |_|_|  \_\___||___/ 
        CYBERSECURITY AWARENESS BOT 🔒
------------------------------------------------";

            AppendText(logo + "\n\n");
        }

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            string input = inputBox.Text;

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Please enter a message.");
                return;
            }

            AppendText("You: " + input + "\n");

            string response = bot.GetResponse(input);

            AppendText("Bot: " + response + "\n\n");

            inputBox.Clear();
        }

        // Helper method to write into RichTextBox
        private void AppendText(string text)
        {
            chatBox.Document.Blocks.Add(new Paragraph(new Run(text)));
            chatBox.ScrollToEnd();
        }
    }
}