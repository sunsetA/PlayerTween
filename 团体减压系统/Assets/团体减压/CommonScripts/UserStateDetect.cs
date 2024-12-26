using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityWebSocket;

//[Serializable]
//public enum TrainType
//{
//    //对抗
//    single,
//    //合作
//    multi,
//}
/*转为UTF-8*/
public class UserStateDetect : MonoBehaviour
{
    public static UserStateDetect Instance;

    public UserStateDetect()
    {
        Instance = this;
    }


    public delegate void RelaxValueChangeHandle(float realtimeRelaxValue);

    /// <summary>
    /// 脑波数据变化事件
    /// </summary>
    public event RelaxValueChangeHandle OnBrainWaveValueChangeEvent;


    /// <summary>
    /// 呼吸数据变化事件
    /// </summary>
    public event RelaxValueChangeHandle OnBreathValueChangeEvent;


    /// <summary>
    /// 心率变化事件
    /// </summary>
    public event RelaxValueChangeHandle OnHeartValueChangeEvent;


    /// <summary>
    /// 用户数量改变事件
    /// </summary>
    public event Action<List<UserData>> OnUserLenghChangeEvent;

    public string address = "wss://echo.websocket.events";
    WebSocket socket;

    private UserData userInfo;


    ObservableCollection<UserData> users = new ObservableCollection<UserData>();

    private void Awake()
    {
        users.CollectionChanged += Users_CollectionChanged;
    }

    private void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnUserLenghChangeEvent?.Invoke(((ObservableCollection<UserData>)sender).ToList());
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
        OnHeartValueChangeEvent?.Invoke(20);
        OnBrainWaveValueChangeEvent?.Invoke(20);
        OnBreathValueChangeEvent?.Invoke(20);

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
