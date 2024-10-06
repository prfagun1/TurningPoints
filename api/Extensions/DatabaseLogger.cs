using MySql.Data.MySqlClient;

public class MySqlLogger : ILogger
{
    private readonly string _connectionString;
    private readonly string _categoryName;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MySqlLogger(string connectionString, string categoryName, IHttpContextAccessor httpContextAccessor)
    {
        _connectionString = connectionString;
        _categoryName = categoryName;
        _httpContextAccessor = httpContextAccessor;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true; // enable all log levels
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        //if (exception == null)
        //    return;

        var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        Guid guid = Guid.NewGuid();
        string message = formatter(state, exception);

        using var command = connection.CreateCommand();
        if (message.Length > 100)
            message = message.Substring(0, 100);

        command.CommandText = "INSERT INTO log (Id, Process, Type, EventId, Message, Error, RecordUser, RecordDate) VALUES (@Id, @Process, @Type, @EventId, @Message, @Error, @RecordUser, @RecordDate)";
        command.Parameters.AddWithValue("@Id", guid);
        command.Parameters.AddWithValue("@Process", _categoryName);
        command.Parameters.AddWithValue("@Type", logLevel.ToString());
        command.Parameters.AddWithValue("@EventId", eventId.Id);
        command.Parameters.AddWithValue("@Message", message);
        command.Parameters.AddWithValue("@Error", exception?.ToString());
        command.Parameters.AddWithValue("@RecordUser", username);
        command.Parameters.AddWithValue("@RecordDate", DateTime.Now);

        command.ExecuteNonQuery();
    }
}
