using example1Exam.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace example1Exam.Services
{
    public class DbService : IdbService
    {
        public IEnumerable<Animal> getAnimals(string sortBy)
        {
            IEnumerable<Animal> animals = new List<Animal>();
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19017;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                com.CommandText = "SELECT a.Name, a.Type, a.AdmissionDate, o.LastName " +
                    "FROM Animal a" +
                    "JOIN Owner o" +
                    "ON a.IdOwner = o.IdOwner" +
                    "ORDER BY @sortBy";
                com.Parameters.AddWithValue("sortBy", sortBy);

                client.Open();
                var dr = com.ExecuteReader();
                
                while (dr.Read())
                {
                    Animal animal = new Animal()
                    {
                        Name = dr["Name"].ToString(),
                        AnimalType = dr["Type"].ToString(),
                        DateOfAdmission = DateTime.Parse(dr["AdmissionDate"].ToString()),
                        LastNameOfOwner = dr["LastName"].ToString()

                    };

                    animals.Append(animal);
                    
                }
            }

            return animals;
        }
    }
}
