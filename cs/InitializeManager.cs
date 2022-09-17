using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InitializeManager : MonoBehaviour
{
    public static bool allInited = false;
    public TextMeshProUGUI initText;
    public GameObject titleButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(FlagManager.initedFlag)
        {
            if(SoundManager.initedSound)
            {
                if(SeManager.initedSe)
                {
                    if(allInited == false)
                    {
                        allInited = true;
                        initText.text = "initialized!";
                        titleButton.SetActive(true);
                    }
                }
            }
        }
    }
}
