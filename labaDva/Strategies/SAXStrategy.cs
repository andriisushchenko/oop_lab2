using labaDva.Models;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq; 

namespace labaDva.Strategies
{
    public class SAXStrategy : IStrategyAnlys
    {
        public List<Student> Search(SearchParameters query)
        {
            var students = new List<Student>();
            var linqParser = new LINQStrategy(); 

            using (var reader = XmlReader.Create(query.FilePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Student")
                    {
                        string attrValue = reader.GetAttribute(query.AttributeToSearch);
                        if (attrValue != null && attrValue == query.ValueToSearch)
                        {
                            var s = (XElement)XNode.ReadFrom(reader);
                            var student = linqParser.ParseStudentFromElement(s);
                            students.Add(student);
                        }
                    }
                }
            }
            return students;
        }
    }
}