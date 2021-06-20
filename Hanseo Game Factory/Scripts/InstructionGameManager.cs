using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //게임화면으로 전환을 위해 

public class InstructionGameManager : MonoBehaviour
{
    public Text userId;

    // Start is called before the first frame update
    void Start()
    {
        userId.text = PlayerInformation.auth.CurrentUser.Email + "님\n한서대학교 게임공장에 오신 것을 환영합니다!";
        
    }

    public void Back()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
