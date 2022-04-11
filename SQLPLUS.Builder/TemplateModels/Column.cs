namespace SQLPLUS.Builder.TemplateModels
{
    using SQLPLUS.Builder.ConfigurationModels;
    using SQLPLUS.Builder.Mappings;
    using System.Collections.Generic;

    public class Column : DBField
    {
        public bool IsNullable { get; }
        public int? CharacterMaxLength { set; get; }
        public string Comment { get; }
        public override string PropertyType
        {
            get
            {
                if(IsNullable)
                {
                    if(dataTypeMapping.IsCSharpValueType || build.BuildOptions.UseNullableReferenceTypes)
                    {
                        return $"{dataTypeMapping.CSharpType}?";
                    }
                }
                return dataTypeMapping.CSharpType;
            }
        }

        public string AdoGetterFormatter => dataTypeMapping.AdoGetterFormatter;
        public Column(
            int index,
            string name,
            string propertyName,
            bool isNullable,
            int? characterMaxLength,
            List<string> annotations,
            ProjectInformation project,
            BuildDefinition build,
            DataTypeMapping dataTypeMapping
            ) : base(index, name, propertyName, project, build, dataTypeMapping)
        {
            IsNullable = isNullable;
            CharacterMaxLength = characterMaxLength;
            Comment = $"Maps to table value column {name}.";
            Annotations = annotations;
        }
    }
}