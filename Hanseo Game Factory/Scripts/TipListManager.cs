using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //게임화면으로 전환을 위해 
using UnityEngine.UI;
using System.IO;
using System;

public class TipListManager : MonoBehaviour
{
    public List<Text> tipContent; //텍스트 12개 드래그, 유니티에서 사이즈 12로
    List<string> tipList;

    // Start is called before the first frame update
    void Start()
    {
        tipList = new List<string>();

        tipList = PlayerInformation.getTipList();

        if(tipList.Count != 0)
        {
            for(int i = 0; i < tipList.Count; i++)
            {
                TextAsset textAsset = Resources.Load<TextAsset>("Tips/" + tipList[i]);
                StringReader stringReader = new StringReader(textAsset.text);

                tipContent[i].text = i + 1 + ". " + stringReader.ReadToEnd()
                    + "\n\n----------------------------------------------------------------------------------"; //전체 읽기
            }
        }
        else
        {
            for (int i = 0; i < tipContent.Count; i++)
            {
                tipContent[i].text = i + 1 + ". 게임을 플레이하여 정보를 획득하세요. (튜토리얼은 정보가 제공되지 않습니다.)";
            }
        }
    }

    public void TipListExit() //버튼의 OnClick()에 넣기
    {
        SceneManager.LoadScene("StageSelectScene");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
