using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CyberSecurityAwarenessBotGUI
{
    public partial class MainWindow : Window
    {
        private ChatbotEngine bot;
        private DatabaseHelper db;

        public MainWindow()
        {
            InitializeComponent();

            bot = new ChatbotEngine();
            db = new DatabaseHelper();

            AppendText("Bot: Welcome to the Cybersecurity Awareness Bot!\n\n");

            LoadTasks();
            LoadLogs();

            SetupQuiz();
        }

        // =========================
        // CHATBOT
        // =========================
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

            db.LogActivity("User asked chatbot: " + input);

            LoadLogs();

            inputBox.Clear();
        }

        private void AppendText(string text)
        {
            chatBox.Document.Blocks.Add(
                new Paragraph(new Run(text))
            );

            chatBox.ScrollToEnd();
        }

        // =========================
        // TASKS
        // =========================
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = taskTitleBox.Text;
            string description = taskDescriptionBox.Text;
            string reminder = reminderBox.Text;

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Please enter a task title.");
                return;
            }

            db.AddTask(title, description, reminder);

            db.LogActivity("Task added: " + title);

            LoadTasks();
            LoadLogs();

            taskTitleBox.Clear();
            taskDescriptionBox.Clear();
            reminderBox.Clear();

            MessageBox.Show("Task added successfully.");
        }

        private void LoadTasks()
        {
            taskListBox.Items.Clear();

            foreach (var task in db.GetTasks())
            {
                taskListBox.Items.Add(task);
            }
        }

        // =========================
        // ACTIVITY LOGS
        // =========================
        private void LoadLogs()
        {
            logListBox.Items.Clear();

            foreach (var log in db.GetLogs())
            {
                logListBox.Items.Add(log);
            }
        }

        // =========================
        // QUIZ SYSTEM
        // =========================

        private int currentQuestion = 0;
        private int score = 0;

        private string[] questions =
        {
            "What does VPN stand for?",
            "Is phishing a scam technique?",
            "What should strong passwords contain?"
        };

        private string[,] options =
        {
            { "Virtual Private Network", "Very Personal Network", "Verified Public Network", "Visible Private Node" },

            { "Yes", "No", "Only sometimes", "Unknown" },

            { "Only letters", "Only numbers", "Letters, numbers, and symbols", "Your name" }
        };

        private int[] answers = { 0, 0, 2 };

        private void SetupQuiz()
        {
            DisplayQuestion();
        }

        private void DisplayQuestion()
        {
            questionText.Text = questions[currentQuestion];

            optionA.Content = options[currentQuestion, 0];
            optionB.Content = options[currentQuestion, 1];
            optionC.Content = options[currentQuestion, 2];
            optionD.Content = options[currentQuestion, 3];

            scoreText.Text = $"Score: {score}";
        }

        private void Option_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            int selectedAnswer = 0;

            if (clickedButton == optionA) selectedAnswer = 0;
            if (clickedButton == optionB) selectedAnswer = 1;
            if (clickedButton == optionC) selectedAnswer = 2;
            if (clickedButton == optionD) selectedAnswer = 3;

            if (selectedAnswer == answers[currentQuestion])
            {
                score++;
                MessageBox.Show("Correct!");
            }
            else
            {
                MessageBox.Show("Incorrect.");
            }

            db.LogActivity("Quiz question answered.");

            LoadLogs();

            currentQuestion++;

            if (currentQuestion >= questions.Length)
            {
                MessageBox.Show($"Quiz complete! Final score: {score}");

                currentQuestion = 0;
                score = 0;
            }

            DisplayQuestion();
        }
    }
}