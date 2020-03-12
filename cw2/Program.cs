using System;
using System.Collections.Generic;
using System.IO;

namespace cw2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string INPUTDATAPATH = "data.csv";
            string OUTPUTDATAPATH = "żesult.xml";
            string OUTPUTFORMAT = "XML";

            try {

                INPUTDATAPATH = args[0];
                OUTPUTDATAPATH = args[1];
                OUTPUTFORMAT = args[2];

            } catch (IndexOutOfRangeException e) {
                
                Console.WriteLine("PROGRAM ARGUMENTS EXCEPTION " + e);
            }

            List<Student> studentList = new List<Student>();

            try
            {
                var lines = File.ReadLines(INPUTDATAPATH);
                foreach (var line in lines)
                {

                    var data = line.Split(",");
            
                    Student student = new Student();
                    student.Imie = data[0];
                    student.Nazwisko = data[1];
                    student.Kierunek = data[2];
                    student.TrybStudiow = data[3];
                    student.NumerIndeksu = data[4];
                    if (data.Length < 9)
                    {
                        log(student + " Brakujące dane");
                    }
                    else
                    {
                       

                        student.DataUrodzenia = DateTime.Parse(data[5]).ToShortDateString();
                        student.Email = data[6];
                        student.ImieMatki = data[7];
                        student.ImieOjca = data[8];

                        studentList.Add(student);
                    }

                }

                HashSet<Student> studentHash = new HashSet<Student>(new OwnComparer());


                foreach (var student in studentList)
                {
                    bool isAnyOfPropertyNullOrEmpty = false;
                    for (int i = 0; i < student.GetType().GetProperties().Length; i++)
                    {
                        var propertyName = student.GetType().GetProperties().GetValue(i).ToString().Split(" ")[1];
                        var propertyValue = student.GetType().GetProperty(propertyName).GetValue(student,null).ToString();


                        if (string.IsNullOrEmpty(propertyValue))
                        {
                            isAnyOfPropertyNullOrEmpty = true;
                        }                     
                    }

                    if (!isAnyOfPropertyNullOrEmpty)
                    {
                        if (!studentHash.Add(student))
                        {
                            log(student + " Powtórzenie danych");
                        }
                    }
                    else
                    {                  
                        log(student + " Brakujące dane");
                    }
                }

                if (OUTPUTFORMAT.ToUpper() == "JSON")
                {
                    new StudentJSONGenerator(studentHash, OUTPUTDATAPATH);
                }
                else if (OUTPUTFORMAT.ToUpper() == "XML") 
                {
                    new StudentXMLGenerator(studentHash, OUTPUTDATAPATH);
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Podana ścieżka jest niepoprawna " + e);
                log("Podana ścieżka jest niepoprawna " + e);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Plik " + INPUTDATAPATH + " nie istnieje" + e);
                log("Plik " + INPUTDATAPATH + " nie istnieje " + e);
            }


        }

        public static void log(string text)
        {
            using (StreamWriter sw = new StreamWriter("log.txt",true))
            {
                sw.WriteLine(text);
            }

        }
    }
}



