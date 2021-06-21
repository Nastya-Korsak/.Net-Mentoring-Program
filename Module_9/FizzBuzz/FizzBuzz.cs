using System;
using System.Collections.Generic;
using System.Text;

namespace FizzBuzz
{
    public class FizzBuzz
    {
        public string HandleNumber(int number)
        {
            if (number < 1 || number > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }

            var returnedValue = new StringBuilder();

            if (number % 3 == 0)
            {
                returnedValue.Append("Fizz");
            }

            if (number % 5 == 0)
            {
                returnedValue.Append("Buzz");
            }

            if (returnedValue.Length == 0)
            {
                return number.ToString();
            }

            return returnedValue.ToString();
        }

        public List<string> GetNumbers()
        {
            var listOfHandledNumbers = new List<string>();

            for (int i = 1; i <= 100; i++)
            {
                listOfHandledNumbers.Add(HandleNumber(i));
            }

            return listOfHandledNumbers;
        }
    }
}
