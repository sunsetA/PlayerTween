using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityWebSocket;

[Serializable]
public enum TrainType
{
    //对抗
    Competitive,
    //合作
    Collaborate,
}
/*转为UTF-8*/
public class UserStateDetect : MonoBehaviour
{
    public static UserStateDetect Instance;

    public TrainType trainType = TrainType.Collaborate;
    public UserStateDetect()
    {
        Instance = this;
    }




    /// <summary>
    /// 团体合作脑波数据变化事件
    /// </summary>
    public event Action<float> OnBrainWaveValueChangeEvent;

    /// <summary>
    /// 团体竞赛脑波数据变化事件
    /// </summary>
    public event Action<List<UserData> > OnCompetitiveBrainWaveValueChangeEvent;


    /// <summary>
    /// 团体合作呼吸数据变化事件
    /// </summary>
    public event Action<float> OnBreathValueChangeEvent;

    /// <summary>
    /// 团体竞赛呼吸数据变化事件
    /// </summary>
    public event Action<List<UserData>> OnCompetitiveBreathValueChangeEvent;

    /// <summary>
    /// 团体合作心率变化事件
    /// </summary>
    public event Action<float> OnHeartValueChangeEvent;

    /// <summary>
    /// 团体竞赛心率数据变化事件
    /// </summary>
    public event Action<List<UserData>> OnCompetitiveHeartValueChangeEvent;

    /// <summary>
    /// 用户数量改变事件
    /// </summary>
    public event Action<List<UserData>, System.Collections.Specialized.NotifyCollectionChangedEventArgs> OnUserLenghChangeEvent;

    /// <summary>
    /// 游戏开始事件
    /// </summary>
    public event Action OnGameStartEvent;

    public string address = "wss://echo.websocket.events";
    WebSocket socket;
    public WebSocket Socket
    {
        get { return socket; }
    }
    private UserData userInfo;


    public ObservableCollection<UserData> users = new ObservableCollection<UserData>();

    private void Awake()
    {
        users.CollectionChanged += Users_CollectionChanged;
    }

    private void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnUserLenghChangeEvent?.Invoke(((ObservableCollection<UserData>)sender).ToList(),e);
    }

    private void Start()
    {
        // 创建实例
        socket = new WebSocket(address);

        socket.OnOpen += Socket_OnOpen;
        socket.OnClose += Socket_OnClose;
        socket.OnMessage += Socket_OnMessage;
        socket.OnError += Socket_OnError;

        socket.ConnectAsync();

    }


    private void Socket_OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError(e.ToString());
    }

    private void Socket_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("Receive Message:"+e.Data);

        //TODO:对广播的数据进行拆分，分为两类
        //1.广播有人上/下线
        //2.广播更新心率、脑波等数据

        //UserData LoginData = e.Data;
        UserData LoginData = new UserData(2); ;

        //1.如果是上线消息
        if (userInfo!=null)
        {
            if (!users.Contains(LoginData))
            {
                users.Add(LoginData);
            }
        }
        else
        {
            userInfo = LoginData;
            users.Add(LoginData);
            //此处没有将userinfo加入到list中，因为下一帧会更新所有用户数据
        }
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
            OnHeartValueChangeEvent?.Invoke(20);
            OnBrainWaveValueChangeEvent?.Invoke(20);
            OnBreathValueChangeEvent?.Invoke(20);
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

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            OnBrainWaveValueChangeEvent?.Invoke(19);
            OnBreathValueChangeEvent?.Invoke(19);
            OnHeartValueChangeEvent?.Invoke(19); 
        }
        else if (Input.GetKey(KeyCode.W))
        {
            OnBrainWaveValueChangeEvent?.Invoke(34);
            OnBreathValueChangeEvent?.Invoke(34);
            OnHeartValueChangeEvent?.Invoke(34);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            OnBrainWaveValueChangeEvent?.Invoke(90);
            OnBreathValueChangeEvent?.Invoke(90);
            OnHeartValueChangeEvent?.Invoke(90);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            socket.SendAsync("delay");
        }


        if (Input.GetKeyDown(KeyCode.F1))
        {
            users.Add(new UserData(1));
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            var user = users.FirstOrDefault(item=>item.userID==1);
            if (user != null) 
            {
                users.Remove(user);
            }
        }


        if (Input.GetKeyDown(KeyCode.F3))
        {
            users.Add(new UserData(3));
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            var user = users.FirstOrDefault(item => item.userID == 3);
            if (user != null)
            {
                users.Remove(user);
            }
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            users.Add(new UserData(4));
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            var user = users.FirstOrDefault(item => item.userID == 4);
            if (user != null)
            {
                users.Remove(user);
            }
        }


        if (Input.GetKeyDown(KeyCode.F7))
        {
            users.Add(new UserData(5));
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            var user = users.FirstOrDefault(item => item.userID == 5);
            if (user != null)
            {
                users.Remove(user);
            }
        }






        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnGameStartEvent?.Invoke();
        }
    }

    public class UserData
    {
        public int userID;


        public UserData(int id)
        {
            userID = id;
        }
    }

}
