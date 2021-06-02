using System.Collections.Generic;

namespace HarryPotter
{
    public interface IBookstore
    {
        public double GetFinalPrice(List<Books> books);
    }
}
