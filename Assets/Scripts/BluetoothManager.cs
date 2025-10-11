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

    public TMP_Text Log;
    public Button b_SearchDevices;
    public Button b_Connect;
    private Stream _stream;

    private BluetoothClient bluetoothClient;
    private IReadOnlyCollection<BluetoothDeviceInfo> devices;

    private Queue<int> _queue = new Queue<int>();
    private bool _isSend;

    public void Init()
    {
        _isSend = true;
        //bluetoothClient = new BluetoothClient();
        devices = new List<BluetoothDeviceInfo>();
        b_SearchDevices.onClick.AddListener(SearchDevices);
        b_Connect.onClick.AddListener(OnConnect);
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
        //devices.Clear();
        // Получаем список устройств
        devices = bluetoothClient.DiscoverDevices();
        //devices.AddRange(array);
        
        // Выводим информацию о найденных устройствах
        foreach (var device in devices)
        {
            
            Log.text +=
                $"Device Name: {device.DeviceName}, Address: {device.DeviceAddress}, Connected: {device.Connected}" +
                "\r\n";
            Debug.Log($"Device Name: {device.DeviceName}, Address: {device.DeviceAddress}, Connected: {device.Connected}");
        }
    }

    private void OnConnect()
    {
        ConnectToDevice(null);
        foreach (var device in devices)
        {
            if (device.DeviceName == "TESTER")
            {
                ConnectToDevice(device.DeviceAddress);
            }
        }
        
    }

    // Подключение к устройству по адресу
    public void ConnectToDevice(BluetoothAddress address)
    {
        BluetoothAddress ba = new BluetoothAddress(0x00211301E35D);
        if(_stream!=null)
            _stream.Close();
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
            Debug.Log("Connected successfully!");
        }
        catch (Exception ex)
        {
            Log.text += "Connection failed: "+ ex.Message + "\r\n";
            Debug.LogError($"Connection failed: {ex.Message}");
        }
    }

    private void BluetootheClientConnectCallback(IAsyncResult result)
    {
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

    // Чтение данных
    public string ReceiveData()
    {
        if (bluetoothClient.Connected)
        {
            var stream = bluetoothClient.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            return System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }
        return null;
    }

    void OnDestroy()
    {
        bluetoothClient?.Close();
    }
    
    private async Task MySendMessage(int number)
    {
        _isSend = false;
        Debug.Log(number);
        if (bluetoothClient.Connected)
        {
            try
            {
                byte[] data = BitConverter.GetBytes(number);
                //mySerialPort.Write(data, 0, 1);
                _stream = bluetoothClient.GetStream();
                _stream.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                Log.text += e + "\r\n";
                OnConnect();
                Debug.Log(e);
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
        Log.text += "Focus: " + hasFocus + "\r\n";
    }
    
}
