using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeManager : MonoBehaviour
{
    public static bool initedSe = false;

    //se
    public AudioClip seBattle12;//キャラ衝突 in BattleScene
    public AudioClip seBattle18;//キャラがダメージを受ける in BattleScene
    public AudioClip seExplosion02;//キャラ消滅 in BattleScene
    public AudioClip seJingle;//戦闘勝利 in BattleScene
    public AudioClip seSystemDeter11;//ボタン押しin MessageScene
    public AudioClip seRetro16;//発言後のタップ in MessageScene
    public AudioClip seHeal01;//回復音 in BattleScene
    public AudioClip seWind03;//風 in MessageScene
    public AudioClip seThunder01Goro;//雷ゴロゴロ in MessageScene
    public AudioClip seThunder03Pika;//雷ピカ in MessageScene
    public AudioClip seJetThrough01;//S.O.I.L.飛行音
    public AudioClip seQuake;//S.O.I.L.発射音
    public AudioClip seQue;//ボタン押し in BattleScene

    public static SeManager seManager;

    private void Awake()
    {
        if(seManager == null)
        {
            seManager = this;
            DontDestroyOnLoad(gameObject);
            initedSe = true;
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

    public void PlaySeFromString(string seType)
    {
        if(seType == "Battle12")
        {
            GetComponent<AudioSource>().PlayOneShot(seBattle12);
        }
        else if(seType == "Battle18")
        {
            GetComponent<AudioSource>().PlayOneShot(seBattle18);
        }
        else if(seType == "Explosion02")
        {
            GetComponent<AudioSource>().PlayOneShot(seExplosion02);
        }
        else if(seType == "Jingle")
        {
            GetComponent<AudioSource>().PlayOneShot(seJingle);
        }
        else if(seType == "SystemDeter11")
        {
            GetComponent<AudioSource>().PlayOneShot(seSystemDeter11);
        }
        else if(seType == "Retro16")
        {
            GetComponent<AudioSource>().PlayOneShot(seRetro16);
        }
        else if(seType == "Heal01")
        {
            GetComponent<AudioSource>().PlayOneShot(seHeal01);
        }
        else if(seType == "Wind03")
        {
            GetComponent<AudioSource>().PlayOneShot(seWind03);
        }
        else if(seType == "Thunder01Goro")
        {
            GetComponent<AudioSource>().PlayOneShot(seThunder01Goro);
        }
        else if(seType == "Thunder03Pika")
        {
            GetComponent<AudioSource>().PlayOneShot(seThunder03Pika);
        }
        else if(seType == "JetThrough01")
        {
            GetComponent<AudioSource>().PlayOneShot(seJetThrough01);
        }
        else if(seType == "Quake")
        {
            GetComponent<AudioSource>().PlayOneShot(seQuake);
        }
        else if(seType == "Que")
        {
            GetComponent<AudioSource>().PlayOneShot(seQue);
        }
        //Debug.Log("PlaySEFromString終了");
    }

    public void StopSe()
    {
        GetComponent<AudioSource>().Stop();
    }
}
