using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTS.CreateData
{
    public class ServerStart
    {
        public Dictionary<string, DTSEquip> DTS;
        private static ServerStart _instance;
        public static ServerStart Create()
        {
            return _instance ?? (_instance = new ServerStart());
        }

        public void Start()
        {
            DTS = Main.Equips;
            try
            {
                foreach (KeyValuePair<string, DTSEquip> kvp in DTS)
                {
                    kvp.Value.tcpServer.equipnum = kvp.Key;
                    kvp.Value.Start();
                    kvp.Value.tcpServer.Start();                   
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public void ServerStop()
        {
            foreach (KeyValuePair<string, DTSEquip> kvp in DTS)
            {
                kvp.Value.tcpServer.Stop();
                kvp.Value.Stop();
            }
        }

        public RegistData PosRegistdata(string equipnum,string channelnum)
        {
            RegistData registdata = null;
            if (DTS != null)
            {
                if (DTS.Keys.Contains(equipnum))
                {
                    {
                        int count = DTS[equipnum].ChannelRegistData[channelnum].Count;
                        if (count > 0)
                        {
                            registdata = DTS[equipnum].ChannelRegistData[channelnum][count - 1];
                        }

                        if (count > 50)
                        {
                            DTS[equipnum].ChannelRegistData[channelnum].RemoveRange(0, DTS[equipnum].ChannelRegistData[channelnum].Count - 1);
                        }
                    }
                }
            }
            return registdata;
        }
    }
}
