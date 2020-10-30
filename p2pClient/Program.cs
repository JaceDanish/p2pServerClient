using System;

namespace p2pClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Worker worker = new Worker();
            worker.Start();
            Console.ReadKey();
        }
    }
}
