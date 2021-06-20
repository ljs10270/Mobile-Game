using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBehavior : MonoBehaviour
{
    public int noteType; //총 4개의 라인. 1은 첫번쨰 라인, 유니티에서 각 라인 노트 값 설정
    private RhythmGameManager.judges judge; //판정선 값
    private KeyCode keyCode; //키보드 이벤트

    // Start is called before the first frame update
    void Start()
    {
        if (noteType == 1) //첫번째 라인은 D 키보드로 설정
            keyCode = KeyCode.D;
        else if (noteType == 2)
            keyCode = KeyCode.F;
        else if (noteType == 3)
            keyCode = KeyCode.J;
        else if (noteType == 4)
            keyCode = KeyCode.K;
    }

    public void Initialize() //오브젝트풀 리스트에 있는 노트가 활성화 되었을 때
    {
        judge = RhythmGameManager.judges.NONE; //초기화
        //judge는 판정선의 값을 의미, 판정선에 닿아서 비활성화된 노트 오브젝트가
        //다시 활성화 되었을 때 이전 judge값이 노트에 적용되어 있으면 안되기에 초기화함
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * RhythmGameManager.rhythmGameManager.noteSpeed); //노트 아래로 떨어짐

        //사용자가 키보드를 입력한 경우 처리
        if (Input.GetKey(keyCode))
        {
            //Debug.Log(judge); // 해당 노트에 대한 판정 처리 테스트
            RhythmGameManager.rhythmGameManager.processJudge(judge, noteType); //판정처리 함수 호출

            //노트가 판정 선에 닿으면 해당 노트 제거
            if (judge != RhythmGameManager.judges.NONE) //노트가 판정선에 닿은 상태라면
                gameObject.SetActive(false); //비활성화
        }
    }

    public void Judge() //모바일을 위한 판정 처리
    {
        //Debug.Log(judge); // 해당 노트에 대한 판정 처리 테스트
        RhythmGameManager.rhythmGameManager.processJudge(judge, noteType); //판정처리 함수 호출

        //노트가 판정 선에 닿으면 해당 노트 제거
        if (judge != RhythmGameManager.judges.NONE) //노트가 판정선에 닿은 상태라면
            gameObject.SetActive(false); //비활성화
    }

    //각 노트가 판정선에 닿으면 현재 위치에 따라 판정선값 설정
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Bad Line") //노트가 접촉한 판정선이 배드라인이라면
        {
            judge = RhythmGameManager.judges.BAD; //리듬게임매니저에서 enum 자료형으로 BAD는 1로 설정
        }
        else if(other.gameObject.tag == "Good Line")
        {
            judge = RhythmGameManager.judges.GOOD;
        }
        else if (other.gameObject.tag == "Perfect Line")
        {
            judge = RhythmGameManager.judges.PERFECT;

            if (RhythmGameManager.rhythmGameManager.autoPerfect) //자동판정
            {
                RhythmGameManager.rhythmGameManager.processJudge(judge, noteType);
                gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.tag == "Miss Line")
        {
            judge = RhythmGameManager.judges.MISS;
            RhythmGameManager.rhythmGameManager.processJudge(judge, noteType); //판정처리 함수 호출
            // 사용자가 아무런 동작을 안해도 판정처리 함수가 호출되어야 되기에 호출
            gameObject.SetActive(false); //비활성화
        }
    }
}
