
public class SqlConfiguration
{
    public SqlConfiguration(string connectionString)
    {
        this.ConnectionString = connectionString;
    }

    public string ConnectionString { get; set; }

}