using System;

public class EchoClient : xx.UvTcpClient
{
    int counter;
    public EchoClient(xx.UvLoop loop) : base(loop) { }
    public override void ReceiveImpl(IntPtr bufPtr, int len)
    {
        xx.UvInterop.xxuv_write_(ptr, bufPtr, 0, (uint)len);        // echo
        ++counter;
        if (counter > 100000) Disconnect();
    }
}

class Program
{
    static void Main(string[] args)
    {
        var uv = new xx.UvLoop();
        var conn = new EchoClient(uv);
        conn.SetAddress("0.0.0.0", 12345);
        conn.OnConnect = status =>
        {
            if (status != 0)
            {
                Console.WriteLine("connect failed.");
                return;
            }
            conn.SendBytes(new byte[1] { 1 });
        };
        conn.Connect();
        var sw = System.Diagnostics.Stopwatch.StartNew();
        uv.Run();
        Console.WriteLine(sw.ElapsedMilliseconds);
        Console.ReadLine();
    }
}
