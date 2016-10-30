using System.Collections.Generic;

public class LogMessage {

    private string logMessage;
    private string logTime;
    private string callingMethod;

    private LogLevel level;

    public LogMessage(string logMessage) : this(logMessage, null, LogLevel.Warning) { }
    
    public LogMessage(object logMessage, LogLevel level) : this(logMessage, null, level) { }

    public LogMessage(object logMessage, object callingMethod) : this(logMessage, callingMethod, LogLevel.Warning) { }

    public LogMessage(object logMessage, object callingMethod, LogLevel level)
    {
        this.logMessage = logMessage.ToString();
        logTime = System.DateTime.Now.ToString();
        this.callingMethod = callingMethod.GetType().ToString(); //TODO
        this.level = level;
    }

    public override string ToString()
    {
        return logMessage;
    }
}
