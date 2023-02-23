using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;


public class SocketUDPServer
{
    private string ip = "169.254.26.10";
    private int port = 7788;
    private Socket socket;
    private static SocketUDPServer socketServer;
    public List<string> listMessage = new List<string>();

    public static SocketUDPServer getInstance()
    {
        if (socketServer == null)
        {
            socketServer = new SocketUDPServer();
            socketServer.Init();
        }
        return socketServer;
    }

    private void Init()
    {
        IPAddress ipAddress = IPAddress.Parse(ip);
        IPEndPoint IPE = new IPEndPoint(ipAddress, port);
        //Udp¥Ó≈‰SocketType.Dgram   Tcp¥Ó≈‰SocketType.Stream
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(IPE);
        Thread threadReceive = new Thread(new ThreadStart(ReceiveMessage));
        threadReceive.Start();
    }
    private void ReceiveMessage()
    {
        while (true)
        {
            byte[] buff = new byte[1024];
            int iBytes = socket.Receive(buff, SocketFlags.None);
            if (iBytes <= 0)
                break;
            string strGetMessage = Encoding.ASCII.GetString(buff, 0, iBytes);
            listMessage.Add(strGetMessage);
        }
    }
    public void Close()
    {
        if (socket != null)
            socket.Close();
    }


}