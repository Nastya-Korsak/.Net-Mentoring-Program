using System;

namespace Task1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"Application started.{Environment.NewLine}");

            try
            {
                var stringArray = new StringArray(args);

                foreach (var letter in stringArray.GetStringsFirstLetters())
                {
                    Console.WriteLine(letter);
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine($"{Environment.NewLine}Application finished.");
        }
    }
}
