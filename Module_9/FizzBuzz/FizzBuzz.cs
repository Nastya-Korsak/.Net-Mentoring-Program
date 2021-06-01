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

            if (isNumberDivisibleBy3(number))
            {
                returnedValue.Append("Fizz");
            }

            if (isNumberDivisibleBy5(number))
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
            throw new NotImplementedException();
        }

        private bool isNumberDivisibleBy3(int number)
        {
            return number % 3 == 0;
        }

        private bool isNumberDivisibleBy5(int number)
        {
            return number % 5 == 0;
        }
    }
}
