using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Timeline;
using Newtonsoft.Json;

public class ServerHelperDemo : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern string StringReturnHostFunction();  //获取域名和端口号



    public string signalRHubURL = "http://192.168.30.10:22240/hubs/chat?uid=012";



    private SignalR signalR;

    private Dictionary<string, ChatRoomData> onlineUsers = new Dictionary<string, ChatRoomData>();
    // 配置信息
    private Dictionary<string, string> config = new Dictionary<string, string>();


    public Button EnterHomeBtn;
    public Button SendMsgBtn;
    public Button ExitHomeBtn;


    public string serverUrl = "192.168.30.10:22240";
    public string userName = "hf";
    public string userId = "1";
    public string gameName= "GroupTrain";

    /// <summary>
    /// 用户数量变化事件
    /// </summary>
    public event Action<ChatRoomData> OnUserLengthChanged;
    void Start()
    {
#if UNITY_EDITOR
        //编辑器下的实例化
        config.Add("server", serverUrl);
        config.Add("userName", userName);
        config.Add("userId", userId);
        config.Add("gameName", gameName);


#else
        InitConfig();

#endif

        signalRHubURL = $"http://{config["server"]}/hubs/chat?uid={config["userId"]}";
        EnterHomeBtn.onClick.AddListener(EnterGame);
        SendMsgBtn.onClick.AddListener(() =>
        {
            SendMsg("MyTest");
        });

        ExitHomeBtn.onClick.AddListener(LeaveHome);


        signalR = new SignalR();
        signalR.Init(signalRHubURL);


        signalR.On("PlayerOnline", (string data) =>
        {
            Debug.Log("Receive data:"+data);
            var playerData = JsonConvert.DeserializeObject<ChatRoomData>(data);

            if (!onlineUsers.ContainsKey(playerData.userId.ToString()))
            {
                //onlineUsers.Add(playerData.userId.ToString(), playerData);
                OnUserChange(playerData.userId.ToString(), playerData,true);
            }
            if (playerData.userId.ToString() == config["userId"])
            {

            }
            else
            {

                var welcomePayload = new
                {
                    recipientIds = new string[] { playerData.userId.ToString() },
                    message = new
                    {
                        userId = config["userId"],
                        userName = config["userName"]
                    },
                    GameName = config["gameName"]
                };
                string welcomStr = JsonConvert.SerializeObject(welcomePayload);
                Debug.Log("welcome and send message:" + welcomStr);

                signalR.Invoke("WelcomePlayer", welcomStr);
            }

        });
        signalR.On("Thanks", (string res) =>
        {
            Debug.Log("Receive thanks wait for msg:" + res);
            var data = JsonConvert.DeserializeObject<ServerBoardData>(res);
            if (data!=null)
            {
                if (!onlineUsers.ContainsKey(data.recipientIds[0]))
                {
                    OnUserChange(data.recipientIds[0], new ChatRoomData(Convert.ToInt32(data.recipientIds[0]), data.message.userName, data.GameName),true);
                    //onlineUsers.Add(data.recipientIds[0], new ChatRoomData(Convert.ToInt32(data.recipientIds[0]) ,data.message.userName,data.GameName));
                    Debug.Log("add user to current userList:"+data.message.userName);
                }
                else
                {
                    if (!onlineUsers.ContainsKey(data.message.userId))
                    {
                        OnUserChange(data.message.userId, new ChatRoomData(Convert.ToInt32(data.message.userId), data.message.userName, data.GameName),true);
                        //onlineUsers.Add(data.message.userId, new ChatRoomData(Convert.ToInt32(data.message.userId), data.message.userName, data.GameName));
                    }
                }
            }
            else
            {
                Debug.LogError("序列化失败，检查传入字符串格式");
            }


        });


        signalR.On("PlayerMessage", (string res) =>
        {
            //msgList.Add(res);

            // 更新消息列表 UI
            Debug.Log("用户消息列表+1");
        });

        signalR.Connect();


    }
    private void InitConfig()
    {
        var totalUrl = StringReturnHostFunction();
        var originurl = totalUrl.Split('&')[0];
        var url = originurl.Split('=')[1];

        var userName = totalUrl.Split('&')[1].Split('=')[1];
        var userId = totalUrl.Split('&')[2].Split('=')[1];
        var gameName = totalUrl.Split('&')[3].Split('=')[1];

        config.Add("server", url);
        config.Add("userName", userName);
        config.Add("userId", userId);
        config.Add("gameName", gameName);
    }

    private void EnterGame()
    {
        //=====测试进入房间======
        var json1 = new ChatRoomData
        {
            userId = Convert.ToInt32(config["userId"]),
            userName = config["userName"],
            gameName = config["gameName"]
        };
        signalR.Invoke("EnterGameRoom", JsonConvert.SerializeObject(json1));

    }

    private void SendMsg()
    {
        //=====测试发送消息======
        var json1 = new MessageData();

        json1.userId = 8;
        json1.userName = "car1";
        json1.gameName = "GroupTrain";
        json1.msg = "aloha";

        signalR.Invoke("SendGameMessage", JsonConvert.SerializeObject(json1));
    }


    /// <summary>
    /// 用户数量变化事件
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="chatRoomData"></param>
    /// <param name="add"></param>
    private void OnUserChange(string userId, ChatRoomData chatRoomData,bool add) 
    {
        if (add)
        {
            onlineUsers.Add(userId,chatRoomData);
        }
        else
        {
            onlineUsers.Remove(userId);
        }
        //TODO:Add unityeventy
        OnUserLengthChanged?.Invoke(chatRoomData);
    }
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg">具体的消息</param>
    private void SendMsg(string msg)
    {
        var data = new MessageData
        {
            userId = Convert.ToInt32(config["userId"]),
            userName = config["userName"],
            gameName = config["gameName"],
            msg = msg
        };
        var Finaldata = JsonConvert.SerializeObject(data);
        signalR.Invoke("SendGameMessage", Finaldata);

    }

    private void LeaveHome()
    {
        //ChatRoomData chatRoomData = new ChatRoomData();

        var chatRoomData = new ChatRoomData
        {
            userId = Convert.ToInt32(config["userId"]),
            userName = config["userName"],
            gameName = config["gameName"]
        };
        var data = JsonConvert.SerializeObject(chatRoomData);
        signalR.Invoke("LeaveGameRoom", data);
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (var item in onlineUsers)
            {
                Debug.Log(item.Value.userName);
            }
        }
    }



    [Serializable]
    public class MessageData
    {
        public int userId;
        public string userName;
        public string gameName;
        public string msg;
    }
    [Serializable]
    public class ChatRoomData
    {
        public int userId;
        public string userName;
        public string gameName;

        public ChatRoomData() 
        {

        }
        public ChatRoomData(int userId, string userName, string gameName)
        {
            this.userId = userId;
            this.userName = userName;
            this.gameName = gameName;
        }
    }
    [Serializable]
    // 玩家数据类
    public class PlayerData
    {
        public string userId;
        public string userName;
    }


    public class ServerBoardData
    {
        public string[] recipientIds;
        public PlayerData message;
        public string GameName;
    }

}
