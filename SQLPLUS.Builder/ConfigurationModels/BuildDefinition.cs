namespace SQLPLUS.Builder.ConfigurationModels
{
    using System.Collections.Generic;

    public class BuildDefinition
    {
        public BuildDefinition() { }

        public List<BuildQuery> ConstantQueries { set; get; }
        public List<BuildQuery> EnumQueries { set; get; }
        public BuildOptions BuildOptions { set; get; }
        public List<BuildRoutine> BuildRoutines { set; get; }
        public List<BuildSchema> BuildSchemas { set; get; }
        public List<BuildQuery> StaticQueries { set; get; }
        public string SQLClientNamespace { set; get; } = "System.Data.SqlClient";
        public string SQLExceptionNamespace { set; get; } = "System.Data";
        public string Template { set; get; } = "DotNetCore";
        public string SingleLineComment { set; get; } = "--";
        public string CommentBlockOpen { set; get; } = "/*";
        public string CommentBlockClose { set; get; } = "*/";
        public string PrimaryTagIndicator { set; get; } = "+";
        public string SupplementalTagIndicator { set; get; } = "&";
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
        public bool UseNullableReferenceTypes { set; get; } = true;

    }
}
