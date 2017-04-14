using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Markup.Localizer;
using System.Windows.Media;

namespace OnedriveSwitch
{
    public class ViewModel:ViewModelBase
    {
        private Model Model = Model.Instance;

        public ICommand Enable { get; }
        public ICommand Disable { get; }

        public ViewModel()
        {
            Enable = new RelayCommand(()=>
            {
                Model.DoEnable();
                CurrentState = true;
                ChangeButton();

                if (MessageBox.Show("有効にしました。" + Environment.NewLine + "サインアウトしますか？。", "OnedriveSwitch", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    SignOut();
            });

            Disable = new RelayCommand(() =>
            {
                Model.DoDisable();
                CurrentState = false;
                ChangeButton();

                if (MessageBox.Show("無効にしました。" + Environment.NewLine + "サインアウトしますか？。", "OnedriveSwitch", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    SignOut();
            });

            Model.GetCurrentStatus();
            ChangeButton();
        }

        public bool CurrentState
        {
            get => Model.CurrentState;
            set
            {
                Model.CurrentState = value;
                OnPropertyChanged("CurrentStateText");
            }
        }

        public string CurrentStateText => Model.CurrentState ? "OneDrive：有効" : "OneDrive：無効";

        private bool _enableButton;
        public bool EnableButton
        {
            get => _enableButton;
            set
            {
                _enableButton = value;
                OnPropertyChanged("EnableButton");
            }
        }

        private bool _disableButton;
        public bool DisableButton
        {
            get => _disableButton;
            set
            {
                _disableButton = value;
                OnPropertyChanged("DisableButton");
            }
        }

        public void ChangeButton()
        {
            if (CurrentState)
            {
                EnableButton = false;
                DisableButton = true;
            }
            else
            {
                EnableButton = true;
                DisableButton = false;
            }
        }

        private static void SignOut()
        {
            var signOut = new ProcessStartInfo
            {
                FileName = "shutdown.exe",
                Arguments = "/l",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process.Start(signOut);
        }
    }
}
