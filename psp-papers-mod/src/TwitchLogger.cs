using System;
using Microsoft.Extensions.Logging;

namespace psp_papers_mod;

public class TwitchLogger<T> : ILogger<T> {

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
        switch (logLevel) {
            case LogLevel.Trace:
            case LogLevel.Debug:
                PapersPSP.Log.LogDebug(formatter(state, exception));
                break;
            case LogLevel.None:
            case LogLevel.Information:
                PapersPSP.Log.LogInfo(formatter(state, exception));
                break;
            case LogLevel.Warning:
                PapersPSP.Log.LogWarning(formatter(state, exception));
                break;
            case LogLevel.Error:
                PapersPSP.Log.LogError(formatter(state, exception));
                break;
            case LogLevel.Critical:
                PapersPSP.Log.LogFatal(formatter(state, exception));
                break;
        }
    }

    public bool IsEnabled(LogLevel logLevel) {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state) {
        throw new NotImplementedException();
    }
}