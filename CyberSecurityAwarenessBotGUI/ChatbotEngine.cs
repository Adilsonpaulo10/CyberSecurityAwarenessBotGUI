using System;
using System.Collections.Generic;

namespace CyberSecurityAwarenessBotGUI
{
    public class ChatbotEngine
    {
        private Dictionary<string, string> memory;
        private Random random;

        private string currentTopic = "";
        private string lastTaskTitle = "";
        private bool waitingForReminder = false;
        private bool waitingForReminderDate = false;


        public ChatbotEngine()
        {
            memory = new Dictionary<string, string>();
            random = new Random();
        }

        public string GetResponse(string input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input))
                    return "Please type something so I can help you.";

                input = input.ToLower().Trim();
                // =========================
                // TASK REMINDER CONVERSATION
                // =========================

                if (input.StartsWith("add task"))
                {
                    string taskName = input.Replace("add task", "").Trim();

                    if (!string.IsNullOrEmpty(taskName))
                    {
                        lastTaskTitle = taskName;

                        waitingForReminder = true;
                        waitingForReminderDate = false;

                        return $"Task '{taskName}' added successfully. Would you like a reminder?";
                    }

                    return "Please provide a task name.";
                }

                if (waitingForReminder)
                {
                    if (input == "yes" ||
                        input.Contains("yes please"))
                    {
                        waitingForReminder = false;
                        waitingForReminderDate = true;

                        return "Sure. When would you like to be reminded?";
                    }

                    if (input == "no")
                    {
                        waitingForReminder = false;

                        return "Okay. The task was saved without a reminder.";
                    }
                }

                if (waitingForReminderDate)
                {
                    if (input.Contains("day") ||
                        input.Contains("week") ||
                        input.Contains("month"))
                    {
                        waitingForReminderDate = false;

                        return $"Reminder noted. I'll associate '{lastTaskTitle}' with that reminder period.";
                    }
                }
                // =========================
                // MEMORY
                // =========================

                if (input.Contains("i like"))
                {
                    string topic = input.Replace("i like", "").Trim();

                    if (!string.IsNullOrEmpty(topic))
                    {
                        memory["interest"] = topic;
                        return $"Got it! I'll remember you're interested in {topic}.";
                    }
                }

                if (input.Contains("what do you remember"))
                {
                    if (memory.ContainsKey("interest"))
                        return $"You told me you're interested in {memory["interest"]}.";
                    else
                        return "I don't have anything stored yet.";
                }

                // =========================
                // FOLLOW-UP QUESTIONS
                // =========================

                if (input.Contains("tell me more") ||
                    input.Contains("more info") ||
                    input.Contains("explain more"))
                {
                    switch (currentTopic)
                    {
                        case "privacy":
                            return "Privacy can be improved by limiting personal information shared online, reviewing app permissions, and using strong security settings.";

                        case "phishing":
                            return "Phishing attacks often pretend to be trusted organisations and try to trick users into revealing passwords or banking information.";

                        case "passwords":
                            return "A password manager can help generate and store strong unique passwords securely.";

                        case "vpn":
                            return "VPNs encrypt your internet traffic, especially useful when using public Wi-Fi networks.";

                        case "malware":
                            return "Malware includes viruses, worms, ransomware, spyware, and trojans that can damage systems or steal information.";

                        case "2fa":
                            return "2FA adds an additional security layer by requiring a second verification method beyond your password.";

                        case "scams":
                            return "Online scams often use fake websites, emails, or social media messages to steal money or personal information.";

                        default:
                            return "Can you tell me which cybersecurity topic you'd like to learn more about?";
                    }
                }

                // =========================
                // EMOTIONS
                // =========================

                if (input.Contains("worried") || input.Contains("scared"))
                {
                    if (currentTopic == "privacy")
                        return "Many people worry about privacy online. Reviewing account settings and limiting personal information sharing can help.";

                    if (currentTopic == "scams")
                        return "Being cautious about scams is a good thing. Always verify links and never share sensitive information unexpectedly.";

                    return "It's understandable to feel worried. Cybersecurity awareness is the first step toward staying safe.";
                }

