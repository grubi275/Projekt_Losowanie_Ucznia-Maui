using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using Zadanie.Models;

namespace Zadanie
{
    public partial class DodajUczniaPage : ContentPage
    {
        public event EventHandler<Student> UczenDodany;
        public event EventHandler<Student> UczenEdytowany;

        private ObservableCollection<Student> Students; 
        string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "students.txt");
        private Student nowyStudent;

        public DodajUczniaPage(ObservableCollection<Student> students)
        {
            InitializeComponent();
            Students = students;
            nowyStudent = null;
        }

        public DodajUczniaPage(Student studentToEdit)
        {
            InitializeComponent();
            nowyStudent = studentToEdit;

            if (nowyStudent != null)
            {
                
                entryImie.Text = nowyStudent.Imie;
                entryNazwisko.Text = nowyStudent.Nazwisko;
                entryKlasa.Text = nowyStudent.Klasa;
            }
        }

        private void DodajUcznia_Clicked(object sender, EventArgs e)
        {
            
       

                var nowyUczen = new Student
                {
                    Numer = entryNumer.Text,
                    Imie = entryImie.Text,
                    Nazwisko = entryNazwisko.Text,
                    Klasa = entryKlasa.Text
                };

                
                    UczenDodany?.Invoke(this, nowyUczen);
            

                Navigation.PopAsync(); 
            
        }

        
    }
}

