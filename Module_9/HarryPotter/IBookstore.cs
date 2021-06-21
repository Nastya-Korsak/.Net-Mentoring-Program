using System.Collections.Generic;

namespace HarryPotter
{
    public interface IBookstore
    {
        double GetFinalPrice(List<Books> books);
    }
}
