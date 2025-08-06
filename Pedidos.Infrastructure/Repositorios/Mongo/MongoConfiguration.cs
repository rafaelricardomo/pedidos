public class MongoConfiguration
{
    public string ConnectionString { get; }

    public MongoConfiguration(string connectionString)
    {
        ConnectionString = connectionString;
    }
}