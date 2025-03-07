using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using XCharts.Runtime;

public class TestScript : MonoBehaviour
{
    public string signalRHubURL = "http://localhost:5000/mainhub";

    public string hubMethodAll = "SendPayloadAll";
    public string hubMethodCaller = "SendPayloadCaller";

    public string messageToSendAll = "Hello World All";
    public string messageToSendCaller = "Hello World Caller";

    public string statusText = "Awaiting Connection...";
    public string connectedText = "Connection Started";
    public string disconnectedText = "Connection Disconnected";

    private const string HANDLER_ALL = "ReceivePayloadAll";
    private const string HANDLER_CALLER = "ReceivePayloadCaller";

    private Text uiText;
    private string currentText = "";

    private SignalR signalR;
    void Start()
    {
        uiText = GetComponentInChildren<Text>();

        DisplayMessage(statusText);

        signalR = new SignalR();
        signalR.Init(signalRHubURL);

        signalR.On(HANDLER_ALL, (string payload) =>
        {
            var json = JsonUtility.FromJson<JsonPayload>(payload);
            DisplayMessage($"{HANDLER_ALL}: {json.message}");
        });
        signalR.On(HANDLER_CALLER, (string payload) =>
        {
            var json = JsonUtility.FromJson<JsonPayload>(payload);
            DisplayMessage($"{HANDLER_CALLER}: {json.message}");
        });

        signalR.ConnectionStarted += (object sender, ConnectionEventArgs e) =>
        {
            Debug.Log($"Connected: {e.ConnectionId}");
            DisplayMessage(connectedText);

            var json1 = new JsonPayload
            {
                message = messageToSendAll
            };
            signalR.Invoke(hubMethodAll, JsonUtility.ToJson(json1));
            //var json2 = new JsonPayload
            //{
            //    message = messageToSendCaller
            //};
            //signalR.Invoke(hubMethodCaller, JsonUtility.ToJson(json2));
        };
        signalR.ConnectionClosed += (object sender, ConnectionEventArgs e) =>
        {
            Debug.Log($"Disconnected: {e.ConnectionId}");
            DisplayMessage(disconnectedText);
        };

        signalR.Connect();
    }

    void Update()
    {
        if (uiText.text != currentText)
        {
            //StartCoroutine(RebuildLayout());
            currentText = uiText.text;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            var json1 = new JsonPayload1
            {
                message = "nihao",
                userID="123"
            };
            signalR.Invoke(hubMethodAll, JsonUtility.ToJson(json1));
        }
    }

    void DisplayMessage(string message)
    {
        uiText.text += $"{message}\n";
        //var text = Instantiate(uiText.gameObject, this.transform).GetComponent<Text>();
        //text.GetComponent<Text>().text = message;
    }

    IEnumerator RebuildLayout()
    {
        yield return null;

        LayoutRebuilder.MarkLayoutForRebuild(uiText.rectTransform);
    }

    [Serializable]
    public class JsonPayload
    {
        public string message;
    }

    [Serializable]
    public class JsonPayload1
    {
        public string message;
        public string userID;
    }
}
