using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public string nextScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load()
    {
        SoundManager.soundManager.StopBgm();
        SeManager.seManager.PlaySeFromString("SystemDeter11");//ボタン押しin MessageScene
        SceneManager.LoadScene(nextScene);
    }
}
