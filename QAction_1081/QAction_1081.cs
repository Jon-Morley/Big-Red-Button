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

			/*
			CachesQActionRow newRow = new CachesQActionRow
			{
				Endpointsinstance_1001 = DateTime.Now.ToLongTimeString(),
				Cache_name_1002 = "test " + DateTime.Now.ToString(),
				Endpoints_ipv4_1003 = "ip address",
				Endpoints_nginx_api_1004 = "API string",
				Endpoints_http_response_1005 = "no response"
			};

			tableRows.Add(newRow);
			*/

			//json processing to EndpointsQActionRow objects
			string jsonResponse = (string)protocol.GetParameter(Parameter.getinitialendpointinfo_output_1081);

			//protocol.SetParameter(Parameter.getinitialendpointinfo_debug_1082, "QAction 1081 ran at " + DateTime.Now.ToString());


			JObject obj = JObject.Parse(jsonResponse);

			var h = obj.Value<JObject>("GosEleDeltaLive001").Value<JArray>("peers")[0].SelectToken("server").ToString();
			var k = obj.SelectTokens("GosEleDeltaLive001").ToList<JToken>().Count.ToString();
			//k = obj.Root.Children().ToList<JToken>().Count.ToString();

			List<JToken> toks = obj.Root.Children().ToList<JToken>();


			string output = "";
			foreach(JToken token in toks)
			{
				JObject prop = (JObject)token;
				output += prop.Root.Value<string>("zone");

				//output += token.ToString();

				//output += token.Value<string>("zone");
				//JObject newObj = (JObject)token;
				//output += newObj.SelectToken("zone").Value<string>();

				/*
				JToken outer = JToken.Parse(token.ToString());
				JObject inner = outer.Root.Value<JObject>();

				List<string> keys = inner.Properties().Select(p => p.Name).ToList();
				string bla = (string)inner.Properties().First().Value;
				output += bla;
				*/
			}

			try
			{
				string objw = obj.SelectToken("GosEleDeltaLive001").ToString();
				protocol.SetParameter(Parameter.getinitialendpointinfo_debug_1082, "QAction 1081 ran and  " + output + " | " + DateTime.Now.ToString());
			}
			catch (Exception e)
			{
				protocol.SetParameter(Parameter.getinitialendpointinfo_debug_1082, "QAction 1081 ran and found error " + e + " | " + DateTime.Now.ToString());
			}


			string debug = (string)obj.SelectToken("zone");
			//protocol.SetParameter(Parameter.getinitialendpointinfo_debug_1082, debug);

			for (int i=0; i< obj.Count; i++)
			{		
				string zoneName = (string)obj[i];

				JArray jArrayPeersinZone = (JArray)obj[i]["peers"];

				for(int j=0; j< jArrayPeersinZone.Count; j++)
				{


					CachesQActionRow newRow = new CachesQActionRow
					{
						Endpointsinstance_1001 = DateTime.Now.ToLongTimeString() + "_" + zoneName + "_peer_" + j.ToString(),
						Cache_zone_name_1002 = zoneName,
						Cache_peer_id_1003 = jArrayPeersinZone[j]["id"],
						Cache_peer_server_1004 = jArrayPeersinZone[j]["server"],
						Cache_peer_name_1005 = jArrayPeersinZone[j]["server"],
						Cache_peer_state_1006 = jArrayPeersinZone[j]["state"]
					};

					tableRows.Add(newRow);

				}

			}

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
