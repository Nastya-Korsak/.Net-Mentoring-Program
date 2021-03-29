using System;

namespace HelloLibrary
{
    public class HelloClass
    {
        public static string HelloMethod(string name)
        {
            var time = DateTime.Now.ToString("HH:mm:ss");

            return $"{time} Hello {name}!";
        }
    }
}
