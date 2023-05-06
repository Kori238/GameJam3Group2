using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour        //https://stackoverflow.com/questions/67704820/how-do-i-print-unitys-debug-log-to-the-screen-gui
{
    uint qsize = 15;  // number of messages to keep
    Queue myLogQueue = new Queue();

    public static DebugMode Instance { get; private set; }  //https://gamedev.stackexchange.com/questions/186437/how-can-i-access-a-variable-on-a-script-in-another-scene-that-i-made-it-dontdest

    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Debug.Log("Started up logging.");
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLogQueue.Enqueue("[" + type + "] : " + logString);
        if (type == LogType.Exception)
            myLogQueue.Enqueue(stackTrace);
        while (myLogQueue.Count > qsize)
            myLogQueue.Dequeue();
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 400, 0, 400, Screen.height));
        GUILayout.Label("\n" + string.Join("\n", myLogQueue.ToArray()));
        GUILayout.EndArea();
    }
}

