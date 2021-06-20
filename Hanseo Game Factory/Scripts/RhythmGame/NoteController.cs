using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class NoteController : MonoBehaviour
{
    //노트 움직임을 컨트롤하는 클래스

    //하나의 노트에 대한 정보를 담는 노트 클래스 정의
    class Note
    {
        public int noteType { get; set; } //노트 라인
        public int order { get; set; } //노트가 떨어지는 순서, 노트가 노트들 중 몇번째로 떨어지는지

        public Note(int noteType, int order) //생성자
        {
            this.noteType = noteType;
            this.order = order;
        }
    }

    public GameObject[] Notes; //각 라인 노트 오브젝트가 저장될 배열 
                               //유니티에서 각 노트 1~4까지 차례대로 대입해주기, 총 4개의 라인이니까 4개의 노트가 담김.

    private List<Note> notes = new List<Note>();  //실제로 떨어질 노트를 담는 리스트
    private NoteObjectPooler noteObjectPooler; //오브젝트 풀 클래스에 접근하기 위한 변수

    private float x, z, startY = 8.0f;
    //판정선에 노트 오브젝트가 닿아서 비활성화 되었고 다시 활성화 된다면
    // 노트의 좌표는 판정선 위치인 아래로 되어 있다. 다시 노트가 떨어지게 위치를 위로 조작해야 된다.

    void MakeNote(Note note) //오브젝트 풀 리스트에 접근하여 노트 오브젝트 활성화
    {
        GameObject obj = noteObjectPooler.getObject(note.noteType);
        //오브젝트 풀러 클래스에서 오브젝트 풀 리스트의 비활성화된 오브젝트를 반환받아서

        // 활성화 시켜서 사용될 노트의 초기 좌표값 설정
        x = obj.transform.position.x; 
        z = obj.transform.position.z;
        //x,z축은 그대로 설정, x는 유니티에서 설정한 각 노트의 라인의 좌표값이기에 건들지 말자
        //x축은 유니티에서 각 라인에 맞게 노트 설정함

        obj.transform.position = new Vector3(x, startY, z);
        //startY는 8.0, 노트 오브젝트 위치 위로 초기화
        obj.GetComponent<NoteBehavior>().Initialize(); //노트의 판정선 값을 초기값으로 설정
        obj.SetActive(true); //노트 활성화하여 사용 
    }

    private int bpm;
    private int divider; //박자
    private float startingPoint; //노트가 떨어지는 시작 시간
    private float beatCount; //떨어지는 비트 개수 카운트
    private float beatInterval; //노트간의 떨어질 시간 간격(박자), 코루틴 함수에 적용
    // 텍스트 파일에서 읽어 올 것
    TextAsset textAsset; //비트 파일 읽어올 변수

    IEnumerator AwaitMakeNote(Note note) // 떨어질 다음 노트 시간처리 코루틴 함수
    {
        int noteType = note.noteType;
        int order = note.order;
        yield return new WaitForSeconds(startingPoint + order * beatInterval);
        // 첫 노트가 생성되어지는 시간 + 노트 순서 * BPM 분석된 떨어지는 시간 간격
        MakeNote(note);
        // yield 위의 코드로 인해 BPM 분석된 떨어지는 시간 간격 뒤에 MakeNote(note) 함수가 실행되며 노트 활성화
    }

    // Start is called before the first frame update
    void Start()
    {
        noteObjectPooler = gameObject.GetComponent<NoteObjectPooler>(); //오브젝트 풀러 클래스로 초기화 

        if (PlayerInformation.tutorialselected == false) //튜토리얼 모드가 아니라면
        {
            // 리소스 폴더에서 작성한 비트 txt 파일 불러옴
            textAsset = Resources.Load<TextAsset>("Beats/" + RhythmGameManager.rhythmGameManager.gamebeat);
        }
        else //튜토리얼 모드라면
        {
            textAsset = Resources.Load<TextAsset>("Beats/" + RhythmGameManager.rhythmGameManager.tutorialbeat);
        }

        StringReader reader = new StringReader(textAsset.text);
        // 첫번째 줄(BPM, Divider(박자), +시작 시간) 읽기
        string beatInformation = reader.ReadLine();
        // beatInformation = "BPM 박자 +시작시간"

        bpm = Convert.ToInt32(beatInformation.Split(' ')[0]);
        //beatInformation 문자열을 공백을 기준으로 나누고 0인덱스인 bpm부분만 짤라서 정수로 바꿈
        divider = Convert.ToInt32(beatInformation.Split(' ')[1]);
        startingPoint = (float)Convert.ToDouble(beatInformation.Split(' ')[2]);

        // 1초마다 떨어지는 비트 개수
        beatCount = (float)bpm / divider;

        // 비트가 떨어지는 간격 시간
        beatInterval = 1 / beatCount;

        // 각 노트들이 떨어지는 라인, 순서를 텍스트 파일에 읽음
        string line;
        while((line = reader.ReadLine()) != null)
        {
            Note note = new Note(
                Convert.ToInt32(line.Split(' ')[0]) + 1, //라인은 1부터 시작
                Convert.ToInt32(line.Split(' ')[1])
            );
            notes.Add(note);
        }


        // 각 라인들의 노트를 정해진 시간 간격마다 떨어지도록 설정
        //코루틴 기법 적용
        for (int i = 0; i < notes.Count; i++)
        {
            StartCoroutine(AwaitMakeNote(notes[i])); //코루틴 함수 실행
        }

        // 마지막 노트를 기준으로 리듬게임이 끝나면 최종 랭킹 씬(화면)으로 이동
        StartCoroutine(AwaitGameResult(notes[notes.Count - 1].order));
    }

    IEnumerator AwaitGameResult(int order) //최종 랭킹 씬으로 화면 전환 코루틴 함수
    {
        yield return new WaitForSeconds(startingPoint + order * beatInterval + 7.0f);
        // 마지막 노트가 떨어진 뒤 7초 뒤에 자동으로 게임 최종 결과 화면으로 이동
        GameResult();
    }

    void GameResult()
    {
        if(PlayerInformation.tutorialselected == false) //튜토리얼 모드가 아니라면
        {
            if((int)RhythmGameManager.rhythmGameManager.score <= 300)
            {
                PlayerInformation.rhythmGameClear = false;
                SceneManager.LoadScene("StageSelectScene");
            }
            else
            {
                PlayerInformation.gameScore += (int)RhythmGameManager.rhythmGameManager.score;
                PlayerInformation.rhythmScore = (int)RhythmGameManager.rhythmGameManager.score;
                PlayerInformation.rhythmMaxCombo = RhythmGameManager.rhythmGameManager.maxCombo;
                PlayerInformation.rhythmGameClear = true;

                PlayerInformation.UpdateTip(); //팁 제공
                AddRank(); //

                if(PlayerInformation.fullTipUser)
                {
                    SceneManager.LoadScene("GameResultScene");
                }
                else
                {
                    SceneManager.LoadScene("TipViewScene");
                }
            }
        }
        else
        {
            SceneManager.LoadScene("StageSelectScene");
        }
    }

    // 순위 정보를 담는 Rank 클래스 정의
    class Rank
    {
        public string email;
        public int totalScore;
        public double timestamp; //DB에서 언제 데이터가 들어왔는지 확인하기 위해

        public Rank(string email, int totalScore, double timestamp)
        {
            this.email = email;
            this.totalScore = totalScore;
            this.timestamp = timestamp;
        }
    }

    void AddRank() //최종 점수 DB 삽입
    {
        //Firebase 접속 설정, 싱글톤 기법 이용
        DatabaseReference reference = PlayerInformation.GetDatabaseReference();
        
        DateTime now = DateTime.Now.ToLocalTime(); //현지 시간, 한국 = UTC+9
        TimeSpan span = (now - new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime());
        // 한국의 현지시간의 1970년 1월 1일 0시 0분 0초를 현재 시간에서 뺀다.
        // 즉 현재 시간이 1970년 1월 1일부터 얼마나 지나있는지 span에 대입한다.
        int timestamp = (int)span.TotalSeconds; //초로 환산하여 대입

        Rank rank = new Rank(PlayerInformation.auth.CurrentUser.Email, PlayerInformation.gameScore, timestamp);

        string json = JsonUtility.ToJson(rank); //rank 객체를 DB로 보내기 위해 json 형식으로 바꾼다.

        //삽입
        reference.Child("~~").Child("~~").Child(PlayerInformation.auth.CurrentUser.UserId).SetRawJsonValueAsync(json);
        //Firebase 규칙에서 루트의 자식인 ranks 데이터 set 자식인 Key의 값과 UserId에 현재 사용자의 uid를 값으로 설정하여 json 데이터 보냄 
        //SetRawJsonValueAsync - 비동기적으로 데이터를 설정, 갱신(삽입)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
