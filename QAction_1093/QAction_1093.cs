using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Skyline.DataMiner.Scripting;

/// <summary>
/// DataMiner QAction Class: Build PATCH pids ALL PEER[1] DOWN PEER[0] UP.
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
			//List<string> allPeersDown = new List<string>();
			//List<string> allPeersUp = new List<string>();

			List<string> peerCommands = new List<string>();

			//read the current of servers
			CachesQActionTable table = protocol.caches;

			for (int i = 0; i < table.RowCount; i++)
			{
				object[] rowStuff = table.GetRow(i);
				CachesQActionRow nextRow = (CachesQActionRow)rowStuff;

				//string peerServer = (string)nextRow.Caches_peer_server_1004;
				string peerId = (string)nextRow.Caches_peer_id_1003;

				if (peerId.Equals("0"))
				{
					string command = (string)nextRow.Caches_zone_name_1002 + "|" + (string)nextRow.Caches_peer_id_1003 + "|UP";
					peerCommands.Add(command);

					//allPeersDown.Add((string)nextRow.Caches_zone_name_1002);

				}
				else
				{
					string command = (string)nextRow.Caches_zone_name_1002 + "|" + (string)nextRow.Caches_peer_id_1003 + "|DOWN";
					peerCommands.Add(command);

					//allPeersUp.Add((string)nextRow.Caches_zone_name_1002);
				}

			}

			string firstCommand = peerCommands[0];
			string[] commArr = firstCommand.Split('|');

			peerCommands.RemoveAt(0);

			protocol.SetParameter(Parameter.patch_commandlist_12, string.Join(";", peerCommands));

			if (commArr[2].Equals("DOWN"))
				protocol.SetParameter(Parameter.patch_data_8, "{\"down\": true }");
			else
				protocol.SetParameter(Parameter.patch_data_8, "{\"down\": false }");

			//Update param7 to start the session off
			protocol.SetParameter(Parameter.patch_uri_7, "api/6/http/upstreams/" + commArr[0] + "/servers/" + commArr[1]);

			protocol.SetParameter(Parameter.patch_failover_status_11, "Failover Started");

			protocol.SetParameter(Parameter.getinitialendpointinfo_debug_1082, "QAction 1092 ran| allPeerCommands = " + string.Join(";", peerCommands) + " at " + DateTime.Now.ToString());

		}
		catch (Exception ex)
		{
			protocol.Log("QA" + protocol.QActionID + "|" + protocol.GetTriggerParameter() + "|Run|Exception thrown:" + Environment.NewLine + ex, LogType.Error, LogLevel.NoLogging);
		}
	}
}