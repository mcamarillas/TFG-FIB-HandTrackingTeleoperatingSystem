using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class BackendManager : MonoBehaviour
{
    Thread m_receiveThread;
    UdpClient m_client;
    public int m_port;
    public bool m_startRecieving = true;
    public bool m_printToConsole = false;
    public string m_data;


    public void Start()
    {
        m_receiveThread = new Thread(new ThreadStart(ReceiveData));
        m_receiveThread.IsBackground = true;
        m_receiveThread.Start();
    }


    // receive thread
    protected void ReceiveData()
    {
        m_client = new UdpClient(m_port);
        while (m_startRecieving)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = m_client.Receive(ref anyIP);
                m_data = Encoding.UTF8.GetString(dataByte);

                if (m_printToConsole) { print(m_data); }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
}