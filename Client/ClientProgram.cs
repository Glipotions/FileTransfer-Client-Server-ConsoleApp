using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class ClientProgram
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("That program can transfer small file. I've test up to 850kb file");
                IPAddress[] ipAddress = Dns.GetHostAddresses("localhost");
                IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5656);
                Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                Console.WriteLine("Dosya Adını Yazmadan Dosya Yolunu Giriniz..");
                string filePath = @Console.ReadLine() + "\\";

                Console.WriteLine("Dosya Adını Giriniz..");
                string fileName = Console.ReadLine();

                byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);

                byte[] fileData = File.ReadAllBytes(filePath + fileName);
                byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
                byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

                fileNameLen.CopyTo(clientData, 0);
                fileNameByte.CopyTo(clientData, 4);
                fileData.CopyTo(clientData, 4 + fileNameByte.Length);

                clientSock.Connect(ipEnd);
                clientSock.Send(clientData);
                Console.WriteLine("File:{0} has been sent.", fileName);
                clientSock.Close();
                Console.ReadLine();
            }

            catch (Exception ex)
            {
                Console.WriteLine("File Sending fail." + ex.Message);
                Console.ReadKey();
            }
        }
    }
}