                if (input.Contains("curious"))
                {
                    if (!string.IsNullOrEmpty(currentTopic))
                        return $"Curiosity is great. Learning more about {currentTopic} can help you stay safer online.";

                    return "Curiosity is one of the best ways to improve cybersecurity awareness.";
                }

                if (input.Contains("confused"))
                {
                    return "That's okay. Cybersecurity can be complicated. Tell me which topic is confusing and I'll explain it simply.";
                }

                if (input.Contains("frustrated"))
                {
                    return "I understand it can be frustrating. Let's work through it together.";
                }

                // =========================
                // PRIVACY
                // =========================

                if (input.Contains("privacy"))
                {
                    currentTopic = "privacy";

                    string[] responses =
                    {
                        "Online privacy helps protect your personal information from being misused.",
                        "Privacy settings control who can access your information online.",
                        "Good privacy habits reduce the risk of identity theft.",
                        "Always review the privacy settings on social media platforms.",
                        "Privacy is about controlling your digital footprint."
                    };

                    return responses[random.Next(responses.Length)];
                }

                // =========================
                // PASSWORDS
                // =========================

                if (input.Contains("password"))
                {
                    currentTopic = "passwords";

                    string[] responses =
                    {
                        "Strong passwords should contain letters, numbers, and symbols.",
                        "Avoid using personal information in passwords.",
                        "Every account should have a unique password.",
                        "Password managers help create strong passwords.",
                        "Long passwords are generally more secure."
                    };

                    return responses[random.Next(responses.Length)];
                }

                // =========================
                // PHISHING
                // =========================

                if (input.Contains("phishing"))
                {
                    currentTopic = "phishing";

                    string[] responses =
                    {
                        "Phishing is a scam designed to steal personal information.",
                        "Phishing emails often contain fake links.",
                        "Never click suspicious links from unknown senders.",
                        "Phishing attacks often create urgency to pressure victims.",
                        "Always verify unexpected emails before responding."
                    };

                    return responses[random.Next(responses.Length)];
                }

                // =========================
                // MALWARE
                // =========================

                if (input.Contains("malware"))
                {
                    currentTopic = "malware";

                    return "Malware is malicious software designed to harm devices or steal data.";
                }

                // =========================
                // VPN
                // =========================

                if (input.Contains("vpn"))
                {
                    currentTopic = "vpn";

                    return "A VPN encrypts your internet connection and improves privacy.";
                }

                // =========================
                // 2FA
                // =========================

                if (input.Contains("2fa") ||
                    input.Contains("two factor") ||
                    input.Contains("authentication"))
                {
                    currentTopic = "2fa";

                    return "Two-Factor Authentication adds an extra layer of account security.";
                }

                // =========================
                // RANSOMWARE
                // =========================

                if (input.Contains("ransomware"))
                {
                    currentTopic = "ransomware";

                    return "Ransomware encrypts files and demands payment for their release.";
                }

                // =========================
                // SCAMS
                // =========================

                if (input.Contains("scam"))
                {
                    currentTopic = "scams";

                    return "Online scams attempt to trick users into giving away money or personal information.";
                }

                // =========================
                // SOCIAL ENGINEERING
                // =========================

                if (input.Contains("social engineering"))
                {
                    currentTopic = "social engineering";

                    return "Social engineering manipulates people into revealing confidential information.";
                }

                // =========================
                // FIREWALL
                // =========================

                if (input.Contains("firewall"))
                {
                    currentTopic = "firewall";

                    return "A firewall monitors and filters network traffic for security.";
                }

                // =========================
                // ANTIVIRUS
                // =========================

                if (input.Contains("antivirus"))
                {
                    currentTopic = "antivirus";

                    return "Antivirus software detects and removes malicious programs.";
                }

                // =========================
                // SAFE BROWSING
                // =========================

                if (input.Contains("browser") ||
                    input.Contains("safe browsing"))
                {
                    currentTopic = "safe browsing";

                    return "Always check website URLs and avoid suspicious downloads.";
                }

                return "I didn't quite understand that. Try asking about passwords, phishing, malware, VPNs, privacy, ransomware, scams, antivirus, firewalls, or 2FA.";
            }
            catch
            {
                return "Something went wrong, but I'm still running safely.";
            }
        }
    }
}