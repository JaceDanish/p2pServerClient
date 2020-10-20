using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using p2pServerClient.Model;
using System.Text.Json;

namespace p2pServerClient
{
    public class ServerWorker
    {
        private const string Path = @"..\..\sharedFiles";
        private const string URL = "https://p2prest.azurewebsites.net/api/";
        private const int MyPort = 1025;
        public void Start()
        {
            
        }

        private async void UpdateDb()
        {
            FileEndPoint fep = new FileEndPoint();
            fep.Ipaddress = IPAddress.Loopback.ToString();
            fep.Port = MyPort;

            IList<string> filenames = new List<string>();

            if (Directory.Exists(Path))
            {
                foreach (string file in Directory.GetFiles(Path))
                {
                    filenames.Add(file);
                }
            }

            using (HttpClient client = new HttpClient())
            {
                foreach (string file in filenames)
                {
                    string jstr = JsonSerializer.Serialize(file);
                    StringContent content = new StringContent(jstr, Encoding.UTF8, "application/json");
                    await client.PostAsync(URL, content);

                    //await resultmessage. get int-status code. do something

                }
            }
            



        }
    }
}
