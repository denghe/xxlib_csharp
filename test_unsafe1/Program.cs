using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class PinnedBuffer : IDisposable
{
    public byte[] data;
    public GCHandle handle;
    public IntPtr ptr;

    public PinnedBuffer(byte[] bytes)
    {
        data = bytes;
        handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        ptr = handle.AddrOfPinnedObject();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            handle.Free();
            data = null;
        }
    }
}

class Program
{
    unsafe static void Main(string[] args)
    {
        //{
        //    var bytes = new byte[1024 * 1024 * 1024];
        //    var d = 0x1234567812345678u;
        //    var sw = Stopwatch.StartNew();
        //    for (int i = 0; i < 1024 * 1024 * 1024 - 8; i++)
        //    {
        //        bytes[i + 0] = (byte)(d >> 0);
        //        bytes[i + 1] = (byte)(d >> 8);
        //        bytes[i + 2] = (byte)(d >> 16);
        //        bytes[i + 3] = (byte)(d >> 24);
        //        bytes[i + 4] = (byte)(d >> 32);
        //        bytes[i + 5] = (byte)(d >> 40);
        //        bytes[i + 6] = (byte)(d >> 48);
        //        bytes[i + 7] = (byte)(d >> 56);
        //    }
        //    Console.WriteLine(sw.ElapsedMilliseconds);
        //}
        //{
        //    var pb = new PinnedBuffer(new byte[1024 * 1024 * 1024]);
        //    var bytes = (byte*)pb.ptr;
        //    var d = 0x1234567812345678u;
        //    var sw = Stopwatch.StartNew();
        //    for (int i = 0; i < 1024 * 1024 * 1024 - 8; i++)
        //    {
        //        *(ulong*)&bytes[i] = d;
        //    }
        //    Console.WriteLine(sw.ElapsedMilliseconds);
        //}
        //{
        //    var bytes = new byte[1024 * 1024 * 1024];
        //    var d = 0x1234567812345678u;
        //    var sw = Stopwatch.StartNew();
        //    for (int i = 0; i < 1024 * 1024 * 1024 - 8; i++)
        //    {
        //        fixed (byte* buf = &bytes[i])
        //        {
        //            *(ulong*)buf = d;
        //        }
        //    }
        //    Console.WriteLine(sw.ElapsedMilliseconds);
        //}
        {
            var pb = new PinnedBuffer(new byte[1024 * 1024 * 1024]);
            var bytes = (byte*)pb.ptr;
            var d = 0x1234567812345678u;
            for (int j = 0; j < 8; j++)
            {
                *(ulong*)&bytes[16 * j + j] = d;
            }
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100000000; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    d = *(ulong*)&bytes[16 * j + j];
                }
            }
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
        {
            var pb = new PinnedBuffer(new byte[1024 * 1024 * 1024]);
            var bytes = (byte*)pb.ptr;
            var d = 0x1234567812345678u;
            for (int j = 0; j < 8; j++)
            {
                *(ulong*)&bytes[16 * j + j] = d;
            }
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100000000; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var idx = 16 * j + j;
                    d = (ulong)(bytes[idx + 0])
                        + ((ulong)(bytes[idx + 1]) << 8)
                        + ((ulong)(bytes[idx + 2]) << 16)
                        + ((ulong)(bytes[idx + 3]) << 24)
                        + ((ulong)(bytes[idx + 4]) << 32)
                        + ((ulong)(bytes[idx + 5]) << 40)
                        + ((ulong)(bytes[idx + 6]) << 48)
                        + ((ulong)(bytes[idx + 7]) << 56);
                }                                 
            }
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

    }
}

/*
    {
        var bytes = new byte[1024 * 1024 * 1024];
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 1024 * 1024 * 1024; i++)
        {
            bytes[i] = 123;
        }
        Console.WriteLine(sw.ElapsedMilliseconds);
    }
    {
        var pb = new PinnedBuffer(new byte[1024 * 1024 * 1024]);
        var bytes = (byte*)pb.ptr;
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 1024 * 1024 * 1024; i++)
        {
            bytes[i] = 123;
        }
        Console.WriteLine(sw.ElapsedMilliseconds);
    }
 
*/
