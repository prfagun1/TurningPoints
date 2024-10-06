public class MySqlLoggerProvider : ILoggerProvider
{
    private readonly string _connectionString;
    IHttpContextAccessor _httpContextAccessor;

    public MySqlLoggerProvider(string connectionString, IHttpContextAccessor httpContextAccessor)
    {
        _connectionString = connectionString;
        _httpContextAccessor = httpContextAccessor;


    }

    public ILogger CreateLogger(string categoryName)
    {
        return new MySqlLogger(_connectionString, categoryName, _httpContextAccessor);
    }

    public void Dispose()
    {
        // não é necessário implementar nada aqui
    }
}