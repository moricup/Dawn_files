using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public GameObject csvObj;//emptyなCSVReaderをアタッチ
    List<string[]> csvDatas;//CSVReaderからコピーされる

    //GameObjectたち
    public GameObject playerImage;
    public GameObject enemyImage;
    public TextMeshProUGUI playerNameObj;
    public TextMeshProUGUI playerHpObj;
    public TextMeshProUGUI playerMpObj;
    public TextMeshProUGUI enemyNameObj;
    public TextMeshProUGUI enemyHpObj;
    public GameObject commandButtonPanel;
    public GameObject mpCommandButton1;
    public GameObject mpCommandButton2;
    public GameObject mpCommandButton3;
    public GameObject playerCommandWhite;
    public GameObject enemyCommandWhite;
    public TextMeshProUGUI playerCommandText;
    public TextMeshProUGUI enemyCommandText;
    public GameObject commandTextPanel;
    public GameObject playerDamageObj;
    public GameObject enemyDamageObj;
    public GameObject winObj;
    public GameObject loseObj;
    public GameObject redCrossAtHealObj;
    public GameObject twinkleAtWinObj;
    
    //statusの数値たち
    public int playerHpMax = 6;
    public int playerMpMax = 2;
    public int enemyHpMax = 10;
    public int playerHp;
    public int playerMp;
    public int enemyHp;
    public int playerDamage;
    public int enemyDamage;
    public int phaseResult;// -1:Draw, 0:PlayerAdvantage, 1:EnemyAdvantage

    //Mp_numのボタンの使用可能フラグ
    public bool usableMp1 = true;
    public bool usableMp2 = true;
    public bool usableMp3 = false;

    //imageの横位置
    public int nearImagePos = 2;//近づく限界位置
    public int defImagePos = 4;//通常の位置
    public int farImagePos = 6;//遠のく限界位置

    //imageの縦位置
    public int downImagePos = - 4;

    //playerの技の変数
    public int nowPlayerCommandType; //-1:none, 0:fire, 1:wind, 2:soil, 3:aqua
    public string nowPlayerCommandName;
    public int playerMp1Type = 0;
    public string playerMp1Name = "フレアブレード";
    public int playerMp2Type = 2;
    public string playerMp2Name = "ソイルナックル";
    public int playerMp3Type = 0;
    public string playerMp3Name;

    //enemyの技の変数
    public int nowEnemyCommandType; //-1:none, 0:fire, 1:wind, 2:soil, 3:aqua
    public string nowEnemyCommandName;
    public int enemyMp1Type = 1;
    public string enemyMp1Name = "ウィンドサイズ";
    public int enemyMp2Type = 3;
    public string enemyMp2Name = "アクアキャノン";
    public int enemyMp3Type = 0;
    public string enemyMp3Name;

    //enemyが選択するボタン番号
    public List<int> enemyBtArray = new List<int>();
    public int enemyBt;

    //ターン数
    public int turn = -1;
    public int enemyTurnPeriod = 3;

    //バトルが終了したら立つフラグ
    bool endBattle = false;

    //バトル終了後に移動するシーン名
    public string nextBranchScene;
    public string nextWinScene;
    public string nextLoseScene;

    //Imagesが接近中に立つフラグ
    public bool imagesMoving = false;

    //ImagesAnimation起動中に立つフラグ
    public bool imagesAnimationRunning = false;

    //Imagesの単位移動時間
    public float secImage = 0.2f;

    //DamageAnimation起動中に立つフラグ
    public bool damageAnimationRunning = false;

    //DamageObjsの初期位置
    Vector3 defDamageAbsPos;

    //DamageObjsの単位移動時間と速度
    public float secDamage = 0.8f;
    float velDamage;

    //Blinkの所要時間と、所要時間内の周期数、1周期の時間
    public float blinkTime = 0.2f;
    public float blinkPeriod = 2.0f;
    float blinkOnePeriodTime;

    //final Battleか?
    public bool isFinal = false;

    //画像集
    public Sprite[] imageSet = new Sprite[10];

    //条件分岐で用いる変数
    public bool isBranch = false;
    public bool isOnceWin = false;
    public bool isTwiceWin = false;

    //色
    Color flareDark = new Color(231.0f/255.0f, 0.0f/255.0f, 18.0f/255.0f);
    Color flareThin = new Color(255.0f/255.0f, 187.0f/255.0f, 192.0f/255.0f);
    Color windDark = new Color(143.0f/255.0f, 195.0f/255.0f, 32.0f/255.0f);
    Color windThin = new Color(205.0f/255.0f, 236.0f/255.0f, 142.0f/255.0f);
    Color soilDark = new Color(252.0f/255.0f, 201.0f/255.0f, 0.0f/255.0f);
    Color soilThin = new Color(255.0f/255.0f, 235.0f/255.0f, 153.0f/255.0f);
    Color aquaDark = new Color(1.0f/255.0f, 134.0f/255.0f, 209.0f/255.0f);
    Color aquaThin = new Color(181.0f/255.0f, 230.0f/255.0f, 255.0f/255.0f);
    Color whiteColor = new Color(1,1,1);

    //回復処理で用いる
    public GameObject healButton;
    bool isHealed = false;

    //バトル遭遇、勝利、ヒール無し勝利のフラグ番号
    int numEncounted;
    int numWon;
    int numWonWithoutHeal;

    // Start is called before the first frame update
    void Start()
    {
        //csv読み込みと、それに応じたデータ初期化
        csvDatas = csvObj.GetComponent<CSVReader>().csvDatas;
        InitDatasFromCSV();

        //ステータスの初期化
        playerHp = playerHpMax;
        playerMp = playerMpMax;
        enemyHp = enemyHpMax;

        //DamageAnimationのための初期化、Canvasの子のため確実にこうするべき
        defDamageAbsPos = enemyDamageObj.transform.position;//enemyDamageObjを右上に置いておくこと
        velDamage = (2.0f*defDamageAbsPos.y)/secDamage;

        //Blinkのための時間計算
        blinkOnePeriodTime = blinkTime/blinkPeriod;

        //最終戦闘BGM以外が再生中なら停止する
        if(SoundManager.playingBGM != BGMType.LastBattle)
        {
            SoundManager.soundManager.StopBgm();
        }

        //音楽再生
        if(isFinal == false)//最終戦闘か
        {
            SoundManager.soundManager.PlayBgmFromString("Battle");
        }
        else
        {
            SoundManager.soundManager.PlayBgmFromString("LastBattle");
        }

        btAllColoring();
        btAllSetActive();
        StartCoroutine("UpdatePlayerStatusText");
        StartCoroutine("UpdateEnemyStatusText");
        StartCoroutine("StartTurn");

        //バトル遭遇フラグを立てる
        FlagManager.flag[numEncounted] = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitDatasFromCSV()
    {
        //プレイヤー情報
        playerNameObj.text = csvDatas[0][0];
        playerImage.GetComponent<SpriteRenderer>().sprite = imageSet[int.Parse(csvDatas[0][1])];
        playerHpMax = int.Parse(csvDatas[0][2]);
        playerMpMax = int.Parse(csvDatas[0][3]);
        playerImage.SetActive(true);//プレイヤー画像表示
        //プレイヤーMp技type
        playerMp1Type = int.Parse(csvDatas[1][0]);
        playerMp2Type = int.Parse(csvDatas[1][1]);
        playerMp3Type = int.Parse(csvDatas[1][2]);
        //プレイヤーMp技name
        playerMp1Name = csvDatas[2][0];
        playerMp2Name = csvDatas[2][1];
        playerMp3Name = csvDatas[2][2];
        mpCommandButton1.GetComponentInChildren<TextMeshProUGUI>().text = playerMp1Name + "   MP1";//ボタンの名前更新
        mpCommandButton2.GetComponentInChildren<TextMeshProUGUI>().text = playerMp2Name + "   MP1";//ボタンの名前更新
        mpCommandButton3.GetComponentInChildren<TextMeshProUGUI>().text = playerMp3Name + "   MP1";//ボタンの名前更新
        //プレイヤーMp技許可フラグ
        usableMp1 = FlagManager.flag[int.Parse(csvDatas[3][0])];
        usableMp2 = FlagManager.flag[int.Parse(csvDatas[3][1])];
        usableMp3 = FlagManager.flag[int.Parse(csvDatas[3][2])];
        //エネミー情報
        enemyNameObj.text = csvDatas[4][0];
        enemyImage.GetComponent<SpriteRenderer>().sprite = imageSet[int.Parse(csvDatas[4][1])];
        enemyHpMax = int.Parse(csvDatas[4][2]);
        enemyImage.SetActive(true);//エネミー画像表示
        //エネミーMp技type
        enemyMp1Type = int.Parse(csvDatas[5][0]);
        enemyMp2Type = int.Parse(csvDatas[5][1]);
        enemyMp3Type = int.Parse(csvDatas[5][2]);
        //エネミーMp技name
        enemyMp1Name = csvDatas[6][0];
        enemyMp2Name = csvDatas[6][1];
        enemyMp3Name = csvDatas[6][2];
        //エネミー使う技の周期
        enemyTurnPeriod = int.Parse(csvDatas[7][0]);
        //エネミー使う技順
        for(int i=0; i<enemyTurnPeriod; i++)
        {
            enemyBtArray.Add(int.Parse(csvDatas[8][i]));
        }
        //条件分岐するか
        if(int.Parse(csvDatas[9][0]) == 1)
        {
            isBranch = true;
        }
        else
        {
            isBranch = false;
        }
        isOnceWin = FlagManager.flag[int.Parse(csvDatas[9][1])];
        isTwiceWin = FlagManager.flag[int.Parse(csvDatas[9][2])];
        nextBranchScene = csvDatas[9][3];
        //勝利シーン名
        nextWinScene = csvDatas[10][0];
        //敗北シーン名
        nextLoseScene = csvDatas[11][0];
        //遭遇フラグ、勝利フラグ、ヒール無し勝利フラグ番号
        numEncounted = int.Parse(csvDatas[12][0]);
        numWon = int.Parse(csvDatas[12][1]);
        numWonWithoutHeal = int.Parse(csvDatas[12][2]);
    }

    bool IsClicked()
    {
        if (Input.GetMouseButtonDown(0)) return true;
        return false;
    }

    IEnumerator Skip()
    {
        while(!IsClicked()) yield return null;
    }

    void objOneColoring(GameObject obj, int type, bool isDark)
    {
        if(type < 0)
        {
            obj.GetComponent<Image>().color = whiteColor;
        }
        else if(type%4 == 0)
        {
            if(isDark)
            {
                obj.GetComponent<Image>().color = flareDark;
            }
            else
            {
                obj.GetComponent<Image>().color = flareThin;
            }
        }
        else if(type%4 == 1)
        {
            if(isDark)
            {
                obj.GetComponent<Image>().color = windDark;
            }
            else
            {
                obj.GetComponent<Image>().color = windThin;
            }
        }
        else if(type%4 == 2)
        {
            if(isDark)
            {
                obj.GetComponent<Image>().color = soilDark;
            }
            else
            {
                obj.GetComponent<Image>().color = soilThin;
            }
        }
        else if(type%4 == 3)
        {
            if(isDark)
            {
                obj.GetComponent<Image>().color = aquaDark;
            }
            else
            {
                obj.GetComponent<Image>().color = aquaThin;
            }
        }
    }

    void btAllColoring()
    {
        objOneColoring(mpCommandButton1, playerMp1Type, false);
        objOneColoring(mpCommandButton2, playerMp2Type, false);
        objOneColoring(mpCommandButton3, playerMp3Type, false);
    }

    void btOneSetActive(GameObject obj, bool usableMp_num)
    {
        if(usableMp_num)
        {
            obj.SetActive(true);
        }
        else
        {
            obj.SetActive(false);
        }
    }

    void btAllSetActive()
    {
        btOneSetActive(mpCommandButton1, usableMp1);
        btOneSetActive(mpCommandButton2, usableMp2);
        btOneSetActive(mpCommandButton3, usableMp3);
    }

    void btOneInteractable(Button bt)
    {
        if(playerMp > 0)
        {
            bt.interactable = true;
        }
        else
        {
            bt.interactable = false;
        }
    }

    void btAllInteractable()
    {
        Button bt1 = mpCommandButton1.GetComponent<Button>();
        Button bt2 = mpCommandButton2.GetComponent<Button>();
        Button bt3 = mpCommandButton3.GetComponent<Button>();
        btOneInteractable(bt1);
        btOneInteractable(bt2);
        btOneInteractable(bt3);
    }

    IEnumerator StartTurn()
    {
        turn++;
        btAllInteractable();
        commandButtonPanel.SetActive(true);
        commandTextPanel.SetActive(false);
        yield return null;
    }

    IEnumerator UpdatePlayerStatusText()
    {
        playerHpObj.text = playerHp.ToString();
        playerMpObj.text = playerMp.ToString();
        yield return null;
    }


    IEnumerator UpdateEnemyStatusText()
    {
        enemyHpObj.text = enemyHp.ToString();
        yield return null;
    }

    IEnumerator UpdateCommandColor()
    {
        objOneColoring(playerCommandWhite, nowPlayerCommandType, true);
        objOneColoring(enemyCommandWhite, nowEnemyCommandType, true);
        yield return null;
    }

    IEnumerator UpdateCommandText()
    {
        playerCommandText.text = nowPlayerCommandName;
        enemyCommandText.text = nowEnemyCommandName;
        yield return null;
    }

    public void PressedNormalButton()
    {
        if(playerMp < playerMpMax) playerMp++; //こうげき時は最大値を超えない範囲でMp回復
        nowPlayerCommandType = -1;
        nowPlayerCommandName = "こうげき";
        StartCoroutine("BattlePhase");
    }

    public void PressedMpButton1()
    {
        playerMp--;
        nowPlayerCommandType = playerMp1Type;
        nowPlayerCommandName = playerMp1Name;
        StartCoroutine("BattlePhase");
    }

    public void PressedMpButton2()
    {
        playerMp--;
        nowPlayerCommandType = playerMp2Type;
        nowPlayerCommandName = playerMp2Name;
        StartCoroutine("BattlePhase");
    }

    public void PressedMpButton3()
    {
        playerMp--;
        nowPlayerCommandType = playerMp3Type;
        nowPlayerCommandName = playerMp3Name;
        StartCoroutine("BattlePhase");
    }

    void getEnemyCommand()
    {
        enemyBt = enemyBtArray[turn%enemyTurnPeriod];
        if(enemyBt == 0)
        {
            nowEnemyCommandType = -1;
            nowEnemyCommandName = "こうげき";
        }
        else if(enemyBt == 1)
        {
            nowEnemyCommandType = enemyMp1Type;
            nowEnemyCommandName = enemyMp1Name;
        }
        else if(enemyBt == 2)
        {
            nowEnemyCommandType = enemyMp2Type;
            nowEnemyCommandName = enemyMp2Name;
        }
        else//enemyBt == 3
        {
            nowEnemyCommandType = enemyMp3Type;
            nowEnemyCommandName = enemyMp3Name;
        }
    }

    int ComputePhaseResult()
    {
        if((nowPlayerCommandType < 0) || (nowEnemyCommandType < 0))//どちらかが通常のこうげき
        {
            return -1;
        }
        else if(((nowPlayerCommandType + 1) % 4) == ((nowEnemyCommandType + 0) % 4))//PlayerAdvantage
        {
            return 0;
        }
        else if(((nowPlayerCommandType + 0) % 4) == ((nowEnemyCommandType + 1) % 4))//EnemyAdvantage
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    IEnumerator BattlePhase()
    {
        commandButtonPanel.SetActive(false);
        StartCoroutine("UpdatePlayerStatusText");//playerMpを反映
        getEnemyCommand();//enemyもbtを選択
        StartCoroutine("UpdateCommandColor");//CommandのWhiteを色つけ
        StartCoroutine("UpdateCommandText");//Commandを下部に表示
        commandTextPanel.SetActive(true);
        phaseResult = ComputePhaseResult();
        SeManager.seManager.PlaySeFromString("Que");//ボタン押し in BattleScene
        if(phaseResult == -1)
        {
            StartCoroutine("TradeOff");
        }
        else if(phaseResult == 0)
        {
            StartCoroutine("PlayerAdvantage");
        }
        else//phaseResult == 1
        {
            StartCoroutine("EnemyAdvantage");
        }
        yield return null;
    }

    IEnumerator ImagesCloser()
    {
        float playerVel = (defImagePos - nearImagePos)/secImage;
        float enemyVel = -(defImagePos - nearImagePos)/secImage;
        while((playerImage.transform.position.x < - nearImagePos) || (enemyImage.transform.position.x > nearImagePos))
        {
            yield return null;//1F待機
            playerImage.transform.Translate(new Vector3(playerVel*Time.deltaTime, 0, 0));
            enemyImage.transform.Translate(new Vector3(enemyVel*Time.deltaTime, 0, 0));
        }
        imagesMoving = false;
    }

    IEnumerator ImagesAwayer()
    {
        float playerVel = -(defImagePos - nearImagePos)/secImage;
        float enemyVel = (defImagePos - nearImagePos)/secImage;
        while((playerImage.transform.position.x > - defImagePos) || (enemyImage.transform.position.x < defImagePos))
        {
            yield return null;//1F待機
            playerImage.transform.Translate(new Vector3(playerVel*Time.deltaTime, 0, 0));
            enemyImage.transform.Translate(new Vector3(enemyVel*Time.deltaTime, 0, 0));
        }
        imagesMoving = false;
    }

    IEnumerator EnemyImageAwayerAndTurner()
    {
        float playerVel = 0.0f;
        float enemyVel = (farImagePos - nearImagePos)/secImage;
        while(enemyImage.transform.position.x < farImagePos)
        {
            yield return null;//1F待機
            enemyImage.transform.Translate(new Vector3(enemyVel*Time.deltaTime, 0, 0));
        }

        playerVel = -(defImagePos - nearImagePos)/secImage;
        enemyVel = -(farImagePos - defImagePos)/secImage;
        while((playerImage.transform.position.x > - defImagePos) || (enemyImage.transform.position.x > defImagePos))
        {
            yield return null;//1F待機
            playerImage.transform.Translate(new Vector3(playerVel*Time.deltaTime, 0, 0));
            enemyImage.transform.Translate(new Vector3(enemyVel*Time.deltaTime, 0, 0));
        }

        imagesMoving = false;
    }

    IEnumerator PlayerImageAwayerAndTurner()
    {
        float playerVel = -(farImagePos - nearImagePos)/secImage;
        float enemyVel = 0.0f;
        while(playerImage.transform.position.x > - farImagePos)
        {
            yield return null;//1F待機
            playerImage.transform.Translate(new Vector3(playerVel*Time.deltaTime, 0, 0));
        }

        playerVel = (farImagePos - defImagePos)/secImage;
        enemyVel = (defImagePos - nearImagePos)/secImage;
        while((playerImage.transform.position.x < - defImagePos) || (enemyImage.transform.position.x < defImagePos))
        {
            yield return null;//1F待機
            playerImage.transform.Translate(new Vector3(playerVel*Time.deltaTime, 0, 0));
            enemyImage.transform.Translate(new Vector3(enemyVel*Time.deltaTime, 0, 0));
        }

        imagesMoving = false;
    }

    IEnumerator ImagesAnimation()
    {
        imagesMoving = true;
        StartCoroutine("ImagesCloser");
        while(imagesMoving) yield return null;

        SeManager.seManager.PlaySeFromString("Battle12");//キャラ衝突 in BattleScene

        imagesMoving = true;
        if(phaseResult == -1)
        {
            StartCoroutine("ImagesAwayer");
        }
        else if(phaseResult == 0)
        {
            StartCoroutine("EnemyImageAwayerAndTurner");
        }
        else//phaseResult == 1
        {
            StartCoroutine("PlayerImageAwayerAndTurner");
        }
        while(imagesMoving) yield return null;

        //位置初期化
        playerImage.transform.position = new Vector3(- defImagePos, 0, 0);
        enemyImage.transform.position = new Vector3( defImagePos, 0, 0);

        imagesAnimationRunning = false;
    }

    IEnumerator TradeOff()
    {
        //ダメージ計算
        if(nowPlayerCommandType == -1)
        {
            enemyDamage = 1;
        }
        else
        {
            enemyDamage = 2;
        }
        if(nowEnemyCommandType == -1)
        {
            playerDamage = 1;
        }
        else if(nowEnemyCommandType <= 3)
        {
            playerDamage = 2;
        }
        else
        {
            playerDamage = 4;
        }

        imagesAnimationRunning = true;
        StartCoroutine("ImagesAnimation");
        while(imagesAnimationRunning) yield return null;

        StartCoroutine("DamagePhase");
        yield return null;
    }

    IEnumerator PlayerAdvantage()
    {
        enemyDamage = 4;
        playerDamage = 0;
        
        imagesAnimationRunning = true;
        StartCoroutine("ImagesAnimation");
        while(imagesAnimationRunning) yield return null;

        StartCoroutine("DamagePhase");
        yield return null;
    }

    IEnumerator EnemyAdvantage()
    {
        enemyDamage = 0;
        if(nowEnemyCommandType <= 3)
        {
            playerDamage = 4;
        }
        else
        {
            playerDamage = 8;
        }
        
        imagesAnimationRunning = true;
        StartCoroutine("ImagesAnimation");
        while(imagesAnimationRunning) yield return null;

        StartCoroutine("DamagePhase");
        yield return null;
    }

    IEnumerator ImagesBlink(int whoBlink, float blinkScale)
    {
        float elapsedTime = 0.0f;
        bool blinkOn;
        while(elapsedTime < (blinkTime*blinkScale))
        {
            yield return null;//1F休憩
            blinkOn = (Mathf.Repeat(elapsedTime, (blinkOnePeriodTime*blinkScale)) < (blinkOnePeriodTime*blinkScale)/2.0f);
            if(whoBlink == -1)//TradeOff
            {
                playerImage.SetActive(blinkOn);
                enemyImage.SetActive(blinkOn);
            }
            else if(whoBlink == 0)//PlayerAdvantage
            {
                enemyImage.SetActive(blinkOn);
            }
            else//whoBlink == 1 //EnemyAdvantage
            {
                playerImage.SetActive(blinkOn);
            }

            elapsedTime += Time.deltaTime;
        }

        //blink終わり、表示する
        playerImage.SetActive(true);
        enemyImage.SetActive(true);

        imagesAnimationRunning = false;
    }

    IEnumerator DamageAnimation()
    {
        SeManager.seManager.PlaySeFromString("Battle18");//キャラがダメージを受ける in BattleScene

        //Blink
        imagesAnimationRunning = true;
        StartCoroutine(ImagesBlink(phaseResult, 1.0f));
        while(imagesAnimationRunning) yield return null;

        //ここからダメージの数字表示
        playerDamageObj.GetComponent<TextMeshProUGUI>().text = playerDamage.ToString();
        enemyDamageObj.GetComponent<TextMeshProUGUI>().text = enemyDamage.ToString();

        //位置初期化
        playerDamageObj.transform.position = new Vector3(- defDamageAbsPos.x, - defDamageAbsPos.y, 0);
        enemyDamageObj.transform.position = new Vector3( defDamageAbsPos.x, - defDamageAbsPos.y, 0);

        if(phaseResult == -1)
        {
            playerDamageObj.SetActive(true);
            enemyDamageObj.SetActive(true);
        }
        else if(phaseResult == 0)
        {
            enemyDamageObj.SetActive(true);
        }
        else//phaseResult == 1
        {
            playerDamageObj.SetActive(true);
        }

        while((playerDamageObj.transform.position.y < defDamageAbsPos.y) || (enemyDamageObj.transform.position.y < defDamageAbsPos.y))
        {
            yield return null;//1F待機
            playerDamageObj.transform.Translate(new Vector3(0, velDamage*Time.deltaTime, 0));
            enemyDamageObj.transform.Translate(new Vector3(0, velDamage*Time.deltaTime, 0));
        }

        playerDamageObj.SetActive(false);
        enemyDamageObj.SetActive(false);

        damageAnimationRunning = false;
    }

    IEnumerator DamagePhase()
    {
        playerHp = playerHp - playerDamage;
        if(playerHp <= 0) playerHp = 0;
        if((playerHp == 0) && (enemyHp - enemyDamage <= 0))//同時に倒れる場合、enemyがHp1で耐える
        {
            enemyDamage = enemyHp -1;
        }
        enemyHp = enemyHp - enemyDamage;
        if(enemyHp <= 0) enemyHp = 0;

        damageAnimationRunning = true;
        StartCoroutine("DamageAnimation");
        while(damageAnimationRunning) yield return null;

        StartCoroutine("UpdatePlayerStatusText");
        StartCoroutine("UpdateEnemyStatusText");
        //StartCoroutine("ShowVariables");//デバッグで変数の値を見る
        if(playerHp <= 0)//playerHp消滅で敗北
        {
            StartCoroutine("LoseTime");
            yield return StartCoroutine("Skip");
        }
        if(enemyHp <= 0)//enemyHp消滅で勝利
        {
            StartCoroutine("WinTime");
            yield return StartCoroutine("Skip");
        }
        if(endBattle==false) StartCoroutine("StartTurn");//endBattleがtrueならバトル終了
    }


    IEnumerator EnemyDowner()
    {
        float enemyVel = downImagePos/secImage;
        while(enemyImage.transform.position.y > downImagePos)
        {
            yield return null;//1F待機
            enemyImage.transform.Translate(new Vector3(0, enemyVel*Time.deltaTime, 0));
        }

        imagesMoving = false;
    }

    IEnumerator PlayerDowner()
    {
        float playerVel = downImagePos/secImage;
        while(playerImage.transform.position.y > downImagePos)
        {
            yield return null;//1F待機
            playerImage.transform.Translate(new Vector3(0, playerVel*Time.deltaTime, 0));
        }

        imagesMoving = false;
    }

    IEnumerator WinTime()
    {
        endBattle = true;
        commandTextPanel.SetActive(false);

        FlagManager.flag[numWon] = true;//勝利フラグを立てる
        if(isHealed == false)//ヒール未使用であれば
        {
            FlagManager.flag[numWonWithoutHeal] = true;//ヒール無し勝利フラグを立てる
        }

        //enemyが沈むアニメ
        //imagesMoving = true;
        //StartCoroutine("EnemyDowner");
        //while(imagesMoving) yield return null;

        SeManager.seManager.PlaySeFromString("Explosion02");//キャラ消滅 in BattleScene

        //Blink1/4
        imagesAnimationRunning = true;
        StartCoroutine(ImagesBlink(0, 1.0f));
        while(imagesAnimationRunning) yield return null;

        //Blink2/4
        imagesAnimationRunning = true;
        StartCoroutine(ImagesBlink(0, 1.0f));
        while(imagesAnimationRunning) yield return null;

        //Blink3/4
        imagesAnimationRunning = true;
        StartCoroutine(ImagesBlink(0, 3.0f));
        while(imagesAnimationRunning) yield return null;

        //Blink4/4
        imagesAnimationRunning = true;
        StartCoroutine(ImagesBlink(0, 3.0f));
        while(imagesAnimationRunning) yield return null;

        //enemy退場
        enemyImage.SetActive(false);

        //音楽停止
        SoundManager.soundManager.StopBgm();

        //消滅効果音を一度止めてから勝利SE
        SeManager.seManager.StopSe();
        SeManager.seManager.PlaySeFromString("Jingle");//戦闘勝利 in BattleScene

        winObj.SetActive(true);
        //Debug.Log("しょうりした!");
        yield return StartCoroutine("Skip");

        //音楽停止
        SoundManager.soundManager.StopBgm();
        SeManager.seManager.StopSe();

        //条件分岐するかのチェック
        BranchLoad();

        //条件分岐せず勝利時のシーンへ
        Load();
    }

    IEnumerator LoseTime()
    {
        endBattle = true;
        commandTextPanel.SetActive(false);

        //playerが沈むアニメ
        //imagesMoving = true;
        //StartCoroutine("PlayerDowner");
        //while(imagesMoving) yield return null;

        SeManager.seManager.PlaySeFromString("Explosion02");//キャラ消滅 in BattleScene

        //Blink1/4
        imagesAnimationRunning = true;
        StartCoroutine(ImagesBlink(1, 1.0f));
        while(imagesAnimationRunning) yield return null;

        //Blink2/4
        imagesAnimationRunning = true;
        StartCoroutine(ImagesBlink(1, 1.0f));
        while(imagesAnimationRunning) yield return null;

        //Blink3/4
        imagesAnimationRunning = true;
        StartCoroutine(ImagesBlink(1, 3.0f));
        while(imagesAnimationRunning) yield return null;

        //Blink4/4
        imagesAnimationRunning = true;
        StartCoroutine(ImagesBlink(1, 3.0f));
        while(imagesAnimationRunning) yield return null;

        //player退場
        playerImage.SetActive(false);

        //最終戦闘以外は音楽停止
        if(isFinal == false)
        {
            SoundManager.soundManager.StopBgm();
        }

        loseObj.SetActive(true);
        //Debug.Log("はいぼくした");
        yield return StartCoroutine("Skip");
        Load();
    }

    void Load()
    {
        if(enemyHp<=0)
        {
            SceneManager.LoadScene(nextWinScene);
        }
        else//playerHp<=0
        {
            SceneManager.LoadScene(nextLoseScene);
        }
    }

    void BranchLoad()
    {
        if(isBranch)
        {
            if(isOnceWin)//過去に1度は勝ったか
            {
                if(isTwiceWin == false)//過去に2度も勝っていない場合
                {
                    FlagManager.flag[int.Parse(csvDatas[9][2])] = true;//次のisTwiceWin用の2度目勝利の記録
                    nextWinScene = nextBranchScene;//2度目限定のシーンを指定
                }
            }
            else//過去に1度も勝っていなかった、つまり初勝利
            {
                FlagManager.flag[int.Parse(csvDatas[9][1])] = true;//次のisOnceWin用の初勝利の記録
            }
        }
    }

    public void PressedHealButton()
    {
        healButton.GetComponent<Button>().interactable = false;//ヒールは1度までのため、使用不可にする
        playerHp = playerHpMax;//プレイヤーのHpを回復
        StartCoroutine("UpdatePlayerStatusText");//プレイヤーのステータス文を更新
        SeManager.seManager.PlaySeFromString("Heal01");//回復音 in BattleScene
        //twinkleAtHealObj.SetActive(false);//ヒールボタンのキラキラを消す
        redCrossAtHealObj.SetActive(true);//ヒールボタンのキラキラにバッテンをつける
        twinkleAtWinObj.SetActive(false);//Winボックスのキラキラを消す
        isHealed = true;//ヒール使用のフラグを立てる、勝利後に使用
    }

    IEnumerator ShowVariables()
    {
        Debug.Log("ターン" + turn.ToString());
        Debug.Log("nowPlayerCommandName" + nowPlayerCommandName);
        Debug.Log("nowPlayerCommandType" + nowPlayerCommandType.ToString());
        Debug.Log("nowEnemyCommandName" + nowEnemyCommandName);
        Debug.Log("nowEnemyCommandType" + nowEnemyCommandType.ToString());
        Debug.Log("phaseResult" + phaseResult.ToString());
        Debug.Log("playerDamage" + playerDamage.ToString());
        Debug.Log("enemyDamage" + enemyDamage.ToString());
        yield return null;
    }
}
