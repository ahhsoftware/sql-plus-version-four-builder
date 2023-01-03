namespace SQLPLUS.Builder.TemplateModels
{
    #region usings

    using SQLPLUS.Builder.ConfigurationModels;
    using SQLPLUS.Builder.Helpers;
    using SQLPLUS.Builder.Tags;
    using System;
    using System.Collections.Generic;
    using System.IO;

    #endregion

    public class Routine : ErrorBase
    {
        /// <summary>
        /// This is the actual routine name from database or the filename of a query.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Namespace assigned from the build definition build definition or schema.
        /// Note that this value could be the sql plus identifier plus.
        /// </summary>
        public string Namespace { get; }
        
        /// <summary>
        /// This is the routine type as defined on the database, or a concrete query.
        /// </summary>
        public string RoutineType { get; }

        /// <summary>
        /// This is most applicable for SQL scalar functions.
        /// </summary>
        public string DataType { get; }

        /// <summary>
        /// This is the DB schema where the routine resides.
        /// When using queries this value is set to the database default schema
        /// For instance, MSSQL uses dbo.
        /// </summary>
        public string Schema { get; }

        /// <summary>
        /// The data and time the routine was last modified.
        /// </summary>
        public DateTime LastModified { get; }

        #region Routine Tag Property Getters

        private SQLPlusRoutine routineTag { get; }
        public string Author => routineTag.Author;
        public SelectTypes SelectType => routineTag.SelectType;
        public int? CommandTimeout => routineTag.CommandTimeout;
        public string Comment => routineTag.Comment;

        #endregion Routine Tag Property Getters

        #region Project Information Property Getters

        private ProjectInformation project;
        public string ServiceNamespace => Namespace == "+" ? project.RootNamespace : $"{project.RootNamespace}.{Namespace}";
        public string ModelNamespace => Namespace == "+" ? $"{project.RootNamespace}.Models" : $"{project.RootNamespace}.{Namespace}.Models";
        public string ServiceDirectory => Namespace == "+" ? Path.Combine(project.RootDirectory, "Services") : Path.Combine(project.RootDirectory, Namespace, "Services");
        public string ModelDirectory => Namespace == "+" ? Path.Combine(project.RootDirectory, "Models") : Path.Combine(project.RootDirectory, Namespace, "Models");

        #endregion Project Information Property Getters

        #region Build Definition Property Getters

        private BuildDefinition build;
        public bool INotifyPropertyChanged => build.BuildOptions.ImplementINotifyPropertyChanged;
        public bool IChangeTracking => build.BuildOptions.ImplementIRevertibleChangeTracking;
        public bool IRevertibleChangeTracking => build.BuildOptions.ImplementIRevertibleChangeTracking;

        private string inputImplements = null;
        public string InputImplements
        {
            get
            {
                if (inputImplements is null)
                {
                    List<string> list = new List<string>();
                    list.Add("ValidInput");
                    if (INotifyPropertyChanged)
                    {
                        list.Add("INotifyPropertyChanged");
                    }
                    if (IRevertibleChangeTracking)
                    {
                        list.Add("IRevertibleChangeTracking");
                    }
                    else if (IChangeTracking)
                    {
                        list.Add("IChangeTracking");
                    }
                    inputImplements = string.Join(", ", list);
                }
                return inputImplements;
            }
        }

        #endregion Build Definition Property Getters

        /// <summary>
        /// This is the actual name for the service.
        /// </summary>
        public string ServiceName => Name;

        /// <summary>
        /// Name of the input object
        /// </summary>
        public string InputObjectName => $"{Name}Input";

        /// <summary>
        /// Name of the output object
        /// </summary>
        public string OutputObjectName => $"{Name}Output";

        /// <summary>
        /// Name for the result object.
        /// When multisets are used this will be the parent object name.
        /// </summary>
        public string ResultObjectName => $"{Name}Result";

        public string BuildCommandName => $"{Name}_BuildCommand";

        public string BuildCommandParameters
        {
            get
            {
                if (InputParameters.Count != 0)
                {
                    return $"SqlConnection cnn, {InputObjectName} input";
                }
                return "SqlConnection cnn";
            }
        }

        public string ServiceParameters
        {
            get
            {
                if (InputParameters.Count != 0)
                {
                    return $"{InputObjectName} input";
                }
                return String.Empty;
            }
        }
        public string SetParametersName => $"{Name}_SetParameters";
        
        /// <summary>
        /// Actual text content of the routine.
        /// </summary>
        public string[] RoutineLines { set; get; }
        
        /// <summary>
        /// List of intput, output, and return value parameters.
        /// </summary>
        public List<Parameter> Parameters { get; }
       
        public Parameter ReturnValueParameter
        {
            get
            {
                return Parameters.Find(p => p.IsReturnValue); 
            }
        }

        public bool? hasTableValueParameters;
        public bool HasTableValueParameters
        {
            get
            {
                if(!hasTableValueParameters.HasValue)
                {
                    foreach(Parameter parameter in Parameters)
                    {
                        if(parameter.IsTableValueParameter)
                        {
                            hasTableValueParameters = true;
                            break;
                        }
                    }
                    hasTableValueParameters = false;
                }
                return hasTableValueParameters.Value;
            }
        }
        
        /// <summary>
        /// This provides the method signature for the parameterized contructor in the input object.
        /// </summary>
        public string InputParametersConcat
        {
            get
            {
                List<string> properties = new List<string>();
                foreach (Parameter parameter in InputParameters)
                {
                    properties.Add($"{parameter.PropertyType} {parameter.PropertyName}");
                }
                return string.Join(", ", properties);
            }
        }

        /// <summary>
        /// The command type depends on the type of routine.
        /// </summary>
        public string AdoCommandType { get; }

        /// <summary>
        /// The command text depends on the command type.
        /// Procedures, functions, and queries have a slightly different format.
        /// </summary>
        public string AdoCommandText {get; }

        public Routine(
            string name,
            string @namespace,
            string schema,
            string routineType,
            string dataType,
            string adoCommandType,
            string adoCommandText,
            string[] routineLines,
            ProjectInformation project,
            BuildDefinition build,
            SQLPlusRoutine routineTag,
            List<QueryStart> queryTags,
            List<Parameter> parameters,
            List<EnumDefinition> returnValueEnums,
            DateTime lastModified)
        {
            Name = name;
            Namespace = @namespace;
            Schema = schema;
            RoutineType = routineType;
            DataType = dataType;
            AdoCommandType = adoCommandType;
            AdoCommandText = adoCommandText;
            LastModified = lastModified;
            RoutineLines = routineLines;
            this.project = project;
            this.build = build;
            this.routineTag = routineTag;
            Queries = queryTags;
            Parameters = parameters;
            ReturnValueEnums = returnValueEnums;
            this.LastModified = lastModified;
        }

        /// <summary>
        /// Used for mutli set queries.
        /// </summary>
        public List<QueryStart> Queries { set; get; } = new List<QueryStart>();

        /// <summary>
        /// Result sets for routines and functions.
        /// Multiset queries will have more that one.
        /// </summary>
        public List<ResultSet> ResultSets { set; get; } = new List<ResultSet>();
        
        /// <summary>
        /// The enumerations assigend to return values.
        /// </summary>
        public List<EnumDefinition> ReturnValueEnums { set; get; } = new List<EnumDefinition>();


        private List<Parameter> inputParameters;
        /// <summary>
        /// Subset of all parameters that are exclusively inputs.
        /// </summary>
        public List<Parameter> InputParameters
        {
            get
            {
                if (inputParameters == null)
                {
                    inputParameters = Parameters.FindAll(p => p.IsInput);
                }
                return inputParameters;
            }
        }

        public List<Parameter> outputParameters;
        /// <summary>
        /// Subset of all parameters that are exclusively outputs.
        /// </summary>
        public List<Parameter> OutputParameters
        {
            get
            {
                if(outputParameters == null)
                {
                    outputParameters = Parameters.FindAll(p => p.IsOutput);
                }
                return outputParameters;
            }
        }
      
        public List<Parameter> NonInputParameters => Parameters.FindAll(p => p.IsOutput == true || p.IsReturnValue == true);
        
        /// <summary>
        /// List of ignore tags.
        /// </summary>
        public List<Ignore> Ignores { set; get; } = new List<Ignore>();

        /// <summary>
        /// List of cache keys.
        /// </summary>
        public List<CacheKey> CacheKeys { private set; get; }
        public void AddCacheKey(CacheKey cacheKey)
        {
            if(CacheKeys == null)
            {
                CacheKeys = new List<CacheKey>();
            }
            CacheKeys.Add(cacheKey);
        }

        public bool UseNullableReferenceTypes
        {
            get
            {
                return build.BuildOptions.UseNullableReferenceTypes;
            }
        }

        #region Get Using Functions

        private List<string> inputUsings;
        public List<string> InputUsings
        {
            get
            {
                if (inputUsings == null)
                {
                    inputUsings = new List<string>();

                    inputUsings.TryAddItem(project.SQLPLUSBaseNamespace);
                   
                    //System Component Model for the intergaces
                    if (build.BuildOptions.ImplementIChangeTracking || build.BuildOptions.ImplementIRevertibleChangeTracking || build.BuildOptions.ImplementINotifyPropertyChanged)
                    {
                        inputUsings.TryAddItem("System.ComponentModel");
                    }

                    // Changed properties for revertible
                    if (build.BuildOptions.ImplementIRevertibleChangeTracking)
                    {
                        inputUsings.TryAddItem("System.Collections.Generic");
                    }

                    // Get usings for the properties. (paramters)
                    foreach (Parameter parameter in InputParameters)
                    {
                        if (parameter.IsTableValueParameter)
                        {
                            inputUsings.TryAddItem(project.UserDefinedTypeNamepace);
                            inputUsings.TryAddItem("System.Collections.Generic");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(parameter.EnumerationName))
                            {
                                inputUsings.TryAddItem(project.EnumerationsNamespace);
                            }
                            inputUsings.TryAddItem(parameter.Using);
                            
                        }
                        if (parameter.Annotations.Count != 0)
                        {
                            inputUsings.TryAddItem("System.ComponentModel.DataAnnotations");
                        }
                    }

                }
                inputUsings.Sort();
                return inputUsings;
            }
        }

        private List<string> outputUsings;
        public List<string> OutputUsings
        {
            get
            {
                if (outputUsings == null)
                {
                    outputUsings = new List<string>();
                    foreach (Parameter parameter in OutputParameters)
                    {
                        outputUsings.TryAddItem(parameter.Using);
                       
                    }
                    if(ReturnValueParameter != null)
                    {
                        outputUsings.TryAddItem(ReturnValueParameter.Using);
                    }
                    foreach (ResultSet rs in ResultSets)
                    {
                        if (rs.SelectType == SelectTypes.MultiRow)
                        {
                            outputUsings.TryAddItem("System.Collections.Generic");
                        }
                        foreach (Column column in rs.Columns)
                        {
                            outputUsings.TryAddItem(column.Using);
                        }
                    }
                }
                outputUsings.Sort();
                return outputUsings;
            }

        }

        private List<string> serviceUsings;
        public List<string> ServiceUsings
        {
            get
            {
                if (serviceUsings == null)
                {
                    serviceUsings = new List<string>();
                    serviceUsings.TryAddItem("System");
                    serviceUsings.TryAddItem("System.Threading");
                    serviceUsings.TryAddItem(ModelNamespace);
                    serviceUsings.TryAddItem(build.SQLClientNamespace);
                    serviceUsings.TryAddItem(build.SQLExceptionNamespace);
                    
                    if (SelectType == SelectTypes.Json || SelectType == SelectTypes.Json)
                    {
                        serviceUsings.TryAddItem("System.Text");
                    }

                    foreach (Parameter parameter in NonInputParameters)
                    {
                        serviceUsings.TryAddItem(parameter.Using);
                    }
                    foreach(Parameter parameter in InputParameters)
                    {
                        if (parameter.IsTableValueParameter)
                        {
                            serviceUsings.TryAddItem(project.SQLPLUSBaseNamespace);
                        }
                    }
                    
                    foreach (ResultSet rs in ResultSets)
                    {
                        if (rs.SelectType == SelectTypes.MultiRow)
                        {
                            serviceUsings.TryAddItem("System.Collections.Generic");
                        }
                        if (rs.SelectType == SelectTypes.Json || rs.SelectType == SelectTypes.Json)
                        {
                            serviceUsings.TryAddItem("System.Text");
                        }
                        foreach (Column column in rs.Columns)
                        {
                            serviceUsings.TryAddItem(column.Using);
                        }
                    }
                }
                serviceUsings.Sort();
                return serviceUsings;
            }

        }

        #endregion
    }
}