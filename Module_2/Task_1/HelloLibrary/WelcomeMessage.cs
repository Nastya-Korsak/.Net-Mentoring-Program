using System;

namespace HelloLibrary
{
    public class WelcomeMessage
    {
        public static string GetWelcomeMessage(string name)
        {
            var time = DateTime.Now.ToString("HH:mm:ss");

            if (string.IsNullOrEmpty(name))
            {
                name = "unknown person";
            }

            return $"{time} Hello {name}!";
        }
    }
}
