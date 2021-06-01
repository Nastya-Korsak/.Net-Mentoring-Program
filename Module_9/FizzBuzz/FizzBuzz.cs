using System;
using System.Collections.Generic;
using System.Text;

namespace FizzBuzz
{
    public class FizzBuzz
    {
        public string NumberHandler(int number)
        {
            if (number < 1 || number > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }

            var returnedValue = new StringBuilder();

            if (IsNumberDivisibleBy3(number))
            {
                returnedValue.Append("Fizz");
            }

            if (IsNumberDivisibleBy5(number))
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

            for (int i = 1; i <=100; i++)
            {
                listOfHandledNumbers.Add(NumberHandler(i));
            }

            return listOfHandledNumbers;
        }

        private bool IsNumberDivisibleBy3(int number)
        {
            return number % 3 == 0;
        }

        private bool IsNumberDivisibleBy5(int number)
        {
            return number % 5 == 0;
        }
    }
}
