using System.Windows;
using System.Windows.Documents;
using System.Media;
using System.IO;

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
    try
    {
        string path = @"C:\Users\Student\Documents\CyberSecurityChatbot\CyberSecurityAwarenessBotGUI\CyberSecurityAwarenessBotGUI\greeting.wav";

        if (System.IO.File.Exists(path))
        {
            SoundPlayer player = new SoundPlayer(path);
            player.Load();      // 👈 IMPORTANT
            player.PlaySync();  // 👈 Better for testing
        }
        else
        {
            MessageBox.Show("File not found!");
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error: " + ex.Message);
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