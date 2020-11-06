using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using p2pClient.Model;

namespace p2pClient
{
    public class Worker
    {
        private List<FileEndPoint> search;
        private const string URL = "https://p2prest.azurewebsites.net/api/";
        private String path = "C:\\p2pFiles\\";

        public async Task Start()
        {
            Console.WriteLine($"Search \"file.extension\"");
            string filename = Console.ReadLine();
            Console.WriteLine("woop");
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(URL + filename);
                if (result.IsSuccessStatusCode)
                {
                    string jstring = await result.Content.ReadAsStringAsync();
                    search = JsonSerializer.Deserialize<List<FileEndPoint>>(jstring);
                }
            }

            if (search == null || search.Count == 0)
            {
                Console.WriteLine("File not found");
                return;
            }
            Console.WriteLine("woop");
            TcpClient tcpClient = new TcpClient(search[0].Ipaddress,search[0].Port);

            NetworkStream ns = tcpClient.GetStream();

            StreamWriter sw = new StreamWriter(ns);

            sw.WriteLine("GET " + filename);
            sw.Flush();

            BinaryReader br = new BinaryReader(ns);

            byte[] bytes = br.ReadBytes(200000);

            Console.WriteLine("woop3");
            File.WriteAllBytes(path + filename, bytes);
            Console.WriteLine(filename);

            Console.WriteLine("Done.");
            br.Close();
            sw.Close();
            



            //foreach (FileEndPoint fileEndPoint in search)
            //{
            //    Console.WriteLine(fileEndPoint.Ipaddress);
            //}


        }
    }
}
