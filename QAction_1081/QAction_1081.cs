using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Skyline.DataMiner.Scripting;
using Newtonsoft.Json;
using Parameter = Skyline.DataMiner.Scripting.Parameter;
using System.Threading;

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
			List<QActionTableRow> tableRows = new List<QActionTableRow>();


			EndpointsQActionRow newRow = new EndpointsQActionRow
			{
				Endpointsinstance_1001 = DateTime.Now.ToLongTimeString(),
				Endpoints_name_1002 = "test " + DateTime.Now.ToString(),
				Endpoints_ipv4_1003 = "ip address",
				Endpoints_nginx_api_1004 = "API string",
				Endpoints_http_response_1005 = "no response"
			};

			tableRows.Add(newRow);

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
