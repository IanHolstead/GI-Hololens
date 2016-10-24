using System.Collections.Generic;
using System;

public static class GameManager
{

    static bool useUnityLogger = true;

    static LogLevel currentLogLevel = LogLevel.Warning;

    public static UnityEngine.GameObject inGameLogConsole;

    static LinkedList<LogMessage> log = new LinkedList<LogMessage>();
    
    private static LinkedList<KeyValuePair<DateTime, LinkedListNode<LogMessage>>> toBeRemoved = new LinkedList<KeyValuePair<DateTime, LinkedListNode<LogMessage>>>();

    public static void Log(object toPrint, object callingClass = null, LogLevel logLevel = LogLevel.Warning)
    {

    }

    public static void Log(object toPrint, object callingClass = null, LogLevel logLevel = LogLevel.Warning, float duration = 0)
    {
        if (currentLogLevel != LogLevel.None && currentLogLevel >= logLevel)
        {
            if (useUnityLogger)
            {
                UnityEngine.Debug.Log(toPrint);
            }
            else
            {
                log.AddLast(new LogMessage(toPrint, callingClass != null ? callingClass.ToString() : "", logLevel));
                if (duration > 0)
                {
                    
                    //DateTime.Now.se;
                }
                inGameLogConsole.SendMessage("LogMesasge", log.Last);
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
/// <term>Error: </term>
/// <description>something has gone quite wrong.</description>
/// </item>
/// <item>
/// <term>Warning: </term>
/// <description>the default level for debuging.</description>
/// </item>
/// <item>
/// <term>Log: </term>
/// <description>for useful information.</description>
/// </item>
/// <item>
/// <term>Verbose: </term>
/// <description>for calls which occure every frame.</description>
/// </item>
/// <item>
/// <term>Facist: </term>
/// <description>for extrainious information.</description>
/// </item>
/// </list>
/// </summary>
public enum LogLevel
{
    None, Error, Warning, Log, Verbose, Facist
}
