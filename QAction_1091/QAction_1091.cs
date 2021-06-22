using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
			//read in the list of peers to patch
			string allPeersDown = (string)protocol.GetParameter(Parameter.patch_down_list_9);
			string allPeersUp = (string)protocol.GetParameter(Parameter.patch_up_list_10);

			string peerCommandStr = (string)protocol.GetParameter(Parameter.patch_commandlist_12);

			if(string.IsNullOrEmpty(peerCommandStr))
			{
				protocol.SetParameter(Parameter.getinitialendpointinfo_debug_1082, "QAction 1091 finished looping at " + DateTime.Now.ToString());
				protocol.SetParameter(Parameter.patch_failover_status_11, "Failover Complete");
				return;
			}


			List<string> peerCommands = peerCommandStr.Split(';').ToList();

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

			//protocol.SetParameter(Parameter.patch_failover_status_11, "Failover Started");



			/*
			List<string> allPeersArr = new List<string>();


			List<string> peerCommandsList = peerCommand.Split(';').ToList();


			if (string.IsNullOrEmpty(allPeersDown) && string.IsNullOrEmpty(allPeersUp))
			{
				protocol.SetParameter(Parameter.getinitialendpointinfo_debug_1082, "QAction 1091 finished looping at " + DateTime.Now.ToString());
				protocol.SetParameter(Parameter.patch_failover_status_11, "failover complete");
				return;
			}

			if (!string.IsNullOrEmpty(allPeersDown))
			{
				protocol.SetParameter(Parameter.patch_failover_status_11, "failing over");
			allPeersArr = allPeersDown.Split(';').ToList();

				//write params to trigger the Session off
				//Change param 8
				protocol.SetParameter(Parameter.patch_data_8, "{\"down\": true }");

				//build the pids describing the VERB contents and Data
				//Change param 7 /api/6/http/upstreams/test/servers/0
				protocol.SetParameter(Parameter.patch_uri_7, "api/6/http/upstreams/" + allPeersArr[0] + "/servers/0");



			//write the new patch list
			allPeersArr.RemoveAt(0);

			protocol.SetParameter(Parameter.patch_down_list_9, string.Join(";", allPeersArr));

			}

			else
			{
				allPeersArr = allPeersUp.Split(';').ToList();
				protocol.SetParameter(Parameter.getinitialendpointinfo_debug_1082, "QAction 1091 allpeers is " + allPeersUp);

				//write params to trigger the Session off
				//Change param 8
				protocol.SetParameter(Parameter.patch_data_8, "{\"down\": false }");

				protocol.SetParameter(Parameter.getinitialendpointinfo_debug_1082, "QAction 1091 Setting peer " + allPeersArr[0]);
				//build the pids describing the VERB contents and Data
				//Change param 7 /api/6/http/upstreams/test/servers/0
				protocol.SetParameter(Parameter.patch_uri_7, "api/6/http/upstreams/" + allPeersArr[0] + "/servers/1");



				//write the new patch list
				allPeersArr.RemoveAt(0);

				protocol.SetParameter(Parameter.patch_up_list_10, string.Join(";", allPeersArr));
				protocol.SetParameter(Parameter.getinitialendpointinfo_debug_1082, "QAction 1091 Setting peers list " + string.Join(";", allPeersArr));

			}
			*/




			protocol.SetParameter(Parameter.getinitialendpointinfo_debug_1082, "QAction 1091 ran at " + DateTime.Now.ToString());

		}
		catch (Exception ex)
        {
            protocol.Log("QA" + protocol.QActionID + "|" + protocol.GetTriggerParameter() + "|Run|Exception thrown:" + Environment.NewLine + ex, LogType.Error, LogLevel.NoLogging);
        }
    }
}
