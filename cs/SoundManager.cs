using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMType
{
    None,
    Flog, //テストのflog
    Battle, //通常戦闘
    Village, //村
    Title, //タイトル
    Dungeon23,//燃える村
    Dungeon19,//襲撃隊基地、塔
    Rain,//嵐の雨
    Theme,//テーマ
    DawnHill,//夜明けの丘
    LastBattle,//最終戦闘
    Theme11b,//襲撃隊司令塔、ENDルートの一部
}

public class SoundManager : MonoBehaviour
{
    public static bool initedSound = false;

    public AudioClip bgmFlog;
    public AudioClip bgmBattle01;
    public AudioClip bgmBattle02;
    public AudioClip bgmVillage;
    public AudioClip bgmTitle;
    public AudioClip bgmDungeon23;
    public AudioClip bgmDungeon19;
    public AudioClip bgmTheme;
    public AudioClip bgmStormHill;
    public AudioClip bgmDawnHill;
    public AudioClip bgmLastBattle01;
    public AudioClip bgmLastBattle02;
    public AudioClip bgmTheme11b;

    public AudioClip seStormRain;
    public AudioClip seStormWind03;
    public AudioClip seStormThunder01Goro;
    //public AudioClip seStormThunder03Pika;
    
    public static SoundManager soundManager;

    public static BGMType playingBGM = BGMType.None;

    public float wind03MinTime = 3.0f;
    public float wind03MaxTime = 6.0f;
    public float thunder01GoroMinTime = 3.0f;
    public float thunder01GoroMaxTime = 6.0f;
    //public float thunder03PikaMinTime = 3.0f;
    //public float thunder03PikaMaxTime = 6.0f;

    private void Awake()
    {
        if(soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
            initedSound = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OverlapFlog()
    {
        GetComponent<AudioSource>().PlayOneShot(bgmFlog);
        Invoke("OverlapFlog", 13.7f);
    }

    public void OverlapBattleSecond()
    {
        GetComponent<AudioSource>().PlayOneShot(bgmBattle02);
        Invoke("OverlapBattleSecond", 29.538f);
    }

    public void  OverlapBattleFirst()
    {
        GetComponent<AudioSource>().PlayOneShot(bgmBattle01);
        Invoke("OverlapBattleSecond", 7.385f);
    }

    public void OverlapLastBattleSecond()
    {
        GetComponent<AudioSource>().PlayOneShot(bgmLastBattle02);
        Invoke("OverlapLastBattleSecond", 57.6f);
    }

    public void  OverlapLastBattleFirst()
    {
        GetComponent<AudioSource>().PlayOneShot(bgmLastBattle01);
        Invoke("OverlapLastBattleSecond", 16.8f);
    }

    public void  OverlapStormHill()
    {
        GetComponent<AudioSource>().PlayOneShot(bgmStormHill);
        Invoke("OverlapStormHill", 28.0f);
    }

    public void  OverlapDawnHill()
    {
        GetComponent<AudioSource>().PlayOneShot(bgmDawnHill);
        Invoke("OverlapDawnHill", 34.286f);
    }

    IEnumerator PlaySeEnvironment(AudioClip se, float minTime, float maxTime)
    {
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        GetComponent<AudioSource>().PlayOneShot(se);
        StartCoroutine(PlaySeEnvironment(se, minTime, maxTime));
    }

    public void PlayBgm(BGMType type)
    {
        if(type != playingBGM)
        {
            StopBgm();
            playingBGM = type;
            AudioSource audio = GetComponent<AudioSource>();
            if(type== BGMType.None)
            {
                audio.clip = null;
            }
            else if(type == BGMType.Village)
            {
                audio.clip = bgmVillage;
            }
            else if(type == BGMType.Title)
            {
                audio.clip = bgmTitle;
            }
            else if(type == BGMType.Dungeon23)
            {
                audio.clip = bgmDungeon23;
            }
            else if(type == BGMType.Dungeon19)
            {
                audio.clip = bgmDungeon19;
            }
            else if(type == BGMType.Rain)
            {
                audio.clip = seStormRain;//効果音だがBGMのPlayで再生
            }
            else if(type == BGMType.Theme)
            {
                audio.clip = bgmTheme;
            }
            else if(type == BGMType.Theme11b)
            {
                audio.clip = bgmTheme11b;
            }
            audio.Play();
        }
        //Debug.Log("PlayBgm終了");
    }

    public void PlayBgmOverlap(BGMType type)
    {
        playingBGM = type;
        if(type == BGMType.Flog)
        {
            OverlapFlog();
        }
        else if(type == BGMType.Battle)
        {
            //GetComponent<AudioSource>().PlayOneShot(bgmBattle01);
            //Invoke("OverlapBattleSecond", 7.385f);
            OverlapBattleFirst();//上2行でなく別の関数で処理するとInvokeのズレがなくなる
        }
        //Debug.Log("PlayBgmOverlap終了");
    }

    public void PlayBgmFromString(string bgmType)
    {
        if(bgmType == "Flog")
        {
            PlayBgmOverlap(BGMType.Flog);
        }
        else if(bgmType == "Battle")
        {
            PlayBgmOverlap(BGMType.Battle);
        }
        else if(bgmType == "Village")
        {
            PlayBgm(BGMType.Village);
        }
        else if(bgmType == "TestBattle01")
        {
            GetComponent<AudioSource>().PlayOneShot(bgmBattle01);
        }
        else if(bgmType == "TestBattle02")
        {
            GetComponent<AudioSource>().PlayOneShot(bgmBattle02);
        }
        else if(bgmType == "Title")
        {
            PlayBgm(BGMType.Title);
        }
        else if(bgmType == "Dungeon23")
        {
            PlayBgm(BGMType.Dungeon23);
        }
        else if(bgmType == "Dungeon19")
        {
            PlayBgm(BGMType.Dungeon19);
        }
        else if(bgmType == "Storm")
        {
            Random.InitState(System.DateTime.Now.Millisecond);//乱数を使用するためシードを現在時刻で指定
            PlayBgm(BGMType.Rain);
            OverlapStormHill();
            GetComponent<AudioSource>().PlayOneShot(seStormWind03);
            GetComponent<AudioSource>().PlayOneShot(seStormThunder01Goro);
            StartCoroutine(PlaySeEnvironment(seStormWind03, wind03MinTime, wind03MaxTime));
            StartCoroutine(PlaySeEnvironment(seStormThunder01Goro, thunder01GoroMinTime, thunder01GoroMaxTime));
            //StartCoroutine(PlaySeEnvironment(seStormThunder03Pika, thunder03PikaMinTime, thunder03PikaMaxTime));
        }
        else if(bgmType == "Theme")
        {
            PlayBgm(BGMType.Theme);
        }
        else if(bgmType == "DawnHill")
        {
            if(BGMType.DawnHill != playingBGM)
            {
                playingBGM = BGMType.DawnHill;
                OverlapDawnHill();
            }
        }
        else if(bgmType == "LastBattle")
        {
            if(BGMType.LastBattle != playingBGM)
            {
                playingBGM = BGMType.LastBattle;
                OverlapLastBattleFirst();
            }
        }
        else if(bgmType == "Theme11b")
        {
            PlayBgm(BGMType.Theme11b);
        }
        //Debug.Log("PlayBgmFromString終了");
    }

    public void StopBgm()
    {
        CancelInvoke();
        StopAllCoroutines();
        GetComponent<AudioSource>().Stop();
        playingBGM = BGMType.None;
    }
}
