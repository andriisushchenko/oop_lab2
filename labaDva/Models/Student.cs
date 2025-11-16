using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace labaDva.Models
{
    public class Discipline
    {
        public string SubjectName { get; set; }
        public int Grade { get; set; }

        public override string ToString()
        {
            return $"{SubjectName}: {Grade}";
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
        public List<Discipline> Disciplines { get; set; }
        public Dictionary<string, string> AdditionalAttributes { get; set; }

        public Student()
        {
            Disciplines = new List<Discipline>();
            AdditionalAttributes = new Dictionary<string, string>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Студент: {Name} ({Faculty}, {Department})");
            foreach (var attr in AdditionalAttributes)
            {
                sb.AppendLine($"  - {attr.Key}: {attr.Value}");
            }
            foreach (var disc in Disciplines)
            {
                sb.AppendLine($"    - {disc}");
            }
            return sb.ToString();
        }
    }
}