using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Zadanie.Models
{
    public class Student
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Klasa { get; set; }
        public string Numer { get; set; }

        public override string ToString()
        {
            return $"{Imie} {Nazwisko}, {Numer}";
        }

        public static void SaveStudentsList(List<Student> students, string filePath)
        {
            
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var student in students)
                    {
                        writer.WriteLine($"{student.Klasa},{student.Imie},{student.Nazwisko},{student.Numer}");
                    }
                }
            
            
        }

        public static List<Student> LoadStudentsList(string filePath)
        {
            List<Student> students = new List<Student>();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length == 4)
                    {
                        Student student = new Student
                        {
                            Klasa = parts[0],
                            Imie = parts[1],
                            Nazwisko = parts[2],
                            Numer = parts[3]

                        };

                        students.Add(student);
                    }
                }
            }

            return students;
        }
       



    }
}
