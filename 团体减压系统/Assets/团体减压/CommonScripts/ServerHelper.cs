using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*转为UTF-8*/
public class ServerHelper : MonoBehaviour
{
    public string signalRUrl= "http://localhost:5000/chatHub";

    public Button EnterRoomBtn;
    public Button SendMsgBtn;
    public Button ExitRoomBtn;

    public string MethodName;

    public Text ReceiveMsgText;





}
