using System.Collections.Generic;

public static class GameManager
{

    static bool useUnityLogger = true;

    static LogLevel currentLogLevel = LogLevel.Medium;

    public static UnityEngine.GameObject inGameLogger;

    public static void Log(object toPrint, object callingClass = null, LogLevel logLevel = LogLevel.Medium)
    {
        if (currentLogLevel != LogLevel.None && currentLogLevel >= logLevel)
        {
            if (callingClass != null)
            {
                if (useUnityLogger)
                {
                    UnityEngine.Debug.Log(callingClass + ": " + toPrint);
                }
                else
                {
                    //TODO
                }
            }
            else
            {
                if (useUnityLogger)
                {
                    UnityEngine.Debug.Log(toPrint);
                }
                else
                {
                    //TODO
                }
            }

        }
    }
}

/// <summary>
/// Log Levels control if messages are printed.
/// <list type="number">
/// <listheader>Levels</listheader>
/// <item>
/// <term>None: </term>
/// <description>should never be used as no messages are printed at this level.</description>
/// </item>
/// <item>
/// <term>Low: </term>
/// <description>something has gone quite wrong.</description>
/// </item>
/// <item>
/// <term>Medium: </term>
/// <description>the default level for debuging.</description>
/// </item>
/// <item>
/// <term>High: </term>
/// <description>for useful information.</description>
/// </item>
/// <item>
/// <term>Verbose: </term>
/// <description>for extrainious information.</description>
/// </item>
/// <item>
/// <term>Facist: </term>
/// <description>for calls which occure every frame.</description>
/// </item>
/// </list>
/// </summary>
public enum LogLevel
{
    None, Low, Medium, High, Verbose, Facist
}
