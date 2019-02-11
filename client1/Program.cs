using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        PKG.AllTypes.Register();
        var loop = new xx.UvLoop();
        loop.InitRpcManager(1000, 10);
        loop.InitTimeoutManager(1000, 30, 5);
        var client = new xx.UvTcpClient(loop);
        client.SetAddress("127.0.0.1", 10001);
        client.OnConnect = status =>
        {
            if (status != 0) return;

            client.SendRequestEx(new PKG.Client_Login.Auth { username = "a", password = "a" }, recv =>
            {
                if (recv == null)
                {
                    Console.WriteLine("recv == null( timeout )");
                }
                else
                {
                    Console.WriteLine("recv: " + recv);
                }
            });
        };
        var timer = new xx.UvTimer(loop, 500, 500, () =>
        {
            if (client.state == xx.UvTcpStates.Disconnected)
            {
                client.Connect();
            }
        });
        loop.Run();
    }
}
