using System.Collections.Generic;
using System.Linq;

namespace HarryPotter
{
    public class Bookstore : IBookstore
    {
        private readonly double _costOfOneBook = 8;
        private readonly Dictionary<double, double> _typesBooksAndDscount = new ()
        {
            { 5, 0.25 },
            { 4, 0.2 },
            { 3, 0.1 },
            { 2, 0.05 }
        };

        public double GetFinalPrice(List<Books> books)
        {
            if (books is null)
            {
                throw new System.ArgumentNullException(nameof(books));
            }

            var otherBooks = 0;

            Dictionary<Books, int> harryPotterBooks = GetHarryPotterBooksCount(books, ref otherBooks);

            double finallyPrice = 0;

            finallyPrice += otherBooks * _costOfOneBook;

            var countOfharryPotterBooksDifferentTypes = 5;

            while (countOfharryPotterBooksDifferentTypes != 1)
            {
                while (harryPotterBooks.Count(v => v.Value > 0) == countOfharryPotterBooksDifferentTypes)
                {
                    finallyPrice += (countOfharryPotterBooksDifferentTypes * _costOfOneBook)
                        - (countOfharryPotterBooksDifferentTypes * _costOfOneBook * _typesBooksAndDscount[countOfharryPotterBooksDifferentTypes]);
                    foreach (var v in harryPotterBooks)
                    {
                        if (harryPotterBooks[v.Key] > 0)
                        {
                            harryPotterBooks[v.Key] -= 1;
                        }
                    }
                }

                countOfharryPotterBooksDifferentTypes--;
            }

            foreach (var v in harryPotterBooks)
            {
                finallyPrice += harryPotterBooks[v.Key] * _costOfOneBook;
            }

            return finallyPrice;
        }

        private Dictionary<Books, int> GetHarryPotterBooksCount(List<Books> books, ref int otherBooks)
        {
            otherBooks = 0;
            Dictionary<Books, int> harryPotterBooks = new Dictionary<Books, int>()
            {
                { Books.HarryPotter1, 0 },
                { Books.HarryPotter2, 0 },
                { Books.HarryPotter3, 0 },
                { Books.HarryPotter4, 0 },
                { Books.HarryPotter5, 0 }
            };

            foreach (var book in books)
            {
                if (harryPotterBooks.ContainsKey(book))
                {
                    harryPotterBooks[book] += 1;
                }
                else
                {
                    otherBooks += 1;
                }
            }

            return harryPotterBooks;
        }
    }
}
