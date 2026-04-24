using System.Collections.Generic;

namespace CyberSecurityAwarenessBotGUI
{
    public class ChatbotEngine
    {
        private Dictionary<string, string> responses;

        public ChatbotEngine()
        {
            responses = new Dictionary<string, string>()
            {
                { "password", "Use strong passwords." },
                { "phishing", "Phishing is a scam." },
                { "vpn", "VPN protects your privacy." }
            };
        }

        public string GetResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please enter a message.";

            input = input.ToLower();

            // Sentiment
            if (input.Contains("worried"))
                return "It's okay to feel worried. I'm here to help.";

            if (input.Contains("curious"))
                return "Curiosity is great for learning cybersecurity!";

            foreach (var key in responses.Keys)
            {
                if (input.Contains(key))
                    return responses[key];
            }

            return "I'm not sure I understand.";
        }
    }
}