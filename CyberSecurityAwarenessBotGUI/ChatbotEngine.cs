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
            input = input.ToLower();

            foreach (var key in responses.Keys)
            {
                if (input.Contains(key))
                    return responses[key];
            }

            return "I don't understand. Try another question.";
        }
    }
}