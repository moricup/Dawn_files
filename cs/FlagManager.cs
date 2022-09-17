using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    public static bool[] flag = new bool[40];
    public static bool initedFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        if(initedFlag == false)
        {
            flag[0] = true;
            for(int i=1; i<flag.Length; i++)
            {
                flag[i] = false;
            }
            initedFlag = true;
        }
    }

    //flag[0]:恒等的にtrue
    //flag[1]:恒等的にfalse

    //flag[2]:test用

    //flag[3]:NORMAL END1を迎え、悪その1が解放されたか
    //flag[4]:NORMAL END2を迎え、悪その2が解放されたか
    //flag[5]:ENDを迎え、善その2が解放されたか
    //flag[6]:TRUE ENDを迎えたか

    //flag[7]:b000で1回勝利
    //flag[8]:b000で2回以上勝利、フレアのウィンドサイズ解放

    //flag[9]:b011で1回勝利
    //flag[10]:b011で2回以上勝利、フレアのソイルナックル解放

    //flag[11]:b041で1回勝利
    //flag[12]:b041で2回以上勝利、ウィンドのソイルナックル解放

    //flag[13]:b050で1回勝利
    //flag[14]:b050で2回勝利、ウィンドのアクアキャノン解放

    //flag[15]:b000に遭遇
    //flag[16]:b000で勝利
    //flag[17]:b000でヒール無し勝利

    //flag[18]:b010 or b011に遭遇
    //flag[19]:b010 or b011で勝利
    //flag[20]:b010 or b011でヒール無し勝利

    //flag[21]:b020に遭遇
    //flag[22]:b020で勝利
    //flag[23]:b020でヒール無し勝利

    //flag[24]:b030に遭遇
    //flag[25]:b030で勝利
    //flag[26]:b030でヒール無し勝利

    //flag[27]:b040 or b041に遭遇
    //flag[28]:b040 or b041で勝利
    //flag[29]:b040 or b041でヒール無し勝利

    //flag[30]:b050に遭遇
    //flag[31]:b050で勝利
    //flag[32]:b050でヒール無し勝利

    //flag[33]:b060に遭遇
    //flag[34]:b060で勝利
    //flag[35]:b060でヒール無し勝利


    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool ActivateExtra()
    {
        //全てのバトルでヒール無し勝利していたらエクストラボタン解放 in RecordScene (TitleManager.cs)
        bool activateExtra = true;
        activateExtra &= flag[17];
        activateExtra &= flag[20];
        activateExtra &= flag[23];
        activateExtra &= flag[26];
        activateExtra &= flag[29];
        activateExtra &= flag[32];
        activateExtra &= flag[35];
        return activateExtra;
    }
}
