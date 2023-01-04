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
        private ConfigurationService configurationService;
        private ProjectInformation projectInformation;
        private BuildDefinition buildDefinition;
        private DatabaseConnection databaseConnection;
        private Project vsProject;

        private List<SQLPLUS.Builder.TemplateModels.Routine> allBuildRoutines;
        private List<SQLPLUS.Builder.TemplateModels.Routine> allBuildQueries;
        private List<string> files;
        public SQLPlusConfigurationWindowViewModel(ConfigurationService configurationService, ProjectInformation projectInformation, BuildDefinition buildDefinition, SQLPLUS.Builder.ConfigurationModels.DatabaseConnection databaseConnection, Project vsProject)
        {
            this.configurationService = configurationService;
            this.projectInformation = projectInformation;
            this.buildDefinition = buildDefinition;
            this.databaseConnection = databaseConnection;
            this.vsProject = vsProject;
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


        private void AddFileIfNotExists(string file)
        {
            if (!files.Contains(file))
            {
                files.Add(file);
            }
        }

        public void SaveBuildItems()
        {
            BuildDefinition newBuildDefinition = new BuildDefinition();
            newBuildDefinition.BuildSchemas = new List<BuildSchema>();
            newBuildDefinition.BuildRoutines = new List<BuildRoutine>();

            if (_SQLRoutines is not null)
            {
                foreach (var schema in _SQLRoutines)
                {
                    if (schema.IsSelected)
                    {
                        newBuildDefinition.BuildSchemas.Add(new BuildSchema()
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
                                newBuildDefinition.BuildRoutines.Add(new BuildRoutine
                                {
                                    Name = routine.Name,
                                    Schema = routine.Schema,
                                    Namespace = routine.Namespace
                                });
                            }
                        }
                    }
                }
                if (newBuildDefinition.BuildSchemas.Count == 0)
                {
                    newBuildDefinition.BuildSchemas = null;
                }
                if (newBuildDefinition.BuildRoutines.Count == 0)
                {
                    newBuildDefinition.BuildRoutines = null;
                }
            }

            if (_SQLQueries is not null)
            {
                newBuildDefinition.BuildQuerySchemas = new List<BuildSchema>();
                newBuildDefinition.BuildQueryRoutines = new List<BuildRoutine>();

                foreach (var schema in _SQLQueries)
                {
                    if (schema.IsSelected)
                    {
                        newBuildDefinition.BuildQuerySchemas.Add(new BuildSchema
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
                                newBuildDefinition.BuildQueryRoutines.Add(new BuildRoutine
                                {
                                    Name = routine.Name,
                                    Schema = routine.Schema,
                                    Namespace = routine.Namespace
                                });
                            }
                        }
                    }
                }
                if (newBuildDefinition.BuildQuerySchemas.Count == 0)
                {
                    newBuildDefinition.BuildQuerySchemas = null;
                }
                if (newBuildDefinition.BuildQueryRoutines.Count == 0)
                {
                    newBuildDefinition.BuildQueryRoutines = null;
                }
            }

            if (StaticQueries is not null && StaticQueries.Count != 0)
            {
                newBuildDefinition.StaticQueries = new List<BuildQuery>();

                foreach (StaticQuery query in StaticQueries)
                {
                    newBuildDefinition.StaticQueries.Add(new BuildQuery()
                    {
                        Name = query.Name,
                        Query = query.Query
                    });
                }
            }

            if (EnumQueries is not null && EnumQueries.Count != 0)
            {
                newBuildDefinition.EnumQueries = new List<BuildQuery>();
                foreach (EnumQuery query in EnumQueries)
                {
                    newBuildDefinition.EnumQueries.Add(new BuildQuery()
                    {
                        Name = query.Name,
                        Query = query.Query
                    });
                }
            }

            newBuildDefinition.BuildOptions = new BuildOptions()
            {
                ImplementIChangeTracking = ImplementIChangeTracking,
                ImplementINotifyPropertyChanged = ImplementINotifyPropertyChanged,
                ImplementIRevertibleChangeTracking = ImplementIRevertibleChangeTracking,
                IncludeAsyncServices = IncludeAsynchronousMethods,
                UseNullableReferenceTypes = UseNullableReferenceTypes
            };

            configurationService.SaveBuildDefinition(newBuildDefinition);
            buildDefinition = newBuildDefinition;
        }

        public async Task BuildProject()
        {
            SaveBuildItems();

            files = new List<string>();

            MSSQLDataCollector collector = new MSSQLDataCollector(buildDefinition, databaseConnection, projectInformation);
            IRenderProvider renderProvider = new NetRenderProvider(projectInformation, buildDefinition);
            BuildService service = new BuildService(buildDefinition, projectInformation, collector, renderProvider);
            collector.DataCollectorMode = DataCollectorModes.Build;
            service.OnDirectoryCreated += Service_OnDirectoryCreated;
            service.OnFileCreated += Service_OnFileCreated;
            service.OnFileWrite += Service_OnFileWrite;
            service.OnProgressChanged += Service_OnProgressChanged;
            service.Run();

            if(files.Count != 0)
            {
                await vsProject.AddExistingFilesAsync(files.ToArray());
            }
        }

        public void AppendBuildText(string text, bool isError)
        {
            BuildOutput.Add(new BuildItem() { IsError = isError, Text = text });
            RaisePropertyChanged(nameof(BuildOutput));
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Delegate)(() => { }));
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

        private void Service_OnFileCreated(object sender, FileCreatedEventArgs e)
        {
            files.Add(e.NewFileName);
            AppendBuildText($"File Created: {e.NewFileName}", false);
        }

        private void Service_OnDirectoryCreated(object sender, DirectoryCreatedEventArgs e)
        {
            AppendBuildText($"Directory Created: {e.NewDirectoryPath}", false);
        }


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

        #region SetupFromBuildItems

        public void SetupFromBuildItems()
        {
            RefreshBuildRoutines();
            SetupEnumQueries();
            SetupStaticQueries();
            SetUpBuildOptions();
        }

        public void RefreshBuildRoutines()
        {
            IsBusy = true;

            MSSQLDataCollector collector = new MSSQLDataCollector(buildDefinition, databaseConnection, projectInformation);
            collector.DataCollectorMode = DataCollectorModes.Configuration;
            var allRoutines = collector.CollectRoutines();
            allBuildRoutines = allRoutines.FindAll(r => r.RoutineType != MSSQLDataCollector.ROUTINE_TYPE_QUERY);
            allBuildQueries = allRoutines.FindAll(r => r.RoutineType == MSSQLDataCollector.ROUTINE_TYPE_QUERY);

            SetupBuildRoutines();
            SetupQueryRoutines();

            IsBusy = false;
        }


        private void SetupBuildRoutines()
        {
            HasAnyDBRoutineErrors = false;

            if (allBuildRoutines is not null)
            {
                var schemas = new ObservableCollection<Schema>();
                var currentSchema = new Schema { Name = string.Empty };

                foreach (var routine in allBuildRoutines)
                {
                    if (currentSchema.Name != routine.Schema)
                    {
                        currentSchema = new Schema()
                        {
                            IsSelected = false,
                            Name = routine.Schema,
                            Namespace = routine.Schema,
                            Routines = new ObservableCollection<SQLPlusExtension.Models.Routine>()
                        };
                        schemas.Add(currentSchema);
                    }

                    Routine routineToAdd = new Routine
                    {
                        IsSelected = false,
                        Name = routine.Name,
                        Schema = currentSchema.Name,
                        Namespace = currentSchema.Name,
                        IsSelectedChangedCallback = currentSchema.CheckAllSelected
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

                if (buildDefinition.BuildSchemas is not null)
                {
                    foreach (var buildDefinitionSchema in buildDefinition.BuildSchemas)
                    {
                        var schema = schemas.FirstOrDefault(s => s.Name == buildDefinitionSchema.Schema);
                        if (schema is not null)
                        {
                            schema.IsSelected = true;
                            schema.Namespace = buildDefinitionSchema.Namespace;
                        }
                    }
                }

                if (buildDefinition.BuildRoutines is not null)
                {
                    foreach (var buildDefinitionRoutine in buildDefinition.BuildRoutines)
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
        }
        private void SetupQueryRoutines()
        {
            HasAnyQueryRoutineErrors = false;

            if (allBuildQueries is not null)
            {
                var schemas = new ObservableCollection<Schema>();
                var currentSchema = new Schema { Namespace = string.Empty };

                foreach (var routine in allBuildQueries)
                {
                    if (currentSchema.Namespace != routine.Namespace)
                    {
                        currentSchema = new Schema()
                        {
                            IsSelected = false,
                            Name = routine.Schema,
                            Namespace = routine.Namespace,
                            Routines = new ObservableCollection<Routine>()
                        };
                        schemas.Add(currentSchema);
                    }

                    Routine routineToAdd = new Routine
                    {
                        IsSelected = false,
                        Name = routine.Name,
                        Schema = routine.Schema,
                        Namespace = routine.Namespace,
                        IsSelectedChangedCallback = currentSchema.CheckAllSelected
                    };

                    if (routine.HasError)
                    {
                        routineToAdd.RoutineError = routine.ErrorMessage;
                        routineToAdd.HasError = true;
                        currentSchema.HasError = true;
                        HasAnyQueryRoutineErrors = true;
                    }

                    currentSchema.Routines.Add(routineToAdd);

                }

                if (buildDefinition.BuildQuerySchemas is not null)
                {
                    foreach (var buildDefnitionSchema in buildDefinition.BuildQuerySchemas)
                    {
                        var schema = schemas.FirstOrDefault(s => s.Namespace == buildDefnitionSchema.Namespace);
                        if (schema is not null)
                        {
                            schema.IsSelected = true;
                        }
                    }
                }

                if (buildDefinition.BuildQueryRoutines is not null)
                {
                    foreach (var buildDefnitionRoutine in buildDefinition.BuildQueryRoutines)
                    {
                        var colectionSchema = schemas.FirstOrDefault(s => s.Namespace == buildDefnitionRoutine.Namespace);
                        if (colectionSchema is not null)
                        {
                            var routine = colectionSchema.Routines.FirstOrDefault(r => r.Name == buildDefnitionRoutine.Name);
                            if (routine is not null)
                            {
                                routine.IsSelected = true;
                            }
                        }
                    }
                }


                if (schemas.Count == 0)
                {
                    schemas = null;
                }

                SQLQueries = schemas;
            }
        }
        public void SetupEnumQueries()
        {
            if (buildDefinition.EnumQueries is not null && buildDefinition.EnumQueries.Count != 0)
            {
                EnumQueries = new ObservableCollection<EnumQuery>();
                foreach (var item in buildDefinition.EnumQueries)
                {
                    EnumQueries.Add(new EnumQuery(RemoveItemFromCollection)
                    {
                        Query = item.Query,
                        Name = item.Name
                    });
                }
            }
        }
        private void SetupStaticQueries()
        {
            if (buildDefinition.StaticQueries is not null && buildDefinition.StaticQueries.Count != 0)
            {
                StaticQueries = new ObservableCollection<StaticQuery>();

                foreach (var item in buildDefinition.StaticQueries)
                {
                    StaticQueries.Add(new StaticQuery(RemoveItemFromCollection)
                    {
                        Name = item.Name,
                        Query = item.Query
                    });

                }
            }
        }
        private void SetUpBuildOptions()
        {
            SQLClientNamespace = buildDefinition.SQLClientNamespace;
            ImplementINotifyPropertyChanged = buildDefinition.BuildOptions.ImplementINotifyPropertyChanged;
            ImplementIChangeTracking = buildDefinition.BuildOptions.ImplementIChangeTracking;
            ImplementIRevertibleChangeTracking = buildDefinition.BuildOptions.ImplementIRevertibleChangeTracking;
            IncludeAsynchronousMethods = buildDefinition.BuildOptions.IncludeAsyncServices;
            UseNullableReferenceTypes = buildDefinition.BuildOptions.UseNullableReferenceTypes;
        }

        #endregion SetupFromBuildItems

        #region Model Properties

        public string DatabaseConnectionDatabaseType
        {
            set
            {
                if (databaseConnection.DatabaseType != value)
                {
                    databaseConnection.DatabaseType = value;
                    RaisePropertyChanged(nameof(DatabaseConnectionDatabaseType));
                }
            }
            get
            {
                return databaseConnection.DatabaseType;
            }
        }
        public string DatabaseConnectionConnectionString
        {
            set
            {
                //Clear the error so that the command can be trigger;

                ConnectionError = null;

                if (databaseConnection.ConnectionString != value)
                {
                    databaseConnection.ConnectionString = value;
                    RaisePropertyChanged(nameof(DatabaseConnectionConnectionString));
                    ConnectPaneConnect.RaiseCanExecuteChanged();
                }
            }
            get
            {
                return databaseConnection.ConnectionString;
            }
        }

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

        public bool ImplementINotifyPropertyChanged
        {
            get
            {
                return buildDefinition.BuildOptions.ImplementINotifyPropertyChanged;
            }
            set
            {
                if (buildDefinition.BuildOptions.ImplementINotifyPropertyChanged != value)
                {
                    buildDefinition.BuildOptions.ImplementINotifyPropertyChanged = value;
                    RaisePropertyChanged(nameof(ImplementINotifyPropertyChanged));
                }
            }
        }
        public bool ImplementIChangeTracking
        {
            get
            {
                return buildDefinition.BuildOptions.ImplementIChangeTracking;
            }
            set
            {
                if (buildDefinition.BuildOptions.ImplementIChangeTracking != value)
                {
                    buildDefinition.BuildOptions.ImplementIChangeTracking = value;
                    RaisePropertyChanged(nameof(ImplementIChangeTracking));
                    if (value == true)
                    {
                        ImplementIRevertibleChangeTracking = false;
                    }
                }
            }
        }
        public bool ImplementIRevertibleChangeTracking
        {
            get
            {
                return buildDefinition.BuildOptions.ImplementIRevertibleChangeTracking;
            }
            set
            {
                if (buildDefinition.BuildOptions.ImplementIRevertibleChangeTracking != value)
                {
                    buildDefinition.BuildOptions.ImplementIRevertibleChangeTracking = value;
                    RaisePropertyChanged(nameof(ImplementIRevertibleChangeTracking));
                    if (value == true)
                    {
                        ImplementIChangeTracking = false;
                    }
                }
            }
        }

        public bool IncludeAsynchronousMethods
        {
            get
            {
                return buildDefinition.BuildOptions.IncludeAsyncServices;
            }
            set
            {
                if (buildDefinition.BuildOptions.IncludeAsyncServices != value)
                {
                    buildDefinition.BuildOptions.IncludeAsyncServices = value;
                    RaisePropertyChanged(nameof(IncludeAsynchronousMethods));
                }
            }
        }

        public bool UseNullableReferenceTypes
        {
            set
            {
                if (buildDefinition.BuildOptions.UseNullableReferenceTypes != value)
                {
                    buildDefinition.BuildOptions.UseNullableReferenceTypes = value;
                    RaisePropertyChanged(nameof(UseNullableReferenceTypes));
                }
            }
            get
            {
                return buildDefinition.BuildOptions.UseNullableReferenceTypes;
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
                return _HasAnyDBRoutineErrors;
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

            QueriesCommand = new RelayCommand
                (
                    (o) =>
                    {
                        return IsConnected;
                    },
                    (o) =>
                    {
                        QueriesActive = true;
                        RefreshBuildRoutines();
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
                        RefreshBuildRoutines();
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

            EnumsCommand = new RelayCommand
                (
                    (o) =>
                    {
                        return IsConnected;
                    },
                    (o) =>
                    {
                        EnumsActive = true;
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
                        //TODO: Move to on build command
                        SaveBuildItems();
                        IsConnected = true;
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
                        if (string.IsNullOrEmpty(databaseConnection.ConnectionString))
                        {
                            return false;
                        }
                        return true;
                    },
                    async (o) =>
                    {
                        IsBusy = true;

                        try
                        {
                            using (var connection = new SqlConnection(databaseConnection.ConnectionString))
                            {
                                await connection.OpenAsync();
                                connection.Close();
                                configurationService.SaveDatabaseConnection(new SQLPLUS.Builder.ConfigurationModels.DatabaseConnection
                                {
                                    ConnectionString = DatabaseConnectionConnectionString,
                                    DatabaseType = DatabaseConnectionDatabaseType
                                });

                                IsConnected = true;
                                SetupFromBuildItems();
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

            SaveConfigurationCommand = new RelayCommand
             (
                 (o) =>
                 {
                     return true;
                 },
                 (o) =>
                 {
                     SaveBuildItems();
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
                     SaveBuildItems();
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
    }
}
