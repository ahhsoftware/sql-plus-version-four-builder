namespace SQLPLUS.Builder.ConfigurationModels
{
    using System.Collections.Generic;

    public class TableValueParameterTags
    {
        public string ParameterName { set; get; }

        public List<string> Annotations { set; get; }
    }
}
