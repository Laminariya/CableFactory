using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject MenuPanel;
    public Button b_ShowMenu;
    public Button b_Close;
    public Color ColorButton;
    public Color StartColor;
    public Color FinishColor;

    public Button Obshiy;
    public Button WorkDay;
    
    public Button Korpus1;
    public Button Korpus5;
    public Button Korpus6;
    public Button ABK;
    public Button OBK;
    public Button Obshaga;

    public Button b_Play;
    public Button b_Pause;
    public Button b_Stop;
    public Button b_Back;
    
    public TMP_Text demoText;
    public string OffAllText;
    [HideInInspector] public SendComPort sendComPort;
    [HideInInspector] public BluetoothManager bluetoothManager;
    [HideInInspector] public JsonClass jsonClass;

    private Coroutine _coroutine;
    private bool _isDemo;
    private bool _isPlaying;
    
    void Start()
    {
        b_Close.onClick.AddListener(OnClose);
        Obshiy.onClick.AddListener(OnObshiy);
        WorkDay.onClick.AddListener(OnWorkDay);
        Korpus1.onClick.AddListener(OnKorpus1);
        Korpus5.onClick.AddListener(OnKorpus5);
        Korpus6.onClick.AddListener(OnKorpus6);
        ABK.onClick.AddListener(OnABK);
        OBK.onClick.AddListener(OnOBK);
        Obshaga.onClick.AddListener(OnObshaga);
        b_Play.onClick.AddListener(OnPlay);
        b_Stop.onClick.AddListener(OnStop);
        b_Pause.onClick.AddListener(OnPause);
        b_Back.onClick.AddListener(OnBack);
        b_ShowMenu.onClick.AddListener(OnShowMenu);
        
        _isDemo = false;
        sendComPort = GetComponent<SendComPort>();
        //sendComPort.Init();
        bluetoothManager = GetComponent<BluetoothManager>();
        bluetoothManager.Init();
        jsonClass = GetComponent<JsonClass>();
        jsonClass.Init();
        
        MenuPanel.SetActive(false);
    }

    public void Init()
    {
        Obshiy.image.color = Color.white;
        WorkDay.image.color = Color.white;
        Korpus1.image.color = Color.white;
        Korpus5.image.color = Color.white;
        Korpus6.image.color = Color.white;
        ABK.image.color = Color.white;
        OBK.image.color = Color.white;
        Obshaga.image.color = Color.white;
        b_Play.image.color = Color.white;
        b_Stop.image.color = Color.white;
        b_Pause.image.color = Color.white;
        b_Back.image.color = Color.white;
    }

    private void OnShowMenu()
    {
        MenuPanel.SetActive(true);
        Init();
    }

    IEnumerator OnDemo()
    {
        _isDemo = true;
        demoText.text = OffAllText;
        OffAllLed();
        while (_isDemo)
        {
            yield return new WaitForSeconds(1f);
            float lastTime = 0;
            foreach (var command in jsonClass.jsonData.commands)       
            {
                yield return new WaitForSeconds(GetTime(command.time-lastTime));
                lastTime = command.time;
                bluetoothManager.AddMessage(GetMessage(command));
            }
            
            for (int i = 0; i < 11; i++)
            {
                sendComPort.AddMessage(177+i);
                yield return new WaitForSeconds(2f);
            }
        }
    }

    private int GetMessage(Commands command)
    {
        if (command.pin == 15)
        {
            return 1;
        }
        if (command.pin == 16)
        {
            return 2;
        }

        if (command.activate == 1)
        {
            return 176 + command.pin;
        }
        else
        {
            return 160 + command.pin;
        }
    }

    private float GetTime(float value)
    {
        return value * 60f;
    }

    private void OffAllLed()
    {
        for (int i = 0; i < 14; i++)
        {
            sendComPort.AddMessage(161+i);
        }
    }

    private void OnClose()
    {
        Application.Quit();
    }

    private void OnObshiy()
    {
        
    }

    private void OnWorkDay()
    {
        
    }

    private void OnKorpus1()
    {
        if (Korpus1.image.color == Color.white)
        {
            Korpus1.image.color = ColorButton;
            bluetoothManager.AddMessage(177);
        }
        else
        {
            Korpus1.image.color = Color.white;
            bluetoothManager.AddMessage(161);
        }
    }

    private void OnKorpus5()
    {
        if (Korpus5.image.color == Color.white)
        {
            Korpus5.image.color = ColorButton;
            bluetoothManager.AddMessage(182);
        }
        else
        {
            Korpus5.image.color = Color.white;
            bluetoothManager.AddMessage(166);
        }
    }

    private void OnKorpus6()
    {
        if (Korpus6.image.color == Color.white)
        {
            Korpus6.image.color = ColorButton;
            bluetoothManager.AddMessage(185);
        }
        else
        {
            Korpus6.image.color = Color.white;
            bluetoothManager.AddMessage(169);
        }
    }
    
    private void OnABK()
    {
        if (ABK.image.color == Color.white)
        {
            ABK.image.color = ColorButton;
            bluetoothManager.AddMessage(188);
        }
        else
        {
            ABK.image.color = Color.white;
            bluetoothManager.AddMessage(172);
        }
    }
    private void OnOBK()
    {
        if (OBK.image.color == Color.white)
        {
            OBK.image.color = ColorButton;
            bluetoothManager.AddMessage(181);
        }
        else
        {
            OBK.image.color = Color.white;
            bluetoothManager.AddMessage(165);
        }
    }
    private void OnObshaga()
    {
        if (Obshaga.image.color == Color.white)
        {
            Obshaga.image.color = ColorButton;
            bluetoothManager.AddMessage(180);
        }
        else
        {
            Obshaga.image.color = Color.white;
            bluetoothManager.AddMessage(164);
        }
    }
    private void OnPlay()
    {
        _isPlaying = true;
        b_Play.image.color = ColorButton;
        b_Pause.image.color = Color.white;
    }
    private void OnPause()
    {
        _isPlaying = false;
        b_Play.image.color = Color.white;
        b_Pause.image.color = ColorButton;
    }
    private void OnStop()
    {
        _isPlaying = false;
        //TODO Выключаем цикл и гасим всё
    }
    
    private void OnBack()
    {
        MenuPanel.SetActive(false);
        //TODO Выключаем всё!!!
        bluetoothManager.AddMessage(160);
    }

}


