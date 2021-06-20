using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO; //파일에서 게임 제목 등 정보 읽어오기
using System;

public class StageSelectManager : MonoBehaviour
{
    public Image stageImageUI; //선택한 스테이지 이미지
    public Text stageTitleUI;
    public Text gameContentUI; //선택한 스테이지의 게임 제목
    public Text score;
    public Text TextUI;

    private int selectIndex;
    private int selectCount = 3; //3개만 넘김(게임개수)

    private void UpdateSelect(int selectIndex) //게임을 선택하면 해당 스테이지 이미지 보여줌
    {
        if(selectIndex == 1)
        {
            stageTitleUI.text = "이학관";
            gameContentUI.text = "<퀴즈 게임>";
            stageImageUI.sprite = Resources.Load<Sprite>("Sprites/" + selectIndex.ToString());
        }

        else if (selectIndex == 2)
        {
            stageTitleUI.text = "보건관";
            gameContentUI.text = "<바이러스 Defense Game>";
            stageImageUI.sprite = Resources.Load<Sprite>("Sprites/" + selectIndex.ToString());
        }

        else if (selectIndex == 3)
        {
            stageTitleUI.text = "예술관";
            gameContentUI.text = "<리듬 게임>";
            stageImageUI.sprite = Resources.Load<Sprite>("Sprites/" + selectIndex.ToString());

            //리소스 폴더에서 Sprites 폴더에 있는 파일들 불러와 UI 변경
            //TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + selectIndex.ToString()); //텍스트 파일 불러옴
            //StringReader stringReader = new StringReader(textAsset.text); //텍스트 파일 읽는 객체 생성
        }
    }

    public void Right()
    {
        selectIndex = selectIndex + 1;

        if (selectIndex > selectCount)
            selectIndex = 1;

        UpdateSelect(selectIndex);
    }

    public void Left()
    {
        selectIndex = selectIndex - 1;

        if (selectIndex < 1)
            selectIndex = selectCount;

        UpdateSelect(selectIndex);
    }

    void Start()
    {
        selectIndex = 1;
        TextUI.text = "알림: 진행해야 되는 스테이지는 이학관 입니다.";

        if(PlayerInformation.quizGameClear)
        {
            selectIndex = 2;
            TextUI.text = "알림: 진행해야 되는 스테이지는 보건관 입니다.";

            if (PlayerInformation.defenseGameClear)
            {
                selectIndex = 3;
                TextUI.text = "알림: 진행해야 되는 스테이지는 예술관 입니다.";
            }
        }

        UpdateSelect(selectIndex);

        score.text = "현재 SCORE : " + PlayerInformation.gameScore.ToString();
        //모두 플레이 한 경험이 있는 사용자는 토탈 스코어와 랭킹도 보여주기로 코드 수정하기 
    }

    public void GameStart() //Start UI에 버튼 컴포넌트 만들어서 넣기
    {
        PlayerInformation.selectedGame = selectIndex.ToString();

        if (PlayerInformation.selectedGame == "1")
        {
            SceneManager.LoadScene("QuizStartScene");
        }
        else if(PlayerInformation.selectedGame == "2")
        {
            SceneManager.LoadScene("DefenseStartScene");
        }
        else if (PlayerInformation.selectedGame == "3")
        {
            SceneManager.LoadScene("RhythmStartScene");
        }
    }

    public void Instruction() //설명서 이동
    {
        SceneManager.LoadScene("InstructionScene");
    }

    public void tipList() //나의 수집함 이동
    {
        SceneManager.LoadScene("TipListScene");
    }

    public void showRank() //랭킹 보기 이동
    {
        SceneManager.LoadScene("RankShowScene");
    }

    public void OnClickEvnet()
    {
        string Email = "ljs1027s@naver.com";
        string Emailtitle = EscapeURL(PlayerInformation.auth.CurrentUser.Email + "님의 버그 및 요구사항/문의사항");
        string Emailcontent = EscapeURL
            (
             "버그 및 불편한 점, 추가 했으면 하는 점 등등 요구사항을 보내주세요.\n\n\n\n" +
             "********\n" +
             "Device Model : " + SystemInfo.deviceModel + "\n\n" +
             "Device OS : " + SystemInfo.operatingSystem + "\n\n" +
             "********\n\n"
            );
        Application.OpenURL("mailto:" + Email + "?subject=" + Emailtitle + "&body=" + Emailcontent);
    }

    private string EscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public void LogOut()
    {
        PlayerInformation.auth.SignOut(); //로그아웃
        SceneManager.LoadScene("LoginScene");
    }
}
