﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace xx
{
    /// <summary>
    /// ByteBuffer 序列化类. Stream 的替代品. 带各种 Write Read 函数.
    /// </summary>
    public class BBuffer : Object
    {
        public byte[] buf;
        public int offset, dataLen;     // 读偏移, 数据长

        public int dataLenBak;          // 用于 WritePackage 过程中计算包长
        public int offsetRoot;          // offset值写入修正
        public int readLengthLimit;     // 主用于传递给容器类进行长度合法校验

        #region write & read funcs

        #region byte
        public void Write(byte v)
        {
            if (dataLen + 1 > buf.Length)
            {
                Reserve(dataLen + 1);
            }
            buf[dataLen++] = v;
        }
        public void Read(ref byte v)
        {
            v = buf[offset++];
        }

        public void Write(byte? v)
        {
            if (v.HasValue)
            {
                Write((byte)1);
                Write(v.Value);
            }
            else
            {
                Write((byte)0);
            }
        }
        public void Read(ref byte? v)
        {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1)
            {
                var tmp = default(byte);
                Read(ref tmp);
                v = tmp;
            }
            else
            {
                v = null;
            }
        }
        #endregion

        #region sbyte
        public void Write(sbyte v)
        {
            Write((byte)v);
        }
        public void Read(ref sbyte v)
        {
            v = (sbyte)buf[offset++];
        }
        public void Write(sbyte? v)
        {
            if (v.HasValue)
            {
                Write((byte)1);
                Write(v.Value);
            }
            else
            {
                Write((byte)0);
            }
        }
        public void Read(ref sbyte? v)
        {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1)
            {
                var tmp = default(sbyte);
                Read(ref tmp);
                v = tmp;
            }
            else
            {
                v = null;
            }
        }
        #endregion

        #region ushort
        public void Write(ushort v)
        {
            if (dataLen + 3 > buf.Length)
            {
                Reserve(dataLen + 3);
            }
            Bit7Write(buf, ref dataLen, v);
        }
        public void Read(ref ushort v)
        {
            Bit7Read(ref v, buf, ref offset, dataLen);
        }
        public void Write(ushort? v)
        {
            if (v.HasValue)
            {
                Write((byte)1);
                Write(v.Value);
            }
            else
            {
                Write((byte)0);
            }
        }
        public void Read(ref ushort? v)
        {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1)
            {
                var tmp = default(ushort);
                Read(ref tmp);
                v = tmp;
            }
            else
            {
                v = null;
            }
        }
        #endregion

        #region short
        public void Write(short v)
        {
            if (dataLen + 3 > buf.Length)
            {
                Reserve(dataLen + 3);
            }
            Bit7Write(buf, ref dataLen, ZigZagEncode(v));
        }
        public void Read(ref short v)
        {
            ushort tmp = 0;
            Bit7Read(ref tmp, buf, ref offset, dataLen);
            v = ZigZagDecode(tmp);
        }
        public void Write(short? v)
        {
            if (v.HasValue)
            {
                Write((byte)1);
                Write(v.Value);
            }
            else
            {
                Write((byte)0);
            }
        }
        public void Read(ref short? v)
        {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1)
            {
                var tmp = default(short);
                Read(ref tmp);
                v = tmp;
            }
            else
            {
                v = null;
            }
        }
        #endregion

        #region uint
        public void Write(uint v)
        {
            if (dataLen + 5 > buf.Length)
            {
                Reserve(dataLen + 5);
            }
            Bit7Write(buf, ref dataLen, v);
        }
        public void Read(ref uint v)
        {
            Bit7Read(ref v, buf, ref offset, dataLen);
        }
        public bool TryRead(ref uint v)
        {
            return Bit7TryRead(ref v, buf, ref offset, dataLen);
        }
        public void Write(uint? v)
        {
            if (v.HasValue)
            {
                Write((byte)1);
                Write(v.Value);
            }
            else
            {
                Write((byte)0);
            }
        }
        public void Read(ref uint? v)
        {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1)
            {
                var tmp = default(uint);
                Read(ref tmp);
                v = tmp;
            }
            else
            {
                v = null;
            }
        }
        #endregion

        #region int
        public void Write(int v)
        {
            if (dataLen + 5 > buf.Length)
            {
                Reserve(dataLen + 5);
            }
            Bit7Write(buf, ref dataLen, ZigZagEncode(v));
        }
        public void Read(ref int v)
        {
            uint tmp = 0;
            Bit7Read(ref tmp, buf, ref offset, dataLen);
            v = ZigZagDecode(tmp);
        }
        public bool TryRead(ref int v)
        {
            uint tmp = 0;
            if (!Bit7TryRead(ref tmp, buf, ref offset, dataLen)) return false;
            v = ZigZagDecode(tmp);
            return true;
        }
        public void Write(int? v)
        {
            if (v.HasValue)
            {
                Write((byte)1);
                Write(v.Value);
            }
            else
            {
                Write((byte)0);
            }
        }
        public void Read(ref int? v)
        {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1)
            {
                var tmp = default(int);
                Read(ref tmp);
                v = tmp;
            }
            else
            {
                v = null;
            }
        }
        #endregion

        #region ulong
        public void Write(ulong v)
        {
            if (dataLen + 10 > buf.Length)
            {
                Reserve(dataLen + 10);
            }
            Bit7Write(buf, ref dataLen, v);
        }
        public void Read(ref ulong v)
        {
            Bit7Read(ref v, buf, ref offset, dataLen);
        }
        public void Write(ulong? v)
        {
            if (v.HasValue)
            {
                Write((byte)1);
                Write(v.Value);
            }
            else
            {
                Write((byte)0);
            }
        }
        public void Read(ref ulong? v)
        {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1)
            {
                var tmp = default(ulong);
                Read(ref tmp);
                v = tmp;
            }
            else
            {
                v = null;
            }
        }
        #endregion

        #region long
        public void Write(long v)
        {
            if (dataLen + 10 > buf.Length)
            {
                Reserve(dataLen + 10);
            }
            Bit7Write(buf, ref dataLen, ZigZagEncode(v));
        }
        public void Read(ref long v)
        {
            ulong tmp = 0;
            Bit7Read(ref tmp, buf, ref offset, dataLen);
            v = ZigZagDecode(tmp);
        }
        public void Write(long? v)
        {
            if (v.HasValue)
            {
                Write((byte)1);
                Write(v.Value);
            }
            else
            {
                Write((byte)0);
            }
        }
        public void Read(ref long? v)
        {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1)
            {
                var tmp = default(long);
                Read(ref tmp);
                v = tmp;
            }
            else
            {
                v = null;
            }
        }
        #endregion

        #region float
        public void WriteDirect(float v)
        {
            var fu = new FloatingInteger { f = v };
            buf[dataLen + 0] = fu.b0;
            buf[dataLen + 1] = fu.b1;
            buf[dataLen + 2] = fu.b2;
            buf[dataLen + 3] = fu.b3;
            dataLen += 4;
        }
        public void Write(float v)
        {
            if (dataLen + 4 > buf.Length)
            {
                Reserve(dataLen + 4);
            }
            WriteDirect(v);
        }
        public void Read(ref float v)
        {
            var fu = new FloatingInteger
            {
                b0 = buf[offset + 0],
                b1 = buf[offset + 1],
                b2 = buf[offset + 2],
                b3 = buf[offset + 3]
            };
            v = fu.f;
            offset += 4;
        }
        public void Write(float? v)
        {
            if (v.HasValue)
            {
                Write((byte)1);
                Write(v.Value);
            }
            else
            {
                Write((byte)0);
            }
        }
        public void Read(ref float? v)
        {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1)
            {
                var tmp = default(float);
                Read(ref tmp);
                v = tmp;
            }
            else
            {
                v = null;
            }
        }
        #endregion

        #region double
        public void WriteDirect(double v)
        {
            if (v == 0)
            {
                buf[dataLen++] = 0;
            }
            else
            {
                if (double.IsNaN(v))
                {
                    buf[dataLen++] = 1;
                }
                else if (double.IsNegativeInfinity(v))
                {
                    buf[dataLen++] = 2;
                }
                else if (double.IsPositiveInfinity(v))
                {
                    buf[dataLen++] = 3;
                }
                else
                {
                    int intv = (int)v;
                    if (v == (double)intv)
                    {
                        buf[dataLen++] = 4;
                        Bit7Write(buf, ref dataLen, ZigZagEncode(intv));    // Write(intv);
                    }
                    else
                    {
                        buf[dataLen++] = 5;
                        var du = new FloatingInteger { d = v };
                        buf[dataLen + 0] = du.b0;
                        buf[dataLen + 1] = du.b1;
                        buf[dataLen + 2] = du.b2;
                        buf[dataLen + 3] = du.b3;
                        buf[dataLen + 4] = du.b4;
                        buf[dataLen + 5] = du.b5;
                        buf[dataLen + 6] = du.b6;
                        buf[dataLen + 7] = du.b7;
                        dataLen += 8;
                    }
                }
            }
        }
        public void Write(double v)
        {
            if (dataLen + 10 > buf.Length)
            {
                Reserve(dataLen + 10);
            }
            WriteDirect(v);
        }
        public void Read(ref double v)
        {
            switch (buf[offset++])
            {
                case 0:
                    v = 0;
                    break;
                case 1:
                    v = double.NaN;
                    break;
                case 2:
                    v = double.NegativeInfinity;
                    break;
                case 3:
                    v = double.PositiveInfinity;
                    break;
                case 4:
                    uint tmp = 0;
                    Bit7Read(ref tmp, buf, ref offset, dataLen);
                    v = ZigZagDecode(tmp);
                    break;
                case 5:
                    var du = new FloatingInteger
                    {
                        b0 = buf[offset + 0],
                        b1 = buf[offset + 1],
                        b2 = buf[offset + 2],
                        b3 = buf[offset + 3],
                        b4 = buf[offset + 4],
                        b5 = buf[offset + 5],
                        b6 = buf[offset + 6],
                        b7 = buf[offset + 7]
                    };
                    v = du.d;
                    offset += 8;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
        public void Write(double? v)
        {
            if (v.HasValue)
            {
                Write((byte)1);
                Write(v.Value);
            }
            else
            {
                Write((byte)0);
            }
        }
        public void Read(ref double? v)
        {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1)
            {
                var tmp = default(double);
                Read(ref tmp);
                v = tmp;
            }
            else
            {
                v = null;
            }
        }
        #endregion

        #region bool
        public void Write(bool v)
        {
            Write(v ? (byte)1 : (byte)0);
        }
        public void Read(ref bool v)
        {
            v = buf[offset++] == 1;
        }
        public void Write(bool? v)
        {
            if (v.HasValue)
            {
                Write((byte)1);
                Write(v.Value);
            }
            else
            {
                Write((byte)0);
            }
        }
        public void Read(ref bool? v)
        {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1)
            {
                var tmp = default(bool);
                Read(ref tmp);
                v = tmp;
            }
            else
            {
                v = null;
            }
        }
        #endregion

        #region string
        public void WriteCore(string v)
        {
            var sbuf = Encoding.UTF8.GetBytes(v);
            int sbufLen = sbuf.Length;
            if (dataLen + 5 + sbufLen > buf.Length)
            {
                Reserve(dataLen + 5 + sbufLen);
            }
            Bit7Write(buf, ref dataLen, (uint)sbufLen);
            Buffer.BlockCopy(sbuf, 0, buf, dataLen, sbufLen);
            dataLen += sbufLen;
        }
        public void Write(string v)
        {
            // 和 C++ 那边保持一致, typeId 打头. 空就是写入 0
            if (v == null)
            {
                Write((ushort)0);
                return;
            }
            else
            {
                Write((ushort)1);
            }
            if (ptrStore != null)
            {
                var rtv = ptrStore.Add(v, (uint)(dataLen - offsetRoot));       // 试将 v 和 相对offset 放入字典, 得到下标和是否成功
                Write(ptrStore.ValueAt(rtv.index));                             // 取 offset ( 不管是否成功 )
                if (rtv.success) WriteCore(v);
            }
            else
            {
                WriteCore(v);
            }
        }
        public void ReadCore(ref string v)
        {
            uint len = 0;
            Read(ref len);
            if (readLengthLimit != 0 && len > readLengthLimit) throw new Exception("overflow of limit");
            if (len == 0)
            {
                v = "";
            }
            else
            {
                v = Encoding.UTF8.GetString(buf, offset, (int)len);
                offset += (int)len;
            }
        }
        public void Read(ref string v)
        {
            var typeId = buf[offset++];
            if (typeId == 0)
            {
                v = null;
                return;
            }
            else if (typeId != 1)
            {
                throw new Exception("wrong typeId");
            }
            if (idxStore != null)
            {
                uint ptr_offset = 0, bb_offset_bak = (uint)this.offset - (uint)offsetRoot;
                Read(ref ptr_offset);
                if (ptr_offset == bb_offset_bak)
                {
                    ReadCore(ref v);
                    idxStore.Add(ptr_offset, v);
                }
                else
                {
                    int idx = idxStore.Find(ptr_offset);
                    if (idx == -1) throw new Exception("ptr_offset is not found");
                    v = (string)idxStore.ValueAt(idx);
                    System.Diagnostics.Debug.Assert(v != null);
                }
            }
            else
            {
                ReadCore(ref v);
            }
        }
        #endregion

        #region DateTime
        public void WriteDirect(DateTime v)
        {
            short year = (short)v.Year;
            buf[dataLen + 0] = (byte)year;
            buf[dataLen + 1] = (byte)(year >> 8);
            buf[dataLen + 2] = (byte)v.Month;
            buf[dataLen + 3] = (byte)v.Day;
            buf[dataLen + 4] = (byte)v.Hour;
            buf[dataLen + 5] = (byte)v.Minute;
            buf[dataLen + 6] = (byte)v.Second;
            buf[dataLen + 7] = (byte)0;
            dataLen += 8;
        }
        public void Write(DateTime v)
        {
            if (dataLen + 8 > buf.Length)
            {
                Reserve(dataLen + 8);
            }
            WriteDirect(v);
        }
        public void Read(ref DateTime v)
        {
            v = new DateTime(buf[offset + 0] + (buf[offset + 1] << 8),
                buf[offset + 2],
                buf[offset + 3],
                buf[offset + 4],
                buf[offset + 5],
                buf[offset + 6]);
            offset += 8;
        }
        #endregion

        #region T : IObject

        // 不带引用的类编码规则: 类型编号 + 类数据
        // 类型编号兼 null 表达, 0 代表 null

        // 引用类 编码规则: 类型编号 + offset数据 + [类数据]
        // 类型编号兼 null 表达, 0 代表 null
        // 如果 offset数据 == bb.offset 则表示当前类为首次序列化, 后面会跟 类数据. 否则就是引用

        // 当 objs / offsets 不为空时, 启用引用读写
        Dict<object, uint> ptrStore = null;
        Dict<uint, object> idxStore = null;

        // 写前准备( 创建指针字典, 存起始 offset )
        public void BeginWrite()
        {
            if (ptrStore == null) ptrStore = new Dict<object, uint>();
            else ptrStore.Clear();
            offsetRoot = dataLen;
        }
        // 写后收拾( 回收字典 )
        public void EndWrite()
        {
            ptrStore.Clear();
        }

        // 读前准备( 创建指针字典, 并不重置 读长度限制 )
        public void BeginRead()
        {
            if (idxStore == null) idxStore = new Dict<uint, object>();
            else idxStore.Clear();
            offsetRoot = offset;
        }
        // 读后收拾( 回收字典 )
        public void EndRead()
        {
            idxStore.Clear();
        }

        // 一波流, 写入一套引用类的根
        public void WriteRoot<T>(T v) where T : IObject
        {
            BeginWrite();
            Write(v);
            EndWrite();
        }

        // 一波流, 读出一套引用类
        public void ReadRoot<T>(ref T v) where T : IObject
        {
            BeginRead();
            Read(ref v);
            EndRead();
        }

        public void Write<T>(T v) where T : IObject
        {
            if (v == null)
            {
                Write((ushort)0);
            }
            else
            {
                System.Diagnostics.Debug.Assert(v.GetPackageId() != ushort.MaxValue);   // 通常是没有执行 XXXPKG.AllTypes.Register() 所致
                Write(v.GetPackageId());
                if (ptrStore != null)
                {
                    var rtv = ptrStore.Add(v, (uint)(dataLen - offsetRoot));        // 试将 v 和 相对offset 放入字典, 得到下标和是否成功
                    Write(ptrStore.ValueAt(rtv.index));                             // 取 offset ( 不管是否成功 )
                    if (!rtv.success) return;                                       // 如果首次出现就序列化类本体
                }
                v.ToBBuffer(this);
            }
        }

        public void Read<T>(ref T v) where T : IObject
        {
            var typeId = (ushort)0;
            Read(ref typeId);
            if (typeId == 0)
            {
                v = default(T);
                return;
            }
            if (typeId == 1)
            {
                throw new Exception("Read<T> does not support string type");
            }
            if (idxStore != null)
            {
                uint ptr_offset = 0, bb_offset_bak = (uint)this.offset - (uint)offsetRoot;
                Read(ref ptr_offset);
                if (ptr_offset == bb_offset_bak)
                {
                    v = (T)CreateByTypeId(typeId);
                    System.Diagnostics.Debug.Assert(v != null);
                    idxStore.Add(ptr_offset, v);
                    v.FromBBuffer(this);
                }
                else
                {
                    int idx = idxStore.Find(ptr_offset);
                    if (idx == -1) throw new Exception("ptr_offset is not found");
                    v = (T)idxStore.ValueAt(idx);
                    System.Diagnostics.Debug.Assert(v != null);
                }
            }
            else
            {
                v = (T)CreateByTypeId(typeId);
                System.Diagnostics.Debug.Assert(v != null);
                v.FromBBuffer(this);
            }
        }

        // 为方便直接返回 new T. 通常返回 null 表示解析失败.
        public T TryReadRoot<T>() where T : IObject
        {
            T t = default(T);
            try
            {
                ReadRoot(ref t);
            }
            catch {
                t = default(T);
            }
            return t;
        }

        /// <summary>
        /// 自定义序列化函数的类型
        /// </summary>
        /// <param name="bb">当前 BBuffer</param>
        /// <param name="c">字段所在容器类/结构体</param>
        /// <param name="fn">字段名</param>
        public delegate void CustomWriteDelegate(BBuffer bb, object c, string fn);

        /// <summary>
        /// 自定义序列化函数支持. 用前必须赋值. 用后建议清除.
        /// </summary>
        public CustomWriteDelegate CustomWrite;


        // 试适配 Ref<T>

        public void Write<T>(Weak<T> v) where T : Object
        {
            if (v.NotNull()) Write(v.pointer);
            else Write((byte)0);
        }

        public void Read<T>(ref Weak<T> v) where T : Object
        {
            Read(ref v.pointer);
        }


        #endregion

        #region byte[]

        /// <summary>
        /// 直接追加写入一段2进制数据
        /// </summary>
        public void WriteBuf(byte[] buf, int offset, int dataLen)
        {
            Reserve(dataLen + this.dataLen);
            Buffer.BlockCopy(buf, offset, this.buf, this.dataLen, dataLen);
            this.dataLen += dataLen;
        }

        /// <summary>
        /// 直接追加写入一段2进制数据之指针版
        /// </summary>
        public void WriteBuf(IntPtr bufPtr, int len)
        {
            Reserve(dataLen + len);
            Marshal.Copy(bufPtr, buf, dataLen, len);
            dataLen += len;
        }

        #endregion

        #endregion

        #region misc funcs

        public BBuffer()
        {
            Assign(new byte[32], 0);
        }
        public BBuffer(int capacity)
        {
            Assign(new byte[capacity < 32 ? 32 : Round2n((uint)capacity)], 0);
        }

        public BBuffer(byte[] buf, int dataLen = 0)
        {
            Assign(buf, dataLen);
        }

        public BBuffer(byte[] buf, int offset, int pkgLen)
        {
            Assign(buf, offset, pkgLen);
        }

        public static implicit operator BBuffer(byte[] buf)
        {
            return new BBuffer(buf, buf.Length);
        }

        public void Assign(byte[] buf, int dataLen = 0)
        {
            offset = 0;
            this.buf = buf;
            this.dataLen = dataLen;
        }

        public void Assign(byte[] buf, int offset = 0, int dataLen = 0)
        {
            this.buf = buf;
            this.offset = offset;
            this.dataLen = offset + dataLen;
        }

        public void Clear()
        {
            dataLen = offset = 0;
        }

        public bool Empty()
        {
            return dataLen == 0;
        }

        public bool Full()
        {
            return dataLen == offset;
        }

        public byte[] Collapse()
        {
            Array.Resize(ref buf, dataLen);
            return buf;
        }

        public byte[] DumpData()
        {
            var rtv = new byte[dataLen];
            Buffer.BlockCopy(buf, 0, rtv, 0, dataLen);
            return rtv;
        }

        public void Ensure(int len)
        {
            if (dataLen + len > buf.Length)
            {
                Reserve(dataLen + len);
            }
        }

        public void Reserve(int capacity)
        {
            if (capacity <= buf.Length) return;
            Array.Resize(ref buf, (int)Round2n((uint)(capacity * 2)));
        }

        public byte At(int idx)
        {
            return buf[idx];
        }

        public void Dump(ref StringBuilder s)
        {
            Dump(ref s, buf, dataLen);
        }

        public void Dump()
        {
            var s = new StringBuilder();
            Dump(ref s);
            Console.Write(s);
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            ToString(s);
            return s.ToString();
        }

        public void WriteLength(int len)
        {
            Write((uint)len);
        }
        public int ReadLength()
        {
            uint len = 0;
            Bit7Read(ref len, buf, ref offset, dataLen);
            return (int)len;
        }
        public int ReadLength(int minLen, int maxLen)
        {
            var len = ReadLength();
            if (len < minLen || (maxLen > 0 && len > maxLen))
            {
                throw new OverflowException();
            }
            return len;
        }
        public void Resize(int len)
        {
            if (len == dataLen) return;
            else if (len < dataLen)
            {
                Array.Clear(buf, len, dataLen - len);
            }
            else // len > dataLen
            {
                Reserve(len);
            }
            dataLen = len;
        }

        // 跳过 typeId & offset, 定位到首个 field 的数据区, 为直读首个 field 或 按基类直读创造便利
        public bool SeekToFirstField()
        {
            this.offset = 0;
            ushort typeId = 0;
            Read(ref typeId);
            if (typeId == 0) return false;
            uint offset = 0;
            int offsetBak = this.offset;
            Read(ref offset);
            return offset == offsetBak;
        }

        // interface impl

        public override ushort GetPackageId() { return TypeId<BBuffer>.value; }

        public override void ToBBuffer(BBuffer bb)
        {
            var len = dataLen;
            bb.WriteLength(len);
            if (len == 0) return;
            bb.Ensure(len);
            Buffer.BlockCopy(buf, 0, bb.buf, bb.dataLen, len);
            bb.dataLen += len;
        }

        public override void FromBBuffer(BBuffer bb)
        {
            int len = bb.ReadLength();
            if (bb.readLengthLimit != 0 && len > bb.readLengthLimit) throw new Exception("overflow of limit");
            Resize(len);
            offset = 0;
            if (len == 0) return;
            Buffer.BlockCopy(bb.buf, bb.offset, buf, 0, len);
            bb.offset += len;
        }

        public override void ToString(StringBuilder s)
        {
            s.Append("{ \"len\":" + dataLen + ", \"offset\":" + offset + ", \"data\":[ ");
            for (var i = 0; i < dataLen; i++)
            {
                s.Append(buf[i] + ", ");
            }
            if (dataLen > 0) s.Length -= 2;
            s.Append(" ] }");
        }

        #endregion

        #region static utils

        /// <summary>
        /// 统计有多少个 1
        /// </summary>
        public static int Popcnt(uint x)
        {
            x -= ((x >> 1) & 0x55555555);
            x = (((x >> 2) & 0x33333333) + (x & 0x33333333));
            x = (((x >> 4) + x) & 0x0f0f0f0f);
            x += (x >> 8);
            x += (x >> 16);
            return (int)(x & 0x0000003f);
        }

        /// <summary>
        /// 统计高位有多少个 0
        /// </summary>
        public static int Clz(uint x)
        {
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (int)(32 - Popcnt(x));
        }

        /// <summary>
        /// 求大于 v 的 2^n 值
        /// </summary>
        public static uint Round2n(uint v)
        {
            int bits = 31 - Clz(v);
            var rtv = (uint)(1u << bits);
            if (rtv == v) return v;
            return rtv << 1;
        }

        // 负转正：利用单数来存负数，双数来存正数
        // 等效代码： if( v < 0 ) return -v * 2 - 1; else return v * 2;

        public static ushort ZigZagEncode(short v) { return (ushort)((v << 1) ^ (v >> 15)); }

        public static uint ZigZagEncode(int v) { return (uint)((v << 1) ^ (v >> 31)); }

        public static ulong ZigZagEncode(long v) { return (ulong)((v << 1) ^ (v >> 63)); }

        // 等效代码： if( (v & 1) > 0 ) return -(v + 1) / 2; else return v / 2;

        public static short ZigZagDecode(ushort v) { return (short)((short)(v >> 1) ^ (-(short)(v & 1))); }

        public static int ZigZagDecode(uint v) { return (int)(v >> 1) ^ (-(int)(v & 1)); }

        public static long ZigZagDecode(ulong v) { return (long)(v >> 1) ^ (-(long)(v & 1)); }

        // need ensure 5
        public static void Bit7Write(byte[] buf, ref int offset, uint v)
        {
            while (v >= 1 << 7)
            {
                buf[offset++] = (byte)(v & 0x7fu | 0x80u);
                v >>= 7;
            };
            buf[offset++] = (byte)v;
        }

        // 同样的实现两份是考虑到 32位 cpu 用 ulong 操作没效率
        // need ensure 10
        public static void Bit7Write(byte[] buf, ref int offset, ulong v)
        {
            while (v >= 1 << 7)
            {
                buf[offset++] = (byte)(v & 0x7fu | 0x80u);
                v >>= 7;
            };
            buf[offset++] = (byte)v;
        }

        public static void Bit7Read(ref ulong v, byte[] buf, ref int offset, int dataLen)
        {
            v = 0;
            for (int shift = 0; shift < /*sizeof(T)*/8 * 8; shift += 7)
            {
                if (offset == dataLen) throw new OverflowException();
                ulong b = buf[offset++];
                v |= (b & 0x7Fu) << shift;
                if ((b & 0x80) == 0) return;
            }
            throw new OverflowException();
        }

        public static void Bit7Read(ref uint v, byte[] buf, ref int offset, int dataLen)
        {
            v = 0;
            for (int shift = 0; shift < /*sizeof(T)*/4 * 8; shift += 7)
            {
                if (offset == dataLen) throw new OverflowException();
                uint b = buf[offset++];
                v |= (b & 0x7Fu) << shift;
                if ((b & 0x80) == 0) return;
            }
            throw new OverflowException();
        }

        public static void Bit7Read(ref ushort v, byte[] buf, ref int offset, int dataLen)
        {
            v = 0;
            for (int shift = 0; shift < /*sizeof(T)*/2 * 8; shift += 7)
            {
                if (offset == dataLen) throw new OverflowException();
                uint b = buf[offset++];
                v |= (ushort)((b & 0x7Fu) << shift);
                if ((b & 0x80) == 0) return;
            }
            throw new OverflowException();
        }

        public static bool Bit7TryRead(ref uint v, byte[] buf, ref int offset, int dataLen)
        {
            v = 0;
            for (int shift = 0; shift < /*sizeof(T)*/4 * 8; shift += 7)
            {
                if (offset == dataLen) return false;
                uint b = buf[offset++];
                v |= (b & 0x7Fu) << shift;
                if ((b & 0x80) == 0) return true;
            }
            return false;
        }

        public static void DumpAscII(ref StringBuilder s, byte[] buf, int offset, int len)
        {
            for (int i = offset; i < offset + len; ++i)
            {
                byte c = buf[i];
                if (c < 32 || c > 126)
                    s.Append('.');
                else
                    s.Append((char)c);
            }
        }

        // write buf's binary dump text to s
        public static void Dump(ref StringBuilder s, byte[] buf, int len = 0)
        {
            if (buf == null || buf.Length == 0)
                return;
            if (len == 0)
                len = buf.Length;
            s.Append("--------  0  1  2  3 | 4  5  6  7 | 8  9  A  B | C  D  E  F");
            s.Append("   dataLen = " + len);
            int i = 0;
            for (; i < len; ++i)
            {
                if ((i % 16) == 0)
                {
                    if (i > 0)
                    {           // refput ascii to the end of the line
                        s.Append("  ");
                        DumpAscII(ref s, buf, i - 16, 16);
                    }
                    s.Append('\n');
                    s.Append(i.ToString("x8"));
                    s.Append("  ");
                }
                else if ((i > 0 && (i % 4 == 0)))
                {
                    s.Append("  ");
                }
                else
                    s.Append(' ');
                s.Append(BitConverter.ToString(buf, i, 1));
            }
            int left = i % 16;
            if (left == 0)
            {
                left = 16;
            }
            if (left > 0)
            {
                len = len + 16 - left;
                for (; i < len; ++i)
                {
                    if (i > 0 && (i % 4 == 0))
                        s.Append("  ");
                    else
                        s.Append(' ');
                    s.Append("  ");
                }
                s.Append("  ");
                DumpAscII(ref s, buf, i - 16, left);
            }
            s.Append('\n');
        }

        public static string Dump(byte[] buf, int len = 0)
        {
            var sb = new StringBuilder();
            Dump(ref sb, buf, len);
            return sb.ToString();
        }

        #endregion
    }
}
