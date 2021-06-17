using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Skyline.DataMiner.Net.Messages;
using Skyline.DataMiner.Scripting;
using Parameter = Skyline.DataMiner.Scripting.Parameter;




/// <summary>
/// 
/// DataMiner QAction Class: Poll endpoints to get intial table
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
			//Send API request to the endpoint using a Trigger object

			/*
			List<QActionTableRow> tableRows = new List<QActionTableRow>();


			EndpointsQActionRow newRow = new EndpointsQActionRow
			{
				Endpointsinstance_1001 = DateTime.Now.ToString(),
				Endpoints_name_1002 = "hello",
				Endpoints_ipv4_1003 = "ip address",
				Endpoints_nginx_api_1004 = "API string",
				Endpoints_http_response_1005 = "no response"
			};

			tableRows.Add(newRow);


			//fill table
			protocol.endpoints.FillArray(tableRows);



			*/
			// sanity check debug


			protocol.SetParameter(Parameter.getinitialendpointinfo_output_1081, "");
			//protocol.SetParameter(Skyline.DataMiner.Scripting.Parameter.getinitialendpointinfo_debug_1082, "QAction 1080 ran at " + DateTime.Now.ToString());

		}
		catch (Exception ex)
		{
			protocol.Log("QA" + protocol.QActionID + "|" + protocol.GetTriggerParameter() + "|Run|Exception thrown:" + Environment.NewLine + ex, LogType.Error, LogLevel.NoLogging);
		}
	}
}