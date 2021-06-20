using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmGameManager : MonoBehaviour
{
    public static RhythmGameManager rhythmGameManager { get; set; } //싱글톤 처리

    private void Awake()
    {
        if (rhythmGameManager == null)
            rhythmGameManager = this;

        else if (rhythmGameManager != this)
            Destroy(gameObject);
    }

    public bool autoPerfect; //자동 판정 모드(테스트를 위함), NoteBehavior에서 조작
    // 배포시 유니티에서 체크 해제하기

    public float noteSpeed; //노트가 떨어지는 스피드, 유니티에서 값 설정

    public enum judges { NONE = 0, BAD, GOOD, PERFECT, MISS };
    /* enum 자료형으로 노트 판정 라인 처리 상수로 관리 
    * BAD: 1
    * GOOD: 2
    * PERFECT: 3
    * MISS: 4 이다.
    * NONE은 디폴트 값, 즉 노트가 판정선에 닿기 이전 값
    */

    public GameObject[] trails; //유니티에서 각 라인 트레일 UI 넣기, 크기는 4가 됨
    private SpriteRenderer[] trailSpriteRenderers; //트레일 UI의 컴포넌트인 렌더러에 접근하여 조작
    // 알파(투명도)값 조작하여 각 라인 이벤트 이펙트를 주기 위함

    public GameObject scoreUI; //유니티에서 스코어UI 넣기
    public float score; //리듬게임 사용자 점수
    private Text scoreText;

    public GameObject comboUI; //유니티에서 콤보UI 넣기
    private int combo; //리듬게임 사용자 콤보
    private Text comboText;
    private Animator comboAnimator; //콤보 애니메이션

    public Text JudgeUI; //유니티에서 판정 결과 UI 넣기
    private Animator judgementSpriteAnimator; //판정 결과 애니메이션

    //BGM 음악(한서대 교가) 변수
    private AudioSource audioSource;
    private string gameMusic = "Hanseo Music 3"; //3절까지 있는 곡
    private string tutorialMusic = "Hanseo Music 1"; //1절까지 있는 곡

    // 작성한 비트 text 파일, NoteController.cs에서 불러옴
    public string gamebeat = "Game Beat";
    public string tutorialbeat = "Tutorial Beat";

    public int maxCombo;


    // 교가(BGM) 실행 함수
    void MusicStart()
    {
        AudioClip audioClip;

        if (PlayerInformation.tutorialselected) //튜토리얼 모드라면
        {
            //리소스 폴더의 비트(Beats) 폴더 안에 있는 튜토리얼(1절) mp3곡 선택
            audioClip = Resources.Load<AudioClip>("Beats/" + tutorialMusic);
        }
        else
        {
            //리소스 폴더의 비트(Beats) 폴더 안에 있는 교가 mp3(3절) 선택
            audioClip = Resources.Load<AudioClip>("Beats/" + gameMusic);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip; //오디오 소스 컴포넌트의 클립으로 재생 곡 설정
        audioSource.Play(); //실행
    }

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = noteObjectPooler.GetComponent<NoteObjectPooler>();

        Invoke("MusicStart", 2); //곡은 게임 시작 이후 2초 뒤에 나오도록 설정

        judgementSpriteAnimator = JudgeUI.GetComponent<Animator>();
        scoreText = scoreUI.GetComponent<Text>();
        comboText = comboUI.GetComponent<Text>();
        comboAnimator = comboUI.GetComponent<Animator>();

        trailSpriteRenderers = new SpriteRenderer[trails.Length]; //trails[]의 크기는 4
        for(int i = 0; i < trails.Length; i++)
        {
            trailSpriteRenderers[i] = trails[i].GetComponent<SpriteRenderer>(); // 렌더러 조작할 수 있게 초기화
        }
    }

    public GameObject noteObjectPooler; //유니티에서 노트 컨트롤러 넣기
    private NoteObjectPooler objectPooler; //스크립트 처리

    // Update is called once per frame
    void Update()
    {
        //모바일 환경에서 터치가 동시에 이루어졌다면 터치 카운터는 동시 터치 수가 된다.
        if (Input.touchCount > 0) //터치가 한개 이상 발생하고 있다면
        {
            for(int i = 0; i < Input.touchCount; i++)
            {
                Touch tempTouch = Input.GetTouch(i); //모든 터치 검사하여 터치 객체에 할당

                if(tempTouch.phase == TouchPhase.Began) //터치가 발생하기 시작했다면
                {
                    Ray ray = Camera.main.ScreenPointToRay(tempTouch.position); //터치가 발생한 위치 가져오기
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity)) //박스 콜라이더가 적용된 오브젝트 출동 감지 했다면
                    {
                        //충돌 발생한 오브젝트, UI는 hit에 담김
                        // 트레일 UI에 박스 콜라이더 추가하기, 2D말고 3D로
                        // 레이캐스트는 3D 전용 라이브러리여서 2D로 콜라이더 추가하면 추가작업 해주어야 됨
                        if (hit.collider.name == "Trail 1")
                        {
                            ShineTrail(0);
                            objectPooler.Judge(1); //오브젝트 풀 cs파일의 판정처리 함수 호출
                                                   // 활성화 된 노트들만 적용하기 위함
                        }
                        if (hit.collider.name == "Trail 2")
                        {
                            ShineTrail(1);
                            objectPooler.Judge(2);
                        }
                        if (hit.collider.name == "Trail 3")
                        {
                            ShineTrail(2);
                            objectPooler.Judge(3);
                        }
                        if (hit.collider.name == "Trail 4")
                        {
                            ShineTrail(3);
                            objectPooler.Judge(4);
                        }
                    }
                }
            }
        }

        /*
        // 리듬게임은 화면을 한번에 여러번 터치해야 되어서 인덱스 0~ 3 넣어줌
        // 총 4번까지 화면 동시 터치 했을 때 해당 터치에 대해 처리해 줄 수 있음
        // 아래 코드는 모바일에서도 동작하나 최적화 된 코드 아님 PC에 최적화임
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) ||
            Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(3))
        {
            
        }   
        */

        // 사용자가 입력한 키에 해당하는 라인을 빛나게 처리
        if (Input.GetKey(KeyCode.D))
            ShineTrail(0);
        if (Input.GetKey(KeyCode.F))
            ShineTrail(1);
        if (Input.GetKey(KeyCode.J))
            ShineTrail(2);
        if (Input.GetKey(KeyCode.K))
            ShineTrail(3);

        // 빛나게 된 라인은 다시 원상태로 돌려놓기
        // Update는 매 프레임마다 실행되기 떄문에 한번에 원상태로 돌리는 것이 아니라
        // 점점 알파값을 감소시켜서 원상태로 돌림
        for(int i = 0; i < trailSpriteRenderers.Length; i++)
        {
            Color color = trailSpriteRenderers[i].color;
            color.a -= 0.01f; //알파값 조작
            trailSpriteRenderers[i].color = color; //알파값 적용
        }

    }

    // 해당 라인을 빛나게 해주는 함수
    public void ShineTrail(int index)
    {
        Color color = trailSpriteRenderers[index].color;
        color.a = 0.32f; //알파값 조작
        trailSpriteRenderers[index].color = color; //알파값 적용
    }

    // 노트 판정 진행, 이 함수는 NoteBehavior.cs에서 매개변수 값(판정값, 라인)을 설정해 호출됨 
    public void processJudge(judges judge, int noteType)
    {
        if(judge == judges.NONE) //판정선 값이 널이라면, 즉 노트가 판정선에 닿지 않았다면
            return;  //그냥 리턴

        //Miss 판정을 받은 경우
        if(judge == judges.MISS)
        {
            JudgeUI.text = "Miss"; //miss 이미지로 바꾸기
            combo = 0; //콤보 초기화

            if (score >= 15) //점수 감소, 음수는 못나오게 15이상일 떄 15감소
                score -= 15;
        }
        else if(judge == judges.BAD) //BAD 판정을 받은 경우
        {
            JudgeUI.text = "Bad";
            combo = 0;

            if (score >= 5)
                score -= 5;
        }
        else //퍼펙트 or 굿 판정을 받은 경우
        {
            if(judge == judges.PERFECT) //퍼펙트
            {
                JudgeUI.text = "Perfect";
                score += 20; //스코어 증가
            }
            else if (judge == judges.GOOD) //굿
            {
                JudgeUI.text = "Good"; ;
                score += 15; //스코어 증가
            }
            combo += 1;
            score += (float)combo * 0.1f; //가산점 부여,
            //현재 100콤보라면 10 추가점수
        }
        showJudgment(); //판정 결과 출력 함수 호출
    }

    // 노트 판정 결과 화면에 출력
    void showJudgment()
    {
        scoreText.text = "점수: " + ((int)score).ToString(); //점수 적용

        //노트 판정 결과 애니메이션 출력
        judgementSpriteAnimator.SetTrigger("Show"); //애니메이터에서 설정한 판정 트리거 불러오기

        // 콤보가 2 이상일 때만 콤보 애니메이션 출력
        if(combo >= 2)
        {
            comboText.text = "콤보 " + combo.ToString();
            comboAnimator.SetTrigger("Show");
        }

        if(maxCombo < combo) //최대 콤보 갱신, 최종 결과 화면에서 보여주기 위해
        {
            maxCombo = combo;
        }
    }
}
