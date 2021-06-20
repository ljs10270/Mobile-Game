using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Game Manager 빈 오브젝트에 적용되는 게임의 총 관리 C# 파일
public class DefenseGameManager : MonoBehaviour
{
    public static DefenseGameManager defenseGameManager { get; set; } //싱글톤 기법 적용

    private void Awake() //모노 비헤이버를 상속하면 Awake를 생성자 처럼 사용
    {
        if (defenseGameManager == null) //씬(화면)이 바뀌면 싱글톤은 자동으로 널이 된다.
        {
            defenseGameManager = this; //this = 오브젝트 자기 자신인 DefenseGameManager로 인스턴스 초기화
                                       //즉, 이 클래스든 다른 클래스든 defenseGameManager 변수에 접근할 경우 DefenseGameManager로 바로 초기화 하겠다는 의미
                                       // defenseGameManager 변수는 DefenseGameManager가 됨.
                                       // MonoBehaviour의 Awake는 객체가 생성될 떄 생성자가 호출되는 시점과 동일하게 호출됨(new 사용 필요 없음)
        }
    }

    public Text seedText; //(골드) 변수
                          //Game Manager 오브젝트에 적용된 이 파일의 위의 변수에
                          //seedImage UI의 (골드) Text를 유니티에서 드래그 해주어야 함
    public int seed = 1000; //처음 시작하는 (골드) 변수

    public Text roundText; //라운드 표시 버튼UI의 텍스트 조작 변수, 버튼 UI의 텍스트 UI 유니티에서 넣어줘야 함
    public Text roundSartText; // 애니메이션 라운드 텍스트 조작
    public int round = 0;

    private AudioSource audioSource; //라운드 시작할 떄마다 오디오 실행을 위한 변수

    public int roundReadyTime = 5; //라운드가 끝났을 떄 다음 라운드까지 대기시간
    public int totalRound = 10; //전체 라운드 수, 유니티에서 10으로 바꿈
    public int reward = 500; //라운드를 꺨 때마다 골드 500 얻는 변수
    public float spawnTime = 2.5f; //몬스터가 리스폰되는 시간
    public int spawnNumber = 5; //몬스터가 한 라운드에 출몰하는 수(변경됨)

    public int nowSelect; //현재 선택된 캐릭터 변수
    public Image select1; //유니티에서 캐릭터1 버튼 UI 드래그해서 넣기
    public Image select2;

    public Text ClearText; //유니티에서 클리어 텍스트 드래그해서 넣기
    public Text lifeText; //유니티에서 라이프 UI의 텍스트 넣기

    public int life = 10; //목숨
    public Text loseText; // 패배 메시지, 유니티에서 루즈텍스트 UI 넣기

    public GameObject respawnSpots; //유니티에서 리스폰스팟 오브젝트 넣기

    //오브젝트 풀링 기법 적용을 위한 가장 많이 사용되는 오브젝트 체크, 이제 오브젝트 풀링 기법 적용해서 안됨
    //public int bulletAddCount = 0;
    //public int characterAddCount = 0;
    //public int monsterAddCount = 0;

    public int decreaseLife() //태극기에 닿으면 목숨 감소 함수
    {
        if(life >= 1)
        {
            life = life - 1;
            lifeText.text = ": " + life;

            if(life == 0) //패배했다면
            {
                loseText.enabled = true;
                respawnSpots.GetComponent<CreateMonster>().enabled = false; //끝났으니 몬스터 출몰 x

                if (PlayerInformation.tutorialselected == false)
                    PlayerInformation.defenseGameClear = true;
            }
        }
        return life; //남아있는 목숨 반환
    }

    public void gameClear() //마지막 라운드까지 클리어했을 시 or 패비시
    {
        if(life != 0)
            ClearText.enabled = true;

        if(PlayerInformation.tutorialselected == false)
        {
            if(round > 4) //그냥 가만히 있고 골드로 점수만 챙기려고 하는 것 방지
            {
                PlayerInformation.gameScore += (life * 1000) + seed;
                PlayerInformation.defenseScore = (life * 1000) + seed;
                PlayerInformation.UpdateTip(); //팁 제공
                PlayerInformation.defenseGameClear = true; // 4라운드 이상 플레이 하지 않으면 게임 클리어 인정 x
            }
            else
            {
                PlayerInformation.defenseScore = 0;
                PlayerInformation.defenseGameClear = false;
            }
        }
    }

    public void select(int number)
    {
        if(number == 1) //1은 첫번째 캐릭터를 의미
        {
            nowSelect = 1;
            select1.GetComponent<Image>().color = Color.gray; //선택되었다고 색상 변경
            select2.GetComponent<Image>().color = Color.white; //선택이 안된 캐릭터 색상 조절
        }
        else
        {
            nowSelect = 2;
            select1.GetComponent<Image>().color = Color.white;
            select2.GetComponent<Image>().color = Color.gray;
        }
    }

    public void ClearRound()
    {
        if(round < totalRound && loseText.enabled == false) //다음 라운드로 넘어가면 and 패배하지 않았다면
        {
            nextRound();
            seed += reward;
            updateText();
            spawnTime -= 0.2f;
            spawnNumber += 3;
            reward += 150;
            //Debug.Log("생성된 총알 오브젝트: " + bulletAddCount);
            //Debug.Log("생성된 캐릭터 오브젝트: " + characterAddCount);
            //Debug.Log("생성된 몬스터 오브젝트: " + monsterAddCount);
        }
    }

    public void nextRound() //다음 라운드 조작 함수
    {
        round = round + 1;
        if(round == 1) //1라운드면 라운드의 텍스트만 바꿔줌
        {
            roundText.text = "Round " + round;
            roundSartText.text = "Round " + round;
            // 1라운드는 애니메이터에서 바로 시작하게 만들어서 다음라운드로 실행하는 코드 없어도 됨
        }

        else if(round < 10)
        {
            roundText.text = "Round " + round;
            roundSartText.text = "Round " + round;
            roundSartText.GetComponent<Animator>().SetTrigger("Round Start");
            //다음 애니메이션 발동 조건인 트리거를 조작해서 다음 라운드 애니메이션 실행
        }
        else //라운드가 10이면 
        {
            roundText.text = "Round " + round;
            roundSartText.text = "Round " + round;
            roundSartText.GetComponent<Animator>().SetTrigger("Round Start");
        }
        audioSource.PlayOneShot(audioSource.clip); //라운드 오디오 한번 실행
    }

    public void updateText()
    {
        seedText.text = "Gold: " + seed;
    }

    // Start is called before the first frame update
    void Start()
    {

        if (PlayerInformation.tutorialselected)
            totalRound = 5;
        else 
            totalRound = 10;

        ClearText.enabled = false; //처음에 클리어 UI false로 설정
        loseText.enabled = false;
        audioSource = roundSartText.GetComponent<AudioSource>();
        updateText();
        nextRound(); //게임이 실행되면 바로 0라운드부터 실행되기에 처음부터 바로 넘김
        select(1); //첫 시작에는 첫 캐릭터가 선택되도록 함
        lifeText.text = life.ToString(); //현재 목숨 양 보여주기
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
