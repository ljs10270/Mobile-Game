using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //게임화면으로 전환을 위해 
using UnityEngine.UI;
using System.IO;

public class TipViewManager : MonoBehaviour
{
    public Text tipTitle;
    public Text tipContent;
    public Text buttonText;

    // Start is called before the first frame update
    void Start()
    {
        string tipNum = PlayerInformation.getTip();

        if(tipNum == "0")
        {
            tipTitle.text = "비정상적인 접근 입니다.";
        }
        else
        {
            tipTitle.text = tipNum + " 번째 정보 획득";
        }

        if (tipNum == "6") // 팁이 매우 길어 폰트 사이즈 줄이기
        {
            tipContent.fontSize = 50;
        }
        else
        {
            tipContent.fontSize = 60;
        }

        TextAsset textAsset = Resources.Load<TextAsset>("Tips/" + tipNum);
        StringReader stringReader = new StringReader(textAsset.text);

        tipContent.text = stringReader.ReadToEnd(); //전체 읽기

        if(PlayerInformation.quizGameClear && PlayerInformation.defenseGameClear && PlayerInformation.rhythmGameClear)
        {
            buttonText.text = "랭킹 확인";
        }
        else
        {
            buttonText.text = "계속하기";
        }
    }

    public void TipExit() //게임엑시트 버튼의 OnClick()에 넣기
    {
        if(PlayerInformation.quizGameClear && PlayerInformation.defenseGameClear && PlayerInformation.rhythmGameClear)
        {
            SceneManager.LoadScene("GameResultScene");
        }
        else
        {
            SceneManager.LoadScene("StageSelectScene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
