﻿namespace YanZhiwei.DotNet4._5.Utilities.Communication
{
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    /// <summary>
    /// Udp 主站
    /// </summary>
    public class UdpAppServer : UdpAppBase
    {
        #region Fields

        private IPEndPoint appUdpServer;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public UdpAppServer()
            : this(new IPEndPoint(IPAddress.Any, 61223))
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="endpoint">IPEndPoint</param>
        public UdpAppServer(IPEndPoint endpoint)
        {
            appUdpServer = endpoint;
            AppUpdClient = new UdpClient(appUdpServer);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">ip地址</param>
        /// <param name="port">端口</param>
        public UdpAppServer(string host, ushort port)
        {
            appUdpServer = new IPEndPoint(IPAddress.Parse(host), port);
            AppUpdClient = new UdpClient(appUdpServer);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 回复终端数据报文
        /// </summary>
        /// <param name="message">数据报文</param>
        /// <param name="endpoint">终端信息</param>
        public void Reply(string message, IPEndPoint endpoint)
        {
            byte[] _datagram = Encoding.UTF8.GetBytes(message);
            AppUpdClient.Send(_datagram, _datagram.Length, endpoint);
        }

        /// <summary>
        /// 回复终端数据报文
        /// </summary>
        /// <param name="datagram">数据报文</param>
        /// <param name="endpoint">终端信息</param>
        public void Reply(byte[] datagram, IPEndPoint endpoint)
        {
            AppUpdClient.Send(datagram, datagram.Length, endpoint);
        }

        #endregion Methods
    }
}