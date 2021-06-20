using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //화면전환

//메뉴 버튼 UI에 들어가는 C# 파일
public class MenuManager : MonoBehaviour
{
    bool pause = false; //메뉴를 누르면 게임이 정지 조작하는 변수
    // true가 정지되어 있는 상태
    // 유니티에서 메뉴 버튼의 OnClick()에 메뉴 버튼 UI 넣고 PauseSwitch() 넣기
    // 또한 메뉴Canvas의 클로즈 버튼에도 OnClick()에 메뉴 버튼 UI 넣기

    public GameObject menuCanvas; //유니티에서 메뉴 버튼에 메뉴Canvas 넣기

    // Start is called before the first frame update
    void Start()
    {
        menuCanvas.SetActive(false); //처음에는 메뉴 canvas가 보이지 않도록 설정(비활성화)
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameReset() //메뉴Canvas의 리셋버튼에 이 함수 Onclick으로 넣어주기
    {
        SceneManager.LoadScene("DefenseGameScene"); //유니티에서 씬 이름 게임씬으로 바꿔주기
        //게임을 다시 불러옴(즉 다시시작)
        Time.timeScale = 1; //유니티는 계속 동작하게 만들어야 적용됨
    }

    public void pauseSwitch() //메뉴 닫기 버튼에 넣기
    {
        if(pause) //만약 게임이 정지되어 있는 상태라면
        {
            pause = false; //정지 풀고
            Time.timeScale = 1; //유니티에서 시간이 정상적으로 흘러가게 만듬
                                //timeScale는 유니티 내에서 시간이 흘러가는 것을 의미, 1은 원래 속도로 진행, 0은 정지
            menuCanvas.SetActive(false); //메뉴켄버스 비활성화
        }
        else //정지상태가 아니면
        {
            pause = true; //정지 
            Time.timeScale = 0; //유니티 정지
            menuCanvas.SetActive(true); //메뉴켄버스 비활성화
        }
    }

    public void startMenu()
    {
        SceneManager.LoadScene("StageSelectScene");
        //시작화면 돌아가기 동작 함수, 메뉴Canvas의 Init 버튼 UI의 Onclick()에
        //기존 Canvas의 메뉴버튼(이파일이 적용된)UI를 넣고 이 함수 적용하기
    }
}
