namespace SQLPLUS.Builder.ConfigurationModels
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class BuildDefinition
    {
        //TODO: JsonIgnoreProperties shoud be replaced with private readonly variables.
        public BuildDefinition() { }

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

        public string SQLClientNamespace { set; get; } = "System.Data.SqlClient";
        public string SQLExceptionNamespace { set; get; } = "System.Data";
        public string Template { set; get; } = "NET";

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

        public List<BuildQuery> EnumQueries { set; get; }
        public BuildOptions BuildOptions { set; get; } = new BuildOptions();
        public List<BuildRoutine> BuildRoutines { set; get; }
        public List<BuildSchema> BuildSchemas { set; get; }
        public List<BuildSchema> BuildQuerySchemas { set; get; }
        public List<BuildRoutine> BuildQueryRoutines { set; get; }
        public List<BuildQuery> StaticQueries { set; get; }

        private List<string> errors = new List<string>();
        public List<string> GetErrors()
        {
            return errors;
        }

        public bool IsValid()
        {
            errors.Clear();
            string[] clients = { "System.Data.SqlClient", "Microsoft.Data.SqlClient" };

            if(string.IsNullOrEmpty(SQLClientNamespace))
            {
                AddMissingParameterError(nameof(SQLClientNamespace));
            }
            else
            {
                if(!clients.Contains(SQLClientNamespace))
                {
                    AddInvalidValueError(nameof(SQLClientNamespace), clients);
                }
            }




            return errors.Count == 0;
        }

        private void AddMissingParameterError(string parameter)
        {
            errors.Add($"{parameter} is missing.");
        }

        private void AddInvalidValueError(string parameter, params string[] values)
        {
            string validValues = $"Valid Values: {string.Join(" | ", values)}";
            errors.Add($"{parameter} is invalid. {validValues}");
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
        public string Query { set; get; }
    }

    public class BuildOptions
    {
        public bool ImplementIChangeTracking { set; get; } = false;
        public bool ImplementIRevertibleChangeTracking { set; get; } = false;
        public bool ImplementINotifyPropertyChanged { set; get; } = false;
        public bool IncludeAsyncServices { set; get; } = false;
        public bool UseNullableReferenceTypes { set; get; } = false;

    }
}
