namespace SQLPLUS.Builder.ConfigurationModels
{
    public class DatabaseConnection
    {
        public string DatabaseType { set; get; } = "MSSQLServer";
        public string ConnectionString { set; get; }

    }
}
