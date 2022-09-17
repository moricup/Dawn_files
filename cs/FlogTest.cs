using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//以下サイトのボタンのテキスト取得をコピペしている
//https://marumaro7.hatenablog.com/entry/buttonsyutoku

public class FlogTest : MonoBehaviour
{
    //For FlogTest

    //DoubleFlogPlay絡み
    public int delaySecondFlogTimeInt = 0;//10進5桁abcdeで ab.cde秒 を表す
    public float delaySecondFlogTimeFloat = 0f;
    public TextMeshProUGUI textObj;//delaySecondFlogTimeIntを表示するオブジェクト

    // eventSystem型の変数を宣言　
    private  EventSystem eventSystem;
   
    //GameObject型の変数を宣言　ボタンオブジェクトを入れる箱
    private GameObject button_ob;

    //GameObject型の変数を宣言　テキストオブジェクトを入れる箱
    private GameObject NameText_ob;

    //Text型の変数を宣言　テキストコンポーネントを入れる箱
    private TextMeshProUGUI name_text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SingleFlogPlay()
    {
        SoundManager.soundManager.PlayBgmFromString("Flog");
        Debug.Log("FlogPlay");
    }

    public void FlogStop()
    {
        SoundManager.soundManager.StopBgm();
    }

    public void DoubleFlogPlay()
    {
        SingleFlogPlay();
        Invoke("SingleFlogPlay", delaySecondFlogTimeFloat);
    }

    //Battleのテスト
    public void SecondBattlePlay()
    {
        SoundManager.soundManager.PlayBgmFromString("TestBattle02");
    }

    //Battleのテスト
    public void BothBattlePlay()
    {
        SoundManager.soundManager.PlayBgmFromString("TestBattle01");
        Invoke("SecondBattlePlay", delaySecondFlogTimeFloat);
        Debug.Log(delaySecondFlogTimeFloat);
    }

    public void SingleStormPlay()
    {
        SoundManager.soundManager.PlayBgmFromString("Storm");
    }
    
    //数字ボタンにこの関数を割り当てて使用
    public void DelayTimeUpdate()
    {
        //有効なイベントシステムを取得
        eventSystem=EventSystem.current;

        //押されたボタンのオブジェクトをイベントシステムのcurrentSelectedGameObject関数から取得　
        button_ob = eventSystem.currentSelectedGameObject;
        
        //ボタンの子のテキストオブジェクトを名前指定で取得 この場合Numと名前が付いているテキストオブジェクトを探す
        NameText_ob = button_ob.transform.Find("Num").gameObject;

        //テキストオブジェクトのテキストを取得
        name_text = NameText_ob.GetComponent<TextMeshProUGUI>();

        //delaySecondFlogTimeIntを更新
        delaySecondFlogTimeInt = 10*delaySecondFlogTimeInt + int.Parse(name_text.text);
        delaySecondFlogTimeInt -= (delaySecondFlogTimeInt/100000)*100000;

        //delaySecondFlogTimeIntを表示
        textObj.text = delaySecondFlogTimeInt.ToString("00000");

        //delaySecondFlogTimeFloatを更新
        delaySecondFlogTimeFloat = delaySecondFlogTimeInt/(1000f);      
    }
}
