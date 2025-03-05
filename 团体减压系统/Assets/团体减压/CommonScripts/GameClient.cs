using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.AspNetCore.SignalR.Client;
using System.Runtime.InteropServices;
using Unity.VisualScripting.FullSerializer;
using XCharts.Runtime;
using static UserStateDetect;
using Newtonsoft.Json;

public class GameClient : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern string StringReturnHostFunction();  //获取域名和端口号
    // UI 元素
    public Text messageText;
    public Text configText;
    public Text connectionStringText;
    public Button enterGameButton;
    public Button leaveGameButton;
    public Button sendMessageButton;
    public GameObject onlineUsersTable;
    public GameObject messageList;

    // 配置信息
    private Dictionary<string, string> config = new Dictionary<string, string>();
    private List<string> msgList = new List<string>();
    private Dictionary<string, PlayerData> onlineUsers = new Dictionary<string, PlayerData>();
    private Microsoft.AspNetCore.SignalR.Client.HubConnection connection;

    // 玩家数据类
    private class PlayerData
    {
        public string userId;
        public string userName;
    }

    void Start()
    {
        // 初始化 UI 按钮点击事件
        enterGameButton.onClick.AddListener(EnterGameRoom);
        leaveGameButton.onClick.AddListener(LeaveGameRoom);
        sendMessageButton.onClick.AddListener(() => SendMsg("hello word"));

        // 获取配置信息并启动 SignalR 连接
        GetConfig();
        StartSignalR();
    }

    private void GetConfig()
    {
        //var totalUrl = StringReturnHostFunction();
        //var originurl = totalUrl.Split('&')[0];
        //var url = originurl.Split('=')[1];

        //var userName = totalUrl.Split('&')[1].Split('=')[1];
        //var userId = totalUrl.Split('&')[2].Split('=')[1];
        //var gameName = totalUrl.Split('&')[3].Split('=')[1];


        //config.Add("server", url);
        //config.Add("userName", userName);
        //config.Add("userId", userId);
        //config.Add("gameName", gameName);


        var address = string.Format("ws://192.168.30.10:22240/hubs/chat?uid={0}", 012);
        config.Add("server", "192.168.30.10:22240");
        config.Add("userName", "lhf");
        config.Add("userId", "001123");
        config.Add("gameName", "GroupTrain");


        // 获取 URL 参数
        //var urlParams = UnityEngine.Networking.UnityWebRequest.UnEscapeURL(UnityEngine.Networking.UnityWebRequest.kHttpVerbGET);
        //var uri = new Uri(urlParams);
        //var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);

        //// 遍历所有的键值对，并添加到字典中
        //foreach (string key in queryParams.AllKeys)
        //{
        //    if (!config.ContainsKey(key))
        //    {
        //        //config[key] = queryParams[key];
        //        config.Add(key, queryParams[key]);
        //    }
        //    else
        //    {
        //        config[key] = queryParams[key];
        //    }
        //}



        // 更新 UI 显示
        configText.text = "从 url 获取的参数: " + string.Join(", ", config);
        connectionStringText.text = "websocket 连接字符串： " + config["server"] + "/hubs/chat?uid=" + config["userId"];
    }

    private void SendMsg(string msg)
    {
        // 构造消息负载
        var payload = new
        {
            userId = config["userId"],
            userName = config["userName"],
            gameName = config["gameName"],
            msg
        };

        // 发送消息
        connection.InvokeAsync("SendGameMessage", System.Text.Json.JsonSerializer.Serialize(payload));
    }

    private void EnterGameRoom()
    {
        ChatRoomData chatRoomData=new ChatRoomData();
        // 构造进入游戏房间的负载
        var payload = new
        {
            userId = config["userId"],
            userName = config["userName"],
            gameName = config["gameName"]
        };
        chatRoomData.userId = Convert.ToInt32(payload.userId) ;
        chatRoomData.userName = payload.userName;
        chatRoomData.gameName = payload.gameName;

        // 发送进入游戏房间的请求
        connection.InvokeAsync("EnterGameRoom", JsonConvert.SerializeObject(chatRoomData));
        messageText.text = "enter gameRoom:"+ JsonConvert.SerializeObject(chatRoomData);
    }

    private void LeaveGameRoom()
    {
        // 构造离开游戏房间的负载
        var payload = new
        {
            userId = config["userId"],
            userName = config["userName"],
            gameName = config["gameName"]
        };

        // 发送离开游戏房间的请求
        connection.InvokeAsync("LeaveGameRoom", System.Text.Json.JsonSerializer.Serialize(payload));
    }

    private void StartSignalR()
    {
        string url = "http://" + config["server"] + "/hubs/chat?uid=" + config["userId"];
        // 初始化 SignalR 连接

        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            return;  
        }
        messageText.text = "SignalR connecting..." + url;

        connection = new Microsoft.AspNetCore.SignalR.Client.HubConnectionBuilder()
        //.WithUrl(url, options =>
        //{
        //    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
        //    options.SkipNegotiation = true;
        //})
       .WithUrl(url)
       .WithAutomaticReconnect()
       .Build();

        // 设置心跳检测和超时时间
        connection.KeepAliveInterval = TimeSpan.FromSeconds(15);
        connection.ServerTimeout = TimeSpan.FromMinutes(30);

        // 启动连接
        connection.StartAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("SignalR 连接失败: " + task.Exception.Message);
                messageText.text = "SignalR failed: " + task.Exception.Message;
            }
            else
            {
                Debug.Log("SignalR 连接成功");
                messageText.text = "SignalR succeed";
            }
        });

        // 注册事件处理程序
        connection.On<string>("PlayerOnline", (res) =>
        {
            var data = System.Text.Json.JsonSerializer.Deserialize<PlayerData>(res);
            onlineUsers[data.userId] = data;

            // 发送欢迎消息
            var welcomePayload = new
            {
                recipientIds = new string[] { data.userId },
                message = new
                {
                    userId = config["userId"],
                    userName = config["userName"]
                }
            };

            connection.InvokeAsync("Welcome", System.Text.Json.JsonSerializer.Serialize(welcomePayload));

            // 更新在线用户列表 UI
            UpdateOnlineUsersTable();
        });

        connection.On<string>("PlayerOffline", (res) =>
        {
            var data = System.Text.Json.JsonSerializer.Deserialize<PlayerData>(res);
            Debug.Log("Player offline: " + data.userId);
            onlineUsers.Remove(data.userId);

            // 更新在线用户列表 UI
            UpdateOnlineUsersTable();
        });

        connection.On<string>("PlayerMessage", (res) =>
        {
            msgList.Add(res);

            // 更新消息列表 UI
            UpdateMessageList();
        });

        connection.On<string>("Thanks", (res) =>
        {
            var data = System.Text.Json.JsonSerializer.Deserialize<PlayerData>(res);
            onlineUsers[data.userId] = data;

            // 更新在线用户列表 UI
            UpdateOnlineUsersTable();
        });
    }

    private void UpdateOnlineUsersTable()
    {
        // 清空在线用户列表 UI
        foreach (Transform child in onlineUsersTable.transform)
        {
            Destroy(child.gameObject);
        }

        // 重新填充在线用户列表 UI
        foreach (var user in onlineUsers.Values)
        {
            var row = new GameObject("UserRow");
            row.transform.SetParent(onlineUsersTable.transform);

            var userIdText = new GameObject("UserIdText");
            userIdText.transform.SetParent(row.transform);
            userIdText.AddComponent<Text>().text = user.userId;

            var userNameText = new GameObject("UserNameText");
            userNameText.transform.SetParent(row.transform);
            userNameText.AddComponent<Text>().text = user.userName;
        }
    }

    private void UpdateMessageList()
    {
        // 清空消息列表 UI
        foreach (Transform child in messageList.transform)
        {
            Destroy(child.gameObject);
        }

        // 重新填充消息列表 UI
        foreach (var msg in msgList)
        {
            var msgObject = new GameObject("Message");
            msgObject.transform.SetParent(messageList.transform);
            msgObject.AddComponent<Text>().text = msg;
        }
    }
}