using Newtonsoft.Json;
using SQLPLUS.Builder.ConfigurationModels;
using SQLPLUS.Builder.Render.Common;
using SQLPLUS.Builder.TemplateModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace SQLPLUS.Builder.BuildServices
{
    public class ConfigurationService
    {
        private readonly ProjectInformation projectInformation;
        
        public ConfigurationService(ProjectInformation projectInformation)
        {
            this.projectInformation = projectInformation;
        }

        public bool SQLPLusFolderExists()
        {
            return Directory.Exists(projectInformation.SQLPLUSFolder);
        }

        public List<string> CreateConfigurationDirectoriesAndFiles()
        {
            List<string> result = new List<string>();
            if (!Directory.Exists(projectInformation.SQLPLUSFolder))
            {
                Directory.CreateDirectory(projectInformation.SQLPLUSFolder);
                result.Add(projectInformation.SQLPLUSFolder);
            }
            
            if(!File.Exists(projectInformation.SQLPLUSDatabaseConnectionPath))
            {
                File.WriteAllText(projectInformation.SQLPLUSDatabaseConnectionPath, JsonConvert.SerializeObject(new DatabaseConnection()));
                result.Add(projectInformation.SQLPLUSDatabaseConnectionPath);
            }
           
            if(!File.Exists(projectInformation.SQLPLUSBuildDefinitionPath))
            {
                File.WriteAllText(projectInformation.SQLPLUSBuildDefinitionPath, JsonConvert.SerializeObject(new BuildDefinition()));
                result.Add(projectInformation.SQLPLUSBuildDefinitionPath);
            }
            
            if(!Directory.Exists(projectInformation.SQLPLUSQueriesFolder))
            {
                Directory.CreateDirectory(projectInformation.SQLPLUSQueriesFolder);
                result.Add(projectInformation.SQLPLUSQueriesFolder);
            }            

            if(!File.Exists(projectInformation.SQLPLUSSampleQueryPath))
            {
                MSSQLSampleQueryTemplate template = new MSSQLSampleQueryTemplate();
                string sampleQueryText = template.TransformText();
                File.WriteAllText(projectInformation.SQLPLUSSampleQueryPath, sampleQueryText);
            }

            return result;
        }

        public void SaveBuildDefinition(BuildDefinition buildDefinition)
        {
            File.WriteAllText(projectInformation.SQLPLUSBuildDefinitionPath, JsonConvert.SerializeObject(buildDefinition));
        }

        public void SaveDatabaseConnection(DatabaseConnection databaseConnection)
        {
            File.WriteAllText(projectInformation.SQLPLUSDatabaseConnectionPath, JsonConvert.SerializeObject(databaseConnection));
        }

        public BuildDefinition GetBuildDefinition()
        {
            if(File.Exists(projectInformation.SQLPLUSBuildDefinitionPath))
            {
                try
                {
                    string json = File.ReadAllText(projectInformation.SQLPLUSBuildDefinitionPath);
                    var result = JsonConvert.DeserializeObject<BuildDefinition>(json);
                    return result;
                }
                catch(Exception ex)
                {
                    File.AppendAllText(projectInformation.SQLPLUSBuildErrorPath, ex.Message);
                }
            }
            return new BuildDefinition();
        }

        public DatabaseConnection GetDatabaseConnection()
        {
            if (File.Exists(projectInformation.SQLPLUSDatabaseConnectionPath))
            {
                try
                {
                    string json = File.ReadAllText(projectInformation.SQLPLUSDatabaseConnectionPath);
                    var result = JsonConvert.DeserializeObject<DatabaseConnection>(json);
                    return result;
                }
                catch (Exception ex)
                {
                    try
                    {
                        File.AppendAllText(projectInformation.SQLPLUSBuildErrorPath, ex.Message);
                    }
                    catch { }
                }
            }

            return new DatabaseConnection();
            
        }
    }
}
