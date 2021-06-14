using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Skyline.DataMiner.Scripting;
using Newtonsoft.Json;
using Parameter = Skyline.DataMiner.Scripting.Parameter;


/// <summary>
/// DataMiner QAction Class.
/// Process the returned json from Session 1030 (GetInitialEndPointInfo) to build the Endpoints Table.
/// </summary>
public static class QAction
{
    /// <summary>
    /// The QAction entry point.
    /// </summary>
    /// <param name="protocol">Link with SLProtocol process.</param>
    public static void Run(SLProtocolExt protocol)
    {
        try
        {
			List<EndpointsQActionRow> tableRows = new List<EndpointsQActionRow>();

			//json processing to EndpointsQActionRow objects


			//fill table
			protocol.endpoints.FillArray(tableRows);

        }
        catch (Exception ex)
        {
            protocol.Log("QA" + protocol.QActionID + "|" + protocol.GetTriggerParameter() + "|Run|Exception thrown:" + Environment.NewLine + ex, LogType.Error, LogLevel.NoLogging);
        }
    }
}
