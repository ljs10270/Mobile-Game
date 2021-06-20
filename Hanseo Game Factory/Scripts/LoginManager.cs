using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Threading.Tasks;

public class LoginManager : MonoBehaviour
{
    // 이 cs 파일은 Login씬의 Login Manager 오브젝트 생성해서 적용해주기
    private FirebaseAuth auth; //파이어베이스 인증 기능 객체

    public InputField emailInputField;
    public InputField passwordInputField; //이메일, 패스워드 UI
    // 유니티에서 이메일, 패스워드 인풋 필드 드래그 해서 대입하기

    public Text messageUI; //로그인 결과를 보여주는 UI
    // 유니티에서 메시지 UI 드래그해서 대입하기

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1200, true); //1920*1200 풀스크린

        auth = FirebaseAuth.DefaultInstance; //파이어베이스 인증 객체 초기화
        messageUI.text = "";
    }

    public void Login() //유니티에서 로그인 버튼에 온클릭 매개변수로 넣기
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        // 인증 객체를 이용해 로그인 수행(SignIn)
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(
            task =>
            {
                if(task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                {
                    //현재 사용자의 아이디를 파이어베이스에서 가져옴
                    //Debug.Log("[로그인 성공] ID: " + auth.CurrentUser.UserId);
                    PlayerInformation.auth = auth;

                    PlayerInformation.createUserTipFile(); //팁 관리 파일 생성

                    SceneManager.LoadScene("StageSelectScene");
                }
                else
                {
                    messageUI.text = "계정을 다시 확인해주세요";
                }
            },TaskScheduler.FromCurrentSynchronizationContext()
        );
    }

    public void GoToJoin() //유니티에서 회원가입 버튼에 넣기
    {
        SceneManager.LoadScene("JoinScene");
    }

    public void Exit()
    {
        Application.Quit(); //프로그램 종료, 유니티상에서는 동작안한다.
        //실제 배포하면 동작함
    }
}
