using System;
using System.Collections.Generic;
using System.Linq;

namespace Task1
{
    public class StringArray
    {
        private readonly string[] _strings;

        public StringArray(string[] strings)
        {
            _strings = strings ?? throw new ArgumentNullException(nameof(strings), "Array cannot be null.");
        }

        public IEnumerable<char> GetStringsFirstLetters()
        {
            if (_strings.Length == 0)
            {
                throw new ArgumentException("Array cannot be empty.", nameof(_strings));
            }

            if (_strings.Contains(string.Empty))
            {
                throw new ArgumentException(
                    "Array cannot contain empty string. String index in array: " +
                    $"{_strings.ToList().IndexOf(string.Empty) + 1}", nameof(_strings));
            }

            if (_strings.Contains(null))
            {
                throw new ArgumentException(
                    "Array cannot contain string with null value. String index in array: " +
                    $"{_strings.ToList().IndexOf(null) + 1}", nameof(_strings));
            }

            foreach (var line in _strings)
            {
                yield return line[0];
            }
        }
    }
}
