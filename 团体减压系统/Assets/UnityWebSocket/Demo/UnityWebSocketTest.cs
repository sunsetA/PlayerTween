using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityWebSocket;
/*转为UTF-8*/
public class UnityWebSocketTest : MonoBehaviour
{
    public string address = "wss://echo.websocket.events";
    public string sendText = "hello";

    public Button ConnectBtn;

    public Button SendBtn;

    public Text ContentText;

    private IWebSocket socket;


    void Start()
    {
        ContentText.text = "no connect";
        ConnectBtn.onClick.AddListener(() => 
        {
            socket = new WebSocket(address);
            socket.OnOpen += Socket_OnOpen;
            socket.OnMessage += Socket_OnMessage;
            socket.OnClose += Socket_OnClose;
            socket.OnError += Socket_OnError;
            socket.ConnectAsync();
        });

        SendBtn.onClick.AddListener(() =>
        {
            sendText = SendBtn.GetComponentInChildren<InputField>().text;
            socket.SendAsync(sendText);
        });

        Debug.Log(GetFormatTime(243));
    }

    private void Socket_OnError(object sender, ErrorEventArgs e)
    {
        Debug.Log("错误事件");
        ContentText.text = "Occur message:" + e.Message;
    }

    private void Socket_OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("关闭事件");
        ContentText.text = "Close message:" + e.Code;
    }

    private void Socket_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("广播事件:"+e.Data);
        ContentText.text = "Receive message:"+e.Data;
    }

    private void Socket_OnOpen(object sender, OpenEventArgs e)
    {
        Debug.Log("打开ws事件");
        ContentText.text = "Open message" ;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 根据传入的整型秒数返回格式化的时间字符串
    /// </summary>
    /// <returns>格式化后的时间字符串</returns>
    private string GetFormatTime(int seconds) 
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}时:{1:D2}分:{2:D2}秒", time.Hours, time.Minutes, time.Seconds);
    }
}
