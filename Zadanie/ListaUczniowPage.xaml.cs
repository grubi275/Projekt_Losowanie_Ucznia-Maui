using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Zadanie.Models;

namespace Zadanie
{
    public partial class ListaUczniowPage : ContentPage, INotifyPropertyChanged
    {
        private ObservableCollection<Student> _students = new ObservableCollection<Student>();
        private string FilePath;

        
        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                if (_students != value)
                {
                    _students = value;
                    OnPropertyChanged(nameof(Students));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ListaUczniowPage()
        {
            InitializeComponent();
            FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "students.txt");
            LoadStudentsList();
            BindingContext = this;

            var distinctClasses = Students.Select(s => s.Klasa).Distinct().ToList();
            distinctClasses.Insert(0, "Wszystkie");
            wybierzKlase.ItemsSource = distinctClasses;
            wybierzKlase.SelectedIndex = 0;

            wybierzKlase.SelectedIndexChanged += WybierzKlase_SelectedIndexChanged;
        }


        private void WybierzKlase_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedClass = (string)wybierzKlase.SelectedItem;

            if (selectedClass == "Wszystkie")
            {
                LoadStudentsList();
            }
            else
            {
                var filteredStudents = Student.LoadStudentsList(FilePath).Where(s => s.Klasa == selectedClass).ToList();
                LoadStudentsList();
             
                var newStudents = new ObservableCollection<Student>(filteredStudents);

                
                Students = newStudents;
            }
        }
        private async void DodajUcznia_Clicked(object sender, EventArgs e)
        {
            var dodajUczniaPage = new DodajUczniaPage(Students);
            dodajUczniaPage.UczenDodany += (s, newUczen) =>
            {
                Students.Add(newUczen);
                Student.SaveStudentsList(Students.ToList(), FilePath);


                string newClass = newUczen.Klasa;

                if (!wybierzKlase.ItemsSource.Contains(newClass))
                {

                    var newClassesList = wybierzKlase.ItemsSource.Cast<object>().ToList();

                    newClassesList.Add(newClass);


                    wybierzKlase.ItemsSource = newClassesList;
                }
            };

            await Navigation.PushAsync(dodajUczniaPage);
        }

        private void LosujUcznia_Clicked(object sender, EventArgs e)
        {
            if (Students.Any())
            {
                Random random = new Random();
                int index = random.Next(0, Students.Count);
                var wylosowanyUczen = Students[index];
                uczen.Text = wylosowanyUczen.ToString();
            }
        }
        private void LoadStudentsList()
        {
            Students = new ObservableCollection<Student>(Student.LoadStudentsList(FilePath));
           
        }

        private async void UsunUcznia(object sender, EventArgs e)
        {
            if (studentsListView.SelectedItem != null)
            {
                var existingStudent = (Student)studentsListView.SelectedItem;
                Students.Remove(existingStudent);
                Student.SaveStudentsList(Students.ToList(), FilePath);

                if (!string.IsNullOrEmpty(existingStudent.Klasa))
                {
                    UpdateClassesPicker();
                }
            }
        }

        private void UpdateClassesPicker()
        {
            var distinctClasses = Students.Select(s => s.Klasa).Distinct().ToList();
            distinctClasses.Insert(0, "Wszystkie");

         
            var uniqueClasses = distinctClasses.Distinct().ToList();

     
            wybierzKlase.ItemsSource = uniqueClasses;

            var existingStudent = (Student)studentsListView.SelectedItem;

           
            if (!uniqueClasses.Contains(existingStudent.Klasa))
            {
   
                uniqueClasses.Add(existingStudent.Klasa);
                wybierzKlase.ItemsSource = uniqueClasses;
            }

            
            wybierzKlase.SelectedItem = existingStudent.Klasa;
        }



        private async void EdytujUcznia_Clicked(object sender, EventArgs e)
        {
            if (studentsListView.SelectedItem != null)
            {
                Student selectedStudent = studentsListView.SelectedItem as Student;

                if (selectedStudent != null)
                {
                    var edytujUczniaPage = new EdytujUczniaPage(selectedStudent, studentsListView, Students.ToList());
                    edytujUczniaPage.UczenEdytowany += (s, editedUczenInfo) =>
                    {
                        var editedUczen = editedUczenInfo.editedStudent;
                        string oldClass = editedUczenInfo.oldClass;

                       
                        if (Students.Contains(selectedStudent))
                        {
                            int index = Students.IndexOf(selectedStudent);
                            Students[index] = editedUczen;

                            string newClass = editedUczen.Klasa;

                            if (!wybierzKlase.ItemsSource.Contains(newClass))
                            {

                                var newClassesList = wybierzKlase.ItemsSource.Cast<object>().ToList();

                                newClassesList.Add(newClass);


                                wybierzKlase.ItemsSource = newClassesList;
                                LoadStudentsList();
                            }


                            Student.SaveStudentsList(Students.ToList(), FilePath);
                        }
                    };

                    await Navigation.PushAsync(edytujUczniaPage);
                }
            }
        }

    }
}