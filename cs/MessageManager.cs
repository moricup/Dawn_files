using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MessageManager : MonoBehaviour
{
    public GameObject csvObj;//emptyなCSVReaderをアタッチ
    List<string[]> csvDatas;//CSVReaderからコピーされる

    //CoDrawText近辺で使用される
    public TextMeshProUGUI textObj;//MessageWhite内のtext部分
    private bool text_drawing;//CoDrawTextが動作中か
    public float textSpeed = 0.1f;
    private float now_text_time;//テキストを描画し始めてからの経過時間
    private int now_text_len;//現在表示しているテキストの長さ
    private bool inAroundCoDrawText = false;//現在AroundCoDrawText実行中か

    //選択肢で使用
    public bool choicing = false;
    public bool choiceSelection1 = true;
    public GameObject selectButton1;
    public GameObject selectButton2;
    public TextMeshProUGUI selectButton1Text;
    public TextMeshProUGUI selectButton2Text;
    public GameObject selectPanel;
    
    //次のシーン、csvDatasで指定される
    public string nextScene1;
    public string nextScene2;

    //WorkingBaseで使用される
    int commandNum;
    int instructionNum;

    //画像操作で使用される
    public GameObject playerImage;
    public GameObject enemyImage;
    public Sprite[] imageSet = new Sprite[10];

    //スキップ操作で使用される
    public bool pressedSkipButton = false;//Skipボタンが押されると、commandNum == 9 とならない限りtrueのまま
    bool breakCoDrawText = false;//Skipボタンが押されると、その時点のlineの処理が終わるまでtrueのまま
    public GameObject skipButton;


    // Start is called before the first frame update
    void Start()
    {
        csvDatas = csvObj.GetComponent<CSVReader>().csvDatas;
        StartCoroutine("WorkingBase");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void ConvertText()
    //{
    //    splitText = textFile.text.split('|');
    //}


    public void PressingSkipButton()
    {
        skipButton.GetComponent<Button>().interactable = false;//skipボタンを使用不可にする
        pressedSkipButton = true;
        breakCoDrawText = true;
        SeManager.seManager.PlaySeFromString("SystemDeter11");//ボタン押しin MessageScene
    }

    IEnumerator CoDrawText(string text)
    {
        text_drawing = true;
        now_text_time = 0;
        while(true)
        {
            yield return null;//一瞬休憩

            now_text_time += Time.deltaTime;//現在の描画時間

            if(Input.GetMouseButtonDown(0)) break;//タップで全て表示

            now_text_len = (int)(now_text_time / textSpeed);//どれだけ表示するか
            if(now_text_len > text.Length) break;//描画範囲が全体を超えたら外の処理へ
            textObj.text = text.Substring(0, now_text_len);//描画範囲が部分のため、そこを表示
        }

        textObj.text = text;//全て表示
        yield return null;//一瞬休憩

        //now_text_len--;//直前にnow_text_len == text.Length + 1 のため、text.Lengthに合わせる

        text_drawing = false;
    }

    IEnumerator CoDrawSkip()//CoDrawTextの直後 in AroundCoDrawTextで使用される
    {
        while(text_drawing)
        {
            if(breakCoDrawText) break;
            yield return null;
        }
        while(!Input.GetMouseButtonDown(0))
        {
            if(breakCoDrawText) break;
            yield return null;
        }
    }

    IEnumerator AroundCoDrawText(string textForDraw)
    {
        if(pressedSkipButton == false)//pressedSkipButton==trueなら発言をスキップ
        {
            StartCoroutine("CoDrawText", textForDraw);
            yield return StartCoroutine("CoDrawSkip");
            SeManager.seManager.PlaySeFromString("Retro16");//発言後のタップ in MessageScene
        }
        inAroundCoDrawText = false;
    }

    IEnumerator LightingOn(bool whichImage)//trueならplayer, falseならenemy
    {
        if(whichImage)
        {
            playerImage.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
        }
        else
        {
            enemyImage.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
        }
        yield return null;
    }

    IEnumerator LightingOff(bool whichImage)//trueならplayer, falseならenemy
    {
        if(whichImage)
        {
            playerImage.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            enemyImage.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        yield return null;
    }

    public void PressingButton1()
    {
        choiceSelection1 = true;
        choicing = false;
    }

    public void PressingButton2()
    {
        choiceSelection1 = false;
        choicing = false;
    }

    IEnumerator WorkingBase()
    {
        for(int line=0; line<csvDatas.Count; line++)
        {
            commandNum = int.Parse(csvDatas[line][0]);
            if(commandNum == 0)//ナレーター発言
            {
                inAroundCoDrawText = true;
                StartCoroutine("AroundCoDrawText", csvDatas[line][1]);
                while(inAroundCoDrawText) yield return null;
            }
            else if(commandNum == 1)//善系主人公(炎)発言
            {
                inAroundCoDrawText = true;
                StartCoroutine("AroundCoDrawText", "フレア" + csvDatas[line][1]);
                while(inAroundCoDrawText) yield return null;
            }
            else if(commandNum == 2)//悪系主人公(風)発言
            {
                inAroundCoDrawText = true;
                StartCoroutine("AroundCoDrawText", "ウィンド" + csvDatas[line][1]);
                while(inAroundCoDrawText) yield return null;
            }
            else if(commandNum == 3)//無情な悪者(土)発言
            {
                inAroundCoDrawText = true;
                StartCoroutine("AroundCoDrawText", "S.O.I.L." + csvDatas[line][1]);
                while(inAroundCoDrawText) yield return null;
            }
            else if(commandNum == 4)//堅物な悪者(水)発言
            {
                inAroundCoDrawText = true;
                StartCoroutine("AroundCoDrawText", "A.Q.U.A." + csvDatas[line][1]);
                while(inAroundCoDrawText) yield return null;
            }
            else if(commandNum == 5)//両画像点灯
            {
                StartCoroutine("LightingOn", true);
                StartCoroutine("LightingOn", false);
            }
            else if(commandNum == 6)//両画像消灯
            {
                StartCoroutine("LightingOff", true);
                StartCoroutine("LightingOff", false);
            }
            else if(commandNum == 7)//左画像操作
            {
                instructionNum = int.Parse(csvDatas[line][1]);
                StartCoroutine("LightingOn", true);
                StartCoroutine("LightingOff", false);
                if(instructionNum < 0)
                {
                    playerImage.SetActive(false);
                }
                else
                {
                    playerImage.GetComponent<SpriteRenderer>().sprite = imageSet[instructionNum];
                    playerImage.SetActive(true);
                }
            }
            else if(commandNum == 8)//右画像操作
            {
                instructionNum = int.Parse(csvDatas[line][1]);
                StartCoroutine("LightingOff", true);
                StartCoroutine("LightingOn", false);
                if(instructionNum < 0)
                {
                    enemyImage.SetActive(false);
                }
                else
                {
                    enemyImage.GetComponent<SpriteRenderer>().sprite = imageSet[instructionNum];
                    enemyImage.SetActive(true);
                }
            }
            else if(commandNum == 9)//選択肢2選択可能のフラグ番号
            {
                if(pressedSkipButton)
                {
                    //スキップ中にここに到達したら解除
                    pressedSkipButton = false;
                    //ひと休み
                    yield return null;
                    //直前の発言を表示してから、改めて戻ってくる
                    line -= 2;
                    continue;
                }
                instructionNum = int.Parse(csvDatas[line][1]);
                if(FlagManager.flag[instructionNum])
                {
                    selectButton2.SetActive(true);
                }
                else
                {
                    selectButton2.SetActive(false);
                }
            }
            else if(commandNum == 10)//選択肢1
            {
                selectButton1Text.text = csvDatas[line][1];
            }
            else if(commandNum == 11)//選択肢2
            {
                selectButton2Text.text = csvDatas[line][1];
                selectPanel.SetActive(true);
                choicing = true;
                while(choicing) yield return null;//ボタンを押すとchoicing==falseとなる
                SeManager.seManager.PlaySeFromString("SystemDeter11");//ボタン押しin MessageScene
                selectPanel.SetActive(false);
            }
            else if(commandNum == 12)//シーン移動1
            {
                nextScene1 = csvDatas[line][1];
            }
            else if(commandNum == 13)//シーン移動2
            {
                nextScene2 = csvDatas[line][1];
            }
            else if(commandNum == 14)//フラグ上げ
            {
                instructionNum = int.Parse(csvDatas[line][1]);
                FlagManager.flag[instructionNum] = true;

                //TestFlag2();//デバッグ用
            }
            else if(commandNum == 15)//フラグ降ろし
            {
                instructionNum = int.Parse(csvDatas[line][1]);
                FlagManager.flag[instructionNum] = false;

                //TestFlag2();//デバッグ用
            }
            else if(commandNum == 16)//BGMPlay
            {
                SoundManager.soundManager.PlayBgmFromString(csvDatas[line][1]);
            }
            else if(commandNum == 17)//BGMStop
            {
                SoundManager.soundManager.StopBgm();
            }
            else if(commandNum == 18)//SEPlay
            {
                if(pressedSkipButton == true) continue;//スキップ中は効果音を再生しない
                SeManager.seManager.PlaySeFromString(csvDatas[line][1]);
            }
            breakCoDrawText = false;//次のlineの処理前にリセット
        }

        Load();
    }

    void Load()
    {
        if(choiceSelection1)
        {
            SceneManager.LoadScene(nextScene1);
        }
        else
        {
            SceneManager.LoadScene(nextScene2);
        }
    }

    void TestFlag2()
    {
        if(FlagManager.flag[2])
        {
            Debug.Log("flag[2]==true");
        }
        else
        {
            Debug.Log("flag[2]==false");
        }
    }
}
