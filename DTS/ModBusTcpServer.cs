using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using IoTServer.Servers;
using DTS.Helpers;
using DTS.CreateData;
using System.Diagnostics;
using DTS.Protocol;
using System.Threading;

namespace DTS
{
    /// <summary>
    /// ModBusTcp 服务端模拟
    /// </summary>
    public class ModBusTcpServer : ServerSocketBase
    {
        private Socket _socketServer;
        public string _ip;
        public int _port;
        private List<Socket> _sockets = new List<Socket>();
        public bool connectsuccessflag;
        public string equipnum;
        public bool startFlag;

        public ModBusTcpServer(int port, string ip = null)
        {
            this._ip = ip;
            this._port = port;
            connectsuccessflag = false;
        }

        public ModBusTcpServer()
        {
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (IPAddress ip in ipadrlist)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    //  this._ip = ip.ToString();         
                    this._ip = "192.168.1.81";
                    break;
                }
            }
            connectsuccessflag = false;
            
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            _socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipaddress = string.IsNullOrWhiteSpace(_ip) ? IPAddress.Any : IPAddress.Parse(_ip);
            IPEndPoint ipEndPoint = new IPEndPoint(ipaddress, _port);
            _socketServer.Bind(ipEndPoint);
            _socketServer.Listen(10);
            connectsuccessflag = true;
            startFlag = true;

