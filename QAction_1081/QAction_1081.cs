using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;


using Skyline.DataMiner.Scripting;
using Newtonsoft.Json;
using Parameter = Skyline.DataMiner.Scripting.Parameter;
using Newtonsoft.Json.Linq;
using System.Linq;

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

			string serverFilterString = (string)protocol.GetParameter(Parameter.server_filter_6);
			List<string> serverFilterList = serverFilterString.Split(':').ToList();

			//Return if the filter is blank
			if (!serverFilterList.Any())
				return;

			//json processing to EndpointsQActionRow objects
			string jsonResponse = (string)protocol.GetParameter(Parameter.getinitialendpointinfo_output_1081);

			//var h = obj.Value<JObject>("GosEleDeltaLive001").Value<JArray>("peers")[0].SelectToken("server").ToString(); //test
			//var k = obj.SelectTokens("GosEleDeltaLive001").ToList<JToken>().Count.ToString(); //test

			string output = "";
			var objJObj = JObject.Parse(jsonResponse);

			foreach (JProperty prop in objJObj.Properties())
			{
				JObject item = (JObject)prop.Value;
				var zone = (string)item["zone"];
				var keepalive = (int)item["keepalive"];
				output += "  | Zone:" + zone + "|" + "Keepalive:" + keepalive;

				var inners = item.Value<JArray>("peers");

				//output += "|Inners:" + inners.Count.ToString();
				//output += inners[0].SelectToken("server").ToString();
				
				foreach(JObject jArr in inners)
				{
					string id = jArr.SelectToken("id").ToString();
					string serverName = jArr.SelectToken("server").ToString();
					string name = jArr.SelectToken("name").ToString();
					string state = jArr.SelectToken("state").ToString();

					output += "|Server:" + serverName + "|ID:" + id + "|State:" + state;

					//Create table objects if the filter list contains the same name
					if (serverFilterList.Contains(zone))
					{
						CachesQActionRow newRow = new CachesQActionRow
						{
							Caches_endpointsinstance_1001 = zone + id + serverName,
							Caches_zone_name_1002 = zone,
							Caches_peer_id_1003 = id,
							Caches_peer_server_1004 = serverName,
							Caches_peer_name_1005 = name,
							Caches_peer_state_1006 = state
						};

						tableRows.Add(newRow);
					}

				}

			}

			//check the status and update status pid for the Visio
			string status = "UP";
			foreach(CachesQActionRow obj in tableRows)
			{
				if (((string)obj.Caches_peer_id_1003).Equals("0") &&  ((string)obj.Caches_peer_state_1006).Equals("down"))
					status = "DOWN";
			}

			protocol.SetParameter(Parameter.patch_failover_status_11, status);



			//fill table
			protocol.caches.FillArray(tableRows);

			protocol.SetParameter(Parameter.getinitialendpointinfo_debug_1082, "QAction 1081 ran at " + DateTime.Now.ToString());



        }
        catch (Exception ex)
        {
            protocol.Log("QA" + protocol.QActionID + "|" + protocol.GetTriggerParameter() + "|Run|Exception thrown:" + Environment.NewLine + ex, LogType.Error, LogLevel.NoLogging);
        }
    }
}
