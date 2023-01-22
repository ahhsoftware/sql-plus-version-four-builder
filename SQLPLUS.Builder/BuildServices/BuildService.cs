namespace SQLPLUS.Builder
{
    using SQLPLUS.Builder.BuildServices;
    using SQLPLUS.Builder.ConfigurationModels;
    using SQLPLUS.Builder.DataCollectors;
    using SQLPLUS.Builder.DataServices.MSSQL.Models;
    using SQLPLUS.Builder.Helpers;
    using SQLPLUS.Builder.Render;
    using SQLPLUS.Builder.TemplateModels;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class BuildService
    {
        private readonly IDataCollector data;
        private readonly IRenderProvider render;
        private readonly BuildDefinition build;
        private readonly ProjectInformation project;
        private List<string> directories = new List<string>();

        public event EventHandler<FileCreatedEventArgs> OnFileCreated;
        public event EventHandler<DirectoryCreatedEventArgs> OnDirectoryCreated;
        public event EventHandler<FileWriteEventArgs> OnFileWrite;
        public event EventHandler<ProgressStatusArgs> OnProgressChanged;

        public void UpdateProgress(string message)
        {
            OnProgressChanged?.Invoke(this, new ProgressStatusArgs(Progress(), message));
        }

        public BuildService(BuildDefinition build, ProjectInformation project, IDataCollector data, IRenderProvider render)
        {
            this.data = data;
            this.render = render;
            this.build = build;
            this.project = project;
        }

        public int renderCount = 0;
        private decimal progress = 0;
        private decimal progressMax = 20;

        public void Run()
        {
            UpdateProgress("Starting build...");

            UpdateProgress($"Starting collect routines.");
            List<Routine> routines = data.CollectDBRoutinesAndQueryRoutines().OrderBy(r => r.ServiceNamespace).ThenBy(r => r.ServiceName).ToList();
            bool errorFound = false;
            foreach (Routine routine in routines)
            {
                if(routine.HasError)
                {
                    if(!errorFound)
                    {
                        UpdateProgress($"Build failed: Errors found in SQL Routine {routine.Name}");
                        errorFound = true;
                    }
                    UpdateProgress($"Error: {routine.Name} - {routine.ErrorMessage}");
                }
            }

            if (errorFound) return;

            UpdateProgress($"Complete collect routines with count: {routines.Count}.");

            UpdateProgress($"Starting collect Enum Queries.");
            List<EnumCollection> enums = data.CollectEnumCollections();
            UpdateProgress($"Complete collect Enum Queries with count: {enums.Count}.");

            UpdateProgress($"Starting collect Static Queries.");
            List<StaticCollection> statics = data.CollectStaticCollections();
            UpdateProgress($"Complete collect Static Queries with count: {statics.Count}.");

            //TODO:
            //Collect Constants

            //If nothing to build
            //UpdateProgress("Nothing to build");
            //return

            UpdateProgress("Starting write base objects.");
            WriteBaseObjectsIfRequired(routines);
            WriteServiceBase(routines);
            WriteTransientErrors();
            WriteTransientErrorsExample();
            UpdateProgress("Write base objects complete.");

            UpdateProgress("Start create schema directories.");
            CreateSchemaDirectoriesIfRequired(routines);
            UpdateProgress("Complete create schema directories.");

            UpdateProgress("Starting write Input objects.");
            WriteInputObjectsIfRequired(routines);
            UpdateProgress("Write Input objects complete.");

            UpdateProgress("Starting write Output objects.");
            WriteOutputObjects(routines);
            UpdateProgress("Write Output objects complete.");

            UpdateProgress("Starting write UserDefinedType objects.");
            WriteUserDefinedTypes(routines);
            UpdateProgress("Write UserDefinedTypes objects complete.");

            UpdateProgress("Starting write Service objects.");
            WriteServices(routines);
            UpdateProgress("Write Service objects complete.");

            UpdateProgress("Starting write Enum objects.");
            WriteEnums(enums);
            UpdateProgress("Write Enum objects complete.");

            UpdateProgress("Starting write Static objects.");
            WriteStatics(statics);
            UpdateProgress("Write Static objects complete.");

        }

        private void WriteStatics(List<StaticCollection> statics)
        {
            foreach(StaticCollection data in statics)
            {
                string content = render.Statics(data);
                WriteText(content, project.StaticDataDirectory, data.Name);
            }
        }


        private void WriteEnums(List<EnumCollection> enums)
        {
            foreach(EnumCollection e in enums)
            {
                string content = render.Enumerations(e);
                WriteText(content, project.EnumerationsDirectory, e.Name);
            }
        }

        private void WriteServices(List<Routine> routines)
        {
            foreach(Routine routine in routines)
            {
                string content = render.ServiceMethod(routine);
                WriteText(content, routine.ServiceDirectory, routine.ServiceName);
            }
        }

        private void WriteServiceBase(List<Routine> routines)
        {
            List<string> nameSpaces = new List<string>();
            foreach(Routine routine in routines)
            {
                if(!nameSpaces.Contains(routine.ServiceNamespace))
                {
                    string content = render.ServiceBase(routine.ServiceNamespace);
                    WriteText(content, routine.ServiceDirectory, "ServiceBase");
                    nameSpaces.Add(routine.ServiceNamespace);
                }
            }
        }

        #region Complete

        private void WriteOutputObjects(List<Routine> routines)
        {
            foreach (Routine routine in routines)
            {
                string content = render.OutputObject(routine);
                WriteText(content, routine.ModelDirectory, routine.OutputObjectName);
            }
        }

        private void WriteUserDefinedTypes(List<Routine> routines)
        {
            List<string> parameterNames = new List<string>();
            List<Parameter> parameters = new List<Parameter>();
            foreach (Routine routine in routines)
            {
                foreach (Parameter parameter in routine.Parameters)
                {
                    if (parameter.IsTableValueParameter)
                    {
                        if (!parameterNames.Contains(parameter.Name))
                        {
                            parameters.Add(parameter);
                            parameterNames.Add(parameter.Name);
                        }
                    }
                }
            }

            foreach (Parameter parameter in parameters)
            {
                string content = render.UserDefinedType(parameter);
                WriteText(content, project.UserDefinedTypeDirectory, parameter.UserDefinedTypeName);
            }
        }

        public void WriteTransientErrors()
        {
            string content = render.TransientErrors();
            string directory = project.SQLPLUSBaseDirectory;
            string fileName = "TransientErrors";
            WriteText(content, directory, fileName);
        }

        public void WriteTransientErrorsExample()
        {
            string content = render.TransientErrorsExample();
            string directory = project.SQLPLUSBaseDirectory;
            string fileName = "TransientErrorsExample";
            WriteText(content, directory, fileName);
        }

        

        private int Progress()
        {
            progress++;
            decimal percent = (progress / progressMax) * 100;
            if (percent >= 100)
            {
                return 99;
            }
            else
            {
                return decimal.ToInt32(percent);
            }
        }

        private bool RequiresValidInput(List<Routine> routines)
        {
            bool result = false;
            foreach (Routine routine in routines)
            {
                if (routine.InputParameters.Count != 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void WriteText(string content, string directory, string fileName)
        {
            if (!directories.Contains(directory))
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    OnDirectoryCreated?.Invoke(this, new DirectoryCreatedEventArgs(directory));
                }
                directories.Add(directory);
            }
            string path = Path.Combine(directory, $"{fileName}.cs");
            bool fileExisted = File.Exists(path);
            File.WriteAllText(path, content);
            if (!fileExisted)
            {
                OnFileCreated?.Invoke(this, new FileCreatedEventArgs(path));
            }
            OnFileWrite?.Invoke(this, new FileWriteEventArgs(path));
            renderCount++;
        }

        private void WriteBaseObjectsIfRequired(List<Routine> routines)
        {
            if (RequiresValidInput(routines))
            {
                WriteValidInput();

                List<string> usings = UsingsForHelpers(routines);
                List<string> types = TypesHelpers(routines);
                List<Parameter> parameters = ParametersForHelpers(routines);

                if (
                    build.BuildOptions.ImplementIChangeTracking ||
                    build.BuildOptions.ImplementINotifyPropertyChanged ||
                    build.BuildOptions.ImplementIRevertibleChangeTracking ||
                    parameters.Count != 0)
                {
                    if (types.Count != 0 || parameters.Count != 0)
                    {
                        WriteHelpers(usings, types, parameters);
                    }
                }
            }
        }

        private List<Parameter> ParametersForHelpers(List<Routine> routines)
        {
            List<string> allParameters = new List<string>();
            List<Parameter> result = new List<Parameter>();
            foreach(var routine in routines)
            {
                foreach(var parameter in routine.Parameters)
                {
                    if(parameter.IsTableValueParameter)
                    {
                        string name = $"{parameter.UserDefinedTypeSchema}.{parameter.UserDefinedTypeName}";
                        if(!allParameters.Contains(name))
                        {
                            allParameters.Add(name);
                            result.Add(parameter);
                        }
                    }
                }
            }
            return result;
        }

        private void WriteValidInput()
        {
            string content = render.ValidInput();
            WriteText(content, project.SQLPLUSBaseDirectory, "ValidInput");
        }
        private void WriteHelpers(List<string> usings, List<string> types, List<Parameter> parameters)
        {
            string content = render.Helpers(usings, types, parameters);
            WriteText(content, project.SQLPLUSBaseDirectory, "Helpers");
        }

        private static List<string> TypesHelpers(List<Routine> routines)
        {
            List<string> result = new List<string>();
            foreach (Routine routine in routines)
            {
                foreach (Parameter parameter in routine.Parameters)
                {
                    if (!parameter.IsTableValueParameter)
                    {
                        if (parameter.EqualityTestType == EqualityTestTypes.ValuesAreEqual)
                        {
                            if (!result.Contains(parameter.PropertyType))
                            {
                                result.Add(parameter.PropertyType);
                            }
                        }
                        if(parameter.EqualityTestType == EqualityTestTypes.VariantsAreEqual)
                        {
                            if (!result.Contains(parameter.PropertyType))
                            {
                                result.Add(parameter.PropertyType);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private List<string> UsingsForHelpers(List<Routine> routines)
        {
            List<string> result = new List<string>();

            foreach (Routine routine in routines)
            {
                foreach (Parameter parameter in routine.Parameters)
                {
                    result.TryAddItem(parameter.Using);
                    if(parameter.TVColumns.Count != 0)
                    {
                        result.TryAddItem("System.Data");
                        result.TryAddItem(project.UserDefinedTypeNamepace);
                        result.TryAddItem(project.SQLPLUSBaseNamespace);
                        foreach(Column column in parameter.TVColumns)
                        {
                            result.TryAddItem(column.Using);
                        }
                    }
                }
            }
            return result;
        }

        private void CreateSchemaDirectoriesIfRequired(List<Routine> routines)
        {
            foreach (Routine routine in routines)
            {
                if (routine.Namespace != "+")
                {
                    string directory = Path.Combine(project.RootDirectory, routine.Namespace);
                    if (!directories.Contains(directory))
                    {
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                            OnDirectoryCreated?.Invoke(this, new DirectoryCreatedEventArgs(directory));
                        }
                        directories.Add(directory);
                    }
                }
            }
        }

        private void WriteInputObjectsIfRequired(List<Routine> routines)
        {
            //Insuring we have all 

            foreach (Routine routine in routines)
            {
                if (routine.InputParameters.Count != 0)
                {

                    string content = render.InputObject(routine);
                    string directory = routine.Namespace == "+" ?
                        Path.Combine(project.RootDirectory, "Models") :
                        Path.Combine(project.RootDirectory, routine.Namespace, "Models");
                    WriteText(content, directory, $"{routine.Name}Input");
                }
            }
        }

        #endregion
    }
}
