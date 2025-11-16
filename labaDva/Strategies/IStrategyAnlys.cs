using labaDva.Models;
using System.Collections.Generic;

namespace labaDva.Strategies
{
    public interface IStrategyAnlys
    {
        List<Student> Search(SearchParameters query);
    }
}