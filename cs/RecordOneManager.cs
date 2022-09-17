using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecordOneManager : MonoBehaviour
{
    public GameObject unknownObj;//???と表示するオブジェクト
    public TextMeshProUGUI winLoseText;//WinかLoseを表示するテキスト
    public GameObject twinkle;//Winの右上の光
    public GameObject resultSet;//結果部分をまとめた集合

    public int numEncounted;//当該バトルの遭遇フラグ番号
    public int numWon;//当該バトルの勝利フラグ番号
    public int numWonWithoutHeal;//当該バトルのヒール無し勝利フラグ番号

    // Start is called before the first frame update
    void Start()
    {
        if(FlagManager.flag[numEncounted])//当該バトルに遭遇しているか
        {
            unknownObj.SetActive(false);//???を消す

            if(FlagManager.flag[numWon])//当該バトルに勝利しているか
            {
                winLoseText.text = "WIN";//WINを表示
            }
            else
            {
                winLoseText.text = "LOSE";//LOSEを表示
            }

            if(FlagManager.flag[numWonWithoutHeal])//当該バトルにヒール無し勝利しているか
            {
                twinkle.SetActive(true);//キラキラを表示
            }

            resultSet.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
