namespace SQLPLUS.Builder.ConfigurationModels
{
    using System.IO;
    public class ProjectInformation
    {
        public ProjectInformation(string rootNamespace, string rootDirectory)
        {
            RootNamespace = rootNamespace;
            RootDirectory = rootDirectory;
            //ConstantsDirectory = Path.Combine(rootDirectory, "Constants");
            //ConstantsNamespace = $"{rootNamespace}.Constants";
            EnumerationsDirectory = Path.Combine(rootDirectory, "Enumerations");
            EnumerationsNamespace = $"{rootNamespace}.Enumerations";
            StaticDataDirectory = Path.Combine(rootDirectory, "Statics");
            StaticDataNamespace = $"{rootNamespace}.Statics";
            SQLPLUSBaseDirectory = Path.Combine(rootDirectory, "SqlPlusBase");
            SQLPLUSBaseNamespace = "SqlPlusBase";
            SQLPLUSValidInputPath = Path.Combine(SQLPLUSBaseDirectory, "ValidInput.cs");
            SQLPLUSValueComparisonsPath = Path.Combine(SQLPLUSBaseDirectory, "ValueComparisons.cs");
            SQLPlusTransientErrorsPath = Path.Combine(SQLPLUSBaseDirectory, "TransientErrors.cs");
            SQLPLUSFolder = Path.Combine(rootDirectory, "SQL+");
            SQLPLUSQueriesFolder = Path.Combine(SQLPLUSFolder, "Queries");
            SQLPLUSDatabaseConnectionPath = Path.Combine(SQLPLUSFolder, "DatabaseConnection.json");
            SQLPLUSBuildDefinitionPath = Path.Combine(SQLPLUSFolder, "BuildDefinition.json");
            UserDefinedTypeDirectory = Path.Combine(rootDirectory, "UserDefinedTypes");
            UserDefinedTypeNamepace = $"{rootNamespace}.UserDefinedTypes";
        }

        //public string ConstantsDirectory { get; }
        //public string ConstantsNamespace { get; }
        public string EnumerationsDirectory { get; }
        public string EnumerationsNamespace { get; }
        public string RootNamespace { get; }
        public string RootDirectory { get; }
        public string StaticDataDirectory { get; }
        public string StaticDataNamespace { get; }
        public string SQLPLUSBaseDirectory { get; }
        public string SQLPLUSBaseNamespace { get; }
        public string SQLPLUSValidInputPath { get; }
        public string SQLPLUSValueComparisonsPath { get; }
        public string SQLPlusTransientErrorsPath { get; }
        public string SQLPLUSQueriesFolder { get; }
        public string UserDefinedTypeNamepace { get; }
        public string UserDefinedTypeDirectory { get; }

        public string SQLPLUSFolder { get; }
        public string SQLPLUSBuildDefinitionPath { get; }
        public string SQLPLUSDatabaseConnectionPath { set; get; }
    }
}