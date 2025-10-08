using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class SendComPort : MonoBehaviour
{

    private Queue<int> _queue = new Queue<int>();
    private SerialPort mySerialPort;
    private bool _isSend;
    private string _read;

    public void Init()
    {
        mySerialPort = new SerialPort();
        _isSend = true;
        CreatPort();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (_queue.Count > 0 && _isSend)
        {
            MySendMessage(_queue.Dequeue());
        }
    }

    private void StartProject()
    {
        CreatPort();
        OffAll();
    }

    private void OffAll()
    {
        MySendMessage(160);
    }

    private void OnDestroy()
    {
        mySerialPort.Close();
    }

    private void CreatPort() //Открываем порт
    {
        mySerialPort.PortName = CheckLastPorts();//"COM18"; //CheckPorts(); //Устанавливаем номер порта, который будем открывать.  "COM4"; //
        mySerialPort.BaudRate = 9600;
        mySerialPort.Parity = Parity.None;
        mySerialPort.StopBits = StopBits.One;
        mySerialPort.DataBits = 8;
        mySerialPort.Handshake = Handshake.None;
        mySerialPort.RtsEnable = true;
        mySerialPort.ReadBufferSize = 100;
        //mySerialPort.DataReceived += new (SerialPort_DataReceived);
        try
        {
            mySerialPort.Open(); //Открываем порт
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        Debug.Log("Connected");
    }

    private string CheckLastPorts() //Проверяем есть ли COM-порт с подключённым устройством.
    {
        string[] ports = SerialPort.GetPortNames();

        Debug.Log(ports.Length);

        if (ports[ports.Length - 1].Contains("COM"))
        {
            return ports[ports.Length - 1]; //Берём первый существующий порт
        }
        else
        {
            return "COM1";
        }

        foreach(string port in ports)
        {
            Debug.Log(port);
            if (port.Contains("COM"))
            {
                return port; //Берём первый существующий порт
            }
        }
        return "COM1";
    }
    
    private string CheckPorts() //Проверяем есть ли COM-порт с подключённым устройством.
    {
        string[] ports = SerialPort.GetPortNames();

        Debug.Log(ports.Length);
        
        foreach(string port in ports)
        {
            Debug.Log(port);
            if (port.Contains("COM"))
            {
                return port; //Берём первый существующий порт
            }
        }
        return "COM1";
    }

    private async Task MySendMessage(int number)
    {
        _isSend = false;
       Debug.Log(number);
        try
        {
            //byte[] data = new byte[0];
            //data[0] = str;
            byte[] data = BitConverter.GetBytes(number);
            mySerialPort.Write(data, 0, 1);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            //throw;
        }
        
        _isSend = true;
        //mySerialPort.DiscardInBuffer();
        //mySerialPort.DiscardOutBuffer();
    }

    public void AddMessage(int message)
    {
        _queue.Enqueue(message);
        Debug.Log(_queue.Count + " "  + _isSend);
    }

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        Debug.Log("Resiver " + e.EventType);
        string receivedData = mySerialPort.ReadLine();
        //Text.text += "->" + receivedData + "\r\n";
        Debug.Log("Received from Arduino: " + receivedData);
    }

}
