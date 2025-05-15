using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Proga3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string filePath = "notes.txt";
        private List<string> notes = new List<string>();
        private int selectedIndex = -1;
        public MainWindow()
        {
            InitializeComponent();
            LoadNotes();
        }
        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            string text = NoteTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(text))
            {
                notes.Add(text);
                RefreshNotes();
                NoteTextBox.Clear();
            }
        }
        private void SaveNote_Click(object sender, RoutedEventArgs e)
        {
            if (selectedIndex >= 0 && selectedIndex < notes.Count)
            {
                string updatedText = NoteTextBox.Text.Trim();
                if (!string.IsNullOrEmpty(updatedText))
                {
                    notes[selectedIndex] = updatedText;
                    RefreshNotes();
                }
            }
        }

        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            if (selectedIndex >= 0 && selectedIndex < notes.Count)
            {
                notes.RemoveAt(selectedIndex);
                RefreshNotes();
                NoteTextBox.Clear();
            }
        }

        private void NotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedIndex = NotesListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < notes.Count)
            {
                NoteTextBox.Text = notes[selectedIndex];
            }
        }

        private void RefreshNotes()
        {
            NotesListBox.ItemsSource = null;
            NotesListBox.ItemsSource = notes;
        }

        private void LoadNotes()
        {
            if (File.Exists(filePath))
            {
                notes = new List<string>(File.ReadAllLines(filePath));
                RefreshNotes();
            }
        }

        private void SaveNotes()
        {
            File.WriteAllLines(filePath, notes);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            SaveNotes();
            base.OnClosing(e);
        }
    }
}
