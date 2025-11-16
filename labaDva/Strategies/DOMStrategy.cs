using labaDva.Models;
using System.Collections.Generic;
using System.Xml;

namespace labaDva.Strategies
{
    public class DOMStrategy : IStrategyAnlys
    {
        public List<Student> Search(SearchParameters query)
        {
            var students = new List<Student>();
            var doc = new XmlDocument();
            doc.Load(query.FilePath);

            string xpath = $"/Students/Student[@{query.AttributeToSearch}='{query.ValueToSearch}']";

            XmlNodeList nodes = doc.SelectNodes(xpath);

            foreach (XmlNode s in nodes)
            {
                var student = ParseStudentFromNode(s);
                students.Add(student);
            }
            return students;
        }

        private Student ParseStudentFromNode(XmlNode s)
        {
            var student = new Student
            {
                Name = s["Name"]?.InnerText,
                Faculty = s["Faculty"]?.InnerText,
                Department = s["Department"]?.InnerText,
                Disciplines = new List<Discipline>(),
                AdditionalAttributes = new Dictionary<string, string>()
            };

            foreach (XmlNode d in s.SelectNodes("Disciplines/Discipline"))
            {
                student.Disciplines.Add(new Discipline
                {
                    SubjectName = d["SubjectName"]?.InnerText,
                    Grade = int.Parse(d["Grade"]?.InnerText ?? "0")
                });
            }

            foreach (XmlAttribute attr in s.Attributes)
            {
                student.AdditionalAttributes[attr.Name] = attr.Value;
            }
            return student;
        }
    }
}