#region usings

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;

#endregion

namespace SQLPlusExtension.Models
{
    public class Schema : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Properties

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
                    if (_IsSelected)
                    {
                        foreach (Routine routine in _Routines)
                        {
                            routine.Namespace = _Namespace;
                        }
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Namespace"));
                }
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
                    if (_IsSelected)
                    {
                        if (Routines != null)
                        {
                            foreach (Routine routine in Routines)
                            {
                                routine.SetToSchemaSelection(_IsSelected, _Namespace);
                            }
                        }
                    }
                    else
                    {

                        if (Routines != null)
                        {
                            bool allSelected = true;

                            foreach (Routine routine in Routines)
                            {
                                if (!routine.IsSelected)
                                {
                                    allSelected = false;
                                    break;
                                }
                            }

                            if (allSelected)
                            {
                                foreach (Routine routine in Routines)
                                {
                                    routine.IsSelected = false;
                                }
                            }
                        }
                    }

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        private ObservableCollection<Routine> _Routines;
        public ObservableCollection<Routine> Routines
        {
            get
            {
                return _Routines;
            }
            set
            {
                if (_Routines != value)
                {
                    _Routines = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Routines"));
                }
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

        #endregion Properties

        #region Callbacks

        /// <summary>
        /// Provides the cummunication from a child object routine to the schema.
        /// Selects the schema if all routines are selected and routines have the same namespace as the schema.
        /// </summary>
        public void RoutineSelectedCallback()
        {
            if (_Routines is not null)
            {
                bool isSelected = true;

                foreach (var item in Routines)
                {
                    if (item.IsSelected == false)
                    {
                        isSelected = false;
                        break;
                    }
                    if(item.Namespace != _Namespace)
                    {
                        isSelected = false;
                        break;
                    }
                }
                IsSelected = isSelected;
            }
        }

        #endregion Callbacks

        #region IDataErrorInfo

        public string Error
        {
            get
            {
                return null;
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

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        
        #endregion INotifyPropertyChanged
 
    }
}
