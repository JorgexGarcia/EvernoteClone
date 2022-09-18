using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace EvernoteClone.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ObservableCollection<Note> Notes { get; set; }
        private Notebook selectedNotebook;
        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                selectedNotebook = value;
                OnPropertyChanged("SelectedNotebook");
                GetNotes();
            }
        }

        private Note selectedNote;

        public Note SelectedNote
        {
            get { return selectedNote; }
            set
            {
                selectedNote = value;
                OnPropertyChanged("SelectedNote");
                SelectedNoteChanged?.Invoke(this, new EventArgs());
            }
        }

        private Visibility isVisible;
        public Visibility IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public EditCommand EditCommand { get; set; }
        public EndEditingCommand EndEditingCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SelectedNoteChanged;

        public NotesVM()
        {
            NewNoteCommand = new NewNoteCommand(this);
            NewNotebookCommand = new NewNotebookCommand(this);
            EditCommand = new EditCommand(this);
            EndEditingCommand = new EndEditingCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            IsVisible = Visibility.Collapsed;

            GetNotebooks();
        }

        public async void CreateNotebook()
        {
            Notebook newNotebook = new Notebook
            {
                Name = "Cuaderno",
                UserId = App.UserId
            };

            await DatabaseHelper.InsertFB(newNotebook);

            var temp = DatabaseHelper.Read<Notebook>().Where(n => n.Name == newNotebook.Name).FirstOrDefault();
            var temList = DatabaseHelper.Read<Notebook>().ToList();

            if (temp != null) temp.Name = $"{temList.Count} - {temp.Name}";

            DatabaseHelper.Update(temp);

            GetNotebooks();
        }

        public async void CreateNote(int notebookId)
        {
            Note newNote = new Note
            {
                NotebookId = notebookId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title = $"Nota"
            };

            await DatabaseHelper.InsertFB(newNote);

            var temp = DatabaseHelper.Read<Note>().Where(n => n.Title == newNote.Title).FirstOrDefault();

            var temList = DatabaseHelper.Read<Note>().Where(n => n.NotebookId == notebookId).ToList();

            if (temp != null) temp.Title = $"{temp.Title} - {temList.Count}";

            DatabaseHelper.Update(temp);

            GetNotes();
        }

        public void GetNotebooks()
        {
            var notebooks = DatabaseHelper.Read<Notebook>().Where(n => n.UserId == App.UserId).ToList();

            Notebooks.Clear();
            foreach(var notebook in notebooks)
            {
                Notebooks.Add(notebook);
            }
        }

        private void GetNotes()
        {
            if (SelectedNotebook != null)
            {
                var notes = DatabaseHelper.Read<Note>().Where(n => n.NotebookId == SelectedNotebook.Id).ToList();

                Notes.Clear();
                foreach (var note in notes)
                {
                    Notes.Add(note);
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void StartEditing()
        {
            IsVisible = Visibility.Visible;
        }

        public void StopEditing(Notebook notebook)
        {
            IsVisible = Visibility.Collapsed;

            var temp = DatabaseHelper.Read<Notebook>().Where(n => n.Name == notebook.Name).FirstOrDefault();

            if (temp != null) notebook.Name = $"{notebook.Name} (2)";

            DatabaseHelper.Update(notebook);
            GetNotebooks();
        }
    }
}
