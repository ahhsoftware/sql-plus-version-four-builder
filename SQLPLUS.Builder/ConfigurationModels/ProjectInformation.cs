namespace SQLPLUS.Builder.ConfigurationModels
{
    using System.IO;
    public class ProjectInformation
    {
        public ProjectInformation(string rootNamespace, string rootDirectory)
        {
            RootNamespace = rootNamespace;
            RootDirectory = rootDirectory;
            EnumerationsDirectory = Path.Combine(rootDirectory, "Enumerations");
            EnumerationsNamespace = $"{rootNamespace}.Enumerations";
            StaticDataDirectory = Path.Combine(rootDirectory, "Statics");
            StaticDataNamespace = $"{rootNamespace}.Statics";
            SQLPLUSBaseDirectory = Path.Combine(rootDirectory, "SqlPlusBase");
            SQLPLUSBaseNamespace = "SqlPlusBase";
            SQLPLUSValidInputPath = Path.Combine(SQLPLUSBaseDirectory, "ValidInput.cs");
            SQLPLUSValueComparisonsPath = Path.Combine(SQLPLUSBaseDirectory, "ValueComparisons.cs");
            SQLPLUSTransientErrorsPath = Path.Combine(SQLPLUSBaseDirectory, "TransientErrors.cs");
            SQLPLUSFolder = Path.Combine(rootDirectory, "SQL+");
            SQLPLUSQueriesFolder = Path.Combine(SQLPLUSFolder, "Queries");
            SQLPLUSSampleQueriesFolder = Path.Combine(SQLPLUSQueriesFolder, "Samples");
            SQLPLUSSampleQueryPath = Path.Combine(SQLPLUSSampleQueriesFolder, "HelloWorld.sql");
            SQLPLUSDatabaseConnectionPath = Path.Combine(SQLPLUSFolder, "DatabaseConnection.json");
            SQLPLUSBuildDefinitionPath = Path.Combine(SQLPLUSFolder, "BuildDefinition.json");
            UserDefinedTypeDirectory = Path.Combine(rootDirectory, "UserDefinedTypes");
            UserDefinedTypeNamespace = $"{rootNamespace}.UserDefinedTypes";
            SQLPLUSBuildErrorPath = Path.Combine(SQLPLUSFolder, "Errors.txt");
        }

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
        public string SQLPLUSTransientErrorsPath { get; }
        public string SQLPLUSQueriesFolder { get; }
        public string SQLPLUSSampleQueriesFolder { get; }
        public string SQLPLUSSampleQueryPath { get; }
        public string UserDefinedTypeNamespace { get; }
        public string UserDefinedTypeDirectory { get; }
        public string SQLPLUSFolder { get; }
        public string SQLPLUSBuildDefinitionPath { get; }
        public string SQLPLUSDatabaseConnectionPath { get; }
        public string SQLPLUSBuildErrorPath { get; }
        
        public void SetUpForIndividualProjects()
        {
            RootNamespace.Replace(".Services", string.Empty);
            RootNamespace.Replace(".Models", string.Empty);
        }
    }
}