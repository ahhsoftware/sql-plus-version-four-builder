namespace SQLPLUS.Builder.ConfigurationModels
{
    using System.IO;
    public class ProjectInformation
    {
        public ProjectInformation(string rootNamespace, string rootDirectory)
        {
            RootNamespace = rootNamespace;
            RootDirectory = rootDirectory;
            ConstantsDirectory = Path.Combine(rootDirectory, "Constants");
            ConstantsNamespace = $"{rootNamespace}.Constants";
            EnumerationsDirectory = Path.Combine(rootDirectory, "Enumerations");
            EnumerationsNamespace = $"{rootNamespace}.Enumerations";
            StaticDataDirectory = Path.Combine(rootDirectory, "Statics");
            StaticDataNamespace = $"{rootNamespace}.Statics";
            SqlPlusBaseDirectory = Path.Combine(rootDirectory, "SqlPlusBase");
            SqlPlusBaseNamespace = "SqlPlusBase";
            SQLPLUSValidInputPath = Path.Combine(SqlPlusBaseDirectory, "ValidInput.cs");
            SQLPLUSValueComparisonsPath = Path.Combine(SqlPlusBaseDirectory, "ValueComparisons.cs");
            SQLPlusTransientErrorsPath = Path.Combine(SqlPlusBaseDirectory, "TransientErrors.cs");
            SQLPLUSQueryFolder = Path.Combine(rootDirectory, "SQL+", "Queries");
            UserDefinedTypeDirectory = Path.Combine(rootDirectory, "UserDefinedTypes");
            UserDefinedTypeNamepace = $"{rootNamespace}.UserDefinedTypes";
        }

        public string ConstantsDirectory { get; }
        public string ConstantsNamespace { get; }
        public string EnumerationsDirectory { get; }
        public string EnumerationsNamespace { get; }
        public string RootNamespace { get; }
        public string RootDirectory { get; }
        public string StaticDataDirectory { get; }
        public string StaticDataNamespace { get; }
        public string SqlPlusBaseDirectory { get; }
        public string SqlPlusBaseNamespace { get; }
        public string SQLPLUSValidInputPath { get; }
        public string SQLPLUSValueComparisonsPath { get; }
        public string SQLPlusTransientErrorsPath { get; }
        public string SQLPLUSQueryFolder { get; }
        public string UserDefinedTypeNamepace { get; }
        public string UserDefinedTypeDirectory { get; }
    }
}