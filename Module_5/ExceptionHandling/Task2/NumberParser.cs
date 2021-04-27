using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Task2
{
    public class NumberParser : INumberParser
    {
        public int Parse(string stringValue)
        {
            int result;

            if (stringValue is null)
            {
                throw new ArgumentNullException(nameof(stringValue));
            }

            stringValue = stringValue.Trim();

            if (!IsStringNumber(stringValue))
            {
                throw new FormatException(nameof(stringValue));
            }

            if (!IsNumberInIntRange(stringValue))
            {
                throw new OverflowException(nameof(stringValue));
            }

            result = ParseStringToInt(stringValue);

            return result;
        }

        private bool IsStringNumber(string stringValue)
        {
            var reg = new Regex("^[0-9]+$");

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            if (char.IsDigit(stringValue[0]))
            {
                return reg.IsMatch(stringValue);
            }

            if (stringValue.StartsWith('-') || stringValue.StartsWith('+'))
            {
                return reg.IsMatch(stringValue[1..]);
            }

            return false;
        }

        private bool IsNumberInIntRange(string stringValue)
        {
            var sign = '+';

            if (stringValue.StartsWith('-') || stringValue.StartsWith('+'))
            {
                sign = stringValue.StartsWith('-') ? '-' : '+';
                stringValue = stringValue[1..];
            }

            if (stringValue.Length > 10)
            {
                return false;
            }

            if (stringValue.Length < 9)
            {
                return true;
            }

            var minIntValue = int.MinValue.ToString()[1..];

            foreach (var s in stringValue)
            {
                var index = stringValue.IndexOf(s);
                if ((int)char.GetNumericValue(s) > (int)char.GetNumericValue(minIntValue[index]))
                {
                    return false;
                }
            }

            if (sign == '+' && (int)char.GetNumericValue(stringValue.Last()) > 7)
            {
                return false;
            }

            return true;
        }

        private int ParseStringToInt(string stringValue)
        {
            var sign = 1;
            int result;

            if (stringValue.StartsWith('-') || stringValue.StartsWith('+'))
            {
                sign = stringValue.StartsWith('-') ? -1 : 1;
                stringValue = stringValue[1..];
            }

            result = (int)char.GetNumericValue(stringValue[0]);

            foreach (var s in stringValue[1..])
            {
                result = (result * 10) + (int)char.GetNumericValue(s);
            }

            return result * sign;
        }
    }
}
