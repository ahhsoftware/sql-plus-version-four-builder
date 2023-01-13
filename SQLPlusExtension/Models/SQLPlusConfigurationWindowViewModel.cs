using SQLPLUS.Builder;
using SQLPLUS.Builder.BuildServices;
using SQLPLUS.Builder.ConfigurationModels;
using SQLPLUS.Builder.DataCollectors;
using SQLPLUS.Builder.Render;
using SQLPLUS.Builder.Render.T4Net;
using SQLPLUS.Builder.TemplateModels;
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

        private IDataCollector GetDataCollector(DataCollectorModes mode, BuildDefinition buildDefinition = null)
        {
            if(buildDefinition == null)
            {
                return new MSSQLDataCollector(_BuildDefinition, _DatabaseConnection, _ProjectInformation) { DataCollectorMode = mode };
            }
            return new MSSQLDataCollector(buildDefinition, _DatabaseConnection, _ProjectInformation) { DataCollectorMode = mode };
        }

        #region UI Bound BuildDefinition Properties and Methods

        private ObservableCollection<Schema> _DBRoutines;
        public ObservableCollection<Schema> DBRoutines
        {
            get
            {
                return _DBRoutines;
            }
            set
            {
                if (_DBRoutines != value)
                {
                    _DBRoutines = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DBRoutines)));
                }
            }
        }

        private ObservableCollection<Schema> _QueryRoutines;
        public ObservableCollection<Schema> QueryRoutines
        {
            get
            {
                return _QueryRoutines;
            }
            set
            {
                if (_QueryRoutines != value)
                {
                    _QueryRoutines = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QueryRoutines)));
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

        private void SetBuildDefinitionFromUi_All()
        {
            BuildDefinition buildDefinition = new BuildDefinition();

            SetBuildDefinitionFromUi_DBRoutines(buildDefinition);
            SetBuildDefintionFromUi_QueryRoutines(buildDefinition);
            SetBuildDefintionFromUi_EnumQueries(buildDefinition);
            SetBuildDefintionFromUi_StaticQueries(buildDefinition);
            SetBuildDefintionFromUi_BuildOptions(buildDefinition);

            _BuildDefinition = buildDefinition;
        }

        private void SetBuildDefinitionFromUi_DBRoutines(BuildDefinition buildDefinition)
        {
            buildDefinition.DBSchemas = new List<BuildSchema>();
            buildDefinition.DBRoutines = new List<BuildRoutine>();

            if (_DBRoutines != null)
            {
                foreach (var schema in _DBRoutines)
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

            if(buildDefinition.DBSchemas.Count == 0)
            {
                buildDefinition.DBSchemas = null;
            }
            if(buildDefinition.DBRoutines.Count == 0)
            {
                buildDefinition.DBRoutines = null;
            }

            IsBusy = false;
        }
        private void SetBuildDefintionFromUi_QueryRoutines(BuildDefinition buildDefinition)
        {
            buildDefinition.QuerySchemas = new List<BuildSchema>();
            buildDefinition.QueryRoutines = new List<BuildRoutine>();

            if (_QueryRoutines != null)
            {
                foreach (var schema in _QueryRoutines)
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

            if(buildDefinition.QuerySchemas.Count == 0)
            {
                buildDefinition.QuerySchemas = null;
            }

            if(buildDefinition.QueryRoutines.Count == 0)
            {
                buildDefinition.QueryRoutines = null;
            }
        }
        private void SetBuildDefintionFromUi_EnumQueries(BuildDefinition buildDefinition)
        {
            buildDefinition.EnumQueries = new List<BuildQuery>();

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

            if(buildDefinition.EnumQueries.Count == 0)
            {
                buildDefinition.EnumQueries = null;
            }
        }
        private void SetBuildDefintionFromUi_StaticQueries(BuildDefinition buildDefinition)
        {
            buildDefinition.StaticQueries = new List<BuildQuery>();

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

            if(buildDefinition.StaticQueries.Count == 0)
            {
                buildDefinition.StaticQueries = null;
            }
        }
        private void SetBuildDefintionFromUi_BuildOptions(BuildDefinition buildDefinition)
        {
            buildDefinition.BuildOptions = new BuildOptions()
            {
                ImplementIChangeTracking = _ImplementIChangeTracking,
                ImplementINotifyPropertyChanged = ImplementINotifyPropertyChanged,
                ImplementIRevertibleChangeTracking = ImplementIRevertibleChangeTracking,
                IncludeAsyncServices = IncludeAsynchronousMethods,
                UseNullableReferenceTypes = UseNullableReferenceTypes
            };
        }
        private void ValidateEnumQueries()
        {
            HasAnyEnumErrors = false;

            if(_EnumQueries == null || _EnumQueries.Count == 0)
            {
                return;
            }

            BuildDefinition buildDefinition = new BuildDefinition();
            SetBuildDefintionFromUi_EnumQueries(buildDefinition);
            var collector = GetDataCollector(DataCollectorModes.Configuration, buildDefinition);
            var results = collector.CollectEnumCollections();

            foreach(EnumCollection enumCollection in results)
            {
                var query = _EnumQueries.FirstOrDefault(q => q.Name == enumCollection.Name);

                if (enumCollection.HasError)
                {
                    HasAnyEnumErrors = true;
                    query.QueryError = enumCollection.ErrorMessage;
                }
                else
                {
                    query.QueryError = null;
                }
            }
        }
        private void ValidateStaticQueries()
        {
            HasAnyStaticErrors = false;

            if (_StaticQueries == null || _StaticQueries.Count == 0)
            {
                return;
            }

            BuildDefinition buildDefinition = new BuildDefinition();
            SetBuildDefintionFromUi_StaticQueries(buildDefinition);
            var collector = GetDataCollector(DataCollectorModes.Configuration, buildDefinition);
            var results = collector.CollectStaticCollections();

            foreach (StaticCollection staticCollection in results)
            {
                var query = _StaticQueries.FirstOrDefault(q => q.Name == staticCollection.Name);

                if (staticCollection.HasError)
                {
                    HasAnyStaticErrors = true;
                    query.QueryError = staticCollection.ErrorMessage;
                }
                else
                {
                    query.QueryError = null;
                }
            }
        }
        private void SetUiModelsFromBuildDefinition_All()
        {
            SetUiModelsFromBuildDefinition_DBRoutines();
            SetUiModelsFromBuildDefinition_QueryRoutines();
            SetUiModelsFromBuildDefinition_EnumQueries();
            SetUiModelsFromBuildDefinition_StaticQueries();
            SetUiModelsFromBuildDefinition_BuildOptions();
        }
        private void SetUiModelsFromBuildDefinition_DBRoutines()
        {
            HasAnyDBRoutineErrors = false;
            
            var routines = GetDataCollector(DataCollectorModes.Configuration).CollectDBRoutines();

            List<Schema> schemas = BuildRoutinesToSchema(routines, _BuildDefinition.DBSchemas, _BuildDefinition.DBRoutines, false);

            HasAnyDBRoutineErrors = SchemaHasAnyError(schemas);

            DBRoutines = new ObservableCollection<Schema>(schemas);
        }
        public void SetUiModelsFromBuildDefinition_QueryRoutines()
        {
            HasAnyQueryRoutineErrors = false;

            var routines = GetDataCollector(DataCollectorModes.Configuration).CollectQueryRoutines();

            List<Schema> schemas = BuildRoutinesToSchema(routines, _BuildDefinition.QuerySchemas, _BuildDefinition.QueryRoutines, true);

            HasAnyQueryRoutineErrors = SchemaHasAnyError(schemas);

            QueryRoutines = new ObservableCollection<Schema>(schemas);
        }
        private void SetUiModelsFromBuildDefinition_EnumQueries()
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
        private void SetUiModelsFromBuildDefinition_StaticQueries()
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
        private void SetUiModelsFromBuildDefinition_BuildOptions()
        {
            SQLClientNamespace = _BuildDefinition.SQLClient;

            ImplementINotifyPropertyChanged = _BuildDefinition.BuildOptions.ImplementINotifyPropertyChanged;
            ImplementIChangeTracking = _BuildDefinition.BuildOptions.ImplementIChangeTracking;
            ImplementIRevertibleChangeTracking = _BuildDefinition.BuildOptions.ImplementIRevertibleChangeTracking;
            IncludeAsynchronousMethods = _BuildDefinition.BuildOptions.IncludeAsyncServices;
            UseNullableReferenceTypes = _BuildDefinition.BuildOptions.UseNullableReferenceTypes;
        }
        private List<Schema> BuildRoutinesToSchema(List<SQLPLUS.Builder.TemplateModels.Routine> routines, List<BuildSchema> buildSchemas, List<BuildRoutine> buildRoutines, bool isQuery)
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
                            Namespace = isQuery? routine.Namespace : routine.Schema,
                            Routines = new ObservableCollection<Routine>()
                        };
                        result.Add(currentSchema);
                    }

                    Routine routineToAdd = new Routine()
                    {
                        IsSelected = false,
                        Name = routine.Name,
                        Schema = currentSchema.Name,
                        Namespace = isQuery? currentSchema.Name : currentSchema.Name,
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
            SetBuildDefinitionFromUi_All();
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
            SetBuildDefinitionFromUi_All();

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
                    BannerImage = imagePrefix + "DBRoutinesIcon.png";
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
                    BannerImage = imagePrefix + "EnumsIcon.png";
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

            switch (_PreviousPane)
            {
                case Panes.RoutinesActive:
                    SetBuildDefinitionFromUi_DBRoutines(_BuildDefinition);
                    break;
                case Panes.QueriesActive:
                    SetBuildDefintionFromUi_QueryRoutines(_BuildDefinition);
                    break;
                case Panes.EnumsActive:
                    SetBuildDefintionFromUi_EnumQueries(_BuildDefinition);
                    ValidateEnumQueries();
                    break;
                case Panes.StaticsActive:
                    SetBuildDefintionFromUi_StaticQueries(_BuildDefinition);
                    ValidateStaticQueries();
                    break;
                case Panes.SettingsActive:
                    SetBuildDefintionFromUi_BuildOptions(_BuildDefinition);
                    break;
            }

            if (_PreviousPane != _ActivePane)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_PreviousPane.ToString()));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_ActivePane.ToString()));
            }

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
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate)(() => { }));
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
                    _HasAnyStaticErrors = value;
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

        private string _DeleteMessage = null;
        public string DeleteMessage
        {
            set
            {
                if(_DeleteMessage != value)
                {
                    _DeleteMessage = value;
                    if(_DeleteMessage == null)
                    {
                        ConfirmDeleteCommand = new RelayCommand
                        (
                            (o) =>
                            {
                                return true;
                            },
                            (o) => { }
                         );
                        CancelDeleteCommand = new RelayCommand
                        (
                            (o) =>
                            {
                                return true;
                            },
                            (o) => { }
                        );
                    }
                    RaisePropertyChanged(nameof(DeleteMessage));
                }
            }
            get
            {
                return _DeleteMessage;
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

        public RelayCommand ConfirmDeleteCommand { private set; get; }

        public RelayCommand CancelDeleteCommand { private set; get; }
        
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
                        IsBusy = true;
                        SetUiModelsFromBuildDefinition_DBRoutines();
                        IsBusy = false;
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
                        IsBusy = true;
                        SetUiModelsFromBuildDefinition_QueryRoutines();
                        IsBusy = false;
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
                        ValidateStaticQueries();
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
                        ValidateEnumQueries();
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
                        return IsConnected;
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
                                SetUiModelsFromBuildDefinition_All();
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
                     SetBuildDefinitionFromUi_All();
                     await BuildProject();
                 }
             );

            ConfirmDeleteCommand = new RelayCommand
            (
                (o) =>
                {
                    return true;
                },
                (o) =>
                {
                    if(enumToDelete != null)
                    {
                        EnumQueries.Remove(enumToDelete);
                    }
                    if(staticToDelete != null)
                    {
                        StaticQueries.Remove(staticToDelete);
                    }
                    DeleteMessage = null;
                }
             );

            CancelDeleteCommand = new RelayCommand
            (
                (o) =>
                {
                    return true;
                },
                (o) =>
                {
                    enumToDelete = null;
                    staticToDelete = null;
                    DeleteMessage = null;
                }
             );

            #endregion RelayCommands

        }

        private EnumQuery enumToDelete = null;
        private StaticQuery staticToDelete = null;

        private void RemoveItemFromCollection(object obj)
        {
            if (obj is EnumQuery enumQuery)
            {
                DeleteMessage = $"Are you sure you want to delete Enum Query: {enumQuery.Name}?";
                enumToDelete = enumQuery;
            }

            if (obj is StaticQuery staticQuery)
            {
                staticToDelete = staticQuery;
                DeleteMessage = $"Are you sure you want to delete List Query {staticQuery.Name}?";
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