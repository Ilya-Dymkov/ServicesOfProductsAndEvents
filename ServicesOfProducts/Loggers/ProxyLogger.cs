using ServicesOfProducts.Loggers.LoggersSource;

namespace ServicesOfProducts.Loggers;

public class ProxyLogger : IProxyLogger
{
    private static readonly ILoggerFactory Factory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
        builder.AddDebug();
    });

    private ILogger? _logger;
    
    public static ProxyLogger CreateInstance<T>() =>
        new() { _logger = Factory.CreateLogger<T>() };

    public static ProxyLogger CreateInstance<T>(Action<ILoggingBuilder> factoryBuilder) =>
        new() { _logger = LoggerFactory.Create(factoryBuilder).CreateLogger<T>() };

    public void ToLogInfo(string messageInfo)
    {
        if (_logger == null)
            throw new Exception("The logger is not ready yet!");
        
        _logger.LogInformation("\n" + messageInfo + "\n");
    }
}