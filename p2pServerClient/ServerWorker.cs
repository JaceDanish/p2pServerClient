using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using p2pServerClient.Model;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace p2pServerClient
{
    public class ServerWorker
    {
        //private const string Path = @"..\..\sharedFiles";
        private const string path = @"C:\Users\janni\source\repos\p2p\shareFiles";
        private const string URL = "https://p2prest.azurewebsites.net/api/";
        private const int MyPort = 1025;
        public async void Start()
        {
            await UpdateDb();

            
        }
        
        private async Task<int> UpdateDb()
        {
            int status = 0;
            FileEndPoint fep = new FileEndPoint();
            fep.Ipaddress = IPAddress.Loopback.ToString();
            fep.Port = MyPort;

            IList<string> filenames = new List<string>();

            if (Directory.Exists(path))
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    filenames.Add(Path.GetFileName(file));
                }
            }
            
            using (HttpClient client = new HttpClient())
            {
                foreach (string file in filenames)
                {
                    Console.WriteLine(file);
                    string jstr = JsonSerializer.Serialize(fep);
                    StringContent content = new StringContent(jstr, Encoding.UTF8, "application/json");
                    HttpResponseMessage result = await client.PostAsync(URL + file, content);
                    Console.WriteLine(file + "fooop");
                    if (result.IsSuccessStatusCode)
                    {
                        jstr = await result.Content.ReadAsStringAsync();
                        status = JsonSerializer.Deserialize<int>(jstr);
                        //do nothing
                        Console.WriteLine(status);
                    }
                    else
                        Console.WriteLine(result.ToString());
                }
            }

            return status;


        }
    }
}
