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

            PlayGreeting();

            AppendText("Bot: Welcome to the Cybersecurity Awareness Bot!\n\n");
        }
        private void PlayGreeting()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");

                if (File.Exists(path))
                {
                    SoundPlayer player = new SoundPlayer(path);
                    player.Play();
                }
            }
            catch
            {
                AppendText("Bot: (Audio greeting could not be played)\n");
            }
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