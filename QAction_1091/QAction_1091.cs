using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Skyline.DataMiner.Scripting;

/// <summary>
/// DataMiner QAction Class.
/// </summary>
public static class QAction
{
    /// <summary>
    /// The QAction entry point.
	/// Process SendPatch return - the "down" json key will flip true/false
	/// {
    ///"server": "192.168.48.36:80",
    ///"weight": 1,
    ///"max_conns": 0,
    ///"max_fails": 0,
    ///"fail_timeout": "10s",
    ///"slow_start": "0s",
    ///"route": "",
    ///"backup": false,
    //"down": false
	/// </summary>
	/// <param name="protocol">Link with SLProtocol process.</param>
public static void Run(SLProtocol protocol)
    {
        try
        {
        }
        catch (Exception ex)
        {
            protocol.Log("QA" + protocol.QActionID + "|" + protocol.GetTriggerParameter() + "|Run|Exception thrown:" + Environment.NewLine + ex, LogType.Error, LogLevel.NoLogging);
        }
    }
}
