using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xx;


public static class Program
{
    static void Main(string[] args)
    {
        // 初始化 PKG 模板生成物代码 序列化的 id:type 映射
        PKG.AllTypes.Register();

        // 创建 libuv 运行核心循环
        var loop = new UvLoop();

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
public class LoginService : UvLoop
{
    // 监听器
    public UvTcpListener listener;

    // 连接器 for db server
    public UvTcpClient dbClient;

    // 计时器 for 连接器 断线重连
    public UvTimer timer;

    public LoginService(UvLoop loop)
    {
        listener = new UvTcpListener(loop);
        listener.Bind("0.0.0.0", 10001);
        listener.Listen();
        listener.OnAccept = OnAccept;

        dbClient = new UvTcpClient(loop);
        dbClient.SetAddress("127.0.0.1", 10000);
        dbClient.OnConnect = OnDbClientConnect;

        // 延迟 500ms 后每 500ms 触发一次
        timer = new UvTimer(loop, 500, 500, OnTimerFire);
    }

    public void OnTimerFire()
    {
        // 如果已断线就重连
        if (dbClient.state == UvTcpStates.Disconnected)
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

    public void OnAccept(UvTcpPeer peer)
    {
        peer.OnReceivePackage = (bbRecv) =>
        {
            // todo: 放置解析 bb 内容的代码, 协议转换

            // 模拟收到了校验包
            var recv = new PKG.Client_Login.Auth { username = "a", password = "b" };

            if (!dbClient.alive)
            {
                // todo: 用原先的协议发回 错误提示
                //peer.SendBytes()
            }
            else
            {
                // 向 db 服务发起问询
                dbClient.SendRequestEx(new PKG.Login_DB.Auth { username = recv.username, password = recv.password }, (serial, pkg) =>
                {
                    // todo: 原先的协议的回发数据容器
                    // byte[] rtv

                    // rpc 超时
                    if (pkg == null)
                    {
                        // todo: 构造回发数据: rpc 超时
                    }
                    else
                    {
                        switch (pkg)
                        {
                            case PKG.Generic.Error o:
                                // todo: 构造回发数据: o.number & text
                                break;
                            case PKG.DB_Login.Auth_Success o:
                                
                                // todo: 通过 lobbyClient 继续 SendRequestEx( PKG.Login_Lobby.Enter ). 在收到 Lobby 返回的 EnterSuccess 之后 通过 peer 发送 lobby 生成的 token

                                break;
                            default:
                                // todo: 构造回发数据: 未处理的包 pkg.ToString
                                break;
                        }
                    }
                    // 判断与客户端的连接是否依然有效
                    if (peer.alive)
                    {
                        // todo: 用原先的协议发回 rtv 包
                        //peer.SendBytes(rtv)
                    }
                });
            }
        };
    }
}
