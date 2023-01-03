namespace SQLPLUS.Builder.ConfigurationModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class BuildDefinition
    {
        public BuildDefinition() { }
        public string SQLClientNamespace { set; get; } = "System.Data.SqlClient";

        [Required]
        public string SQLExceptionNamespace { set; get; } = "System.Data";

        [Required]
        public string Template { set; get; } = "NET";

        [Required]
        public string SingleLineComment { set; get; } = "--";

        [Required]
        public string CommentBlockOpen { set; get; } = "/*";

        [Required]
        public string CommentBlockClose { set; get; } = "*/";

        [Required]
        public string PrimaryTagIndicator { set; get; } = "+";

        [Required]
        public string SupplementalTagIndicator { set; get; } = "&";

        [Required]
        public string ExplicitTagIndicator { set; get; } = "#";

        public string PrimaryTagPrefix
        {
            get
            {
                return SingleLineComment + PrimaryTagIndicator;
            }
        }
        public string SupplementalTagPrefix
        {
            get
            {
                return SingleLineComment + SupplementalTagIndicator;
            }
        }
        public string ExplicitTagPrefix
        {
            get
            {
                return SingleLineComment + ExplicitTagIndicator;
            }
        }
        
        
        public string LicenseType { set; get; } = "Professional"; // Community // Professional // Enterprise

        public List<BuildQuery> EnumQueries { set; get; }
        public BuildOptions BuildOptions { set; get; }
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
