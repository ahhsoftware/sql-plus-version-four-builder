using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLPLUS.Builder;
using SQLPLUS.Builder.ConfigurationModels;
using SQLPLUS.Builder.DataCollectors;
using SQLPLUS.Builder.Render;

namespace SQLPLUS.Builder.ConsoleBuilder
{
    [Flags]
    public enum Vals
    {
        None = 0,
        Two = 2,
        Four = 4,
        Eight = 8
    }

    public static class ValsHelper
    {
        public static void AddValue(this ref Vals source, Vals valueToAdd)
        {
            source |= valueToAdd;
        }

        public static void RemoveValue(this ref Vals source, Vals valueToRemove)
        {
            source &= ~valueToRemove;
        }
    }

    internal class Program
    {

        private static T AddtValue<T> (T source, T value) where T: System.Enum
        {
            return source;
        }


        static void Main(string[] args)
        {

            Vals val = Vals.None;

            val.AddValue(Vals.Two);
            val.AddValue(Vals.Two);
            val.AddValue (Vals.Four);
            val.AddValue(Vals.Four);
            val.AddValue(Vals.Eight);
            val.RemoveValue(Vals.Two);
            val.RemoveValue(Vals.Two);
            val.RemoveValue(Vals.Four);



            for (int idx = 0; idx != 5; idx++)
            {
                BuildDefinition build = GetBuildDefinition(idx);
                DatabaseConnection database = GetDatabaseConnection();
                ProjectInformation project = GetProjectInformation(idx);
                IDataCollector dataCollector = new MSSQLDataCollector(build, database, project);
                IRenderProvider render = new Builder.Render.T4Net.NetRenderProvider(project, build);
                Runner runner = new Runner(build, project, dataCollector, render);
                AttachEvents(runner);
                runner.Run();
                DetachEvents(runner);

                if(!Directory.Exists(project.SQLPLUSFolder))
                {
                    Directory.CreateDirectory(project.SQLPLUSFolder);
                }

                File.WriteAllText(project.SQLPLUSBuildDefinitionPath, JsonConvert.SerializeObject(build));
                File.WriteAllText(project.SQLPLUSDatabaseConnectionPath, JsonConvert.SerializeObject(database));
            }

            Console.Read();
        }
        static void Input_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.Write(e.PropertyName);
        }

        static void AttachEvents(Runner runner)
        {
            runner.OnDirectoryCreated += Runner_OnDirectoryCreated;
            runner.OnFileCreated += Runner_OnFileCreated;
            runner.OnFileWrite += Runner_OnFileWrite;
            runner.OnProgressChanged += Runner_OnProgressChanged;
        }

        static void DetachEvents(Runner runner)
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
                    new BuildQuery{Name = "TestOne", Query="SELECT [Id] [Value], [DisplayName] [Name], [Comment] [Comment] FROM [dbo].[EnumTest]"},
                    new BuildQuery{Name = "TestTwo", Query="SELECT [Id] [Value], [DisplayName] [Name] FROM [dbo].[EnumTest]"}
                },
                BuildOptions = new BuildOptions
                {
                    ImplementIChangeTracking = idx == 1 ? true : false,
                    ImplementIRevertibleChangeTracking = idx == 2 ? true : false,
                    ImplementINotifyPropertyChanged = idx == 3 ? true : false,
                    UseNullableReferenceTypes = idx == 4 ? true : false,
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
                BuildSchemas = new System.Collections.Generic.List<BuildSchema>()
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