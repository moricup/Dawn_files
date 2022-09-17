using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExtraManager : MonoBehaviour
{
    public GameObject uiPanel;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.soundManager.PlayBgmFromString("Theme");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InversionUI()
    {
        uiPanel.SetActive(!(uiPanel.activeInHierarchy));//Canvasの表示非表示を反転
        SeManager.seManager.PlaySeFromString("Retro16");//ボタン押しin MessageScene
    }
}
