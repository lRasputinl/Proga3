using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Proga3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string filePath = "notes.txt";
        private List<Note> notes = new List<Note>();
        private int selectedIndex = -1;

        public MainWindow()
        {
            InitializeComponent();
            LoadNotes();
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            string title = NoteTitleTextBox.Text.Trim();
            string text = NoteTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(text))
            {
                notes.Add(new Note { Title = title, Text = text });
                RefreshNotes();
                NoteTitleTextBox.Clear();
                NoteTextBox.Clear();
            }
        }

        private void SaveNote_Click(object sender, RoutedEventArgs e)
        {
            if (selectedIndex >= 0 && selectedIndex < notes.Count)
            {
                string title = NoteTitleTextBox.Text.Trim();
                string text = NoteTextBox.Text.Trim();
                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(text))
                {
                    notes[selectedIndex].Title = title;
                    notes[selectedIndex].Text = text;
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
                NoteTitleTextBox.Clear();
                NoteTextBox.Clear();
            }
        }

        private void NotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedIndex = NotesListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < notes.Count)
            {
                Note selectedNote = notes[selectedIndex];
                NoteTitleTextBox.Text = selectedNote.Title;
                NoteTextBox.Text = selectedNote.Text;
            }
        }

        private void RefreshNotes()
        {
            // Чтобы обновить список в ListBox, можно заново задать ItemsSource
            NotesListBox.ItemsSource = null;
            NotesListBox.ItemsSource = notes;
        }

        private void LoadNotes()
        {
            if (File.Exists(filePath))
            {
                string[] noteLines = File.ReadAllLines(filePath);
                foreach (var line in noteLines)
                {
                    // Используем уникальный разделитель ";|;" для разделения названия и текста
                    string[] parts = line.Split(new string[] { ";|;" }, StringSplitOptions.None);
                    if (parts.Length >= 2)
                    {
                        notes.Add(new Note { Title = parts[0], Text = parts[1] });
                    }
                }
                RefreshNotes();
            }
        }

        private void SaveNotes()
        {
            List<string> lines = new List<string>();
            foreach (var note in notes)
            {
                // Сохраняем заметку в виде строки: "название;|;содержание"
                lines.Add($"{note.Title};|;{note.Text}");
            }
            File.WriteAllLines(filePath, lines);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            SaveNotes();
            base.OnClosing(e);
        }
    }

    // Класс заметки с полями для названия и содержания
    public class Note
    {
        public string Title { get; set; }
        public string Text { get; set; }

        // Переопределяем ToString, чтобы ListBox отображал название заметки
        public override string ToString()
        {
            return Title;
        }
    }
}
