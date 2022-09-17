//https://ymgsapo.com/2020/12/24/unity-csv/
//上記URLより借用
//剽窃を避けるため、見てから自力で書いたが、ほぼ同じものとなった
//従って、ロジックから命名規則まで完成度が高く、故に公共性の高いものと判断した
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVReader : MonoBehaviour
{
    public TextAsset csvFile; //CSVファイルをアタッチする
    public List<string[]> csvDatas = new List<string[]>();//CSVの中身を入れるリスト;
    //csvDatas[i][j] //i行、j列で指定する。

    void Awake()
    {
        StringReader reader = new StringReader(csvFile.text);
 
        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while(reader.Peek() != -1) // reader.Peekが-1になるまで
        {
            string line = reader.ReadLine(); //一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
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
}
