using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace CyberSecurityAwarenessBotGUI
{
    public class DatabaseHelper
    {
        private string connectionString =
            @"Server=LabVM2049939\SQLEXPRESS;Database=CyberSecurityBotDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // =========================
        // ADD TASK
        // =========================
        public void AddTask(string title, string description, string reminder)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query =
                    @"INSERT INTO Tasks
                    (Title, Description, ReminderDate, IsCompleted)
                    VALUES
                    (@title, @desc, @reminder, 0)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@reminder", reminder);

                cmd.ExecuteNonQuery();
            }
        }

        // =========================
        // DELETE TASK
        // =========================
        public void DeleteTask(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM Tasks WHERE Id = @id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        // =========================
        // COMPLETE TASK
        // =========================
        public void CompleteTask(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query =
                    "UPDATE Tasks SET IsCompleted = 1 WHERE Id = @id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        // =========================
        // GET TASKS
        // =========================
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
                    bool completed =
                        Convert.ToBoolean(reader["IsCompleted"]);

                    string status =
                        completed ? "✅ Completed" : "⏳ Pending";

                    tasks.Add(
                        $"{reader["Id"]}: " +
                        $"{reader["Title"]} | " +
                        $"{reader["Description"]} | " +
                        $"Reminder: {reader["ReminderDate"]} | " +
                        $"{status}"
                    );
                }
            }

            return tasks;
        }

        // =========================
        // LOG ACTIVITY
        // =========================
        public void LogActivity(string text)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query =
                    @"INSERT INTO ActivityLogs
                    (ActivityText, LogDate)
                    VALUES
                    (@text, @date)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@text", text);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);

                cmd.ExecuteNonQuery();
            }
        }

        // =========================
        // GET LOGS
        // =========================
        public List<string> GetLogs()
        {
            List<string> logs = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query =
                    "SELECT * FROM ActivityLogs ORDER BY LogDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    logs.Add(
                        $"{reader["LogDate"]} - {reader["ActivityText"]}"
                    );
                }
            }

            return logs;
        }
    }
}