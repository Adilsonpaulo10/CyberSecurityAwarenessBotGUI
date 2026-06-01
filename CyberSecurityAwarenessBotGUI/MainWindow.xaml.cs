using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CyberSecurityAwarenessBotGUI
{
    public partial class MainWindow : Window
    {
        private ChatbotEngine bot;
        private DatabaseHelper db;

        private int currentQuestion = 0;
        private int score = 0;

        private string[] questions =
        {
            "What does VPN stand for?",
            "Is phishing a scam technique?",
            "What should strong passwords contain?",
            "What is malware?",
            "Should you share passwords with others?",
            "What is 2FA?",
            "Is public Wi-Fi always safe?",
            "What does phishing try to steal?",
            "What should you do with suspicious emails?",
            "What protects a computer from viruses?"
        };

        private string[,] options =
        {
            { "Virtual Private Network", "Very Personal Network", "Verified Public Network", "Visible Private Node" },

            { "Yes", "No", "Only sometimes", "Unknown" },

            { "Only letters", "Only numbers", "Letters, numbers, and symbols", "Your name" },

            { "Helpful software", "A cyber threat", "A password manager", "An internet browser" },

            { "Yes", "No", "Only with friends", "Only online" },

            { "Two-Factor Authentication", "Two File Access", "Fast Login", "Private Browser" },

            { "Yes", "No", "Only at school", "Only during daytime" },

            { "Games", "Personal information", "Music", "Videos" },

            { "Open them immediately", "Ignore security", "Delete/report them", "Forward to strangers" },

            { "Antivirus software", "Paint", "Calculator", "Notepad" }
        };

        private int[] answers =
        {
            0,
            0,
            2,
            1,
            1,
            0,
            1,
            1,
            2,
            0
        };

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

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (taskListBox.SelectedItem == null)
            {
                MessageBox.Show("Select a task first.");
                return;
            }

            string selected = taskListBox.SelectedItem.ToString();

            int id = int.Parse(selected.Split(':')[0]);

            db.DeleteTask(id);

            db.LogActivity("Deleted task ID: " + id);

            LoadTasks();
            LoadLogs();

            MessageBox.Show("Task deleted.");
        }

        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (taskListBox.SelectedItem == null)
            {
                MessageBox.Show("Select a task first.");
                return;
            }

            string selected = taskListBox.SelectedItem.ToString();

            int id = int.Parse(selected.Split(':')[0]);

            db.CompleteTask(id);

            db.LogActivity("Completed task ID: " + id);

            LoadTasks();
            LoadLogs();

            MessageBox.Show("Task marked complete.");
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
        // QUIZ
        // =========================

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
                MessageBox.Show($"Quiz complete!\n\nFinal Score: {score}/{questions.Length}");

                currentQuestion = 0;
                score = 0;
            }

            DisplayQuestion();
        }
    }
}