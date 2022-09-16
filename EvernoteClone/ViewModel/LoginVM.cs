using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace EvernoteClone.ViewModel
{
    public class LoginVM : INotifyPropertyChanged
    {
        private bool isShowRegister = false;

        private User user;
        public User User
        {
            get { return user; }
            set
            { 
                user = value;
                OnPropertyChanged("User");
            }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set 
            {
                username = value;
                User = new User
                {
                    Username = username,
                    Password = this.Password
                };
                OnPropertyChanged("Username");
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                User = new User
                {
                    Username = this.Username,
                    Password = password
                };
                OnPropertyChanged("Password");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private Visibility loginVis;

        public Visibility LoginVis
        {
            get { return loginVis; }
            set 
            { 
                loginVis = value;
                OnPropertyChanged("LoginVis");
            }
        }

        private Visibility registerVis;

        public Visibility RegisterVis
        {
            get { return registerVis; }
            set
            {
                registerVis = value;
                OnPropertyChanged("RegisterVis");
            }
        }


        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }
        public ShowRegisterCommand ShowRegisterCommand { get; set; }

        public LoginVM()
        {
            loginVis = Visibility.Visible;
            registerVis = Visibility.Collapsed;

            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);
            ShowRegisterCommand = new ShowRegisterCommand(this);

            User = new User();
        }

        public void SwitchViews()
        {
            isShowRegister = !isShowRegister;

            if (isShowRegister)
            {
                RegisterVis = Visibility.Visible;
                LoginVis = Visibility.Collapsed;
            }
            else
            {
                RegisterVis = Visibility.Collapsed;
                LoginVis = Visibility.Visible;
            }
        }

        public void Login()
        {

        }

        public void Register()
        {

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
