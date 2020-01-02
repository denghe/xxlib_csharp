using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server_game1
{
    public class Foo
    {
        public List<List<int>> intss;
        public void Write(xx.BBuffer bb)
        {

        }
    }

    public static class ListUtils
    {
        public partial class BBWriter_IObject : BBWriter<xx.IObject>
        {
            public override void Write(xx.BBuffer bb, List<xx.IObject> vs)
            {
                ++counter;
            }
        }

        public partial class BBWriter_Int32 : BBWriter<int>
        {
            public override void Write(xx.BBuffer bb, List<int> vs)
            {
                ++counter;
            }
        }

        public abstract partial class BBWriter<T>
        {
            public abstract void Write(xx.BBuffer bb, List<T> vs);

            public static BBWriter<T> instance;

            static BBWriter()
            {
                var t = typeof(T);
                if (typeof(xx.IObject).IsAssignableFrom(t))
                {
                    instance = new BBWriter_IObject() as BBWriter<T>;
                }
                else
                {
                    switch (Type.GetTypeCode(t))
                    {
                        //    //case TypeCode.Empty:
                        //    //case TypeCode.Object:
                        //    //case TypeCode.DBNull:
                        //    //case TypeCode.Char:
                        //    //case TypeCode.Decimal:

                        //    case TypeCode.Boolean:
                        //        instance = new BBWriter_Boolean() as BBWriter<T>;
                        //        break;
                        //    case TypeCode.SByte:
                        //        instance = new BBWriter_SByte() as BBWriter<T>;
                        //        break;
                        //    case TypeCode.Byte:
                        //        instance = new BBWriter_Byte() as BBWriter<T>;
                        //        break;
                        //    case TypeCode.Int16:
                        //        instance = new BBWriter_Int16() as BBWriter<T>;
                        //        break;
                        //    case TypeCode.UInt16:
                        //        instance = new BBWriter_UInt16() as BBWriter<T>;
                        //        break;
                        case TypeCode.Int32:
                            instance = new BBWriter_Int32() as BBWriter<T>;
                            break;
                        //    case TypeCode.UInt32:
                        //        instance = new BBWriter_UInt32() as BBWriter<T>;
                        //        break;
                        //    case TypeCode.Int64:
                        //        instance = new BBWriter_Int64() as BBWriter<T>;
                        //        break;
                        //    case TypeCode.UInt64:
                        //        instance = new BBWriter_UInt64() as BBWriter<T>;
                        //        break;
                        //    case TypeCode.Single:
                        //        instance = new BBWriter_Single() as BBWriter<T>;
                        //        break;
                        //    case TypeCode.Double:
                        //        instance = new BBWriter_Double() as BBWriter<T>;
                        //        break;
                        //    case TypeCode.DateTime:
                        //        instance = new BBWriter_DateTime() as BBWriter<T>;
                        //        break;
                        //    case TypeCode.String:
                        //        instance = new BBWriter_String() as BBWriter<T>;
                        //        break;
                        default:
                            break;// throw new NotSupportedException(); // 似乎可以在运行时手工初始化 instance 以实现 xx.List< enum type > 的支持
                    }
                }
            }
        }


        public static void Write1<T>(this xx.BBuffer bb, List<T> list)
        {
            BBWriter<T>.instance.Write(bb, list);
        }


        public static void Write1<T>(this xx.BBuffer bb, List<List<T>> list)
        {
            if (list == null)
            {
                bb.Write((uint)0);
                return;
            }
            bb.Write((uint)list.Count);
            foreach (var o in list)
            {
                //bb.Write1(o);
                BBWriter<T>.instance.Write(bb, o);
            }
        }

        public static void Write1<T>(this xx.BBuffer bb, List<List<List<T>>> list)
        {
            if (list == null)
            {
                bb.Write((uint)0);
                return;
            }
            bb.Write((uint)list.Count);
            foreach (var o in list)
            {
                bb.Write1(o);
            }
        }


        public static long counter = 0;

        public static void WriteList<T>(this xx.BBuffer bb, T list) where T : ICollection
        {
            //bb.Write(list.Count);
            foreach (var item in list)
            {
                switch (item)
                {
                    case ICollection a:
                        {
                            WriteList(bb, a);
                        }
                        break;
                    case byte a:
                        {
                            WriteList(bb, a);
                        }
                        break;
                    case sbyte a:
                        {
                            WriteList(bb, a);
                        }
                        break;
                    case bool a:
                        {
                            WriteList(bb, a);
                        }
                        break;
                    case short a:
                        {
                            WriteList(bb, a);
                        }
                        break;
                    case ushort a:
                        {
                            WriteList(bb, a);
                        }
                        break;
                    case uint a:
                        {
                            WriteList(bb, a);
                        }
                        break;
                    case int a:
                        {
                            WriteList(bb, a);
                        }
                        break;
                    case long a:
                        {
                            WriteList(bb, a);
                        }
                        break;
                    case ulong a:
                        {
                            WriteList(bb, a);
                        }
                        break;
                    case double a:
                        {
                            WriteList(bb, a);
                        }
                        break;
               

                }
            }
        }

        public static void WriteList(this xx.BBuffer bb, byte list) { ++counter; }
        public static void WriteList(this xx.BBuffer bb, sbyte list) { ++counter; }
        public static void WriteList(this xx.BBuffer bb, bool list) { ++counter; }
        public static void WriteList(this xx.BBuffer bb, short list) { ++counter; }
        public static void WriteList(this xx.BBuffer bb, ushort list) { ++counter; }
        public static void WriteList(this xx.BBuffer bb, uint list) { ++counter; }
        public static void WriteList(this xx.BBuffer bb, int list) { ++counter; }
        public static void WriteList(this xx.BBuffer bb, long list) { ++counter; }
        public static void WriteList(this xx.BBuffer bb, ulong list) { ++counter; }
        public static void WriteList(this xx.BBuffer bb, double list) { ++counter; }

    }

    class Program
    {
        static void Main(string[] args)
        {
            var bb = new xx.BBuffer();
            List<List<List<int>>> list3 = new List<List<List<int>>>();
            var list2 = new List<List<int>>();
            list3.Add(list2);
            var list1 = new List<int>();
            list2.Add(list1);
            list2.Add(list1);
            list2.Add(list1);
            list2.Add(list1);
            list2.Add(list1);
            list2.Add(list1);
            list2.Add(list1);
            list2.Add(list1);
            list2.Add(list1);
            list2.Add(list1);
            list1.Add(123);
            list1.Add(123);
            list1.Add(123);
            list1.Add(123);
            list1.Add(123);
            list1.Add(123);
            list1.Add(123);
            list1.Add(123);
            list1.Add(123);
            list1.Add(123);
            ListUtils.counter = 0;
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                bb.WriteList(list3);
                bb.WriteList(list2);
                bb.WriteList(list1);
            }
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine(ListUtils.counter);

            //bb.Write1(list);
        }
    }
}
