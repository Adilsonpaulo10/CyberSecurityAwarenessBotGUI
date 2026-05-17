using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace CyberSecurityAwarenessBotGUI
{
    public class DatabaseHelper
    {
        private string connectionString =
         @"Server=LabVM2049939\SQLEXPRESS;Database=CyberSecurityBotDB;Trusted_Connection=True;TrustServerCertificate=True;";
    
        // ADD TASK
        public void AddTask(string title, string description, string reminder)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query =
                    "INSERT INTO Tasks (Title, Description, ReminderDate, IsCompleted) " +
                    "VALUES (@title, @desc, @reminder, 0)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@reminder", reminder);

                cmd.ExecuteNonQuery();
            }
        }

        // GET TASKS
        public List<string> GetTasks()
        {
            List<string> tasks = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Tasks";

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tasks.Add(
                        $"{reader["Id"]}: {reader["Title"]} | " +
                        $"{reader["Description"]} | Reminder: {reader["ReminderDate"]}"
                    );
                }
            }

            return tasks;
        }

        // ACTIVITY LOG
        public void LogActivity(string text)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query =
                    "INSERT INTO ActivityLogs (ActivityText, LogDate) " +
                    "VALUES (@text, @date)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@text", text);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);

                cmd.ExecuteNonQuery();
            }
        }

        public List<string> GetLogs()
        {
            List<string> logs = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM ActivityLogs";

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    logs.Add(
                        $"{reader["LogDate"]}: {reader["ActivityText"]}"
                    );
                }
            }

            return logs;
        }
    }
}