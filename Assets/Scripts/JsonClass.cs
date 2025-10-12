using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonClass : MonoBehaviour
{
    
    private string _nameConfig = "//config.txt";
    
    public JsonData jsonData;

    private void Start()
    {
        //CreateJson();
    }

    public void Init()
    {
        try
        {
            string text = File.ReadAllText(Directory.GetCurrentDirectory()+_nameConfig);
            jsonData = new JsonData();
            jsonData = JsonUtility.FromJson<JsonData>(text);
        }
        catch (FileNotFoundException)
        {
            Debug.Log("Файл не найден!");
        }
    }
    
    private void CreateJson()
    {
        JsonData script = new JsonData();
        script.delaySound = 1f;
        script.AddressBluetooth = "00211301E35D";
        script.workday.Add(new Commands(0, 16, 1));
        script.workday.Add(new Commands(0.0345f, 4, 1));
        script.workday.Add(new Commands(0.078f, 5, 1));
        script.workday.Add(new Commands(0.078f , 13, 1));
        script.workday.Add(new Commands(0.078f, 14, 1));
        script.workday.Add(new Commands(0.1183f, 4, 0));
        script.workday.Add(new Commands(0.1156f, 12, 1));
        script.workday.Add(new Commands(0.142f, 1, 1));
        script.workday.Add(new Commands(0.145f, 6, 1));
        script.workday.Add(new Commands(0.15f, 9, 1));
        script.workday.Add(new Commands(0.15f, 2, 1));
        script.workday.Add(new Commands(0.1858f, 3, 1));
        script.workday.Add(new Commands(0.1858f, 7, 1));
        script.workday.Add(new Commands(0.1858f, 8, 1));
        script.workday.Add(new Commands(0.21765f, 10, 1));
        script.workday.Add(new Commands(0.21765f, 11, 1));
        
        script.workday.Add(new Commands(1f, 2, 0));
        script.workday.Add(new Commands(1f, 3, 0));
        script.workday.Add(new Commands(1f, 7, 0));
        script.workday.Add(new Commands(1f, 8, 0));
        script.workday.Add(new Commands(1f, 10, 0));
        script.workday.Add(new Commands(1f, 11, 0));
        
        script.workday.Add(new Commands(1.0346f, 2, 1));
        script.workday.Add(new Commands(1.0346f, 3, 1));
        script.workday.Add(new Commands(1.0698f, 7, 1));
        script.workday.Add(new Commands(1.0698f, 8, 1));
        script.workday.Add(new Commands(1.1023f, 10, 1));
        script.workday.Add(new Commands(1.1023f, 11, 1));
        
        script.workday.Add(new Commands(1.3412f, 2, 0));
        script.workday.Add(new Commands(1.3412f, 3, 0));
        script.workday.Add(new Commands(1.3412f, 7, 0));
        script.workday.Add(new Commands(1.3412f, 8, 0));
        script.workday.Add(new Commands(1.3412f, 10, 0));
        script.workday.Add(new Commands(1.3412f, 11, 0));
        
        script.workday.Add(new Commands(1.3615f, 1, 0));
        script.workday.Add(new Commands(1.3615f, 6, 0));
        script.workday.Add(new Commands(1.3615f, 9, 0));
        script.workday.Add(new Commands(1.3781f, 5, 0));
        script.workday.Add(new Commands(1.3781f, 14, 0));
        script.workday.Add(new Commands(1.4222f, 12, 0));
        
        script.workday.Add(new Commands(1.487f, 4, 1));
        script.workday.Add(new Commands(1.585f, 4, 0));
        
        script.workday.Add(new Commands(2.01f, 16, 0));
        
        script.common.Add(new Commands(0f, 15, 1));
        script.common.Add(new Commands(1.15f, 15, 0));
       
        
        string json = JsonUtility.ToJson(script);
        
        using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + _nameConfig))
        {
            writer.Write(json);
        }
    }

    
}

[Serializable]
public class JsonData
{
    public string AddressBluetooth;
    public float delaySound;
    public List<Commands> workday = new List<Commands>();
    public List<Commands> common = new List<Commands>();
}

[Serializable]
public class Commands
{
    public float time;
    public int pin;
    public int activate;

    public Commands(float time, int pin, int activate)
    {
        this.time = time;
        this.pin = pin;
        this.activate = activate;
    }
}


