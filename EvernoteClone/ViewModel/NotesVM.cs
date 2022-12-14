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

        public async void DeleteNote(Note note)
        {
            try
            {
                await DatabaseHelper.DeleteFB(note);
                GetNotes();
            }
            catch (Exception e)
            {

            }
        }

        public async void DeleteNotebook(Notebook notebook)
        {
            try
            {
                await DatabaseHelper.DeleteFB(notebook);
                GetNotebooks();
            }
            catch (Exception e)
            {

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
        public DeleteNotebookCommand DeleteNotebookCommand { get; set; }
        public DeleteNoteCommand DeleteNoteCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SelectedNoteChanged;

        public NotesVM()
        {
            NewNoteCommand = new NewNoteCommand(this);
            NewNotebookCommand = new NewNotebookCommand(this);
            EditCommand = new EditCommand(this);
            EndEditingCommand = new EndEditingCommand(this);
            DeleteNotebookCommand = new DeleteNotebookCommand(this);
            DeleteNoteCommand = new DeleteNoteCommand(this);

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

            GetNotebooks();
        }

        public async void CreateNote(string notebookId)
        {
            Note newNote = new Note
            {
                NotebookId = notebookId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title = $"Nota"
            };

            await DatabaseHelper.InsertFB(newNote);

            GetNotes();
        }

        public async void GetNotebooks()
        {
            var notebooks = (await DatabaseHelper.ReadFB<Notebook>()).Where(n => n.UserId == App.UserId);

            Notebooks.Clear();
            foreach(var notebook in notebooks)
            {
                Notebooks.Add(notebook);
            }
        }

        private async void GetNotes()
        {
            if (SelectedNotebook != null)
            {
                var notes =(await DatabaseHelper.ReadFB<Note>()).Where(n => n.NotebookId == SelectedNotebook.Id).ToList();

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

        public async void StopEditing(Notebook notebook)
        {
            IsVisible = Visibility.Collapsed;

            await DatabaseHelper.UpdateFB(notebook);
            GetNotebooks();
        }
    }
}
