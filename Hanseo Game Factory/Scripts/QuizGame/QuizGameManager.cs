using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //게임화면으로 전환을 위해 
using UnityEngine.UI;
using System.IO;

public class QuizGameManager : MonoBehaviour //싱글톤으로 퀴즈 게임 관리
{
    public static QuizGameManager quizGameManager { get; set; } //싱글톤 기법 사용, AnswerCheck.cs파일에 적용함

    private bool tutorialselected;
    private IEnumerator CountUpdate;

    public Text Count;
    public Text Question;
    public Image Answercheck;
    public Text AnswercheckText;
    public Text problem1;
    public Text problem2;
    public Text problem3;
    public Text AnswerMessage;
    public Button button;
    public Text Buttontext;

    private string answer; //텍스트에서 정답 받아와 검사

    private int answerCount; //문제 추가하고 정답 수 체크
    private List<int> problemList = new List<int>(); //제네릭 int로 <> 안하면 오브젝트형이라 반환 귀찮

    private void Awake()
    {
        if (quizGameManager == null) //씬(화면)이 바뀌면 싱글톤은 자동으로 널이 된다.
        {
            quizGameManager = this; //this = 오브젝트 자기 자신인 QuizGameManager로 인스턴스 초기화
                                          //즉, 이 클래스든 다른 클래스든 quizGameManager 변수에 접근할 경우 QuizGameManager로 바로 초기화 하겠다는 의미
                                          // quizGameManager 변수는 QuizGameManager가 됨.
                                          // MonoBehaviour의 Awake는 객체가 생성될 떄 생성자가 호출되는 시점과 동일하게 호출됨(new 사용 필요 없음)
        }
    }

    private void Quiz(int index)
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Quiz/" + index.ToString());
        StringReader stringReader = new StringReader(textAsset.text);
        Question.text = stringReader.ReadLine().Replace("_", " ");
        problem1.text = stringReader.ReadLine().Replace("_", " ");
        problem2.text = stringReader.ReadLine().Replace("_", " ");
        problem3.text = stringReader.ReadLine().Replace("_", " ");
        answer = stringReader.ReadLine().Replace("_", " ");

        Count.text = "60";
        AnswercheckText.text = "정답을 이곳에 드래그 하세요";
        AnswerMessage.text = "";
        Buttontext.text = "게임 중";

        CountUpdate = Countupdate(1.0f);
        StartCoroutine(CountUpdate);
    }

    // 랜덤 생성 (중복 배제)
    void CreateUnDuplicateRandom(int min, int max)
    {
        int currentNumber = Random.Range(min, max);

        for (int i = 0; i < 6;) //6문제만 출제할 것.
        {
            if (problemList.Contains(currentNumber))
            {
                currentNumber = Random.Range(min, max);
                continue;
            }
            else
            {
                problemList.Add(currentNumber);
                i++;
            }
        }
    }

    void ChangeProblem()
    {
        Quiz(problemList[answerCount]);
    }

    // Start is called before the first frame update
    void Start()
    {
        tutorialselected = PlayerInformation.tutorialselected;

        answerCount = 0; //문제 푼 횟수 체크

        button.interactable = false; //버튼 비활성화

        if (tutorialselected)
        {
            Quiz(0);
        }
        else
        {
            CreateUnDuplicateRandom(1, 15); //실제 게임 문제 수 14개, 중복 없이 랜덤 값으로 리스트 대입
            ChangeProblem();
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private IEnumerator Countupdate(float coolTime)
    {
        while (int.Parse(Count.text) > 0)
        {
            yield return new WaitForSeconds(coolTime); //coolTime초 후 다시 while문 실행됨
            Count.text = (int.Parse(Count.text) - 1).ToString();
        }
        AnswerResult(); //시간 초과
    }

    public void AnswerResult() //버튼 UI의 OnClick에 드래그 하여 이 함수 넣기
    {
        
        if (AnswercheckText.text == "정답을 이곳에 드래그 하세요")
        {
            AnswerMessage.text = "시간 초과";
            button.interactable = false; //버튼 비활성화
        }

        if (tutorialselected)
        {
            if(AnswercheckText.text == answer)
            {
                AnswerMessage.text = "정답입니다";
            }
            else if(AnswerMessage.text  != "시간 초과")
            {
                AnswerMessage.text = "틀렸습니다. 정답은 " + answer + " 입니다.";
            }
            button.interactable = false; //버튼 비활성화
            StopCoroutine(CountUpdate); //코루틴 종료
            Buttontext.text = "잠시 후 게임이 종료됩니다.";
            StartCoroutine("Exit");
        }
        else
        {  
            if (AnswercheckText.text == answer)
            {
                AnswerMessage.text = "정답입니다";
                PlayerInformation.gameScore += 250;
                PlayerInformation.quizScore += 250;
            }
            else if(AnswerMessage.text != "시간 초과")
            {
                AnswerMessage.text = "틀렸습니다. 정답은 " + answer + " 입니다.";
            }
            button.interactable = false; //버튼 비활성화
            answerCount++;

            if(answerCount == 6) //총 6문제 출제 후 7문제가 된다면
            {
                PlayerInformation.quizGameClear = true;
                StopCoroutine(CountUpdate); //코루틴 종료

                PlayerInformation.UpdateTip(); //팁 확인

                Buttontext.text = "잠시 후 게임이 종료됩니다.";
                StartCoroutine("Exit");
            }
            else
            {
                StopCoroutine(CountUpdate); //코루틴 종료
                Invoke("ChangeProblem", 2); // 정답 확인과 메시지 바로 다음문제로 넘어가면 사라져서 따로 함수로 만들어줌
                // 2초뒤 호출
            }
        }
    }

    private IEnumerator Exit()
    {
        yield return new WaitForSeconds(3.0f);

        if(tutorialselected || PlayerInformation.fullTipUser)
        {
            SceneManager.LoadScene("StageSelectScene");
        }
        else
        {
            SceneManager.LoadScene("TipViewScene");
        }
        StopCoroutine("Exit");
    }
}
