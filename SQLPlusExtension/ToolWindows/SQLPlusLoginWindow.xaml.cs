using Microsoft.VisualStudio.Debugger.Interop;
using Microsoft.VisualStudio.PlatformUI;
using SQLPlusExtension.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SQLPlusExtension
{

    public enum SubscriptionTypes : System.Byte
    {
        /// <summary>
        /// Enumerated value for IndividualCommunityMonthly
        /// </summary>
        IndividualCommunityMonthly = 1,

        /// <summary>
        /// Enumerated value for IndividualProfessionalMonthly
        /// </summary>
        IndividualProfessionalMonthly = 2,

        /// <summary>
        /// Enumerated value for IndividualProfessionalYearly
        /// </summary>
        IndividualProfessionalYearly = 3,

        /// <summary>
        /// Enumerated value for TeamProfessionalMonthly
        /// </summary>
        TeamProfessionalMonthly = 4,

        /// <summary>
        /// Enumerated value for TeamProfessionalYearly
        /// </summary>
        TeamProfessionalYearly = 5


    }

    public class Customer
    {
        public string CustomerToken { set; get; }
        public string Email { set; get; }
        public DateTime Expires { set; get; }
        public SubscriptionTypes SubscriptionType { set; get; }
        public long FilesRendered { set; get; }
    }

    public class LoginViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        
        
        private EmailAddressAttribute _emailAttribute = new EmailAddressAttribute();
        private MaxLengthAttribute _emailMaxLengthAttribute = new MaxLengthAttribute(64);
        private StringLengthAttribute _passwordStringLengthAttrinute = new StringLengthAttribute(16) { MinimumLength = 8 };

        public event PropertyChangedEventHandler PropertyChanged;

        private string _Email;
        public string Email
        {
            set
            {
                if(value != _Email)
                {
                    _Email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
                    LoginCommand.RaiseCanExecuteChanged();
                }
            }
            get
            {
                return _Email;
            }
        }

        private string _Password = string.Empty;
        public string Password
        {
            set
            {
                if(value != _Password)
                {
                    if(value.Length > _Password.Length)
                    {
                        _Password = _Password + value[value.Length - 1];
                    }
                    else
                    {
                        _Password = _Password.Substring(0, _Password.Length - 1);
                    }
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(PasswordValue)));
                    LoginCommand.RaiseCanExecuteChanged();
                }
            }
            get
            {
                return new string('*', _Password.Length);
            }
        }

        public RelayCommand LoginCommand { set; get; }

        private string error = null;
        public string Error
        {
            get
            {
                return error;
            }
        }

        public string PasswordValue
        {
            get
            {
                return _Password;
            }
        }

        private string _ErrorMessage = null;
        public string ErrorMessage
        {
            set
            {
                if(_ErrorMessage != value)
                {
                    _ErrorMessage = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
                    LoginCommand.RaiseCanExecuteChanged();
                }
            }
            get
            {
                return _ErrorMessage;
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Email))
                {
                    if (string.IsNullOrEmpty(Email))
                    {
                        return "Email is required";
                    }
                    else
                    {
                        if(!_emailAttribute.IsValid(Email))
                        {
                            return "Email is not a valid email address.";
                        }
                        if(!_emailMaxLengthAttribute.IsValid(_Email))
                        {
                            return "Email is too long, max 64 characters";
                        }
                    }
                }
                if(columnName == nameof(Password))
                {
                    if (string.IsNullOrEmpty(_Password))
                    {
                        return "Password is required";
                    }
                    if(_Password.Length < 8 || _Password.Length > 16)
                    {
                        return "Password must be between 8 and 16 characters";
                    }
                }
                return null;
            }
        }

        public LoginViewModel()
        {
            InitCommands();
            LoginCommand.RaiseCanExecuteChanged();
        }

        private void InitCommands()
        {
            LoginCommand = new RelayCommand
               (
                   (o) =>
                   {
                       return IsValid();
                   },
                   (o) =>
                   {
                       TryLogin();
                   }
               );
        }

        private async Task<bool> TryLogin()
        {
            var client = new HttpClient();

            await Task.Delay(1);

            return true;
            
        }

        private bool IsValid()
        {
            return (this[nameof(Email)] is null && this[nameof(Password)] is null);
        }
    }

    public partial class SQLPlusLoginWindow : DialogWindow
    {
        /// <summary>
        /// Used to launch a browser on a hyperlink click
        /// </summary>
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }

        public SQLPlusLoginWindow()
        {
            this.DataContext = new LoginViewModel();
            InitializeComponent();
        }

        public SQLPlusLoginWindow(LoginViewModel login)
        {
            
            InitializeComponent();
            this.DataContext = login;

            //this.DialogResult = true;
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            string password = passwordBox.Password;
            ((LoginViewModel)this.DataContext).Password = password;
        }
    }
}
