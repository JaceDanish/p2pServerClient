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
        private string path = Directory.GetCurrentDirectory();
        private string p2pPath;
        private const string URL = "https://p2prest.azurewebsites.net/api/";
        private const int MyPort = 1025;
        private FileEndPoint fep = new FileEndPoint();
        private IList<string> filenames = new List<string>();
        public async void Start()
        {
            p2pPath = path.Substring(0, path.Length - 55);
            p2pPath += "shareFiles";
            await UpdateDb();

            
        }
        
        private async Task<int> UpdateDb()
        {
            int status = 0;
            fep.Ipaddress = IPAddress.Loopback.ToString();
            fep.Port = MyPort;

            if (Directory.Exists(p2pPath))
            {
                foreach (string file in Directory.GetFiles(p2pPath))
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

        public async void RemoveList()
        {
            using (HttpClient client = new HttpClient())
            {
                foreach (string file in filenames)
                {
                    string jstr = JsonSerializer.Serialize(fep);
                    StringContent content = new StringContent(jstr, Encoding.UTF8, "application/json");
                    await client.PutAsync(URL + file, content);
                }
            }
            
        }
    }
}
