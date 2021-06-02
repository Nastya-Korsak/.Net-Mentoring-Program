using System.Collections.Generic;
using System.Linq;

namespace HarryPotter
{
    public class Bookstore : IBookstore
    {
        public double GetFinalPrice(List<Books> books)
        {
            if (books is null)
            {
                throw new System.ArgumentNullException(nameof(books));
            }

            var otherBooks = 0;

            Dictionary<Books, int> harryPotterBooks = GetHarryPotterBooksCount(books, ref otherBooks);

            double finallyPrice = 0;

            finallyPrice += otherBooks * 8;

            while (harryPotterBooks.All(v => v.Value > 0))
            {
                finallyPrice += (5.0 * 8.0) - (5.0 * 8.0 * 0.25);
                foreach (var v in harryPotterBooks)
                {
                    harryPotterBooks[v.Key] -= 1;
                }
            }

            while (harryPotterBooks.Count(v => v.Value > 0) == 4)
            {
                finallyPrice += (4.0 * 8.0) - (4.0 * 8.0 * 0.2);
                foreach (var v in harryPotterBooks)
                {
                    if (harryPotterBooks[v.Key] > 0)
                    {
                        harryPotterBooks[v.Key] -= 1;
                    }
                }
            }

            while (harryPotterBooks.Count(v => v.Value > 0) == 3)
            {
                finallyPrice += (3.0 * 8.0) - (3.0 * 8.0 * 0.1);
                foreach (var v in harryPotterBooks)
                {
                    if (harryPotterBooks[v.Key] > 0)
                    {
                        harryPotterBooks[v.Key] -= 1;
                    }
                }
            }

            while (harryPotterBooks.Count(v => v.Value > 0) == 2)
            {
                finallyPrice += (2.0 * 8.0) - (2.0 * 8.0 * 0.05);
                foreach (var v in harryPotterBooks)
                {
                    if (harryPotterBooks[v.Key] > 0)
                    {
                        harryPotterBooks[v.Key] -= 1;
                    }
                }
            }

            foreach (var v in harryPotterBooks)
            {
                finallyPrice += harryPotterBooks[v.Key] * 8;
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
                if (book == Books.HarryPotter1)
                {
                    harryPotterBooks[Books.HarryPotter1] += 1;
                }
                else if (book == Books.HarryPotter2)
                {
                    harryPotterBooks[Books.HarryPotter2] += 1;
                }
                else if (book == Books.HarryPotter3)
                {
                    harryPotterBooks[Books.HarryPotter3] += 1;
                }
                else if (book == Books.HarryPotter4)
                {
                    harryPotterBooks[Books.HarryPotter4] += 1;
                }
                else if (book == Books.HarryPotter5)
                {
                    harryPotterBooks[Books.HarryPotter5] += 1;
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
