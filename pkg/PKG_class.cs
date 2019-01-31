﻿using System;
namespace PKG
{
    public static class PkgGenMd5
    {
        public const string value = "b553ca103dc8c7f418d636457824eb73"; 
    }

    public partial class Account : xx.Object
    {
        public int id;
        public string name;
        public int? vipLevel;

        public override ushort GetPackageId()
        {
            return xx.TypeId<Account>.value;
        }

        public override void ToBBuffer(xx.BBuffer bb)
        {
            bb.Write(this.id);
            bb.Write(this.name);
            bb.Write(this.vipLevel);
        }

        public override void FromBBuffer(xx.BBuffer bb)
        {
            bb.Read(ref this.id);
            bb.readLengthLimit = 0;
            bb.Read(ref this.name);
            bb.Read(ref this.vipLevel);
        }
        public override void ToString(System.Text.StringBuilder s)
        {
            if (__toStringing)
            {
        	    s.Append("[ \"***** recursived *****\" ]");
        	    return;
            }
            else __toStringing = true;

            s.Append("{ \"pkgTypeName\":\"Account\", \"pkgTypeId\":" + GetPackageId());
            ToStringCore(s);
            s.Append(" }");

            __toStringing = false;
        }
        public override void ToStringCore(System.Text.StringBuilder s)
        {
            s.Append(", \"id\":" + id.ToString());
            if (name != null) s.Append(", \"name\":\"" + name.ToString() + "\"");
            else s.Append(", \"name\":nil");
            s.Append(", \"vipLevel\":" + (vipLevel.HasValue ? vipLevel.Value.ToString() : "nil"));
        }
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public override void MySqlAppend(System.Text.StringBuilder sb, bool ignoreReadOnly)
        {
        }
    }
    public partial class Player : Account
    {
        public Scene owner;

        public override ushort GetPackageId()
        {
            return xx.TypeId<Player>.value;
        }

        public override void ToBBuffer(xx.BBuffer bb)
        {
            base.ToBBuffer(bb);
            bb.Write(this.owner);
        }

        public override void FromBBuffer(xx.BBuffer bb)
        {
            base.FromBBuffer(bb);
            bb.Read(ref this.owner);
        }
        public override void ToString(System.Text.StringBuilder s)
        {
            if (__toStringing)
            {
        	    s.Append("[ \"***** recursived *****\" ]");
        	    return;
            }
            else __toStringing = true;

            s.Append("{ \"pkgTypeName\":\"Player\", \"pkgTypeId\":" + GetPackageId());
            ToStringCore(s);
            s.Append(" }");

            __toStringing = false;
        }
        public override void ToStringCore(System.Text.StringBuilder s)
        {
            base.ToStringCore(s);
            s.Append(", \"owner\":" + (owner == null ? "nil" : owner.ToString()));
        }
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public override void MySqlAppend(System.Text.StringBuilder sb, bool ignoreReadOnly)
        {
            base.MySqlAppend(sb, ignoreReadOnly);
        }
    }
    public partial class Scene : xx.Object
    {
        public xx.List<xx.Ref<Player>> players;

        public override ushort GetPackageId()
        {
            return xx.TypeId<Scene>.value;
        }

        public override void ToBBuffer(xx.BBuffer bb)
        {
            bb.Write(this.players);
        }

        public override void FromBBuffer(xx.BBuffer bb)
        {
            bb.readLengthLimit = 0;
            bb.Read(ref this.players);
        }
        public override void ToString(System.Text.StringBuilder s)
        {
            if (__toStringing)
            {
        	    s.Append("[ \"***** recursived *****\" ]");
        	    return;
            }
            else __toStringing = true;

            s.Append("{ \"pkgTypeName\":\"Scene\", \"pkgTypeId\":" + GetPackageId());
            ToStringCore(s);
            s.Append(" }");

            __toStringing = false;
        }
        public override void ToStringCore(System.Text.StringBuilder s)
        {
            s.Append(", \"players\":" + (players == null ? "nil" : players.ToString()));
        }
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            ToString(sb);
            return sb.ToString();
        }
        public override void MySqlAppend(System.Text.StringBuilder sb, bool ignoreReadOnly)
        {
        }
    }
    public static class AllTypes
    {
        public static void Register()
        {
            xx.Object.RegisterInternals();
            xx.Object.Register<Account>(6);
            xx.Object.Register<Player>(3);
            xx.Object.Register<Scene>(4);
            xx.Object.Register<xx.List<xx.Ref<Player>>>(5);
        }
    }
}
