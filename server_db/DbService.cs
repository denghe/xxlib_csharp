using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 数据库服务主体
public partial class DbService
{
    // 监听器
    public xx.UvTcpListener listener;

    // 主线程派发器
    public xx.UvAsync dispatcher;

    // 缓存针对 login, lobby, game1 服务的连接对端 以便 主动推送
    xx.UvTcpPeer loginPeer;
    xx.UvTcpPeer lobbyPeer;
    xx.UvTcpPeer game1Peer;

    public DbService(xx.UvLoop loop)
    {
        listener = new xx.UvTcpListener(loop);
        listener.Bind("0.0.0.0", 10000);
        listener.Listen();
        listener.OnAccept = OnAccept;

        dispatcher = new xx.UvAsync(loop);
    }

    public void OnAccept(xx.UvTcpPeer peer)
    {
        // 首次建立连接时, 身份不明, 先 bind 身份检测函数
        peer.OnReceivePackage = (bb) =>
        {
            // 试着解包
            var pkg = bb.TryReadRoot<xx.IObject>();

            // 如果解包失败, 立即踢掉
            if (pkg == null)
            {
                peer.Dispose();
                return;
            }

            // 对于匿名连接, 先进行 ServiceInfo 检测. 如果收到其他类型的包, 直接踢掉
            int r = 0;
            switch (pkg)
            {
                case PKG.Generic.ServerInfo si:
                    r = HandleServerInfo(si, peer);
                    break;
                default:
                    r = -1;
                    break;
            }

            // 判断处理结果
            if (r == 0)                     // 正确执行, 保持连接
            {
            }
            else if (r < 0)                 // 处理失败
            {
                peer.Dispose();             // 直接断开
            }
            else                            // r > 0
            {
                peer.DelayRelease(r);       // 延迟断开
            }
        };
    }

    public int HandleServerInfo(PKG.Generic.ServerInfo si, xx.UvTcpPeer peer)
    {
        switch (si.name)
        {
            case "login":
                SetLoginPeer(peer);
                break;
            case "lobby":
                //SetLobbyPeer(peer);
                break;
            case "game1":
                //SetGame1Peer(peer);
                break;
            default:
                return -1;
        }
        return 0;
    }

    //public void SetLobbyPeer(xx.UvTcpPeer peer)
    //{
    //    if (loginPeer.alive)
    //    {
    //        loginPeer.Send(new PKG.Error { number = -1, text = "another lobby server connected." });
    //        loginPeer.DelayRelease(1);
    //    }
    //    loginPeer = peer;

    //    // todo: bind peer's OnXxxxx 
    //}

    //public void SetGame1Peer(xx.UvTcpPeer peer)
    //{
    //    if (loginPeer.alive)
    //    {
    //        loginPeer.Send(new PKG.Error { number = -1, text = "another game1 server connected." });
    //        loginPeer.DelayRelease(1);
    //    }
    //    loginPeer = peer;

    //    // todo: bind peer's OnXxxxx 
    //}
}
