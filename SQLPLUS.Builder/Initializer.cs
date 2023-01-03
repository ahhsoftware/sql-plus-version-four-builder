using Newtonsoft.Json;
using SQLPLUS.Builder.ConfigurationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SQLPLUS.Builder
{
    [Flags]
    public enum InitializeStates
    {
        NoBuild = 0,

        HasSQLPlusFolder = 1,

        HasDatabaseConnectionFile = 1,
        DataBaseConfigurationValid = 2,

        HasBuildDefintion = 4,
        BuildDefinitionValid = 8,

        HasConcreteQueries = 16,
        ConcreteQueiresValid = 32
    }

    public static class InitializeStateExtensions
    {
        public static void AddValue(this ref InitializeStates source, InitializeStates valueToAdd)
        {
            source |= valueToAdd;
        }

        public static void RemoveValue(this ref InitializeStates source, InitializeStates valueToRemove)
        {
            source &= ~valueToRemove;
        }
    }


    public class Initializer : ValidInput
    {
        private readonly ProjectInformation project;
        private DatabaseConnection databaseConnection;
        private BuildDefinition buildDefinition;
        
        private InitializeStates initializeState = InitializeStates.NoBuild;

        private BuildDefinition MSSQLSettings = new BuildDefinition
        {
            SingleLineComment = "--",
            CommentBlockClose = "*/",
            CommentBlockOpen = "/*",
            PrimaryTagIndicator = "+",
            SupplementalTagIndicator = "&",
            ExplicitTagIndicator = "#"
        };

        private List<string> errors = new List<string>();

        public Initializer(ProjectInformation project)
        {
            this.project = project ?? throw new ArgumentNullException(nameof(project));
        }

        public void Initialize()
        {
            TryGetDatabaseConnection();
            TryGetBuildDefinition();
        }

        public InitializeStates InitializeState
        {
            get
            {
                return initializeState;
            }
        }

        public DatabaseConnection DatabaseConnection
        {
            get
            {
                return databaseConnection;
            }
        }

        public void TryGetBuildDefinition()
        {
            try
            {
                if (File.Exists(project.SQLPLUSBuildDefinitionPath))
                {
                    initializeState.AddValue(InitializeStates.HasBuildDefintion);
                    buildDefinition = JsonConvert.DeserializeObject<BuildDefinition>(File.ReadAllText(project.SQLPLUSBuildDefinitionPath));
                    
                }
            }
            catch(Exception ex)
            {
                errors.Add($"Build Definition Error: {ex.Message}");
            }
        }
        private bool TryGetDatabaseConnection()
        {
            bool result = false;

            try
            {
                if (File.Exists(project.SQLPLUSDatabaseConnectionPath))
                {
                    initializeState.AddValue(InitializeStates.HasDatabaseConnectionFile);
                    this.databaseConnection = JsonConvert.DeserializeObject<DatabaseConnection>(File.ReadAllText(project.SQLPLUSDatabaseConnectionPath));
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            return result;
        }
    }
}
