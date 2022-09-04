namespace SQLPLUS.Builder.TemplateModels
{
    #region usings

    using SQLPLUS.Builder.ConfigurationModels;
    using SQLPLUS.Builder.Helpers;
    using SQLPLUS.Builder.Mappings;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Parameters for routines or queries.
    /// </summary>
    public class Parameter : DBField
    {
        /// <summary>
        /// Value supplied to commands as Parameter Name
        /// </summary>
        public string AdoParameterName { get; }

        /// <summary>
        /// Numeric or date time precision
        /// </summary>
        public int? AdoPrecision { get; }

        /// <summary>
        /// Numeric or datetime scale
        /// </summary>
        public int? AdoScale { get; }

        /// <summary>
        /// Character or binary length
        /// </summary>
        public int? AdoSize { get; }

        /// <summary>
        /// Table value parameter user defined type schema.
        /// </summary>
        public string UserDefinedTypeSchema { set; get; }

        /// <summary>
        /// Table value parameter user defined type name.
        /// </summary>
        public string UserDefinedTypeName { set; get; }


        #region DataTypeMapping (Exclusive)

        //We do this here to simplify access to properties in the templates.

        public string AdoSqlDbType => dataTypeMapping.AdoSqlDbType;
        public string AdoUdtTypeName => dataTypeMapping.AdoUdtTypeName;
        public EqualityTestTypes EqualityTestType => dataTypeMapping.EqualityTestType;
        public string CSharpType => dataTypeMapping.CSharpType;
        public bool IsTableValueParameter => dataTypeMapping.IsTableValueParameter;

        #endregion DataTypeMapping (Exclusive)

        #region ParameterModeMapping Properties

        //We do this here to simplify access to properties in the templates.

        private readonly ParameterModeMapping parameterModeMapping;
        public string AdoDirection => parameterModeMapping.AdoDirection;
        public bool IsInput => parameterModeMapping.IsInput;
        public bool IsOutput => parameterModeMapping.IsOutput;
        public bool IsReturnValue => parameterModeMapping.IsReturnValue;
        public string MSSQLParameterMode => parameterModeMapping.MSSQLParameterMode;
        
        #endregion ParameterModeMapping Properties

        /// <summary>
        /// Calculated Value for property type.
        /// </summary>
        public override string PropertyType
        {
            // This could be a list of csharp type - is table value parameter.
            // Enumeration name is not null enum
            // Nullable Reference types have an impact here.
            get
            {
                if (UserDefinedTypeName != null)
                {
                    if (build.BuildOptions.UseNullableReferenceTypes)
                    {
                        return $"List<{UserDefinedTypeName}>".AsNullable();
                    }
                    return $"List<{UserDefinedTypeName}>";
                }

                if (EnumerationName != null)
                {
                    if(parameterModeMapping.IsProcedureReturnValue)
                    {
                        return EnumerationName;
                    }
                    else
                    {
                        return EnumerationName.AsNullable();
                    }
                }

                if(dataTypeMapping.IsCSharpValueType)
                {
                    return dataTypeMapping.CSharpType.AsNullable();
                }
                else
                {
                    if(build.BuildOptions.UseNullableReferenceTypes)
                    {
                        return dataTypeMapping.CSharpType.AsNullable();
                    }
                    return dataTypeMapping.CSharpType;
                }
            }
        }

        /// <summary>
        /// Used for change tracking or notify property.
        /// </summary>
        public string EqualityTestFormatter
        {
            get
            {
                return string.Format(dataTypeMapping.EqualityTest, $"_{PropertyName}", "value");
            }
        }
        public Parameter(
            int index,
            string name,
            string propertyName,
            string adoParameterName,
            int? adoPrecision,
            int? adoScale,
            int? adoSize,
            string userDefinedTypeSchema,
            string userDefinedTypeName,
            ParameterModeMapping parameterModeMapping,
            DataTypeMapping dataTypeMapping,
            BuildDefinition build,
            ProjectInformation project
            ) : base(index, name, propertyName, project, build, dataTypeMapping)
        {
            AdoParameterName = adoParameterName;
            AdoPrecision = adoPrecision;
            AdoScale = adoScale;
            AdoSize = adoSize;
            UserDefinedTypeSchema = userDefinedTypeSchema;
            UserDefinedTypeName = userDefinedTypeName;
            this.parameterModeMapping = parameterModeMapping;
            this.Comment = $"Maps to parameter {name}.";
        }

        #region Tagged Values

        /// <summary>
        /// When the enum tag is applied.
        /// </summary>
        public string EnumerationName { get; set; }

        /// <summary>
        /// When the comment tag is applied or the default comment for a parameter.
        /// </summary>
        public string Comment { set; get; }

        /// <summary>
        /// When the default tag is applied.
        /// </summary>
        public string DefaultValue { set; get; }

        /// <summary>
        /// When the force encryption tag is applied.
        /// </summary>
        public bool ForceColumnEncryption { set; get; } = false;

        /// <summary>
        /// When the required tag is applied.
        /// </summary>
        public bool IsRequired { set; get; } = false;

        /// <summary>
        /// When a cache key is applied.
        /// </summary>
        public int? CacheOrder { set; get; } = null;

        #endregion

        /// <summary>
        /// List of columns when this parameter is a table value parameter.
        /// </summary>
        public List<Column> TVColumns { set; get; } = new List<Column>();

        private string constructorParameters;
        public string TVColumnsConcat
        {
            get
            {
                if(constructorParameters is null)
                {
                    List<string> properties = new List<string>();
                    foreach (Column column in TVColumns)
                    {
                        properties.Add($"{column.PropertyType} {column.PropertyName}");
                    }
                    constructorParameters = string.Join(", ", properties);
                }
                return constructorParameters;
            }
        }

        private List<string> tvColumnsUsings;
        public List<string> TVColumnsUsings
        {
            get
            {
                if (tvColumnsUsings is null)
                {
                    tvColumnsUsings = new List<string>();
                    tvColumnsUsings.TryAddItem(project.SQLPLUSBaseNamespace);
                    foreach (Column c in TVColumns)
                    {
                        if (c.Using != null)
                        {
                            tvColumnsUsings.TryAddItem(c.Using);
                        }
                        if (c.Annotations.Count != 0)
                        {
                            tvColumnsUsings.TryAddItem("System.ComponentModel.DataAnnotations");
                        }
                    }
                }
                tvColumnsUsings.Sort();
                return tvColumnsUsings;
            }
        }
    }
}
