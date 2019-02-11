using System;

public class EchoPeer : xx.UvTcpPeer
{
    public EchoPeer(xx.UvTcpListener listener) : base(listener) { }
    public override void ReceiveImpl(IntPtr bufPtr, int len)
    {
        xx.UvInterop.xxuv_write_(ptr, bufPtr, 0, (uint)len);        // echo
    }
}

class Program
{
    static void Main(string[] args)
    {
        var uv = new xx.UvLoop();
        var listener = new xx.UvTcpListener(uv);
        listener.Bind("0.0.0.0", 12345);
        listener.OnCreatePeer = () => { return new EchoPeer(listener); };
        listener.Listen();
        uv.Run();
    }
}
