using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// 登陆服务相关
public partial class DbService
{
    public void SetLoginPeer(xx.UvTcpPeer peer)
    {
        if (loginPeer != null && loginPeer.alive)
        {
            loginPeer.Send(new PKG.Generic.Error { number = -1, text = "another login server connected." });
            loginPeer.DelayRelease(1);
        }
        loginPeer = peer;

        loginPeer.OnReceivePackage = null;
        loginPeer.OnReceiveRequest = (serial, bb) =>
        {
            // 试着解包
            var pkg = bb.TryReadRoot<xx.IObject>();

            // 如果解包失败, 立即踢掉 并 清掉
            if (pkg == null)
            {
                loginPeer.Dispose();
                loginPeer = null;
                return;
            }

            Console.WriteLine("recv server_login package: " + pkg);

            // 根据请求来分发到处理函数
            switch (pkg)
            {
                case PKG.Login_DB.Auth a:
                    Handle_Login_Auth(serial, a, peer);
                    break;
                default:
                    Console.WriteLine("unhandled pkg: ", pkg);
                    break;
            }
        };

        Console.WriteLine("server_login connected.");
    }

    public void Handle_Login_Auth(uint serial, PKG.Login_DB.Auth a, xx.UvTcpPeer peer)
    {
        // 参数合法性初步检查
        if (string.IsNullOrEmpty(a.username) || a.password == null)
        {
            peer.Send(new PKG.Generic.Error { number = -2, text = "username is null/empty or password is null." });
        }

        // 模拟数据库异步查询
        new Task(() =>
        {
            // 准备回发结果
            xx.IObject pkg = null;

            try
            {
                // 模拟一个耗时 SQL 操作
                Thread.Sleep(1000);

                if (a.username == "abc" && a.password == "123")
                {
                    pkg = new PKG.DB_Login.Auth_Success
                    {
                        userId = 12
                    };
                }
                else
                {
                    pkg = new PKG.Generic.Error { number = -3, text = "bad username or password." };
                }
            }
            catch (Exception ex)
            {
                pkg = new PKG.Generic.Error { number = -4, text = ex.Message };
            }

            // 核实回发结果是否已经填充
            Debug.Assert(pkg != null);

            // 封送到 uv 主线程去执行
            dispatcher.Dispatch(() =>
            {
                // 检查连接的死活. 如果没断开就回发结果
                if (peer.alive)
                {
                    // 发送处理结果
                    peer.SendResponse(serial, pkg);
                }
            });
        }).Start();
    }
}
