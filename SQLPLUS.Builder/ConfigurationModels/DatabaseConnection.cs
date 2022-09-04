namespace SQLPLUS.Builder.ConfigurationModels
{
    using System.ComponentModel.DataAnnotations;
    public class DatabaseConnection : ValidInput
    {
        [Required]
        public string DatabaseType { set; get; }

        [Required]
        public string ConnectionString { set; get; }

    }
}
