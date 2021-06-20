using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JoinManager : MonoBehaviour
{
    // 이 cs 파일은 Join 씬의 Join Manager 오브젝트 생성해서 적용해주기
    private FirebaseAuth auth; //파이어베이스 인증 기능 객체

    public InputField emailInputField;
    public InputField passwordInputField; //이메일, 패스워드 UI
    // 유니티에서 이메일, 패스워드 인풋 필드 드래그 해서 대입하기

    public Text messageUI; //회원가입 결과를 보여주는 UI
    // 유니티에서 메시지 UI 드래그해서 대입하기

    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance; //파이어베이스 인증 객체 초기화
        messageUI.text = "";
    }

    bool InputCheck()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        if (email.Length < 8)
        {
            messageUI.text = "email은 8자 이상이어야 합니다.";
            return false;
        }
        else if (password.Length < 6)
        {
            messageUI.text = "password는 6자 이상이어야 합니다.";
            return false;
        }
        messageUI.text = "";
        return true;
    }

    public void Check() //사용자가 인풋텍스트에 입력할 때마다 보여주기 위함
    {
        InputCheck();
    }
    // 이메일, 패스워드 인풋 텍스트 UI의 On Value Changed에 JoinManager 오브젝트 넣고 이 함수 넣기

    public void Join() //회원가입. JoinManager 오브젝트를 가입하기 버튼 UI 온클릭 이벤트에 넣고 이 함수 넣기
    {
        if (!InputCheck())
        {
            return;
        }

        string email = emailInputField.text;
        string password = passwordInputField.text;

        //회원가입 결과가 task에 담기게 된다. 인증 데이터 새롭게 생성
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
            task =>
            {
                if (!task.IsCanceled && !task.IsFaulted)
                {
                    Debug.Log("회원가입 성공" + email);
                    SceneManager.LoadScene("LoginScene");
                }
                else
                {
                    messageUI.text = "입력하신 email이 이미 사용중이거나 형식이 바르지 않습니다.";
                }
            }
        );
    } //회원가입 실패, 파이어베이스는 기본적으로 같은 이메일이 들어갈 수 없다. 따로 코딩 안해주어도 된다.

    public void Back() //뒤로가기 UI 버튼에 넣기
    {
        SceneManager.LoadScene("LoginScene");
    }
}
