using System;

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
            if (status != 0)
            {
                Console.WriteLine("connect to server_login failed. status = " + status);
                return;
            }
            Console.WriteLine("connected.");

            var a = new PKG.Client_Login.Auth { username = "abc", password = "a" };
            if (System.Environment.TickCount % 5 == 0)
            {
                a.password = "123";
            }
            client.SendRequestEx(a, recv =>
            {
                if (recv == null)
                {
                    Console.WriteLine("recv == null( timeout )");
                    return;
                }
                Console.WriteLine("PKG.Client_Login.Auth recv: " + recv);

                switch (recv)
                {
                    case PKG.Generic.Error o:
                        client.Disconnect();
                        break;
                    case PKG.Generic.Success o:
                        break;
                    default:
                        break;
                }

            });
        };
        var timer = new xx.UvTimer(loop, 1000, 2000, () =>
        {
            if (client.state == xx.UvTcpStates.Disconnected)
            {
                Console.WriteLine("connect to server_login...");
                client.Connect();
            }
        });
        loop.Run();
    }
}
