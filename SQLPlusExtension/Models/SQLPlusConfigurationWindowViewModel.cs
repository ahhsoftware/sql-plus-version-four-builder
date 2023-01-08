using SQLPLUS.Builder;
using SQLPLUS.Builder.BuildServices;
using SQLPLUS.Builder.ConfigurationModels;
using SQLPLUS.Builder.DataCollectors;
using SQLPLUS.Builder.Render;
using SQLPLUS.Builder.Render.T4Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SQLPlusExtension.Models
{
    public class SQLPlusConfigurationWindowViewModel : INotifyPropertyChanged
    {
        private ConfigurationService _ConfigurationService;
        private ProjectInformation _ProjectInformation;
        private BuildDefinition _BuildDefinition;
        private DatabaseConnection _DatabaseConnection;
        private Project _Project;

        private List<string> filesAdded;

        public SQLPlusConfigurationWindowViewModel(ConfigurationService configurationService, ProjectInformation projectInformation, BuildDefinition buildDefinition, DatabaseConnection databaseConnection, Project vsProject)
        {
            _ConfigurationService = configurationService;
            _ProjectInformation = projectInformation;
            _BuildDefinition = buildDefinition;
            _DatabaseConnection = databaseConnection;
            _Project = vsProject;

            InitCommands();

            DatabaseConnectionToUI();

        }

        private IDataCollector GetDataCollector(DataCollectorModes mode)
        {
            return new MSSQLDataCollector(_BuildDefinition, _DatabaseConnection, _ProjectInformation) { DataCollectorMode = mode };
        }

        #region UI Bound BuildDefinition Properties and Methods

        private ObservableCollection<Schema> _SQLRoutines;
        public ObservableCollection<Schema> SQLRoutines
        {
            get
            {
                return _SQLRoutines;
            }
            set
            {
                if (_SQLRoutines != value)
                {
                    _SQLRoutines = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SQLRoutines)));
                }
            }
        }

        private ObservableCollection<Schema> _SQLQueries;
        public ObservableCollection<Schema> SQLQueries
        {
            get
            {
                return _SQLQueries;
            }
            set
            {
                if (_SQLQueries != value)
                {
                    _SQLQueries = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SQLQueries)));
                }
            }
        }

        private ObservableCollection<StaticQuery> _StaticQueries;
        public ObservableCollection<StaticQuery> StaticQueries
        {
            get
            {
                return _StaticQueries;
            }
            set
            {
                if (_StaticQueries != value)
                {
                    _StaticQueries = value;
                    RaisePropertyChanged(nameof(StaticQueries));
                }
            }
        }

        private ObservableCollection<EnumQuery> _EnumQueries;
        public ObservableCollection<EnumQuery> EnumQueries
        {
            get
            {
                return _EnumQueries;
            }
            set
            {
                if (_EnumQueries != value)
                {
                    _EnumQueries = value;
                }
                RaisePropertyChanged(nameof(EnumQueries));
            }
        }
        
        private ObservableCollection<BuildItem> _BuildOutput;
        public ObservableCollection<BuildItem> BuildOutput
        {
            set
            {
                if (_BuildOutput != value)
                {
                    _BuildOutput = value;
                    RaisePropertyChanged(nameof(BuildOutput));
                }
            }
            get
            {
                return _BuildOutput;
            }
        }

        private string _SQLClientNamespace;
        public string SQLClientNamespace
        {
            get
            {
                return _SQLClientNamespace;
            }
            set
            {
                if (_SQLClientNamespace != value)
                {
                    _SQLClientNamespace = value;
                    RaisePropertyChanged(nameof(SQLClientNamespace));
                }
            }
        }

        private bool _ImplementINotifyPropertyChanged;
        public bool ImplementINotifyPropertyChanged
        {
            get
            {
                return _ImplementINotifyPropertyChanged;
            }
            set
            {
                if (_ImplementINotifyPropertyChanged != value)
                {
                    _ImplementINotifyPropertyChanged = value;
                    RaisePropertyChanged(nameof(ImplementINotifyPropertyChanged));
                }
            }
        }

        private bool _ImplementIChangeTracking;
        public bool ImplementIChangeTracking
        {
            get
            {
                return _ImplementIChangeTracking;
            }
            set
            {
                if (_ImplementIChangeTracking != value)
                {
                    _ImplementIChangeTracking = value;
                    RaisePropertyChanged(nameof(ImplementIChangeTracking));
                    if (value == true)
                    {
                        ImplementIRevertibleChangeTracking = false;
                    }
                }
            }
        }

        private bool _ImplementIRevertibleChangeTracking;
        public bool ImplementIRevertibleChangeTracking
        {
            get
            {
                return _ImplementIRevertibleChangeTracking;
            }
            set
            {
                if (_ImplementIRevertibleChangeTracking != value)
                {
                    _ImplementIRevertibleChangeTracking = value;
                    RaisePropertyChanged(nameof(ImplementIRevertibleChangeTracking));
                    if (value == true)
                    {
                        ImplementIChangeTracking = false;
                    }
                }
            }
        }

        public bool _IncludeAsynchronousMethods;
        public bool IncludeAsynchronousMethods
        {
            get
            {
                return _IncludeAsynchronousMethods;
            }
            set
            {
                if (_IncludeAsynchronousMethods != value)
                {
                    _IncludeAsynchronousMethods = value;
                    RaisePropertyChanged(nameof(IncludeAsynchronousMethods));
                }
            }
        }

        private bool _UseNullableReferenceTypes;
        public bool UseNullableReferenceTypes
        {
            set
            {
                if (_UseNullableReferenceTypes != value)
                {
                    _UseNullableReferenceTypes = value;
                    RaisePropertyChanged(nameof(UseNullableReferenceTypes));
                }
            }
            get
            {
                return _UseNullableReferenceTypes;
            }
        }

        private void RefreshBuildDefinitionFromUi()
        {
            BuildDefinition buildDefinition = new BuildDefinition();

            buildDefinition.DBSchemas = new List<BuildSchema>();
            buildDefinition.DBRoutines = new List<BuildRoutine>();
            buildDefinition.QuerySchemas = new List<BuildSchema>();
            buildDefinition.QueryRoutines = new List<BuildRoutine>();
            buildDefinition.StaticQueries = new List<BuildQuery>();
            buildDefinition.EnumQueries = new List<BuildQuery>();
            buildDefinition.BuildOptions = new BuildOptions();

            if (_SQLRoutines != null)
            {
                foreach (var schema in _SQLRoutines)
                {
                    if (schema.IsSelected)
                    {
                        buildDefinition.DBSchemas.Add(new BuildSchema()
                        {
                            Schema = schema.Name,
                            Namespace = schema.Namespace
                        });
                    }
                    else
                    {
                        foreach (var routine in schema.Routines)
                        {
                            if (routine.IsSelected)
                            {
                                buildDefinition.DBRoutines.Add(new BuildRoutine
                                {
                                    Name = routine.Name,
                                    Schema = routine.Schema,
                                    Namespace = routine.Namespace
                                });
                            }
                        }
                    }
                } 
            }

            if (_SQLQueries != null)
            {
                foreach (var schema in _SQLQueries)
                {
                    if (schema.IsSelected)
                    {
                        buildDefinition.QuerySchemas.Add(new BuildSchema
                        {
                            Namespace = schema.Namespace,
                            Schema = schema.Name
                        });
                    }
                    else
                    {
                        foreach (var routine in schema.Routines)
                        {
                            if (routine.IsSelected)
                            {
                                buildDefinition.QueryRoutines.Add(new BuildRoutine
                                {
                                    Name = routine.Name,
                                    Schema = routine.Schema,
                                    Namespace = routine.Namespace
                                });
                            }
                        }
                    }
                } 
            }

            if (_StaticQueries != null)
            {
                foreach (var query in _StaticQueries)
                {
                    buildDefinition.StaticQueries.Add(new BuildQuery()
                    {
                        Name = query.Name,
                        Query = query.Query
                    });
                } 
            }

            if (_EnumQueries != null)
            {
                foreach (var query in _EnumQueries)
                {
                    buildDefinition.EnumQueries.Add(new BuildQuery()
                    {
                        Name = query.Name,
                        Query = query.Query
                    });
                } 
            }

            buildDefinition.BuildOptions = new BuildOptions();

            buildDefinition.BuildOptions = new BuildOptions()
            {
                ImplementIChangeTracking = _ImplementIChangeTracking,
                ImplementINotifyPropertyChanged = ImplementINotifyPropertyChanged,
                ImplementIRevertibleChangeTracking = ImplementIRevertibleChangeTracking,
                IncludeAsyncServices = IncludeAsynchronousMethods,
                UseNullableReferenceTypes = UseNullableReferenceTypes
            };

            buildDefinition.NullOutZeroLengthCollections();

            _BuildDefinition = buildDefinition;
        }
        private void RefreshUiFromBuildDefinition()
        {
            RefreshDBRoutines();
            RefreshQueryRoutines();
            RefreshEnumQueries();
            RefreshStaticQueries();
            RefreshBuildOptions();
        }
        private void RefreshDBRoutines()
        {
            IsBusy = true;

            HasAnyDBRoutineErrors = false;
            
            var routines = GetDataCollector(DataCollectorModes.Configuration).CollectDBRoutines();

            List<Schema> schemas = BuildRoutinesToSchema(routines, _BuildDefinition.DBSchemas, _BuildDefinition.DBRoutines);

            HasAnyDBRoutineErrors = SchemaHasAnyError(schemas);

            SQLRoutines = new ObservableCollection<Schema>(schemas);

            IsBusy = false;
        }
        public void RefreshQueryRoutines()
        {
            IsBusy = true;

            HasAnyQueryRoutineErrors = false;

            var routines = GetDataCollector(DataCollectorModes.Configuration).CollectQueryRoutines();

            List<Schema> schemas = BuildRoutinesToSchema(routines, _BuildDefinition.QuerySchemas, _BuildDefinition.QueryRoutines);

            HasAnyQueryRoutineErrors = SchemaHasAnyError(schemas);

            SQLQueries = new ObservableCollection<Schema>(schemas);

            IsBusy = false;
        }
        private void RefreshEnumQueries()
        {
            if (_BuildDefinition.EnumQueries is not null && _BuildDefinition.EnumQueries.Count != 0)
            {
                EnumQueries = new ObservableCollection<EnumQuery>();
                foreach (var item in _BuildDefinition.EnumQueries)
                {
                    EnumQueries.Add(new EnumQuery(RemoveItemFromCollection)
                    {
                        Query = item.Query,
                        Name = item.Name
                    });
                }
            }
        }
        private void RefreshStaticQueries()
        {
            if (_BuildDefinition.StaticQueries is not null && _BuildDefinition.StaticQueries.Count != 0)
            {
                StaticQueries = new ObservableCollection<StaticQuery>();

                foreach (var item in _BuildDefinition.StaticQueries)
                {
                    StaticQueries.Add(new StaticQuery(RemoveItemFromCollection)
                    {
                        Name = item.Name,
                        Query = item.Query
                    });

                }
            }
        }
        private void RefreshBuildOptions()
        {
            SQLClientNamespace = _BuildDefinition.SQLClient;

            ImplementINotifyPropertyChanged = _BuildDefinition.BuildOptions.ImplementINotifyPropertyChanged;
            ImplementIChangeTracking = _BuildDefinition.BuildOptions.ImplementIChangeTracking;
            ImplementIRevertibleChangeTracking = _BuildDefinition.BuildOptions.ImplementIRevertibleChangeTracking;
            IncludeAsynchronousMethods = _BuildDefinition.BuildOptions.IncludeAsyncServices;
            UseNullableReferenceTypes = _BuildDefinition.BuildOptions.UseNullableReferenceTypes;
        }
        private List<Schema> BuildRoutinesToSchema(List<SQLPLUS.Builder.TemplateModels.Routine> routines, List<BuildSchema> buildSchemas, List<BuildRoutine> buildRoutines)
        {
            List<Schema> result = new List<Schema>();

            if (routines is not null)
            {
                var currentSchema = new Schema { Name = string.Empty };

                foreach (var routine in routines)
                {
                    if (currentSchema.Name != routine.Schema)
                    {
                        currentSchema = new Schema()
                        {
                            IsSelected = false,
                            Name = routine.Schema,
                            Namespace = routine.Schema,
                            Routines = new ObservableCollection<Routine>()
                        };
                        result.Add(currentSchema);
                    }

                    Routine routineToAdd = new Routine
                    {
                        IsSelected = false,
                        Name = routine.Name,
                        Schema = currentSchema.Name,
                        Namespace = currentSchema.Name,
                        IsSelectedChangedCallback = currentSchema.RoutineSelectedCallback
                    };
                    if (routine.HasError)
                    {
                        routineToAdd.RoutineError = routine.ErrorMessage;
                        routineToAdd.HasError = true;
                        currentSchema.HasError = true;
                    }
                    currentSchema.Routines.Add(routineToAdd);
                }

                if (buildSchemas != null)
                {
                    foreach (var buildSchema in buildSchemas)
                    {
                        var schema = result.FirstOrDefault(s => s.Name == buildSchema.Schema);
                        if (schema is not null)
                        {
                            schema.IsSelected = true;
                            schema.Namespace = buildSchema.Namespace;
                        }
                    }
                }

                if (buildRoutines != null)
                {
                    foreach (var buildDefinitionRoutine in buildRoutines)
                    {
                        var schema = result.FirstOrDefault(s => s.Name == buildDefinitionRoutine.Schema);
                        if (schema is not null)
                        {
                            var routine = schema.Routines.FirstOrDefault(r => r.Name == buildDefinitionRoutine.Name);
                            if (routine is not null)
                            {
                                routine.IsSelected = true;
                                routine.Namespace = buildDefinitionRoutine.Namespace;
                            }
                        }
                    }
                }
            }

            return result;
        }
        private bool SchemaHasAnyError(List<Schema> schemas)
        {
            foreach (var schema in schemas)
            {
                foreach (var routine in schema.Routines)
                {
                    if (routine.HasError)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        private void SaveConfiguration()
        {
            RefreshBuildDefinitionFromUi();
            _ConfigurationService.SaveBuildDefinition(_BuildDefinition);
        }


        #endregion UI Bound BuildDefinition Properties

        #region UI Bound DatabaseConnection Properties and Methods

        private string _DatabaseType;
        public string DatabaseType
        {
            set
            {
                if (_DatabaseType != value)
                {
                    _DatabaseType = value;
                    RaisePropertyChanged(nameof(DatabaseType));
                }
            }
            get
            {
                return _DatabaseType;
            }
        }

        private string _ConnectionString;
        public string ConnectionString
        {
            set
            {

                ConnectionError = null;

                if (_ConnectionString != value)
                {
                    _ConnectionString = value;
                    RaisePropertyChanged(nameof(ConnectionString));
                    ConnectPaneConnect.RaiseCanExecuteChanged();
                }
            }
            get
            {
                return _ConnectionString;
            }
        }
        
        private void SaveDatabaseConnection()
        {
            UIToDatabaseConnection();
            _ConfigurationService.SaveDatabaseConnection(_DatabaseConnection);
        }
        private void UIToDatabaseConnection()
        {
            _DatabaseConnection.ConnectionString = _ConnectionString;
            _DatabaseConnection.DatabaseType = _DatabaseType;
        }

        private void DatabaseConnectionToUI()
        {
            ConnectionString = _DatabaseConnection.ConnectionString;
            DatabaseType = _DatabaseConnection.DatabaseType;
        }

        #endregion UI Bound DatabaseConnection Properties and Methods

        private int _Progress;
        public int Progress
        {
            set
            {
                if (_Progress != value)
                {
                    _Progress = value;
                    RaisePropertyChanged(nameof(Progress));
                }
            }
        }

        private void AddFileIfNotExists(string file)
        {
            if (!filesAdded.Contains(file))
            {
                filesAdded.Add(file);
            }
        }


        public void AppendBuildText(string text, bool isError)
        {
            BuildOutput.Add(new BuildItem() { IsError = isError, Text = text });
            RaisePropertyChanged(nameof(BuildOutput));
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate)(() => { }));
        }

        public async Task BuildProject()
        {
            RefreshBuildDefinitionFromUi();

            filesAdded = new List<string>();

            //TODO: Depends on the DB type
            MSSQLDataCollector collector = new MSSQLDataCollector(_BuildDefinition, _DatabaseConnection, _ProjectInformation);
            collector.DataCollectorMode = DataCollectorModes.Build;

            //TODO: Depends on the Template
            IRenderProvider renderProvider = new NetRenderProvider(_ProjectInformation, _BuildDefinition);
            
            BuildService service = new BuildService(_BuildDefinition, _ProjectInformation, collector, renderProvider);
            AttachEvents(service);
            service.Run();
            DetachEvents(service);
            AppendBuildText("Updating Visual Studio Project...", false);
            if (filesAdded.Count != 0)
            {
                await _Project.AddExistingFilesAsync(filesAdded.ToArray());
            }
            AppendBuildText("Visual Studio Project Updated", false);
            AppendBuildText("Build Complete", false);
        }



        #region Model Properties


        private string _ConnectionError;
        public string ConnectionError
        {
            set
            {
                if (_ConnectionError != value)
                {
                    _ConnectionError = value;
                    RaisePropertyChanged(nameof(ConnectionError));
                }
            }
            get
            {
                return _ConnectionError;
            }
        }


        #endregion Model Properties

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region Banner Properties

        private string _BannerText;
        private string _BannerImage;
        public string BannerImage
        {
            get
            {
                return _BannerImage;
            }
            set
            {
                if (_BannerImage != value)
                {
                    _BannerImage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BannerImage)));
                }
            }

        }
        public string BannerText
        {
            get
            {
                return _BannerText;
            }
            set
            {
                if (_BannerText != value)
                {
                    _BannerText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BannerText)));
                }
            }
        }
        private void SetAndNotifyBanner()
        {
            string imagePrefix = "../Resources/";
            string icon = string.Empty;
            string text = string.Empty;

            //TODO: Static data and enumerations.

            switch (_ActivePane)
            {
                case Panes.BuildActive:
                    BannerImage = imagePrefix + "BuildIcon.png";
                    BannerText = "Click Run to Build Your Project.";
                    break;
                case Panes.RoutinesActive:
                    BannerImage = imagePrefix + "BuildObjectsIcon.png";
                    BannerText = "Choose the Database Routines to Include in Build.";
                    break;
                case Panes.SettingsActive:
                    BannerImage = imagePrefix + "SettingsIcon.png";
                    BannerText = "Taylor Your Build Options.";
                    break;
                case Panes.QueriesActive:
                    BannerImage = imagePrefix + "QueriesIcon.png";
                    BannerText = "Choose the Queries to Include in Build.";
                    break;
                case Panes.ConnectActive:
                    BannerImage = imagePrefix + "ConnectIcon.png";
                    BannerText = "Set the Connection to Your Build Database";
                    break;
                case Panes.EnumsActive:
                    BannerImage = imagePrefix + "StaticsIcon.png";
                    BannerText = "Queries For Enumerations.";
                    break;
                case Panes.HelpActive:
                    BannerImage = imagePrefix + "HelpIcon.png";
                    BannerText = "Get the Help You Need.";
                    break;
                case Panes.StaticsActive:
                    BannerImage = imagePrefix + "StaticsIcon.png";
                    BannerText = "Queries for Static Lists.";
                    break;
            }
        }

        #endregion Banner Properties

        #region UIProperties

        public event PropertyChangedEventHandler PropertyChanged;

        private Panes _PreviousPane = Panes.None;
        private Panes _ActivePane = Panes.None;

        private void HandleNavigationPropertyChanged(Panes activePane)
        {
            _PreviousPane = _ActivePane;
            _ActivePane = activePane;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_PreviousPane.ToString()));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_ActivePane.ToString()));

            SetAndNotifyBanner();
            RaiseCanExecuteChanged();
        }

        public bool BuildActive
        {
            get
            {
                return _ActivePane == Panes.BuildActive;
            }
            set
            {
                if (_ActivePane != Panes.BuildActive)
                {
                    HandleNavigationPropertyChanged(Panes.BuildActive);
                }
            }
        }
        public bool RoutinesActive
        {
            get
            {
                return _ActivePane == Panes.RoutinesActive;
            }
            set
            {
                if (_ActivePane != Panes.RoutinesActive)
                {
                    HandleNavigationPropertyChanged(Panes.RoutinesActive);
                }
            }
        }

        private bool _IsBusy = false;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                if (_IsBusy != value)
                {
                    _IsBusy = value;
                    RaisePropertyChanged(nameof(IsBusy));
                }
            }
        }
        public bool SettingsActive
        {
            get
            {
                return _ActivePane == Panes.SettingsActive;
            }
            set
            {
                if (_ActivePane != Panes.SettingsActive)
                {
                    HandleNavigationPropertyChanged(Panes.SettingsActive);
                }
            }
        }
        public bool QueriesActive
        {
            get
            {
                return _ActivePane == Panes.QueriesActive;
            }
            set
            {
                if (_ActivePane != Panes.QueriesActive)
                {
                    HandleNavigationPropertyChanged(Panes.QueriesActive);
                }
            }
        }
        public bool ConnectActive
        {
            get
            {
                return _ActivePane == Panes.ConnectActive;
            }
            set
            {
                if (_ActivePane != Panes.ConnectActive)
                {
                    HandleNavigationPropertyChanged(Panes.ConnectActive);
                }
            }
        }
        public bool EnumsActive
        {
            get
            {
                return _ActivePane == Panes.EnumsActive;
            }
            set
            {
                if (_ActivePane != Panes.EnumsActive)
                {
                    HandleNavigationPropertyChanged(Panes.EnumsActive);
                }
            }
        }
        public bool HelpActive
        {
            get
            {
                return _ActivePane == Panes.HelpActive;
            }
            set
            {
                if (_ActivePane != Panes.HelpActive)
                {
                    HandleNavigationPropertyChanged(Panes.HelpActive);
                }
            }
        }
        public bool StaticsActive
        {
            get
            {
                return _ActivePane == Panes.StaticsActive;
            }
            set
            {
                if (_ActivePane != Panes.StaticsActive)
                {
                    HandleNavigationPropertyChanged(Panes.StaticsActive);
                }
            }
        }

        private bool _IsConnected = false;
        public bool IsConnected
        {
            get
            {
                return _IsConnected;
            }
            set
            {
                if (_IsConnected != value)
                {
                    _IsConnected = value;
                    RaiseCanExecuteChanged();
                }
            }
        }

        private bool _HasAnyDBRoutineErrors = false;
        public bool HasAnyDBRoutineErrors
        {
            set
            {
                if (_HasAnyDBRoutineErrors != value)
                {
                    _HasAnyDBRoutineErrors = value;
                    RaisePropertyChanged(nameof(HasAnyDBRoutineErrors));
                    RaisePropertyChanged(nameof(HasAnyErrors));
                }
            }
            get
            {
                return _HasAnyDBRoutineErrors;
            }
        }

        private bool _HasAnyQueryRoutineErrors = false;
        public bool HasAnyQueryRoutineErrors
        {
            set
            {
                if (_HasAnyQueryRoutineErrors != value)
                {
                    _HasAnyQueryRoutineErrors = value;
                    RaisePropertyChanged(nameof(HasAnyQueryRoutineErrors));
                    RaisePropertyChanged(nameof(HasAnyErrors));
                }
            }
            get
            {
                return _HasAnyQueryRoutineErrors;
            }
        }

        private bool _HasAnyStaticErrors = false;
        public bool HasAnyStaticErrors
        {
            set
            {
                if (_HasAnyStaticErrors != value)
                {
                    _HasAnyQueryRoutineErrors = value;
                    RaisePropertyChanged(nameof(HasAnyStaticErrors));
                    RaisePropertyChanged(nameof(HasAnyErrors));
                }
            }
            get
            {
                return _HasAnyStaticErrors;
            }
        }

        private bool _HasAnyEnumErrors = false;
        public bool HasAnyEnumErrors
        {
            set
            {
                if (_HasAnyEnumErrors != value)
                {
                    _HasAnyEnumErrors = value;
                    RaisePropertyChanged(nameof(HasAnyEnumErrors));
                    RaisePropertyChanged(nameof(HasAnyErrors));
                }
            }
            get
            {
                return _HasAnyEnumErrors;
            }
        }
        public bool HasAnyErrors
        {
            get
            {
                return _HasAnyDBRoutineErrors || _HasAnyQueryRoutineErrors || _HasAnyEnumErrors || _HasAnyStaticErrors;
            }
        }

        #endregion

        #region Relay Commands

        public void RaiseCanExecuteChanged()
        {
            HelpCommand.RaiseCanExecuteChanged();
            BuildCommand.RaiseCanExecuteChanged();
            ConnectCommand.RaiseCanExecuteChanged();
            RoutinesCommand.RaiseCanExecuteChanged();
            SettingsCommand.RaiseCanExecuteChanged();
            StaticsCommand.RaiseCanExecuteChanged();
            QueriesCommand.RaiseCanExecuteChanged();
            EnumsCommand.RaiseCanExecuteChanged();
            ConnectPaneConnect.RaiseCanExecuteChanged();
        }

        public RelayCommand AddEnumCommand { private set; get; }
        public RelayCommand AddStaticsCommand { private set; get; }
        public RelayCommand BuildCommand { private set; get; }
        public RelayCommand ConnectCommand { private set; get; }
        public RelayCommand HelpCommand { private set; get; }
        public RelayCommand SettingsCommand { private set; get; }
        public RelayCommand QueriesCommand { private set; get; }
        public RelayCommand RoutinesCommand { private set; get; }
        public RelayCommand StaticsCommand { private set; get; }
        public RelayCommand EnumsCommand { private set; get; }
        public RelayCommand DeleteStaticsCommand { private set; get; }
        public RelayCommand ConnectPaneConnect { private set; get; }
        public RelayCommand SaveConfigurationCommand { private set; get; }
        public RelayCommand BuildProjectCommand { private set; get; }
        public void InitCommands()
        {
            HelpCommand = new RelayCommand
                (
                    (o) =>
                    {
                        return true;
                    },
                    (o) =>
                    {
                        HelpActive = true;
                    }
                );

            ConnectCommand = new RelayCommand
                (
                    (o) =>
                    {
                        return true;
                    },
                    (o) =>
                    {
                        ConnectActive = true;
                    }
                );

            RoutinesCommand = new RelayCommand
                (
                    (o) =>
                    {
                        return IsConnected;
                    },
                    (o) =>
                    {
                        RoutinesActive = true;
                        RefreshDBRoutines();
                    }
                );

            QueriesCommand = new RelayCommand
                (
                    (o) =>
                    {
                        return IsConnected;
                    },
                    (o) =>
                    {
                        QueriesActive = true;
                        RefreshQueryRoutines();
                    }
                );

            StaticsCommand = new RelayCommand
                (
                    (o) =>
                    {
                        return IsConnected;
                    },
                    (o) =>
                    {
                        StaticsActive = true;
                        RefreshStaticQueries();
                    }
                );

            EnumsCommand = new RelayCommand
                (
                    (o) =>
                    {
                        return IsConnected;
                    },
                    (o) =>
                    {
                        EnumsActive = true;
                        RefreshEnumQueries();
                    }
                );

            SettingsCommand = new RelayCommand
                (
                    (o) =>
                    {
                        return IsConnected;
                    },
                    (o) =>
                    {
                        SettingsActive = true;
                    }
                );

            

            BuildCommand = new RelayCommand
                (
                    (o) =>
                    {
                        return true;
                    },
                    (o) =>
                    {
                        SaveConfiguration();
                        BuildActive = true;
                    }
                );

            ConnectPaneConnect = new RelayCommand
                (
                    (o) =>
                    {
                        if (!string.IsNullOrEmpty(_ConnectionError))
                        {
                            return false;
                        }
                        if (string.IsNullOrEmpty(_ConnectionString))
                        {
                            return false;
                        }
                        return true;
                    },
                    async (o) =>
                    {
                        IsBusy = true;

                        UIToDatabaseConnection();

                        try
                        {
                            using (var connection = new SqlConnection(_DatabaseConnection.ConnectionString))
                            {
                                await connection.OpenAsync();
                                connection.Close();

                                SaveDatabaseConnection();

                                IsConnected = true;
                                RefreshUiFromBuildDefinition();
                                RoutinesActive = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            ConnectionError = ex.Message;
                        }

                        IsBusy = false;
                    }
                );

            AddStaticsCommand = new RelayCommand
                (
                    (o) =>
                    {
                        return true;
                    },
                    (o) =>
                    {
                        if (StaticQueries is null)
                        {
                            StaticQueries = new ObservableCollection<StaticQuery>();
                        }

                        StaticQueries.Add(new StaticQuery(RemoveItemFromCollection)
                        {
                            Query = "Select <columns> From <table or veiw> Where",
                            Name = "NewStaticDataQuery"
                        });
                    }
                );

            AddEnumCommand = new RelayCommand
                (
                    (o) =>
                    {
                        return true;
                    },
                    (o) =>
                    {
                        if (EnumQueries is null)
                        {
                            EnumQueries = new ObservableCollection<EnumQuery>();
                        }

                        EnumQueries.Add(new EnumQuery(RemoveItemFromCollection)
                        {
                            Query = "SELECT <column> AS [Name], <column> AS [Value], <column> AS [Comment] From <table or view> WHERE ",
                            Name = "MyEnumQuery"
                        });
                    }
                );

           
            BuildProjectCommand = new RelayCommand
             (
                 (o) =>
                 {
                     return true;
                 },
                 async (o) =>
                 {
                     BuildOutput = new ObservableCollection<BuildItem>();
                     AppendBuildText("Ready...", false);
                     RefreshBuildDefinitionFromUi();
                     await BuildProject();
                 }
             );

            #endregion RelayCommands

        }

        private void RemoveItemFromCollection(object obj)
        {
            if (obj is EnumQuery enumQuery)
            {
                EnumQueries.Remove(enumQuery);
                return;
            }

            if (obj is StaticQuery staticQuery)
            {
                StaticQueries.Remove(staticQuery);
                return;
            }
        }

        #region Builder Service Events

        private void AttachEvents(BuildService service)
        {
            service.OnDirectoryCreated += Service_OnDirectoryCreated;
            service.OnFileCreated += Service_OnFileCreated;
            service.OnFileWrite += Service_OnFileWrite;
            service.OnProgressChanged += Service_OnProgressChanged;
        }
        private void DetachEvents(BuildService service)
        {
            service.OnDirectoryCreated += Service_OnDirectoryCreated;
            service.OnFileCreated += Service_OnFileCreated;
            service.OnFileWrite += Service_OnFileWrite;
            service.OnProgressChanged += Service_OnProgressChanged;
        }
        private void Service_OnProgressChanged(object sender, ProgressStatusArgs e)
        {
            AppendBuildText(e.Message, false);
            Progress = e.Progress;
        }
        private void Service_OnFileWrite(object sender, FileWriteEventArgs e)
        {
            AppendBuildText($"File Written: {e.FileName}", false);
        }
        private void Service_OnDirectoryCreated(object sender, DirectoryCreatedEventArgs e)
        {
            AppendBuildText($"Directory Created: {e.NewDirectoryPath}", false);
        }
        private void Service_OnFileCreated(object sender, FileCreatedEventArgs e)
        {
            AddFileIfNotExists(e.NewFileName);
            AppendBuildText($"File Created: {e.NewFileName}", false);
        }

        #endregion Builder Service Events

    }
}
/* //var schemas = new ObservableCollection<Schema>();

            if (buildRoutines is not null)
            {
                var schemas = new ObservableCollection<Schema>();
                var currentSchema = new Schema { Name = string.Empty };

                foreach (var routine in buildRoutines)
                {
                    if (currentSchema.Name != routine.Schema)
                    {
                        currentSchema = new Schema()
                        {
                            IsSelected = false,
                            Name = routine.Schema,
                            Namespace = routine.Schema,
                            Routines = new ObservableCollection<Routine>()
                        };
                        schemas.Add(currentSchema);
                    }

                    Routine routineToAdd = new Routine
                    {
                        IsSelected = false,
                        Name = routine.Name,
                        Schema = currentSchema.Name,
                        Namespace = currentSchema.Name,
                        IsSelectedChangedCallback = currentSchema.RoutineSelectedCallback
                    };
                    if (routine.HasError)
                    {
                        routineToAdd.RoutineError = routine.ErrorMessage;
                        routineToAdd.HasError = true;
                        currentSchema.HasError = true;
                        HasAnyDBRoutineErrors = true;
                    }
                    currentSchema.Routines.Add(routineToAdd);
                }

                if (_BuildDefinition.BuildDBSchemas is not null)
                {
                    foreach (var buildDefinitionSchema in _BuildDefinition.BuildDBSchemas)
                    {
                        var schema = schemas.FirstOrDefault(s => s.Name == buildDefinitionSchema.Schema);
                        if (schema is not null)
                        {
                            schema.IsSelected = true;
                            schema.Namespace = buildDefinitionSchema.Namespace;
                        }
                    }
                }

                if (_BuildDefinition.BuildDBRoutines is not null)
                {
                    foreach (var buildDefinitionRoutine in _BuildDefinition.BuildDBRoutines)
                    {
                        var schema = schemas.FirstOrDefault(s => s.Name == buildDefinitionRoutine.Schema);
                        if (schema is not null)
                        {
                            var routine = schema.Routines.FirstOrDefault(r => r.Name == buildDefinitionRoutine.Name);
                            if (routine is not null)
                            {
                                routine.IsSelected = true;
                                routine.Namespace = buildDefinitionRoutine.Namespace;
                            }
                        }
                    }
                }


                if (schemas.Count == 0)
                {
                    schemas = null;
                }

                SQLRoutines = schemas;
            }
*/