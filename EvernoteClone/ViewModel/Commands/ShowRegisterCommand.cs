using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class ShowRegisterCommand : ICommand
    {
        public LoginVM ViewModel { get; set; }

        public event EventHandler CanExecuteChanged;

        public ShowRegisterCommand(LoginVM vw)
        {
            ViewModel = vw;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute (object parameter)
        {
           ViewModel.SwitchViews();
        }
    }
}
