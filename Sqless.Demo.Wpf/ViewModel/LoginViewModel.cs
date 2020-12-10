using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Sqless.Api;
using Sqless.Demo.Common;
using Sqless.Request;
using Sqless.Demo.Wpf.Helpers;
using System;
using System.Windows;
using System.Windows.Input;

namespace Sqless.Demo.Wpf.ViewModel
{
    class LoginViewModel : ViewModelBase
    {
        private string _account = ConfigHelper.Instance.Account;
        public string Account
        {
            get => _account;
            set => Set(ref _account, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        private ICommand _loginCommand = null;
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new RelayCommand(async () =>
                      {
                          if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(Password))
                          {
                              MessageBox.Show("account and password can't be empty");
                              return;
                          }

                          var request = new SqlessSelectRequest()
                          {
                              Table = "User",
                              AccessParams = new string[] { Account, Password }
                          };
                          request.LoadFromType(typeof(User), null, nameof(User.Password));

                          try
                          {
                              WpfGlobal.LoginUser = await SqlessClient.SelectFirstOrDefault<User>(request);
                              WpfGlobal.LoginUser.Password = Password;
                              ConfigHelper.Instance.Account = Account;

                              OnLoginFinished?.Invoke();
                          }
                          catch (SqlessRequestException ex)
                          {
                              MessageBox.Show(ex.Message);
                              return;
                          }
                      });
                }
                return _loginCommand;
            }
        }

        public Action OnLoginFinished;
    }
}