            Task.Factory.StartNew(() => { Accept(_socketServer); });
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            startFlag = false;
            connectsuccessflag = false;
            if (_socketServer?.Connected ?? false) _socketServer.Shutdown(SocketShutdown.Both);
            _socketServer?.Close();                       
        }

        /// <summary>
        /// 客户端连接到服务端
        /// </summary>
        /// <param name="socket"></param>
        void Accept(Socket socket)
        {
            while (startFlag)
            {
               // if (connectsuccessflag)
                {
                    try
                    {
                        Socket newSocket = null;
                        try
                        {
                            newSocket = socket.Accept();
                            _sockets.Add(newSocket);
                        }
                        catch (SocketException ex)
                        {
                            foreach (var item in _sockets)
                            {
                                if (item.Connected) item.Shutdown(SocketShutdown.Both);
                                item.Close();
                            }
                        }
                        Task.Factory.StartNew(() => { Receive(newSocket); });
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode != SocketError.Interrupted)
                            throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 接收客户端发送的消息
        /// </summary>
        /// <param name="newSocket"></param>
        void Receive(Socket newSocket)
        {
            int oldadd = 0;
            int newadd = 0;
            RegistData rd = null;
            ChannelTempInfo ci = new ChannelTempInfo();
            int SerialNum = -1;      //流水号
            List<byte> response = new List<byte>();
            while (newSocket!=null && newSocket.Connected)
            {                
                try
                {                    
                    byte[] requetData = new byte[12];     //Map报文头+字节数               
                    requetData = SocketRead(newSocket, requetData.Length);                    
                    ushort slavenum = requetData[6];          //站号
                    var address = requetData[8] * 256 + requetData[9];//寄存器地址
                    var registercout = requetData[10] * 256 + requetData[11];//寄存器个数       
                    byte[] responseDataHead = new byte[9];
                    Buffer.BlockCopy(requetData, 0, responseDataHead, 0, 8);
                    responseDataHead[5] = (byte)(3 + registercout * 2);
                    responseDataHead[8] = (byte)(registercout * 2);
                    switch (requetData[7])   //功能码
                    {                       
                        //读保持寄存器
                        case 4:
                            {
                                
                                if (ServerStart.Create().DTS.Keys.Contains(this.equipnum))
                                {
                                    int equipslavenum = ServerStart.Create().DTS[this.equipnum].SlaveNum;
                                    int channelnum = Math.Abs(slavenum - equipslavenum);
                                    if (BitConverter.ToUInt16(requetData, 0) != SerialNum)
                                    {
                                        response.Clear();
                                        object data = ServerStart.Create().DTS[this.equipnum].dataMange.PopData(this.equipnum, slavenum);                                        
                                        if (channelnum == 0)
                                        {
                                            //设备基本信息
                                        }
                                        else
                                        {
                                            if (data != null)
                                            {
                                                byte[] responseData1 = EquipChannelTempInfo.ProtocolConvert((ChannelTempInfo)data);
                                                response.AddRange(responseData1);

                                                /* byte[] holdregisterdata = new byte[9 + registercout * 2];
                                                Buffer.BlockCopy(responseDataHead, 0, holdregisterdata, 0, 8);
                                                Buffer.BlockCopy(response.ToArray(), address * 2, holdregisterdata, 9, registercout * 2);
                                                newSocket.Send(holdregisterdata);
                                                */
                                              
                                                if (this.equipnum == Main.oldEquipNum)
                                                {
                                                    ci = (ChannelTempInfo)data;
                                                    Main.channelCurvers[ci.ChannelNum.ToString()].DrawSendData(ci);
                                                    Main.ObjRefreshZed(ci.ChannelNum.ToString());
                                                  //  Thread.Sleep(1000);
                                                }
                                            }
                                        }
                                        SerialNum = BitConverter.ToUInt16(requetData, 0);
                                    }
                                    
                                    if(response.Count > 0)
                                    {
                                        byte[] holdregisterdata = new byte[9 + registercout * 2];
                                        Buffer.BlockCopy(responseDataHead, 0, holdregisterdata, 0, 9);
                                        Buffer.BlockCopy(response.ToArray(), address * 2, holdregisterdata, 9, registercout * 2);
                                        newSocket.Send(holdregisterdata);

                                        System.Text.StringBuilder builder = new System.Text.StringBuilder();
                                        for (int i = 0; i < requetData.Length; i++)
                                        {
                                            builder.Append(string.Format("{0:X2} ", requetData[i]));
                                        }
                                        Debug.WriteLine("接收：" + builder.ToString());
                                        builder.Clear();

                                        for (int i = 0; i < holdregisterdata.Length; i++)
                                        {
                                            builder.Append(string.Format("{0:X2} ", holdregisterdata[i]));
                                        }
                                        Debug.WriteLine("发送："+builder.ToString());
                                    }
                                }
                            }
                            break;
                        //读输入寄存器
                        case 3:
                            {        
                                /*
                                //当前位置到最后的长度
                                responseData1[4] = (byte)((3 + registercout * 2) / 256);
                                responseData1[5] = (byte)((3 + registercout * 2) % 256);
                                byte[] responseData2 = new byte[1 + registercout * 2];
                                responseData2[0] = (byte)(registercout * 2);
                                for (int i = 1; i < responseData2.Length; i++)
                                    responseData2[i] = 0xff;                                    
                                if (address >= 0 && address < 65536)
                                {
                                    newadd = address;
                                    rd = ServerStart.Create().PosRegistdata(equipnum, slavenum.ToString());
                                    if (rd != null)
                                    {
                                        byte[] byteArray = rd.inputRegistData;
                                            
                                        if (address * 2 < byteArray.Length)
                                        {
                                            if ((address + registercout) * 2 <= byteArray.Length)
                                            {
                                                Buffer.BlockCopy(byteArray, address * 2, responseData2, 1, registercout * 2);
                                            }
                                            else
                                            {
                                                int len = ((address + registercout) * 2) - byteArray.Length;
                                                byte[] b = new byte[len];
                                                for (int i = 0; i < len; i++)
                                                    b[i] = 0xff;

                                                byte[] temp = byteArray.Concat(b).ToArray();
                                                Buffer.BlockCopy(temp, address * 2, responseData2, 1, registercout * 2);
                                            }                                                
                                            var responseData = responseData1.Concat(responseData2).ToArray();
                                            newSocket.Send(responseData);

                                            oldadd = address;
                                                
                                        }
                                    }
                                }              
                                */
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    connectsuccessflag = false;
                    if (newSocket?.Connected ?? false) newSocket?.Shutdown(SocketShutdown.Both);
                     newSocket?.Close();
                }
            }
        }
    }
}
