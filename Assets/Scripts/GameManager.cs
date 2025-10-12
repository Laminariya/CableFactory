using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
    
    
    [HideInInspector] public SendComPort sendComPort;
    [HideInInspector] public BluetoothManager bluetoothManager;
    [HideInInspector] public JsonClass jsonClass;

    public TMP_Text Log;
    public TMP_Text LogTime;

    public GameObject Light1;
    public GameObject Light5;
    public GameObject Light6;
    public GameObject LightABK;
    public GameObject LightOBK;
    public GameObject LightObshaga;
    
    private Coroutine _coroutine;
    private Coroutine _coroutin2;
    private bool _isDemo;
    private bool _isPlaying;
    private bool _isObshee;
    
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
        List<Image> images = Light1.GetComponentsInChildren<Image>(true).ToList();
        foreach (var image in images)
        {
            image.color = StartColor;
        }
        List<Image> images1 = Light5.GetComponentsInChildren<Image>(true).ToList();
        foreach (var image in images1)
        {
            image.color = StartColor;
        }
        List<Image> images2 = Light6.GetComponentsInChildren<Image>(true).ToList();
        foreach (var image in images2)
        {
            image.color = StartColor;
        }
        List<Image> images3 = LightObshaga.GetComponentsInChildren<Image>(true).ToList();
        foreach (var image in images3)
        {
            image.color = StartColor;
        }
        List<Image> images4 = LightABK.GetComponentsInChildren<Image>(true).ToList();
        foreach (var image in images4)
        {
            image.color = StartColor;
        }
        List<Image> images5 = LightOBK.GetComponentsInChildren<Image>(true).ToList();
        foreach (var image in images5)
        {
            image.color = StartColor;
        }
        
    }

    private void OnShowMenu()
    {
        MenuPanel.SetActive(true);
        Init();
    }

    IEnumerator OnDemo()
    {
        _isDemo = true;
        _isPlaying = true;
        //demoText.text = OffAllText;
        OffAllLed();
        float time = 0;
        int number = 0;
        while (_isDemo)
        {
            Log.text = "";
            bluetoothManager.AddMessage(GetMessage(jsonClass.jsonData.workday[0]));
            time = 0;
            number = 0;
            number++;
            while (time < jsonClass.jsonData.workday[jsonClass.jsonData.workday.Count - 1].time * 60f)
            {
                while (!_isPlaying)
                {
                    yield return null;
                }

                time += Time.deltaTime;
                LogTime.text = time.ToString();
                if (time > jsonClass.jsonData.workday[number].time * 60f)
                {
                    Log.text += jsonClass.jsonData.workday[number].pin + "\r\n";
                    bluetoothManager.AddMessage(GetMessage(jsonClass.jsonData.workday[number]));
                    number++;
                }

                yield return null;
            }
        }
    }

    IEnumerator OnCommon()
    {
        _isObshee = true;
        _isPlaying = true;
        //OffAllLed();
        float time = 0;
        int number = 0;
        while (_isObshee)
        {
            Log.text = "";
            bluetoothManager.AddMessage(GetMessage(jsonClass.jsonData.common[0]));
            time = 0;
            number = 0;
            number++;
            while (time < jsonClass.jsonData.common[jsonClass.jsonData.common.Count - 1].time * 60f)
            {
                while (!_isPlaying)
                {
                    yield return null;
                }

                time += Time.deltaTime;
                LogTime.text = time.ToString();
                if (time > jsonClass.jsonData.common[number].time * 60f)
                {
                    Log.text += jsonClass.jsonData.common[number].pin + "\r\n";
                    bluetoothManager.AddMessage(GetMessage(jsonClass.jsonData.common[number]));
                    number++;
                }

                yield return null;
            }
        }
            
    }

    private int GetMessage(Commands command)
        {
            if (command.pin == 4) OnOffLight(command, LightObshaga);
            if (command.pin == 5) OnOffLight(command, LightOBK);
            if (command.pin == 6) OnOffLight(command, Light5);
            if (command.pin == 1) OnOffLight(command, Light1);
            if (command.pin == 9) OnOffLight(command, Light6);
            if (command.pin == 12) OnOffLight(command, LightABK);

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

        private void OnOffLight(Commands command, GameObject light)
    {
        List<Image> images = light.GetComponentsInChildren<Image>(true).ToList();
        if (command.activate == 1)
        {
            foreach (var image in images)
            {
                image.DOColor(FinishColor, 0.5f);
            }
        }
        else
        {
            foreach (var image in images)
            {
                image.DOColor(StartColor, 0.5f);
            }
        }
    }
    
    private void OnOffLight(int onoff, GameObject light)
    {
        List<Image> images = light.GetComponentsInChildren<Image>(true).ToList();
        if (onoff == 1)
        {
            foreach (var image in images)
            {
                image.DOColor(FinishColor, 0.5f);
            }
        }
        else
        {
            foreach (var image in images)
            {
                image.DOColor(StartColor, 0.5f);
            }
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
        if (Obshiy.image.color == Color.white)
        {
            Obshiy.image.color = ColorButton;
            if(_coroutin2!=null)
                StopCoroutine(_coroutin2);
            _coroutin2 = StartCoroutine(OnCommon());
        }
        else
        {
            Obshiy.image.color = Color.white;
            OnStop();
        }
    }

    private void OnWorkDay()
    {
        if (WorkDay.image.color == Color.white)
        {
            WorkDay.image.color = ColorButton;
            if(_coroutine!=null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(OnDemo());
        }
        else
        {
            WorkDay.image.color = Color.white;
            OnStop();
        }
       
    }

    private void OnKorpus1()
    {
        if (Korpus1.image.color == Color.white)
        {
            Korpus1.image.color = ColorButton;
            bluetoothManager.AddMessage(177);
            OnOffLight(1, Light1);
        }
        else
        {
            Korpus1.image.color = Color.white;
            bluetoothManager.AddMessage(161);
            OnOffLight(2, Light1);
        }
    }

    private void OnKorpus5()
    {
        if (Korpus5.image.color == Color.white)
        {
            Korpus5.image.color = ColorButton;
            bluetoothManager.AddMessage(182);
            OnOffLight(1, Light5);
        }
        else
        {
            Korpus5.image.color = Color.white;
            bluetoothManager.AddMessage(166);
            OnOffLight(0, Light5);
        }
    }

    private void OnKorpus6()
    {
        if (Korpus6.image.color == Color.white)
        {
            Korpus6.image.color = ColorButton;
            bluetoothManager.AddMessage(185);
            OnOffLight(1, Light6);
        }
        else
        {
            Korpus6.image.color = Color.white;
            bluetoothManager.AddMessage(169);
            OnOffLight(0, Light6);
        }
    }
    
    private void OnABK()
    {
        if (ABK.image.color == Color.white)
        {
            ABK.image.color = ColorButton;
            bluetoothManager.AddMessage(188);
            OnOffLight(1, LightABK);
        }
        else
        {
            ABK.image.color = Color.white;
            bluetoothManager.AddMessage(172);
            OnOffLight(0, LightABK);
        }
    }
    private void OnOBK()
    {
        if (OBK.image.color == Color.white)
        {
            OBK.image.color = ColorButton;
            bluetoothManager.AddMessage(181);
            OnOffLight(1, LightOBK);
        }
        else
        {
            OBK.image.color = Color.white;
            bluetoothManager.AddMessage(165);
            OnOffLight(0, LightOBK);
        }
    }
    private void OnObshaga()
    {
        if (Obshaga.image.color == Color.white)
        {
            Obshaga.image.color = ColorButton;
            bluetoothManager.AddMessage(180);
            OnOffLight(1, LightObshaga);
        }
        else
        {
            Obshaga.image.color = Color.white;
            bluetoothManager.AddMessage(164);
            OnOffLight(0, LightObshaga);
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
        _isDemo = false;
        _isObshee = false;
        if(_coroutine!=null)
            StopCoroutine(_coroutine);
        if(_coroutin2!=null)
            StopCoroutine(_coroutin2);
        bluetoothManager.AddMessage(160);
        Init();
    }
    
    private void OnBack()
    {
        OnStop();
        MenuPanel.SetActive(false);
        bluetoothManager.AddMessage(160);
    }

}


