using UnityEngine;
using System.IO;
using System;

public class Logger {
    private string _path = Application.persistentDataPath + "/logs.txt";
    
    public Logger() {
//#if UNITY_EDITOR
//        Debug.Log(_path);
//#elif UNITY_WEBGL
//        Console.WriteLine("Unity WebGL");
//#endif
    }

    public void Log(string message) {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            Console.WriteLine(message);
        else
            WriteLogToFile(message);
    }

    private void WriteLogToFile(string message) {
#if UNITY_EDITOR
        Debug.Log(message);
#endif

        using (var writer = new StreamWriter(_path, true)) {
            writer.WriteLine(message);
            writer.Close();
        }
    }
}

