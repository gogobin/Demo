using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace ControlProgram
{
    public class TcpClient
    {
        System.Net.Sockets.Socket m_socket;
        private readonly ManualResetEvent TimeoutObject = new ManualResetEvent(false);
        private const int bufferSize = 1024;
        public string ReadMsg = "";
        public byte[] bufferRead = new byte[bufferSize];
        private void CallBackMethod(IAsyncResult asyncresult)
        {
            TimeoutObject.Set();
        }
        public bool Open(string IP,int endpoint)
        {
            IPAddress upad = IPAddress.Parse(IP);
            IPEndPoint dIP = new IPEndPoint(upad, endpoint);
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_socket.SendTimeout = 1000;
            m_socket.ReceiveTimeout = 1000;
            m_socket.BeginConnect(dIP, CallBackMethod, m_socket);
            if (TimeoutObject.WaitOne(1000))
            {
                if (m_socket.Connected)
                {
                    return true;
                }
            }
            return false;
        }


        static bool IsSocketConnected(Socket s)
        {
            return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
        }

        public bool send(byte[] byteList)
        {
            if (!IsSocketConnected(m_socket))
            {
                return false;
            }
            try
            {
                m_socket.Send(byteList, byteList.Length, SocketFlags.None);
                return true;
            }
            catch
            {}
            return false;
        }
        public byte[] Receive()
        {
            int Receive = m_socket.Available;
            byte[] bList = new byte[Receive];
            if (m_socket.Available > 0)
            {
                m_socket.Receive(bList, SocketFlags.None);
            }
            return bList;
        }

        public bool Close()
        {
            if (m_socket == null)
            {
                return false;
            }
            try
            {
                m_socket.Disconnect(false);
                m_socket.Dispose();
                return true;
            }
            catch
            { }
            return false;
        }

        private static byte[] SplitHexString(string sOriginalString, char chSeparator)
        {
            string[] sResults;
            sResults = sOriginalString.Split(chSeparator);
            byte[] lstResult = new byte[sResults.Length];
            for (int i = 0; i < sResults.Length; i++)
            {
                lstResult[i] = Convert.ToByte(sResults[i], 16);
            }
            return lstResult;
        }
        
        public bool write(string Cmd,int DelayTime,out List<string> List)
        { 
            byte[] buffer=SplitHexString(Cmd,'-');
            List<string> ReadList = new List<string>();
            if (m_socket.Send(buffer) != buffer.Length)
            {
                List = ReadList;
                return false;
            }
            Stopwatch stopW = new Stopwatch();
            stopW.Restart();
            do
            {
                if (m_socket.Available > 0)
                {
                    bufferRead = new byte[bufferSize];
                    m_socket.Receive(bufferRead);
                    string ReadStr = System.Text.Encoding.ASCII.GetString(bufferRead).Replace("\0", "");
                    if (!ReadList.Contains(ReadStr))
                    {
                        ReadList.Add(ReadStr);
                        if (ReadStr.Contains("page13"))
                        {
                            break;
                        }
                    }
                }
                Thread.Sleep(10);
            }
            while (stopW.Elapsed.TotalMilliseconds <DelayTime);
            List = ReadList;
            return true;
        }

        public bool writeCMDACIR(string Cmd, out byte[] Readbuffer)
        {
            byte[] buffer = SplitHexString(Cmd, '-');
            if (m_socket.Send(buffer) != buffer.Length)
            {
                bufferRead = new byte[bufferSize];
                Readbuffer = bufferRead;
                return false;
            }
            Stopwatch st = new Stopwatch();
            st.Restart();
            do
            {
                if (m_socket.Available > 0)
                {
                    bufferRead = new byte[bufferSize];
                    m_socket.Receive(bufferRead);
                    Readbuffer = bufferRead;
                    return true;
                }
                Thread.Sleep(25);
            }
            while (st.Elapsed.TotalMilliseconds < 800);
            bufferRead = new byte[bufferSize];
            Readbuffer = bufferRead;
            return false;
        }

        public bool writeCMD(string Cmd,out byte[] Readbuffer)
        {
            byte[] buffer = SplitHexString(Cmd, '-');
            if (m_socket.Send(buffer) != buffer.Length)
            {
                bufferRead = new byte[bufferSize];
                Readbuffer = bufferRead;
                return false;
            }
            Stopwatch st = new Stopwatch();
            st.Restart();
            do
            {
                if (m_socket.Available > 0)
                {
                    bufferRead = new byte[bufferSize];
                    m_socket.Receive(bufferRead);
                    Readbuffer = bufferRead;
                    return true;
                }
                Thread.Sleep(25);
            }
            while (st.Elapsed.TotalMilliseconds <300);
            bufferRead = new byte[bufferSize];
            Readbuffer = bufferRead;
            return false;
        }
        public bool writeCMDnoReturn(string Cmd)
        {
            byte[] buffer = SplitHexString(Cmd, '-');
            if (m_socket.Send(buffer) != buffer.Length)
            {
                return false;
            }
            return true;
        }


    }
}
