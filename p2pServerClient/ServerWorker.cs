using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using p2pServerClient.Model;

namespace p2pServerClient
{
    public class ServerWorker
    {
        private const string Path = "path";
        private const int MyPort = 1025;
        public void Start()
        {
            
        }

        private void UpdateDb()
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



        }
    }
}
