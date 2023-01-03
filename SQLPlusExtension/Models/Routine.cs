using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SQLPlusExtension.Models
{
    public class Routine : INotifyPropertyChanged, IDataErrorInfo
    {
        public Action IsSelectedChangedCallback { set; get; }

        private string _Schema;
        public string Schema
        {
            get
            {
                return _Schema;
            }
            set
            {
                if (_Schema != value)
                {
                    _Schema = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Schema"));
                }
            }
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }

        private string _Namespace;
        public string Namespace
        {
            get
            {
                return _Namespace;
            }
            set
            {
                if (_Namespace != value)
                {
                    _Namespace = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Namespace"));
                }
            }
        }

        public void SetToSchemaSelection(bool isSelected, string nameSpace)
        {
            if(_IsSelected != isSelected)
            {
                _IsSelected = isSelected;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
            if(_Namespace != nameSpace)
            {
                _Namespace = nameSpace;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Namespace)));
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    IsSelectedChangedCallback?.Invoke();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
                }
            }
        }

        private string error = null;
        public string Error
        {
            get
            {
                return error;
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Namespace))
                {
                    if (string.IsNullOrEmpty(_Namespace))
                    {
                        return "Namespace is required";
                    }
                    else
                    {

                        if (!Regex.IsMatch(_Namespace, "^[a-zA-Z_][a-zA-Z0-9_]*$"))
                        {
                            return "Must be a valid C# Identifier";
                        }

                        if (Constants.CSharpKeywords.Contains(_Namespace))
                        {
                            return $"{_Namespace} is a C# reserved word.";
                        }
                    }
                }
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _RoutineError;
        public string RoutineError
        {
            set
            {
                if(_RoutineError != value)
                {
                    _RoutineError = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RoutineError)));
                }
            }
            get
            {
                return _RoutineError;
            }
        }

        private bool _HasError = false;
        public bool HasError
        {
            set
            {
                if (_HasError != value)
                {
                    _HasError = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasError)));
                }
            }
            get
            {
                return _HasError;
            }
        }
    }
}
