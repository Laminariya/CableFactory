using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

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

    public Button b_Demo;
    public TMP_Text demoText;
    public string OffAllText;
    [HideInInspector] public SendComPort sendComPort;

    private Coroutine _coroutine;
    private bool _isDemo;
    
    void Start()
    {
        b_Close.onClick.AddListener(OnClose);
        _isDemo = false;
        sendComPort = GetComponent<SendComPort>();
        sendComPort.Init();
        b_Demo.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
       
    }

    IEnumerator OnDemo()
    {
        _isDemo = true;
        b_Demo.image.color = ColorButton;
        demoText.text = OffAllText;
        OffAllLed();
        while (_isDemo)
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 11; i++)
            {
                sendComPort.AddMessage(177+i);
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(3f);
            OffAllLed();
            yield return new WaitForSeconds(3f);
        }
    }

    private void OffAllLed()
    {
        for (int i = 0; i < 11; i++)
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
        
    }

    private void OnKorpus5()
    {
        
    }

    private void OnKorpus6()
    {
        
    }
    
    private void OnABK()
    {
        
    }
    private void OnOBK()
    {
        
    }
    private void OnObshaga()
    {
        
    }
    private void OnPlay()
    {
        
    }
    private void OnPause()
    {
        
    }
    private void OnStop()
    {
        
    }
    
    private void OnBack()
    {
        
    }

}
