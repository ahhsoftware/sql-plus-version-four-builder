namespace SQLPLUS.Builder.ConfigurationModels
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class BuildDefinition
    {
       
        public BuildDefinition() { }

        #region Ignored Properties

        //TODO: JsonIgnoreProperties shoud be replaced with private readonly variables.

        [JsonIgnore]
        public string SingleLineComment { set; get; } = "--";

        [JsonIgnore]
        public string CommentBlockOpen { set; get; } = "/*";

        [JsonIgnore]
        public string CommentBlockClose { set; get; } = "*/";

        [JsonIgnore]
        public string PrimaryTagIndicator { set; get; } = "+";

        [JsonIgnore]
        public string SupplementalTagIndicator { set; get; } = "&";

        [JsonIgnore]
        public string ExplicitTagIndicator { set; get; } = "#";
        
        [JsonIgnore]
        public string LicenseType { set; get; } = "Professional"; // Community // Professional // Enterprise

        [JsonIgnore]
        public string PrimaryTagPrefix
        {
            get
            {
                return SingleLineComment + PrimaryTagIndicator;
            }
        }
        [JsonIgnore]
        public string SupplementalTagPrefix
        {
            get
            {
                return SingleLineComment + SupplementalTagIndicator;
            }
        }
        [JsonIgnore]
        public string ExplicitTagPrefix
        {
            get
            {
                return SingleLineComment + ExplicitTagIndicator;
            }
        }

        #endregion Ignored Properties
        
        public string SQLClient { set; get; } = "System.Data.SqlClient";
        
        public string SQLExceptionNamespace { set; get; } = "System.Data";
        
        public string Template { set; get; } = "NET";

        public List<BuildRoutine> DBRoutines { set; get; }
        public List<BuildSchema> DBSchemas { set; get; }
        public List<BuildSchema> QuerySchemas { set; get; }
        public List<BuildRoutine> QueryRoutines { set; get; }
        public List<BuildQuery> EnumQueries { set; get; }
        public List<BuildQuery> StaticQueries { set; get; }
        public BuildOptions BuildOptions { set; get; } = new BuildOptions();

        private List<string> errors = new List<string>();
        public List<string> GetErrors()
        {
            return errors;
        }

        public void NullOutZeroLengthCollections()
        {
            if (DBSchemas?.Count == 0)
            {
                DBSchemas = null;
            }
            if (DBRoutines?.Count == 0)
            {
                DBRoutines = null;
            }
            if (QuerySchemas?.Count == 0)
            {
                QuerySchemas = null;
            }
            if (QueryRoutines?.Count == 0)
            {
                QueryRoutines = null;
            }
            if (StaticQueries?.Count == 0)
            {
                StaticQueries = null;
            }
            if (EnumQueries?.Count == 0)
            {
                EnumQueries = null;
            }
        }
    }

    public class BuildSchema
    {
        public string Schema { set; get; }
        public string Namespace { set; get; }

    }

    public class BuildRoutine 
    {
        public string Schema { set; get; }
        public string Namespace { set; get; }
        public string Name { set; get; }
    }

    public class BuildQuery
    {
        public string Name { set; get; }

        private string _Query;
        public string Query
        {
            set
            {
                if(value != _Query)
                {
                    _Query = value;
                }
            }
            get
            {
                if(string.IsNullOrEmpty(_Query))
                {
                    // TODO: This can be romoved when all customers have migrated to new version.
                    if(!string.IsNullOrEmpty(Table) && !string.IsNullOrEmpty(NameColumn) && !string.IsNullOrEmpty(ValueColumn))
                    {
                        string query = $"SELECT {NameColumn}, {ValueColumn} FROM {Table}";
                        if(!string.IsNullOrEmpty(Filter))
                        {
                            query += $" WHERE {Filter}";
                        }
                        _Query = query;
                    }
                }
                return _Query;
            }
        }

        public string Table { set; get; }

        public string NameColumn { set; get; }

        public string ValueColumn { set; get; }

        public string Filter { set; get; }


    }

    public class BuildOptions
    {
        public bool ImplementIChangeTracking { set; get; } = false;
        public bool ImplementIRevertibleChangeTracking { set; get; } = false;
        public bool ImplementINotifyPropertyChanged { set; get; } = false;
        public bool UseNullableReferenceTypes { set; get; } = false;

    }
}
