namespace SQLPLUS.Builder.TemplateModels
{
    using System.Collections.Generic;

    public class ResultSet
    {
        public List<Column> Columns { get; }
        public SelectTypes SelectType { get; }
        public string Name { get; }

        public string ConcatColumns
        {
            get
            {
                if(Columns != null)
                {
                    string[] array = new string[Columns.Count];
                    for (int idx = 0; idx != Columns.Count; idx++)
                    {
                        array[idx] = Columns[idx].Name;
                    }
                    return string.Join("_", array);
                }
                return string.Empty;
            }
        }

        private string constructorParameters;
        public string ConstructorParameters
        {
            get
            {
                if(constructorParameters == null)
                {
                    string[] array = new string[Columns.Count];
                    for (int idx = 0; idx != Columns.Count; idx++)
                    {
                        array[idx] = $"{Columns[idx].PropertyType} {Columns[idx].PropertyName}"; 
                    }
                    constructorParameters = string.Join(", ", array);
                }
                return constructorParameters;
            }
        }

        public ResultSet(
            string name,
            SelectTypes selectType,
            List<Column> columns)
        {
            Name = name;
            SelectType = selectType;
            Columns = columns;
        }
    }
}
