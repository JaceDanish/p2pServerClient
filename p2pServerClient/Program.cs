using System;

namespace p2pServerClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerWorker run = new ServerWorker();
            run.Start();
        }
    }
}
