using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SQLPlusExtension.Models
{
    public class QueryBase : INotifyPropertyChanged, IDataErrorInfo
    {
        public QueryBase(Action<object> deleteCallback)
        {
            DeleteCommand = new RelayCommand
                (
                    (o) =>
                    {
                        return true;
                    },
                    (o) =>
                    {
                        deleteCallback?.Invoke(this);
                    }
                );
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }


        private string _Query;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Query
        {
            get
            {
                return _Query;
            }
            set
            {
                if (_Query != value)
                {
                    _Query = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Query)));
                }
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Name):
                        if (string.IsNullOrEmpty(_Name))
                        {
                            return "Name field is required";
                        }
                        else
                        {
                            if (!Regex.IsMatch(_Name, "^[a-zA-Z_][a-zA-Z0-9_]*$"))
                            {
                                return "Name must be a valid C# Identifier";
                            }
                            else if (Constants.CSharpKeywords.Contains(_Name))
                            {
                                return $"{_Name} is a C# reserved word.";
                            }
                        }
                        return string.Empty;

                    case nameof(Query):
                        if (string.IsNullOrEmpty(_Query))
                        {
                            return "Text field is required";
                        }
                        return string.Empty;

                    default:
                        return string.Empty;
                }
            }
        }

        private string _QueryError;
        public string QueryError
        {
            set
            {
                if (_QueryError != value)
                {
                    _QueryError = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QueryError)));
                }
                HasError = !string.IsNullOrEmpty(_QueryError);
            }
            get
            {
                return _QueryError;
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

        public RelayCommand DeleteCommand { private set; get; }
    }
}
