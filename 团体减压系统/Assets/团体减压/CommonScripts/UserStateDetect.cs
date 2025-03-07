using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityWebSocket;
using LitJson;
using static UserStateDetect;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
[Serializable]
public enum TrainType
{
    //对抗
    Competitive,
    //合作
    Collaborate,
}
[Serializable]
public class MessageData1
{
    public string Type;
    public UserData Data;
}
/*转为UTF-8*/
public class UserStateDetect : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern string StringReturnHostFunction();  //获取域名和端口号
    public UserStateDetect()
    {
        Instance = this;
    }

    public static UserStateDetect Instance;

    public ServerHelper serverHelper;


    public TrainType trainType = TrainType.Collaborate;


    [Header("游戏是否已经开始")]
    private bool isPlaying;






    /// <summary>
    /// 团体合作脑波数据变化事件
    /// </summary>
    public event Action<List<UserData>> OnBrainWaveValueChangeEvent;

    /// <summary>
    /// 团体竞赛脑波数据变化事件
    /// </summary>
    public event Action<List<UserData> > OnCompetitiveBrainWaveValueChangeEvent;


    /// <summary>
    /// 团体合作呼吸数据变化事件
    /// </summary>
    public event Action<List<UserData>> OnBreathValueChangeEvent;

    /// <summary>
    /// 团体竞赛呼吸数据变化事件
    /// </summary>
    public event Action<List<UserData>> OnCompetitiveBreathValueChangeEvent;

    /// <summary>
    /// 团体合作心率变化事件
    /// </summary>
    public event Action<List<UserData>> OnHeartValueChangeEvent;

    /// <summary>
    /// 团体竞赛心率数据变化事件
    /// </summary>
    public event Action<List<UserData>> OnCompetitiveHeartValueChangeEvent;

    /// <summary>
    /// 用户数量改变事件
    /// </summary>
    public event Action<List<UserData>> OnUserLenghChangeEvent;

    /// <summary>
    /// 游戏开始事件
    /// </summary>
    public event Action OnGameStartEvent;

    /// <summary>
    /// 游戏结束事件
    /// </summary>
    public event Action OnGameEndEvent;

    public string address = "wss://echo.websocket.events";

    UnityWebSocket.WebSocket socket;

    public UnityWebSocket.WebSocket Socket
    {
        get { return socket; }
    }
    public UserData userInfo;


    public ObservableCollection<UserData> users = new ObservableCollection<UserData>();

    private Dictionary<int, UserData> userListDic = new Dictionary<int, UserData>();

    /// <summary>
    /// 是否为测试模式
    /// </summary>
    public bool isTest;


    /// <summary>
    /// 完整的URL，包含当前端口号、用户名、游戏名
    /// </summary>
    private string totalUrl;

    private string url;


    private void Awake()
    {
        //先获取用户数据
        GetUserInfo();
        //Invoke("DelayINvoke", 10);
    }

    //private void Users_CollectionChanged(object sender)
    //{
    //    Debug.Log("length changed");
    //    OnUserLenghChangeEvent?.Invoke(((ObservableCollection<UserData>)sender).ToList());
    //}
  

    private void Start()
    {
        //userInfo = new UserData(110);
        //userInfo.userID = 110;
        //userInfo.userName = "hf";
        //userInfo.gameName = "shuiyuan";

        //MessageData1 messagedata = new MessageData1
        //{
        //    Type = "EnterGameRoom",
        //    Data = userInfo
        //};
        //var content1 = JsonUtility.ToJson(messagedata);
        //Debug.Log(content1);

        // 创建实例
        //address = string.Format("ws://192.168.30.10:22240/hubs/chat?uid={0}", 920);
        address = "ws://echo.websocket.orgws://192.168.30.10:22240/hubs/chat?uid=2";
        socket = new UnityWebSocket.WebSocket(address);
        socket.OnOpen += Socket_OnOpen;
        socket.OnClose += Socket_OnClose;
        socket.OnMessage += Socket_OnMessage;
        socket.OnError += Socket_OnError;
        socket.ConnectAsync();



    }
    
    private void GetUserInfo()
    {
        //TODO:从url中解析出用户id   
        //*1.此处url是从问号开始的
        //*2.暂定第一个就是用户id
        try
        {
            //totalUrl = StringReturnHostFunction();
            //var originurl= totalUrl.Split('&')[0];
            //url= originurl.Split('=')[1];

            //var userName= totalUrl.Split('&')[1].Split('=')[1];
            //var userId=totalUrl.Split('&')[2].Split('=')[1];
            //var gameName = totalUrl.Split('&')[3].Split('=')[1];
            url = "http://192.168.30.10:22240";
            var userId = "10023";
            var userName = "lm";
            var gameName = "GroupTrain";

            userInfo = new UserData(int.Parse(userId));
            userInfo.userName = userName;
            userInfo.gameName = gameName;
            users.Add(userInfo);
            //GameObject.Find("FindText").GetComponent<Text>().text = "message is :" + totalUrl;
        }
        catch (Exception ep)
        {

            Debug.LogError("check url");
            GameObject.Find("FindText").GetComponent<Text>().text = "Get userInfo msg: :" + ep.Message;
        }

    }


    private void Socket_OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError(e.ToString());
    }

    private void Socket_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("Receive Message:"+e.Data);


        //var content = JsonConvert.DeserializeObject<MessageData1>(e.Data);

        //TODO:对广播的数据进行拆分，分为两类
        //1.广播有人上/下线
        //2.广播更新心率、脑波等数据

        //UserData LoginData = e.Data;
        //List<UserData> userDatas= JsonUtility.FromJson<List<UserData>>(e.Data);

        //UserData LoginData = new UserData(2); ;

        ////1.如果是上线消息
        //if (userInfo!=null)
        //{
        //    if (!users.Contains(LoginData))
        //    {
        //        users.Add(LoginData);
        //    }
        //}
        //else
        //{
        //    userInfo = LoginData;
        //    users.Add(LoginData);
        //    //此处没有将userinfo加入到list中，因为下一帧会更新所有用户数据
        //}
        //2.如果是下线消息
        //if (true)
        //{
        //    users.Remove(LoginData);
        //}

        //3.如果是其他消息，则广播所有心率或者其他数据
        //1)先解析
        //2)广播

        //2)

        return;



        ///如果是合作，传平均值
        if (trainType==TrainType.Collaborate)
        {

        }
        else
        {
            OnCompetitiveHeartValueChangeEvent?.Invoke(users.ToList());
            OnCompetitiveBrainWaveValueChangeEvent?.Invoke(users.ToList());
            OnCompetitiveBreathValueChangeEvent?.Invoke(users.ToList());
        }

    }

    private void Socket_OnClose(object sender, CloseEventArgs e)
    {
        Debug.LogError(e.ToString());
    }



    private void Socket_OnOpen(object sender, OpenEventArgs e)
    {
        Debug.LogFormat("sockect is OnOpen,message is :{0}",e.ToString());
    }

    private void Socket_SendEvent(string eventName,string eventData) 
    {
        try
        {
            Socket.SendAsync(eventData);
            Debug.Log("send login msg: :" + eventData) ;
        }
        catch (Exception ex)
        {
            Debug.Log("send login msg error :" + ex.Message); ;

        }


    }


    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.T))
        {
            isTest = true;

            StartCoroutine(FakeUpdate());
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Socket_SendEvent("EnterGameRoom", JsonUtility.ToJson(userInfo));
        }

    }

    private IEnumerator FakeUpdate()
    {
        while (isTest)
        {
            yield return new WaitForSeconds(1);
            List<UserData> obj = new List<UserData>();

            UserData userData = new UserData(0);
            userData.breathRate = UnityEngine.Random.Range(10, 30);
            userData.heartRate = UnityEngine.Random.Range(60, 80);
            userData.brainWave = UnityEngine.Random.Range(60, 80);


            UserData userData1 = new UserData(1);
            userData1.breathRate = UnityEngine.Random.Range(0, 40);
            userData1.heartRate = UnityEngine.Random.Range(60, 80);
            userData1.brainWave = UnityEngine.Random.Range(60, 80);


            obj.Add(userData);
            obj.Add(userData1);
            userInfo = userData;
            users= new ObservableCollection<UserData>(obj);
            isPlaying = true;
            OnGameStartEvent?.Invoke();
            OnHeartValueChangeEvent?.Invoke(obj);
            OnBrainWaveValueChangeEvent?.Invoke(obj);
            OnBreathValueChangeEvent?.Invoke(obj);
            OnCompetitiveHeartValueChangeEvent?.Invoke(obj);
            OnCompetitiveBrainWaveValueChangeEvent?.Invoke(obj);
            OnCompetitiveBreathValueChangeEvent?.Invoke(obj);

        }
    }


    /// <summary>
    /// 接收到数据时的数据解析
    /// </summary>
    private void OnMessageAnalysic(MessageEventArgs e)
    {
        //登录消息
        if (true)
        {
            var NewUser=new UserData(1);
            if (!userListDic.ContainsKey(NewUser.userID))
            {
                userListDic.Add(NewUser.userID, NewUser);
                //并响应新增事件
                //需要沟通确定是否在登录时，返回所有用户数据；或者是在下一次广播生理数据时，返回所有用户数据
                OnUserLenghChangeEvent?.Invoke(userListDic.Values.ToList());
            }
            else
            {
                Debug.LogError("尝试添加一个已缓存的用户");
            }

            /* 注意这里的数据格式
            userListDic=JsonUtility.FromJson<Dictionary<int, UserData>>(e.Data);

            OnUserLenghChangeEvent?.Invoke(userListDic.Values.ToList());
            */
            
        }
        //退出消息
        if (true)
        {
            var oldUser = new UserData(2);
            if (userListDic.ContainsKey(oldUser.userID))
            {
                userListDic.Remove(oldUser.userID);
                //并响应移除事件
                OnUserLenghChangeEvent?.Invoke(userListDic.Values.ToList());
            }
            else
            {
                Debug.LogError("尝试删除一个未缓存的用户");
            }
        }
        //广播数据
        if (true)
        {
            List<UserData> userDatas = JsonUtility.FromJson<List<UserData>>(e.Data);
            users=new ObservableCollection<UserData>(userDatas);
            //如果游戏已经开始，更新数据
            if (isPlaying)
            {
                //如果是合作，传平均值
                if (true)
                {
                    OnHeartValueChangeEvent?.Invoke(userDatas);
                    OnBrainWaveValueChangeEvent?.Invoke(userDatas);
                    OnBreathValueChangeEvent?.Invoke(userDatas);
                    //OnBreathValueChangeEvent?.Invoke((float)userDatas.Average(item=>item.brainWave));
                }
                else
                {
                    OnCompetitiveHeartValueChangeEvent?.Invoke(userDatas);
                    OnCompetitiveBrainWaveValueChangeEvent?.Invoke(userDatas);
                    OnCompetitiveBreathValueChangeEvent?.Invoke(userDatas);
                }
            }
            //如果游戏未开始，根据所有的用户数据更新电脑本地的所有用户数据
            //这样不好，但是暂时没有更好的办法
            else
            {

            }

        }

        //游戏开始事件
        if (true)
        {
            isPlaying = true;
        }







        //=============================


    }


    /// <summary>
    /// 发送消息的数据类型
    /// </summary>

    public class MessageData 
    {
        public ChatRoomData chatRoomData;
        public string msg;
    }

    public class ChatRoomData
    {
        public int userId;
        public string userName;
        public string gameName;
    }
    public class UserData
    {
        public int userID;
        public string userName;
        public string gameName;
        public string gender;
        public float heartRate;
        public float brainWave;
        public float breathRate;
        
        public UserData(int id)
        {
            userID = id;
        }
    }

}
