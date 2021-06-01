using System;
using System.Collections.Generic;
using FizzBuzz;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace Tasks.Test
{
    public class FizzBuzzTests
    {
        private FizzBuzz.FizzBuzz _fizzBuzz;

        [SetUp]
        public void Setup()
        {
            _fizzBuzz = new FizzBuzz.FizzBuzz();
        }

        [Test]
        [TestCase(3)]
        [TestCase(66)]
        [TestCase(99)]
        public void NumberHandler_NumberDivisibleBy3_Fizz(int number)
        {
            var expectedValue = "Fizz";

            var returnedValue = _fizzBuzz.NumberHandler(number);

            returnedValue.Should().Be(expectedValue);
        }

        [Test]
        [TestCase(5)]
        [TestCase(55)]
        [TestCase(100)]
        public void NumberHandler_NumberDivisibleBy5_Buzz(int number)
        {
            var expectedValue = "Buzz";

            var returnedValue = _fizzBuzz.NumberHandler(number);

            returnedValue.Should().Be(expectedValue);
        }

        [Test]
        [TestCase(15)]
        [TestCase(30)]
        [TestCase(90)]
        public void NumberHandler_NumberDivisibleBy3And5_FizzBuzz(int number)
        {
            var expectedValue = "FizzBuzz";

            var returnedValue = _fizzBuzz.NumberHandler(number);

            returnedValue.Should().Be(expectedValue);
        }

        [Test]
        [TestCase(1)]
        [TestCase(17)]
        [TestCase(88)]
        public void NumberHandler_NumberNotDivisibleBy3And5_ImputedNumber(int number)
        {
            var expectedValue = number.ToString();

            var returnedValue = _fizzBuzz.NumberHandler(number);

            returnedValue.Should().Be(expectedValue);
        }

        [Test]
        [TestCase(0)]
        [TestCase(101)]
        public void NumberHandler_NumberOutsideRangeOf1To100_ArgumentOutOfRangeException(int number)
        {
            Func<string> numberHandler = _fizzBuzz.NumberHandler(number);

            numberHandler.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void GetNumbers_ListOfHandledNumbersOf1To100()
        {
            int i = 0;

            List<string> resultList = _fizzBuzz.GetNumbers();

            using (new AssertionScope())
            {
                resultList.Count.Should().Be(100);

                foreach (var value in resultList)
                {
                    resultList[i].Should().Be(_fizzBuzz.NumberHandler(i));
                    i++;
                }
            }
        }
    }
}
