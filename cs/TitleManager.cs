using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject windButton;
    public GameObject extraButton;

    // Start is called before the first frame update
    void Start()
    {
        //TitleScene RecordScene で使用される

        //直前のBGM停止
        SoundManager.soundManager.StopBgm();

        //エクストラボタンがあればRecordSceneである。そのとき限定の処理を行う
        if(extraButton != null)
        {
            //全てのバトルでヒール無し勝利していたらエクストラボタン解放
            if(FlagManager.ActivateExtra())
            {
                extraButton.GetComponent<Button>().interactable = true;
            }
        }

        //ウィンドボタンがあればTitleSceneである。そのとき限定の処理を行う
        if(windButton != null)
        {
            //ウィンドルートは解放されているか
            if(FlagManager.flag[3])
            {
                windButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                windButton.GetComponent<Button>().interactable = false;
            }

            //タイトルBGM再生
            SoundManager.soundManager.PlayBgmFromString("Title");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AllFlagRaise()
    {
        SeManager.seManager.PlaySeFromString("SystemDeter11");//ボタン押しin MessageScene
        for(int i=2; i<FlagManager.flag.Length; i++)
        {
            FlagManager.flag[i] = true;
        }
    }

    public void FlagInLog()
    {
        SeManager.seManager.PlaySeFromString("SystemDeter11");//ボタン押しin MessageScene
        for(int i=0; i<FlagManager.flag.Length; i++)
        {
            Debug.Log(System.Tuple.Create(i,FlagManager.flag[i]));
        }
    }
}
