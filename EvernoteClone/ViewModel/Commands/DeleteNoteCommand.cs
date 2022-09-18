using EvernoteClone.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class DeleteNoteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public NotesVM ViewModel { get; set; }

        public DeleteNoteCommand(NotesVM vm)
        {
            ViewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Note note = parameter as Note;
            if (note != null)
                ViewModel.DeleteNote(note);
        }
    }
}
