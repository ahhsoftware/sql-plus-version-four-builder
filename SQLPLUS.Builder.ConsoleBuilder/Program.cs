using SQLPLUS.Builder.BuildServices;
using SQLPLUS.Builder.ConfigurationModels;
using SQLPLUS.Builder.DataCollectors;
using SQLPLUS.Builder.Render;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SQLPLUS.Builder.ConsoleBuilder
{
    internal class Program
    {
        public enum Test
        {
            /// <summary>
            /// Comment
            /// </summary>
            Name = 1
        }

        static async Task Main(string[] args)
        {
            //var service = new procs.Service("initial catalog = v4tests; server=(local); integrated security=true;");
            //var output = await Task.Run(() => service.AllTags(new procs.Models.AllTagsInput("comment", "5555555555554444", "default", "displa", "alan@alan.com", DayOfWeek.Friday, "For", null)));


            ProjectInformation projectInformation = new ProjectInformation("SQLPLUS.Build.Test.Basic",
                "C:\\Users\\Alan\\source\\repos\\sql-plus-version-four-builder\\SQLPLUS.Builder.ConsoleBuilder");
                //"C:\\Users\\Alan\\source\\repos\\sql-plus-version-four-tests\\SQLPLUS.Build.Test.Basic");

            ConfigurationService configurationService = new ConfigurationService(projectInformation);
                
            if (!configurationService.SQLPLusFolderExists())
            {
                Console.WriteLine("No configuration");
                return;
            }

            var buildDefintion = configurationService.GetBuildDefinition();
            var databaseConnection = configurationService.GetDatabaseConnection();
            IDataCollector dataCollector = new MSSQLDataCollector(buildDefintion, databaseConnection, projectInformation);
            IRenderProvider render = new Builder.Render.T4Net.NetRenderProvider(projectInformation, buildDefintion);
            BuildService builder = new BuildService(buildDefintion, projectInformation, dataCollector, render);
            //AttachEvents(builder);
            builder.Run();
            //DetachEvents(builder);
            
            Console.Read();
        }

        private static void ConfigurationService_OnProgressChanged(object sender, ProgressStatusArgs e)
        {
            Console.WriteLine(e.Progress);
        }

        private static void ConfigurationService_OnFileWrite(object sender, FileWriteEventArgs e)
        {
            Console.WriteLine(e.FileName);
        }

        private static void ConfigurationService_OnFileCreated(object sender, FileCreatedEventArgs e)
        {
            Console.WriteLine(e.NewFileName);
        }

        private static void ConfigurationService_OnDirectoryCreated(object sender, DirectoryCreatedEventArgs e)
        {
            Console.WriteLine(e.NewDirectoryPath);
        }

        private static void WriteErrors(BuildDefinition build)
        {
            
            foreach(string error in build.GetErrors())
            {
                Console.WriteLine(error);
            }
        }

        static void Input_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.Write(e.PropertyName);
        }

        static void AttachEvents(BuildService runner)
        {
            runner.OnDirectoryCreated += Runner_OnDirectoryCreated;
            runner.OnFileCreated += Runner_OnFileCreated;
            runner.OnFileWrite += Runner_OnFileWrite;
            runner.OnProgressChanged += Runner_OnProgressChanged;
        }

        static void DetachEvents(BuildService runner)
        {
            runner.OnDirectoryCreated -= Runner_OnDirectoryCreated;
            runner.OnFileCreated -= Runner_OnFileCreated;
            runner.OnFileWrite -= Runner_OnFileWrite;
            runner.OnProgressChanged -= Runner_OnProgressChanged;
        }


        static void Runner_OnProgressChanged(object sender, ProgressStatusArgs e)
        {
            Console.WriteLine($"Progress: {e.Progress} -  Message: {e.Message}");
        }

        static void Runner_OnFileWrite(object sender, FileWriteEventArgs e)
        {
            Console.WriteLine(e.FileName);
        }

        static void Runner_OnFileCreated(object sender, FileCreatedEventArgs e)
        {
            Console.WriteLine(e.NewFileName);
        }

        static void Runner_OnDirectoryCreated(object sender, DirectoryCreatedEventArgs e)
        {
            Console.WriteLine(e.NewDirectoryPath);
        }

        static DatabaseConnection GetDatabaseConnection()
        {
            return new DatabaseConnection
            {
                ConnectionString = "Server = (local); Database = V4Tests; Integrated Security = true;",
                DatabaseType = "MSSQL"
            };
        }

        static ProjectInformation GetProjectInformation(int idx)
        {
            if (idx == 0)
            {
                return new ProjectInformation("SQLPLUS.Build.Test.Basic", "C:\\Users\\Alan\\source\\repos\\sql-plus-version-four-tests\\SQLPLUS.Build.Test.Basic");
            }
            if (idx == 1)
            {
                return new ProjectInformation("SQLPLUS.Build.Test.IChange", "C:\\Users\\Alan\\source\\repos\\sql-plus-version-four-tests\\SQLPLUS.Build.Test.IChange");
            }
            if (idx == 2)
            {
                return new ProjectInformation("SQLPLUS.Build.Test.IRevertible", "C:\\Users\\Alan\\source\\repos\\sql-plus-version-four-tests\\SQLPLUS.Build.Test.IRevertible");
            }
            if (idx == 3)
            {
                return new ProjectInformation("SQLPLUS.Build.Test.INotify", "C:\\Users\\Alan\\source\\repos\\sql-plus-version-four-tests\\SQLPLUS.Build.Test.INotify");
            }
            if (idx == 4)
            {
                return new ProjectInformation("SQLPLUS.Build.Test.NullableTypes", "C:\\Users\\Alan\\source\\repos\\sql-plus-version-four-tests\\SQLPLUS.Build.Test.NullableTypes");
            }
            return null;
        }
        static BuildDefinition GetBuildDefinition(int idx)
        {
            return new BuildDefinition
            {
                

                StaticQueries = new List<BuildQuery>
                {
                    new BuildQuery {Name="StaticOne", Query="SELECT * FROM [dbo].[AllTypesNotNull]"}
                },

                EnumQueries = new List<BuildQuery>
                {
                    new BuildQuery{Name = "TestOne", Query="SELECT [DisplayName] [Name], [Id] [Value], [Comment], [Description] FROM [dbo].[EnumTest]"},
                    new BuildQuery{Name = "TestTwo", Query="SELECT [Id] [Value], [DisplayName] [Name] FROM [dbo].[EnumTest]"}
                },

                BuildOptions = new BuildOptions
                {
                    ImplementIChangeTracking = idx == 1 ? true : false,
                    ImplementIRevertibleChangeTracking = idx == 2 ? true : false,
                    ImplementINotifyPropertyChanged = idx == 3 ? true : false,
                    UseNullableReferenceTypes = idx == 4 ? true : false,
                },

                QuerySchemas = new List<BuildSchema>
                {
                    new BuildSchema()
                    {
                        Schema = "dbo",
                        Namespace = "Samples"
                    }
                },

                //BuildRoutines = new System.Collections.Generic.List<BuildRoutine>()
                //{
                //    new BuildRoutine
                //    {
                //        Schema = "procs",
                //        Namespace = "Procs",
                //        Name = "MultiSets"
                //    },
                //    //[func].[TestBinaryTypesById]
                //    //[generated].[TestBinaryTypesById]
                //    //[procs].[SQLTypesById]
                //},
                DBSchemas = new System.Collections.Generic.List<BuildSchema>()
                {
                    //new BuildSchema
                    //{
                    //    Schema = "procs",
                    //    Namespace = "Procs"
                    //},
                    new BuildSchema
                    {
                        Schema = "funcs",
                        Namespace = "Funcs"
                    },
                    //new BuildSchema
                    //{
                    //    Schema = "func",
                    //    Namespace = "Default"
                    //},
                    new BuildSchema
                    {
                        Schema = "procs",
                        Namespace = "Procs"
                    }
                },

                Template = "DotNetCore",

            };

        }
    }
}
//for (int idx = 0; idx != 5; idx++)
//{
//    BuildDefinition build = GetBuildDefinition(idx);

//    if(!build.IsValid())
//    {
//        WriteErrors(build);
//        break;
//    }
//    DatabaseConnection database = GetDatabaseConnection();
//    ProjectInformation project = GetProjectInformation(idx);
//    IDataCollector dataCollector = new MSSQLDataCollector(build, database, project);
//    IRenderProvider render = new Builder.Render.T4Net.NetRenderProvider(project, build);
//    BuildService runner = new BuildService(build, project, dataCollector, render);
//    AttachEvents(runner);
//    try
//    {
//        runner.Run();
//    }
//    catch(Exception ex)
//    {
//        Console.WriteLine(ex.Message);
//    }

//    DetachEvents(runner);

//    if(!Directory.Exists(project.SQLPLUSFolder))
//    {
//        Directory.CreateDirectory(project.SQLPLUSFolder);
//    }

//    //File.WriteAllText(project.SQLPLUSBuildDefinitionPath, JsonConvert.SerializeObject(build));
//    //File.WriteAllText(project.SQLPLUSDatabaseConnectionPath, JsonConvert.SerializeObject(database));
//}
