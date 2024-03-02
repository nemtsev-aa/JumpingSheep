using UnityEngine;
using System.IO;

public class Logger {
    public Logger() {
        
    }

    public void Log(string message) {
        Debug.Log(message);

        StreamWriter writer = new StreamWriter(GetLogsPath(), true);
        writer.WriteLine(message);
        writer.Close();
    }

    string GetLogsPath() {
        return Application.persistentDataPath + "/logs.txt";
    }
}

