using System;
namespace PKG
{
    public static class PkgGenMd5
    {
        public const string value = "0765b9066ddf9c68f031f576db570ebf"; 
    }

namespace Generic
{
    public partial class Success : xx.Object
    {

        public override ushort GetPackageId()
        {
            return xx.TypeId<Success>.value;
        }

        public override void ToBBuffer(xx.BBuffer bb)
        {
        }

        public override void FromBBuffer(xx.BBuffer bb)
        {
        }
        public override void ToString(System.Text.StringBuilder s)
        {
            if (__toStringing)
            {
        	    s.Append("[ \"***** recursived *****\" ]");
        	    return;
            }
            else __toStringing = true;

            s.Append("{ \"pkgTypeName\":\"Generic.Success\", \"pkgTypeId\":" + GetPackageId());
            ToStringCore(s);
            s.Append(" }");

            __toStringing = false;
        }
        public override void ToStringCore(System.Text.StringBuilder s)
        {
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
    public partial class Error : xx.Object
    {
        public int number;
        public string text;

        public override ushort GetPackageId()
        {
            return xx.TypeId<Error>.value;
        }

        public override void ToBBuffer(xx.BBuffer bb)
        {
            bb.Write(this.number);
            bb.Write(this.text);
        }

        public override void FromBBuffer(xx.BBuffer bb)
        {
            bb.Read(ref this.number);
            bb.readLengthLimit = 0;
            bb.Read(ref this.text);
        }
        public override void ToString(System.Text.StringBuilder s)
        {
            if (__toStringing)
            {
        	    s.Append("[ \"***** recursived *****\" ]");
        	    return;
            }
            else __toStringing = true;

            s.Append("{ \"pkgTypeName\":\"Generic.Error\", \"pkgTypeId\":" + GetPackageId());
            ToStringCore(s);
            s.Append(" }");

            __toStringing = false;
        }
        public override void ToStringCore(System.Text.StringBuilder s)
        {
            s.Append(", \"number\":" + number.ToString());
            if (text != null) s.Append(", \"text\":\"" + text.ToString() + "\"");
            else s.Append(", \"text\":nil");
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
    public partial class ServerInfo : xx.Object
    {
        public string name;

        public override ushort GetPackageId()
        {
            return xx.TypeId<ServerInfo>.value;
        }

        public override void ToBBuffer(xx.BBuffer bb)
        {
            bb.Write(this.name);
        }

        public override void FromBBuffer(xx.BBuffer bb)
        {
            bb.readLengthLimit = 0;
            bb.Read(ref this.name);
        }
        public override void ToString(System.Text.StringBuilder s)
        {
            if (__toStringing)
            {
        	    s.Append("[ \"***** recursived *****\" ]");
        	    return;
            }
            else __toStringing = true;

            s.Append("{ \"pkgTypeName\":\"Generic.ServerInfo\", \"pkgTypeId\":" + GetPackageId());
            ToStringCore(s);
            s.Append(" }");

            __toStringing = false;
        }
        public override void ToStringCore(System.Text.StringBuilder s)
        {
            if (name != null) s.Append(", \"name\":\"" + name.ToString() + "\"");
            else s.Append(", \"name\":nil");
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
    public partial class UserInfo : xx.Object
    {
        public long id;
        public string name;

        public override ushort GetPackageId()
        {
            return xx.TypeId<UserInfo>.value;
        }

        public override void ToBBuffer(xx.BBuffer bb)
        {
            bb.Write(this.id);
            bb.Write(this.name);
        }

        public override void FromBBuffer(xx.BBuffer bb)
        {
            bb.Read(ref this.id);
            bb.readLengthLimit = 0;
            bb.Read(ref this.name);
        }
        public override void ToString(System.Text.StringBuilder s)
        {
            if (__toStringing)
            {
        	    s.Append("[ \"***** recursived *****\" ]");
        	    return;
            }
            else __toStringing = true;

            s.Append("{ \"pkgTypeName\":\"Generic.UserInfo\", \"pkgTypeId\":" + GetPackageId());
            ToStringCore(s);
            s.Append(" }");

            __toStringing = false;
        }
        public override void ToStringCore(System.Text.StringBuilder s)
        {
            s.Append(", \"id\":" + id.ToString());
            if (name != null) s.Append(", \"name\":\"" + name.ToString() + "\"");
            else s.Append(", \"name\":nil");
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
}
namespace Login_DB
{
    /// <summary>
    /// 校验. 成功返回 DB_Login.Auth_Success 内含 userId. 失败返回 Generic.Error
    /// </summary>
    public partial class Auth : xx.Object
    {
        public string username;
        public string password;

        public override ushort GetPackageId()
        {
            return xx.TypeId<Auth>.value;
        }

        public override void ToBBuffer(xx.BBuffer bb)
        {
            bb.Write(this.username);
            bb.Write(this.password);
        }

        public override void FromBBuffer(xx.BBuffer bb)
        {
            bb.readLengthLimit = 0;
            bb.Read(ref this.username);
            bb.readLengthLimit = 0;
            bb.Read(ref this.password);
        }
        public override void ToString(System.Text.StringBuilder s)
        {
            if (__toStringing)
            {
        	    s.Append("[ \"***** recursived *****\" ]");
        	    return;
            }
            else __toStringing = true;

            s.Append("{ \"pkgTypeName\":\"Login_DB.Auth\", \"pkgTypeId\":" + GetPackageId());
            ToStringCore(s);
            s.Append(" }");

            __toStringing = false;
        }
        public override void ToStringCore(System.Text.StringBuilder s)
        {
            if (username != null) s.Append(", \"username\":\"" + username.ToString() + "\"");
            else s.Append(", \"username\":nil");
            if (password != null) s.Append(", \"password\":\"" + password.ToString() + "\"");
            else s.Append(", \"password\":nil");
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
}
namespace DB_Login
{
    /// <summary>
    /// Login_DB.Auth 的成功返回值
    /// </summary>
    public partial class Auth_Success : xx.Object
    {
        public int userId;

        public override ushort GetPackageId()
        {
            return xx.TypeId<Auth_Success>.value;
        }

        public override void ToBBuffer(xx.BBuffer bb)
        {
            bb.Write(this.userId);
        }

        public override void FromBBuffer(xx.BBuffer bb)
        {
            bb.Read(ref this.userId);
        }
        public override void ToString(System.Text.StringBuilder s)
        {
            if (__toStringing)
            {
        	    s.Append("[ \"***** recursived *****\" ]");
        	    return;
            }
            else __toStringing = true;

            s.Append("{ \"pkgTypeName\":\"DB_Login.Auth_Success\", \"pkgTypeId\":" + GetPackageId());
            ToStringCore(s);
            s.Append(" }");

            __toStringing = false;
        }
        public override void ToStringCore(System.Text.StringBuilder s)
        {
            s.Append(", \"userId\":" + userId.ToString());
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
}
namespace Client_Login
{
    /// <summary>
    /// 失败返回 Generic.Error. 成功返回 Login_Client.EnterLobby 或 EnterGame1
    /// </summary>
    public partial class Auth : xx.Object
    {
        public string username;
        public string password;

        public override ushort GetPackageId()
        {
            return xx.TypeId<Auth>.value;
        }

        public override void ToBBuffer(xx.BBuffer bb)
        {
            bb.Write(this.username);
            bb.Write(this.password);
        }

        public override void FromBBuffer(xx.BBuffer bb)
        {
            bb.readLengthLimit = 0;
            bb.Read(ref this.username);
            bb.readLengthLimit = 0;
            bb.Read(ref this.password);
        }
        public override void ToString(System.Text.StringBuilder s)
        {
            if (__toStringing)
            {
        	    s.Append("[ \"***** recursived *****\" ]");
        	    return;
            }
            else __toStringing = true;

            s.Append("{ \"pkgTypeName\":\"Client_Login.Auth\", \"pkgTypeId\":" + GetPackageId());
            ToStringCore(s);
            s.Append(" }");

            __toStringing = false;
        }
        public override void ToStringCore(System.Text.StringBuilder s)
        {
            if (username != null) s.Append(", \"username\":\"" + username.ToString() + "\"");
            else s.Append(", \"username\":nil");
            if (password != null) s.Append(", \"password\":\"" + password.ToString() + "\"");
            else s.Append(", \"password\":nil");
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
}
namespace Login_Client
{
    /// <summary>
    /// 校验成功
    /// </summary>
    public partial class AuthSuccess : xx.Object
    {
        /// <summary>
        /// 连接大厅后要发送的 token
        /// </summary>
        public string token;

        public override ushort GetPackageId()
        {
            return xx.TypeId<AuthSuccess>.value;
        }

        public override void ToBBuffer(xx.BBuffer bb)
        {
            bb.Write(this.token);
        }

        public override void FromBBuffer(xx.BBuffer bb)
        {
            bb.readLengthLimit = 0;
            bb.Read(ref this.token);
        }
        public override void ToString(System.Text.StringBuilder s)
        {
            if (__toStringing)
            {
        	    s.Append("[ \"***** recursived *****\" ]");
        	    return;
            }
            else __toStringing = true;

            s.Append("{ \"pkgTypeName\":\"Login_Client.AuthSuccess\", \"pkgTypeId\":" + GetPackageId());
            ToStringCore(s);
            s.Append(" }");

            __toStringing = false;
        }
        public override void ToStringCore(System.Text.StringBuilder s)
        {
            if (token != null) s.Append(", \"token\":\"" + token.ToString() + "\"");
            else s.Append(", \"token\":nil");
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
}
    public static class AllTypes
    {
        public static void Register()
        {
            xx.Object.RegisterInternals();
            xx.Object.Register<Generic.Success>(3);
            xx.Object.Register<Generic.Error>(4);
            xx.Object.Register<Generic.ServerInfo>(5);
            xx.Object.Register<Generic.UserInfo>(6);
            xx.Object.Register<Login_DB.Auth>(7);
            xx.Object.Register<DB_Login.Auth_Success>(8);
            xx.Object.Register<Client_Login.Auth>(9);
            xx.Object.Register<Login_Client.AuthSuccess>(10);
        }
    }
}
