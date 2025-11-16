using labaDva.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace labaDva.Strategies
{
    public class LINQStrategy : IStrategyAnlys
    {
        public List<Student> Search(SearchParameters query)
        {
            var students = new List<Student>();
            var doc = XDocument.Load(query.FilePath);

            var studentElements = doc.Descendants("Student")
                .Where(s => s.Attribute(query.AttributeToSearch)?.Value == query.ValueToSearch);

            foreach (var s in studentElements)
            {
                var student = ParseStudentFromElement(s);
                students.Add(student);
            }
            return students;
        }

        // Публічний допоміжний метод, щоб SAX міг його використати
        public Student ParseStudentFromElement(XElement s)
        {
            var student = new Student
            {
                Name = s.Element("Name")?.Value,
                Faculty = s.Element("Faculty")?.Value,
                Department = s.Element("Department")?.Value,
                Disciplines = s.Descendants("Discipline")
                               .Select(d => new Discipline
                               {
                                   SubjectName = d.Element("SubjectName")?.Value,
                                   Grade = int.Parse(d.Element("Grade")?.Value ?? "0")
                               }).ToList(),
                AdditionalAttributes = new Dictionary<string, string>()
            };

            foreach (var attr in s.Attributes())
            {
                student.AdditionalAttributes[attr.Name.ToString()] = attr.Value;    
            }
            return student;
        }
    }
}