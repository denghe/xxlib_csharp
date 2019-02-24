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
        for (int k = 0; k < 10; ++k)
        {
            {
                var pb = new PinnedBuffer(new byte[1024 * 1024 * 1024]);
                var bytes = (byte*)pb.ptr;
                var d = 0x1234567812345678u;
                //for (int j = 0; j < 8; j++)
                {
                    //*(ulong*)&bytes[16 * j + j] = d;
                    *(ulong*)&bytes[1] = d;
                }
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < 100000000; i++)
                {
                    //for (int j = 0; j < 8; j++)
                    {
                        //var idx = 16 * j + j;
                        //d = *(ulong*)&bytes[idx];
                        //fixed (byte* buf = &pb.data[1])
                        var buf = bytes + 1;
                        {
                            d = *(ulong*)buf;
                        }

                    }
                }
                Console.WriteLine(sw.ElapsedMilliseconds);
            }
            {
                var pb = new PinnedBuffer(new byte[1024 * 1024 * 1024]);
                var bytes = (byte*)pb.ptr;
                var d = 0x1234567812345678u;
                //for (int j = 0; j < 8; j++)
                {
                    //*(ulong*)&bytes[16 * j + j] = d;
                    *(ulong*)&bytes[1] = d;
                }
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < 100000000; i++)
                {
                    //for (int j = 0; j < 8; j++)
                    {
                        //var idx = 16 * j + j;
                        //fixed (byte* buf = &pb.data[1])
                        var buf = bytes + 1;
                        {
                            int num3 = (((buf[0] << 0x18) | (buf[1] << 0x10)) | (buf[2] << 8)) | buf[3];
                            int num4 = (((buf[4] << 0x18) | (buf[5] << 0x10)) | (buf[6] << 8)) | buf[7];
                            d = (ulong)(num4 | num3 << 0x20);
                        }
                    }
                }
                Console.WriteLine(sw.ElapsedMilliseconds);
            }
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
