using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //게임화면으로 전환을 위해 
using UnityEngine.UI;

public class RhythmStartManager : MonoBehaviour
{
    public Text title; //리듬 게임을 클리어한 플레이어는 게임을 진행하지 못한다고 메시지 보여주기

    public void TutorialgameStart() //튜토리얼 스타트 버튼의 OnClick()에 넣기
    {
        PlayerInformation.tutorialselected = true;
        SceneManager.LoadScene("RhythmGameScene"); //게임화면 불러오기
    }

    public void gameStart() //게임스타트 버튼의 OnClick()에 넣기
    {
        if (PlayerInformation.defenseGameClear && PlayerInformation.quizGameClear)
        {
            PlayerInformation.tutorialselected = false;
            SceneManager.LoadScene("RhythmGameScene"); //게임화면 불러오기
        }
        else if (PlayerInformation.defenseGameClear == false && PlayerInformation.quizGameClear)
        {
            title.fontSize = 50;
            title.text = "보건관(바이러스 Defense Game)을 진행하지 않았습니다.\n이전 단계 게임을 진행해 주세요.";
        }
        else
        {
            title.fontSize = 50;
            title.text = "이전 단계 게임(이학관, 보건관)을 모두 진행해셔야 됩니다.\n이전 단계 게임을 진행해 주세요.";
        }
    }

    public void gameExit() //게임엑시트 버튼의 OnClick()에 넣기
    {
        SceneManager.LoadScene("StageSelectScene");
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


