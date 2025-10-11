using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth.AttributeIds;

public class BluetoothManager : MonoBehaviour
{
    private BluetoothClient bluetoothClient;
    private List<BluetoothDeviceInfo> devices;

    private Queue<int> _queue = new Queue<int>();
    private bool _isSend;

    public void Init()
    {
        _isSend = false;
        //bluetoothClient = new BluetoothClient();
        //devices = new List<BluetoothDeviceInfo>();
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
        devices.Clear();
        // Получаем список устройств
        BluetoothDeviceInfo[] array = bluetoothClient.DiscoverDevices();
        devices.AddRange(array);

        // Выводим информацию о найденных устройствах
        foreach (var device in devices)
        {
            Debug.Log($"Device Name: {device.DeviceName}, Address: {device.DeviceAddress}, Connected: {device.Connected}");
        }
    }

    // Подключение к устройству по адресу
    public void ConnectToDevice(BluetoothAddress address)
    {
        try
        {
            BluetoothEndPoint ep = new BluetoothEndPoint(address, BluetoothService.SerialPort);
            bluetoothClient.Connect(ep);
            Debug.Log("Connected successfully!");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Connection failed: {ex.Message}");
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
        try
        {
            byte[] data = BitConverter.GetBytes(number);
            //mySerialPort.Write(data, 0, 1);
            //TODO Отправка на блютуз
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        
        _isSend = true;
    }

    public void AddMessage(int message)
    {
        _queue.Enqueue(message);
        Debug.Log(_queue.Count + " "  + _isSend);
    }
}
