﻿using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using cw3.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using cw3.DTOs.Responses;
using System;
using cw3.Handlers;
using System.Diagnostics;

namespace cw3.DAL
{
    public class SqlDbService : Controller, IDbService
    {
        public List<Student> _students;
        public SqlDbService()
        {
            _students = new List<Student>();
        }

        public IEnumerable<Student> GetStudents()
        {
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19017;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                _students.Clear();
                com.Connection = client;
                com.CommandText = "select * from Student";
                client.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student()
                    {
                        firstName = dr["FirstName"].ToString(),
                        lastName = dr["LastName"].ToString(),
                        index = dr["IndexNumber"].ToString()

                    };

                    _students.Add(st);
                }
            }

            return _students;
        }
        public IEnumerable<Enrollment> GetEnrollment(string index)
        {
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19017;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                com.CommandText = "select Semester, IdStudy, StartDate from Enrollment where IdEnrollment = (select IdEnrollment from student where IndexNumber = @index)";
                com.Parameters.AddWithValue("index", index);

                client.Open();
                var dr = com.ExecuteReader();
                var enrollments = new List<Enrollment>();
                while (dr.Read())
                {
                    var enrol = new Enrollment()
                    {
                        sem = int.Parse(dr["Semester"].ToString()),
                        idStud = int.Parse(dr["IdStudy"].ToString()),
                        date = dr["StartDate"].ToString()
                    };

                    enrollments.Add(enrol);
                }
                return enrollments;
            }
        }
        public Student UpdateStudent(string id, string varType, string value)
        {

            foreach (var student in _students)
            {
                if (student.index == id)
                {
                    if (varType == "name")
                    {
                        student.firstName = value;
                    }
                    else if (varType == "lname")
                    {
                        student.lastName = value;
                    }
                }
                return student;
            }
            return null;
        }
        public void DeleteStudent(string id)
        {
            foreach (var student in _students)
            {
                if (student.index == id)
                {
                    _students.ToList().Remove(student);
                }
            }
        }

        public Student GetStudent(string index)
        {
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19017;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                _students.Clear();
                com.Connection = client;
                com.CommandText = "select * from Student where IndexNumber = @index";
                com.Parameters.AddWithValue("index", index);
                client.Open();
                var dr = com.ExecuteReader();
                Student st = null;
                while (dr.Read())
                {
                    st = new Student()
                    {
                        firstName = dr["FirstName"].ToString(),
                        lastName = dr["LastName"].ToString(),
                        index = dr["IndexNumber"].ToString()
                    };
                }

                return st;
            }
        }
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            var st = new Student()
            {
                firstName = request.firstName,
                lastName = request.lastName,
                index = request.indexNUmber,
                birthDate = request.birthDate,
                studies = request.studies
            };

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19017;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;

                try
                {
                    com.CommandText = "select IdStudy from studies where name=@name;";
                    com.Parameters.AddWithValue("name", request.studies);

                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        return BadRequest("Studia nie istnieja");
                    }

                    int idstudies = (int)dr["IdStudy"];
                    dr.Close();
                    com.CommandText = "select e.IdEnrollment, e.Semester, e.IdStudy, e.StartDate from Enrollment e join Studies s on e.idStudy = s.IdStudy where e.semester=1 and s.name=@name;";

                    int idEnrollment;
                    dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        com.CommandText = "insert into Enrollment values (ISNULL(MAX(IdEnrollment) + 1), @Semestr, @IdStudy, GetDate())";
                        com.Parameters.AddWithValue("Semestr", 1);
                        com.Parameters.AddWithValue("IdStudy", request.studies);
                        com.ExecuteNonQuery();
                    }
                    dr.Close();
                    com.CommandText = "select e.IdEnrollment, e.Semester, e.IdStudy, e.StartDate from Enrollment e join Studies s on e.idStudy = s.IdStudy where e.semester=1 and s.name=@name;";
                    dr = com.ExecuteReader();
                    dr.Read();
                    idEnrollment = (int)dr["IdEnrollment"];
                    dr.Close();

                    com.CommandText = "select * from student where IndexNumber=@index;";
                    com.Parameters.AddWithValue("index", request.indexNUmber);
                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        return BadRequest("Student o podanym indexie istnieje w bazie");
                    }
                    dr.Close();

                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES(@Index, @Fname, @Lname, @BirthDate, @IdEnrollment)";
                    com.Parameters.AddWithValue("Fname", request.firstName);
                    com.Parameters.AddWithValue("Lname", request.lastName);
                    com.Parameters.AddWithValue("BirthDate", request.birthDate);
                    com.Parameters.AddWithValue("IdEnrollment", idEnrollment);
                    com.ExecuteNonQuery();
                    tran.Commit();

                }
                catch (SqlException e)
                {
                    tran.Rollback();
                    return BadRequest(e);
                }
                var response = new EnrollStudentResponse()
                {
                    lastName = request.lastName,
                    semester = 1,
                    startDate = DateTime.Now
                };
                return StatusCode(201, response);
            }
        }
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19017;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;

                try
                {
                    com.CommandText = "select e.IdEnrollment from Enrollment e join Studies s on e.idstudy = s.idstudy where s.name=@name and e.semester=@semester;";
                    com.Parameters.AddWithValue("name", request.studies);
                    com.Parameters.AddWithValue("semester", request.semester);

                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        return StatusCode(404, "Brak powiązania");
                    }
                    dr.Close();
                    com.CommandText = "EXEC PromoteStudent @name, @semester;";
                    dr = com.ExecuteReader();
                    dr.Read();
                    var response = new PromoteStudentResponse()
                    {
                        idEnrollment = (int)dr["IdEnrollment"],
                        studies = (string)dr["name"],
                        semester = (int)dr["semester"]
                    };
                    return StatusCode(201, response);
                }
                catch (SqlException e)
                {
                    tran.Rollback();
                    return BadRequest(e);
                }
            }
        }


        public static bool CheckIndex(string index)
        {

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19017;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;


                com.CommandText = "select 1 from Student where IndexNumber = @index";
                com.Parameters.AddWithValue("index", index);

                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    dr.Close();
                    tran.Rollback();
                    return false;
                }
                dr.Close();

                return true;

            }

        }
        public static bool RegisterAccount(Student student,string salt, string hash) {
            if (CheckIndex(student.index)) {
                return false;
            }
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19017;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;

                

                com.CommandText = "INSERT INTO STUDENT VALUES ('" + student.index
                                                                  + "','" + student.firstName
                                                                    + "','" + student.lastName
                                                                    + "','" + student.birthDate
                                                                    + "',1,'"  + hash + "','" + salt + "',null)"; 

                com.ExecuteNonQuery();
                com.Transaction.Commit();

                return true;

            }
        }
        public static bool CheckStudent(LoginRequestDto request)
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19017;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();


                com.CommandText = "select IndexNumber,password,salt from Student where IndexNumber = @index";
                com.Parameters.AddWithValue("index", request.Login);


                var dr = com.ExecuteReader();

                if (!dr.Read()) {
                    dr.Close();
                    return false;
                }
                else
                {
                    var req = new LoginRequestDto()
                    {

                        Login = (string)dr["IndexNumber"],
                        Password = (string)dr["Password"],
                        Salt = dr["salt"] != null ? (string)dr["salt"] : " "
                    };
                    Debug.WriteLine(req.Login);
                    Debug.WriteLine(req.Password);
                    Debug.WriteLine(req.Salt);


                    if (BasicAuthHandler.Validate(request.Password, req.Salt, req.Password))
                        return true;
                }



                return false;

            }
        }
        public static System.Guid? CheckRefreshToken(System.Guid token)
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19017;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();


                com.CommandText = "select 1 from Student where RefreshToken = @token";
                com.Parameters.AddWithValue("token", token);


                var dr = com.ExecuteReader();

                if (!dr.Read())
                {
                    dr.Close();

                    return null;
                }
            }
                using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19017;Integrated Security=True"))
                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    con.Open();

                    System.Guid ntoken = System.Guid.NewGuid();

                    com.CommandText = "update Student set RefreshToken = @ntoken where RefreshToken = @token";
                    com.Parameters.AddWithValue("token", token);
                    com.Parameters.AddWithValue("ntoken", ntoken);

                var dr = com.ExecuteReader();


                    return ntoken;

            }
        }
        public static bool SaveRefreshToken(string indexNumber, System.Guid token) 
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19017;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;



                com.CommandText = "UPDATE STUDENT SET RefreshToken = @token where IndexNumber = @indexNumber";
                com.Parameters.AddWithValue("token", token);
                com.Parameters.AddWithValue("indexNumber", indexNumber);

                if(com.ExecuteNonQuery() < 1) 
                    return false;
                com.Transaction.Commit();
                

                return true;

            }
        }
    }
}
