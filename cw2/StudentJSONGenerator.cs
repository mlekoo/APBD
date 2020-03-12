using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace cw2
{
    public class StudentJSONGenerator
    {
        public HashSet<Student> studenci { get; set; }

        public StudentJSONGenerator(HashSet<Student> studenci, string path) {
            List<Student> s = new List<Student>(studenci);

            List<Course> activeStudies = new List<Course>();
            
            

            foreach (var student in s) {

                var index = activeStudies.FindIndex(x => x.courseName == student.Kierunek);

                if (index >= 0)
                {
                    activeStudies[index].howManyStudents += 1;
                }
                else 
                {
                    activeStudies.Add(new Course { courseName = student.Kierunek, howManyStudents = 1 } );
                }

            }

            var today = DateTime.Parse(DateTime.Now.ToString()).ToShortDateString();
            

            
            JObject jsonNode =
                new JObject(
                 new JProperty("uczelnia",
                     new JObject(
                      new JProperty("createdAt", today),
                      new JProperty("author", "Kacper Urbański"),
                      new JProperty("studenci",
                        new JArray(from student in s
                                   orderby student.NumerIndeksu
                                   select new JObject(
                                       new JProperty("indexNumber",student.NumerIndeksu),
                                       new JProperty("fname", student.Imie),
                                       new JProperty("lname", student.Nazwisko),
                                       new JProperty("birthdate",student.DataUrodzenia),
                                       new JProperty("email",student.Email),
                                       new JProperty("mothersName",student.ImieMatki),
                                       new JProperty("fathersName",student.ImieOjca),
                                       new JProperty("studies",
                                        new JObject(
                                            new JProperty("name",student.Kierunek),
                                            new JProperty("mode",student.TrybStudiow)
                                        )
                                       )
                                   )
                        )
                     ),
                      new JProperty("activeStudies",
                        new JArray(from course in activeStudies
                                   orderby course.howManyStudents
                                   select new JObject(
                                       new JProperty("name", course.courseName),
                                       new JProperty("numberOfStudents",course.howManyStudents)
                                       )
                                   )
                        )
                    )
                )
              );

            File.WriteAllText(path, jsonNode.ToString());

           
        }    
    }

    public class Course 
    {
        public string courseName { get; set; }
        public int howManyStudents { get; set; }
        public Course() {
        
        }       

    }
}
