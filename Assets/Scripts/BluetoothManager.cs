using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using TMPro;
using UnityEngine.UI;

public class BluetoothManager : MonoBehaviour
{

    public GameObject BluPanel;
    public TMP_Text Log;
    public Transform ButtonParent;
    public GameObject ButtonPrefab;
    public Button b_Connect;
    
    private Stream _stream;

    private BluetoothClient bluetoothClient;
    private IReadOnlyCollection<BluetoothDeviceInfo> devices;
    private string _currentDevice;

    private Queue<int> _queue = new Queue<int>();
    private bool _isSend;
    private GameManager _manager;
    private JsonClass _jsonClass;

    public void Init()
    {
        _manager = GetComponent<GameManager>();
        _jsonClass = GetComponent<JsonClass>();
        _isSend = true;
        //bluetoothClient = new BluetoothClient();
        devices = new List<BluetoothDeviceInfo>();
        BluPanel.SetActive(true);
        b_Connect.onClick.AddListener(()=>OnConnect(""));
    }
    
    private void Update()
    {
        if (_queue.Count > 0 && _isSend)
        {
            MySendMessage(_queue.Dequeue());
        }
    }

    public void SearchDevices()
    {
        // Получаем список устройств
        devices = bluetoothClient.DiscoverDevices();
        //devices.AddRange(array);
        
        // Выводим информацию о найденных устройствах
        foreach (var device in devices)
        {
            Button button = Instantiate(ButtonPrefab, ButtonParent).GetComponent<Button>();
            button.GetComponentInChildren<TMP_Text>().text = device.DeviceName;
            button.onClick.AddListener(()=>OnConnect(device.DeviceName));
            //Log.text +=
            //    $"Device Name: {device.DeviceName}, Address: {device.DeviceAddress}, Connected: {device.Connected}" +
             //   "\r\n";
        }
    }

    private void OnConnect(string deviceName)
    {
        Log.text += "OnConnect" + "\r\n";
        ConnectToDevice(null);
        
        // foreach (var device in devices)
        // {
        //     if (device.DeviceName == deviceName)
        //     {
        //         Log.text += deviceName + "\r\n";
        //         _currentDevice = deviceName;
        //         ConnectToDevice(device.DeviceAddress);
        //     }
        // }
        
    }

    private byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length / 2];

        for (int i = 0; i < str.Length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
        }

        return bytes;
    }

    // Подключение к устройству по адресу
    public void ConnectToDevice(BluetoothAddress address)
    {
        ulong u = Convert.ToUInt64(_jsonClass.jsonData.AddressBluetooth, 16);
        BluetoothAddress ba = new BluetoothAddress(u); //0x00211301E35D
       
        if (_stream != null)
        {
            _stream.Close();
        }

        if (bluetoothClient != null)
        {
            bluetoothClient.Close();
            bluetoothClient.Dispose();
        }

        try
        {
            bluetoothClient = new BluetoothClient();
            BluetoothEndPoint ep = new BluetoothEndPoint(ba, BluetoothService.SerialPort);
            bluetoothClient.Connect(ep);
            
            Log.text += "Connected successfully!"+ "\r\n";
            BluPanel.SetActive(false);
            _manager.Init();
            AddMessage(160);
        }
        catch (Exception ex)
        {
            Log.text += "Connection failed: !!!!!" + "\r\n";
            //Debug.LogError($"Connection failed: {ex.Message}");
            BluPanel.SetActive(true);
            //OnConnect("");
        }
    }

    // Отправка данных
    public void SendData(string data)
    {
        if (bluetoothClient.Connected)
        {
            var stream = bluetoothClient.GetStream();
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(data);
            stream.Write(bytes, 0, bytes.Length);
        }
    }

    void OnDestroy()
    {
        bluetoothClient?.Close();
    }
    
    private async Task MySendMessage(int number)
    {
        _isSend = false;
        //Debug.Log(number);
        if (bluetoothClient.Connected)
        {
            try
            {
                byte[] data = BitConverter.GetBytes(number);
                //mySerialPort.Write(data, 0, 1);
                _stream = bluetoothClient.GetStream();
                _stream.Write(data, 0, 1);
            }
            catch (Exception e)
            {
                Log.text += "Сообщение не отправлено!!!!" + "\r\n";
                OnConnect(_currentDevice);
                //Debug.Log(e);
            }
        }
        
        _isSend = true;
    }

    public void AddMessage(int message)
    {
        _queue.Enqueue(message);
        Debug.Log(_queue.Count + " "  + _isSend);
    }
    
    void OnApplicationFocus(bool hasFocus)
    {
        Log.text = "Focus: " + hasFocus + "\r\n";
        BluPanel.SetActive(true);
        //OnConnect("");
    }
    
}
