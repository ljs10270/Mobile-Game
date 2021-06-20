using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //게임화면으로 전환을 위해 
using UnityEngine.UI;

public class DefenseStartManager : MonoBehaviour
{
    public Text title; //디펜스 게임을 클리어한 플레이어는 게임을 진행하지 못한다고 메시지 보여주기

    public void TutorialgameStart() //튜토리얼 스타트 버튼의 OnClick()에 넣기
    {
        PlayerInformation.tutorialselected = true;
        SceneManager.LoadScene("DefenseGameScene"); //게임화면 불러오기
        Time.timeScale = 1;
    }

    public void gameStart() //게임스타트 버튼의 OnClick()에 넣기
    {
        if (PlayerInformation.defenseGameClear == false && PlayerInformation.quizGameClear)
        {
            PlayerInformation.tutorialselected = false;
            SceneManager.LoadScene("DefenseGameScene"); //게임화면 불러오기
            Time.timeScale = 1;
        }
        else if(PlayerInformation.defenseGameClear)
        {
            title.fontSize = 50;
            title.text = "보건관(바이러스 Defense Game)을 이미 클리어 하셨습니다.\n다음 게임을 진행해 주세요.";
        }
        else if (PlayerInformation.quizGameClear == false)
        {
            title.fontSize = 50;
            title.text = "이학관(퀴즈 게임)을 진행하지 않았습니다.\n이전 단계 게임을 진행해 주세요.";
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

