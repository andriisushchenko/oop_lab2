using labaDva.Models;
using labaDva.Strategies;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace labaDva.Services
{
    public class XmlAnalysisContext
    {
        private IStrategyAnlys _strategy;

        public Dictionary<string, List<string>> GetAttributeValueMap(string filePath)
        {
            var doc = XDocument.Load(filePath);

            var allAttributes = doc.Descendants("Student")
                                   .SelectMany(s => s.Attributes());

            var groupedAttributes = allAttributes.GroupBy(a => a.Name.ToString());

            var map = groupedAttributes.ToDictionary(
                group => group.Key, 
                group => group.Select(a => a.Value).Distinct().ToList() 
            );

            return map;
        }

        public void SetStrategy(IStrategyAnlys strategy)
        {
            this._strategy = strategy;
        }

        public List<Student> ExecuteSearch(SearchParameters query)
        {
            if (_strategy == null)
            {
                throw new System.Exception("Стратегію пошуку не встановлено!");
            }
            return _strategy.Search(query);
        }
    }
}