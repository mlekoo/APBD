using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace cw2
{

   
    class StudentXMLGenerator
    {

        public HashSet<Student> studenci { get; set; }

        public StudentXMLGenerator(HashSet<Student> studenci,string path)
        {
            this.studenci = studenci;
            List<Course> activeStudies = new List<Course>();

            var today = DateTime.Parse(DateTime.Now.ToString()).ToShortDateString();
            var xmlNode =
                new XElement("uczelnia",
                    new XAttribute("createdAt", today),
                    new XAttribute("author", "Kacper Urbański"),
                    new XElement("studenci")
                );

            foreach (var student in studenci) {


                var index = activeStudies.FindIndex(x => x.courseName == student.Kierunek);

                if (index >= 0)
                {
                    activeStudies[index].howManyStudents += 1;
                }
                else
                {
                    activeStudies.Add(new Course { courseName = student.Kierunek, howManyStudents = 1 });
                }

                var xmlStudent =
                        new XElement("student",
                        new XAttribute("indexNumber", student.NumerIndeksu),
                            new XElement("fname", student.Imie),
                            new XElement("lname", student.Nazwisko),
                            new XElement("birthdate", student.DataUrodzenia),
                            new XElement("email", student.Email),
                            new XElement("mothersName", student.ImieMatki),
                            new XElement("fathersName", student.ImieOjca),
                            new XElement("studies",
                                new XElement("name", student.Kierunek),
                                new XElement("mode", student.TrybStudiow)
                                )
                        );
                xmlNode.Element("studenci").Add(xmlStudent);
            }
          
            xmlNode.Add(new XElement("activeStudies"));

            foreach (var studies in activeStudies) 
            {
            var xmlStudies = new XElement("studies",
                                new XAttribute("name", studies.courseName),
                                new XAttribute("numberOfStudents", studies.howManyStudents)
                            );
                xmlNode.Element("activeStudies").Add(xmlStudies);
            }

            xmlNode.Save(path);
        }

    }
}
