using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //게임화면으로 전환을 위해 
using UnityEngine.UI;

//첫 시작 씬의 StartManager 오브젝트에 들어가는 코드
public class QuizStartManager : MonoBehaviour
{
    public Text title; //이전 게임 클리어 여부에 따라 리듬게임 플레이 못한다고 보여주기

    public void TutorialgameStart() //게임스타트 버튼의 OnClick()에 넣기
    {
        PlayerInformation.tutorialselected = true;
        SceneManager.LoadScene("QuizGameScene"); //게임화면 불러오기
        Time.timeScale = 1;
    }

    public void gameStart() //게임스타트 버튼의 OnClick()에 넣기
    {
        if(PlayerInformation.quizGameClear == false)
        {
            PlayerInformation.tutorialselected = false;
            SceneManager.LoadScene("QuizGameScene"); //게임화면 불러오기
            Time.timeScale = 1;
        }
        else
        {
            title.fontSize = 50;
            title.text = "이학관(퀴즈 게임)을 이미 클리어 하셨습니다.\n다음 게임을 진행해 주세요.";
        }
    }

    public void gameExit() //게임엑시트 버튼의 OnClick()에 넣기
    {
        SceneManager.LoadScene("StageSelectScene");
        //Application.Quit(); //프로그램 종료, 유니티상에서는 동작안한다.
        //실제 배포하면 동작함
    }

    public void Instruction() //설명서 이동
    {
        SceneManager.LoadScene("GameInstructionScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerInformation.tutorialselected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
