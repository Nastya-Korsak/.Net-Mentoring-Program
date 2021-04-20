using System;
using System.Linq;

namespace Task2
{
    public class NumberParser : INumberParser
    {
        public int Parse(string stringValue)
        {
            if (stringValue is null)
            {
                throw new ArgumentNullException(nameof(stringValue));
            }

            if (!int.TryParse(stringValue, out var result))
            {
                if (IsStringDigit(stringValue))
                {
                    throw new OverflowException(nameof(stringValue));
                }
                else
                {
                    throw new FormatException(nameof(stringValue));
                }
            }

            return result;
        }

        private bool IsStringDigit(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            if (char.IsDigit(stringValue.First()) || stringValue.First() == '-')
            {
                if (stringValue[1..].All(char.IsDigit))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
