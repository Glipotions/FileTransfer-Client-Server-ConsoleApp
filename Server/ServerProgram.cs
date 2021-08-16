using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ServerProgram
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("That program can transfer small file. I've test up to 850kb file");
                IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 5656);
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                sock.Bind(ipEnd);
                sock.Listen(100);
                Socket clientSock = sock.Accept();

                byte[] clientData = new byte[1024 * 5000];
                Console.WriteLine("Kaydedileceği Konumu Giriniz.");
                string receivedPath = Console.ReadLine()+"\\";

                int receivedBytesLen = clientSock.Receive(clientData);
                int fileNameLen = BitConverter.ToInt32(clientData, 0);
                string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);
                Console.WriteLine("Client:{0} connected & File {1} started received.", clientSock.RemoteEndPoint, fileName);
                BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Append)); ;
                bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
                Console.WriteLine("File: {0} received & saved at path: {1}", fileName, receivedPath);

                bWrite.Close();
                clientSock.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File Receiving fail." + ex.Message);
            }


        }
    }
}
