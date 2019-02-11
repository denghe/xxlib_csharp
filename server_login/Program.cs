using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Program
{
    static void Main(string[] args)
    {
        // 初始化 PKG 模板生成物代码 序列化的 id:type 映射
        PKG.AllTypes.Register();

        // 创建 libuv 运行核心循环
        var loop = new xx.UvLoop();

        // 初始化 rpc 管理器, 设定超时参数: 精度(ms), 默认超时 ticks( duration = 精度ms * ticks )
        loop.InitRpcManager(1000, 5);

        // 初始化 peer活动 超时管理器. 精度ms, 计时 ticks 最大值, 默认 ticks ( duration = 精度ms * ticks )
        loop.InitTimeoutManager(1000, 30, 5);

        // 创建登陆服务实例
        var loginService = new LoginService(loop);

        // 开始运行
        loop.Run();
    }
}

// 登陆服务主体
public class LoginService
{
    // 监听器
    public xx.UvTcpListener listener;

    // 连接器 for db server
    public xx.UvTcpClient dbClient;

    // 计时器 for 连接器 断线重连
    public xx.UvTimer timer;

    public LoginService(xx.UvLoop loop)
    {
        listener = new xx.UvTcpListener(loop);
        listener.Bind("0.0.0.0", 10001);
        listener.Listen();
        listener.OnAccept = OnAccept;

        dbClient = new xx.UvTcpClient(loop);
        dbClient.SetAddress("127.0.0.1", 10000);
        dbClient.OnConnect = OnDbClientConnect;

        // 延迟 500ms 后每 500ms 触发一次
        timer = new xx.UvTimer(loop, 500, 500, OnTimerFire);
    }

    public void OnTimerFire()
    {
        // 如果已断线就重连
        if (dbClient.state == xx.UvTcpStates.Disconnected)
        {
            dbClient.Connect();
        }
    }

    public void OnDbClientConnect(int status)
    {
        // 没连上就退出
        if (status != 0) return;

        // 连上后先发送身份认证包
        dbClient.Send(new PKG.Generic.ServerInfo { name = "login" });
    }

    public void OnAccept(xx.UvTcpPeer peer)
    {
        peer.OnReceiveRequest = (serial, bbRecv) =>
        {
            // 试着解码. 失败直接断开
            var recv = bbRecv.TryReadRoot<xx.IObject>();
            if (recv == null)
            {
                peer.Dispose();
                return;
            }

            // 分发到处理函数
            switch (recv)
            {
                case PKG.Client_Login.Auth a:
                    Handle_Auth(serial, a, peer);
                    break;
                default:
                    peer.Dispose();
                    return;
            }

        };
    }

    public void Handle_Auth(uint serial, PKG.Client_Login.Auth a, xx.UvTcpPeer peer)
    {
        // 如果还没有连上
        if (!dbClient.alive)
        {
            peer.SendResponse(serial, new PKG.Generic.Error { number = -1, text = "dbClient disconnected." });
            return;
        }

        // 向 db 服务发起问询
        dbClient.SendRequestEx(new PKG.Login_DB.Auth { username = a.username, password = a.password }, recv =>
        {
            // 如果等到 db 返回结果到达时 peer 已经断开, 则后续都不用继续做了
            if (!peer.alive) return;

            // rpc 超时
            if (recv == null)
            {
                peer.SendResponse(serial, new PKG.Generic.Error { number = -2, text = "dbClient Auth timeout." });
            }
            else
            {
                switch (recv)
                {
                    case PKG.Generic.Error o:
                        peer.SendResponse(serial, new PKG.Generic.Error { number = -3, text = "dbClient Auth Result: " + o.ToString() });
                        break;
                    case PKG.DB_Login.Auth_Success o:
                        // todo: 通过 lobbyClient 继续 SendRequestEx( PKG.Login_Lobby.Enter ). 在收到 Lobby 返回的 EnterSuccess 之后 通过 peer 发送 lobby 生成的 token
                        peer.SendResponse(serial, new PKG.Generic.Success { });
                        break;
                    default:
                        peer.SendResponse(serial, new PKG.Generic.Error { number = -3, text = "dbClient Auth Result: unhandled package " + recv.ToString() });
                        break;
                }
            }
        });
    }
}
