using System;
using System.Collections.Generic;
using FluentAssertions;
using HarryPotter;
using NUnit.Framework;

namespace Tasks.Test
{
    public class HarryPotterTests
    {
        private IBookstore _bookstore;

        [SetUp]
        public void Setup()
        {
            _bookstore = new Bookstore();
        }

        [Test]
        public void GetFinalPrice_Null_ArgumentNullException()
        {
            Action act = () => _bookstore.GetFinalPrice(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void GetFinalPrice_HarryPottersBooksList_CorrectPriceWithDiscount()
        {
            var expectedFinallyPrice = 51.60;
            var books = new List<Books>()
            {
                Books.HarryPotter1, Books.HarryPotter1,
                Books.HarryPotter2, Books.HarryPotter2,
                Books.HarryPotter3, Books.HarryPotter3,
                Books.HarryPotter4,
                Books.HarryPotter5
            };

            _bookstore.GetFinalPrice(books).Should().Be(expectedFinallyPrice);
        }

        [Test]
        public void GetFinalPrice_DifferentBooksList_CorrectPriceWithDiscount()
        {
            var expectedFinallyPrice = 39.2;
            var books = new List<Books>()
            {
                Books.HarryPotter1, Books.HarryPotter1,
                Books.HarryPotter3,
                Books.MobyDick,
                Books.WarAndPeace
            };

            _bookstore.GetFinalPrice(books).Should().Be(expectedFinallyPrice);
        }
    }
}
