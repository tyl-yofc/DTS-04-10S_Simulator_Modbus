using System.Net.Sockets;

namespace DTS.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class SocketHelper
    {
        /// <summary>
        /// 安全关闭
        /// </summary>
        /// <param name="socket"></param>
        public static void SafeClose(this Socket socket)
        {
            try
            {
                if (socket?.Connected ?? false) socket?.Shutdown(SocketShutdown.Both);//正常关闭连接   
            }
            catch { }

            
        }
    }
}
