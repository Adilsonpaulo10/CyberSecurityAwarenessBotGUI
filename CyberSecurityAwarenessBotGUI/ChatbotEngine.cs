using System;
using System.Collections.Generic;

namespace CyberSecurityAwarenessBotGUI
{
    public class ChatbotEngine
    {
        private Dictionary<string, string> responses;
        private Dictionary<string, string> memory;

        public ChatbotEngine()
        {
            responses = new Dictionary<string, string>()
            {
                { "password", "Use a strong password with letters, numbers, and symbols." },
                { "phishing", "Phishing is a scam used to steal personal information." },
                { "vpn", "A VPN encrypts your internet connection and protects your privacy." },
                { "malware", "Malware is harmful software that damages systems." },
                { "privacy", "Always check your privacy settings online." }
            };

            memory = new Dictionary<string, string>();
        }

        public string GetResponse(string input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input))
                    return "Please type something so I can help you.";

                input = input.ToLower().Trim();

                // SENTIMENT
                if (input.Contains("worried") || input.Contains("scared"))
                    return "It's understandable to feel worried. I'm here to help you stay safe.";

                if (input.Contains("curious"))
                    return "Curiosity is great! It helps you learn about cybersecurity.";

                if (input.Contains("frustrated"))
                    return "I understand it's frustrating. Let's work through it.";

                // MEMORY
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

                // NLP
                if (input.Contains("remind") || input.Contains("add task"))
                    return "Reminder saved.";

                // KEYWORDS
                foreach (var key in responses.Keys)
                {
                    if (input.Contains(key))
                        return responses[key];
                }

                return "I didn't quite understand that. Try asking about cybersecurity topics.";
            }
            catch
            {
                return "Something went wrong, but I'm still running safely.";
            }
        }
    }
}