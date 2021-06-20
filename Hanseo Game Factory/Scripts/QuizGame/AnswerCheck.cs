using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerCheck : MonoBehaviour //정답 확인 이미지에 넣기
{
  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Problem")
        {
            QuizGameManager.quizGameManager.AnswercheckText.text = other.gameObject.GetComponent<Text>().text;
            QuizGameManager.quizGameManager.Buttontext.text = "정답 확인";
            QuizGameManager.quizGameManager.button.interactable = true; //버튼 활성화
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
