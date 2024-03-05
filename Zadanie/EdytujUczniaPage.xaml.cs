using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Zadanie.Models;

namespace Zadanie
{
    public partial class EdytujUczniaPage : ContentPage
    {
        public event EventHandler<(Student editedStudent, string oldClass)> UczenEdytowany;


        private Student editedStudent;
        private ListView studentsListView;
        private List<Student> students;

        private string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "students.txt");

        public EdytujUczniaPage(Student studentToEdit, ListView listView, List<Student> studentsList)
        {
            InitializeComponent();
            editedStudent = studentToEdit;
            studentsListView = listView;
            students = studentsList;

            if (editedStudent != null)
            {
               
                entryImie.Text = editedStudent.Imie;
                entryNazwisko.Text = editedStudent.Nazwisko;
                entryKlasa.Text = editedStudent.Klasa;
                entryNumer.Text = editedStudent.Numer;
            }
        }

        private async void EdytujUcznia(object sender, EventArgs e)
        {
            if (studentsListView.SelectedItem != null)
            {
                var existingStudent = studentsListView.SelectedItem as Student;

                if (existingStudent != null)
                {
                    existingStudent.Imie = entryImie.Text;
                    existingStudent.Nazwisko = entryNazwisko.Text;
                    existingStudent.Klasa = entryKlasa.Text;
                    existingStudent.Numer = entryNumer.Text;

                    SaveStudentsList(students, FilePath);

                    UczenEdytowany?.Invoke(this, (existingStudent, existingStudent.Klasa));

                    LoadStudentsList();
                }

                var newListaUczniowPage = new ListaUczniowPage();

                
                Application.Current.MainPage = new NavigationPage(newListaUczniowPage);
            }
        }

    


    private void LoadStudentsList()
        {
            ObservableCollection<Student> Students = new ObservableCollection<Student>(Student.LoadStudentsList(FilePath));
        }

      
        private static void SaveStudentsList(List<Student> students, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var student in students)
                {
                    writer.WriteLine($"{student.Klasa},{student.Imie},{student.Nazwisko},{student.Numer}");
                }
            }
        }
    }
}
