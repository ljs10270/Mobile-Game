using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObjectPooler : MonoBehaviour
{
    // 노트는 총 4개, 라인도 총 4라인이다. 
    // 리스트를 4개 만들어 각 라인 노트를 10개씩 미리 생성
    /*
    * Note 1: 10개 => 리스트 1 - 라인1
    * Note 2: 10개 => 리스트 2 - 라인2
    * Note 3: 10개 => 리스트 3 - 라인3
    * Note 4: 10개 => 리스트 4 - 라인4
    */

    // 효율적인 관리를 위해 이중리스트 사용
    public List<GameObject> Notes; //유니티에서 각 라인 노트1,2,3,4 오브젝트 차례대로 대입하기
    private List<List<GameObject>> poolsOfNotes; //이중리스트, 오브젝트 풀 리스트

    public int noteCount = 10; //각 리스트에 들어갈 노트의 개수는 10개로 설정
    private bool more = true; //미리 생성한 노트외에 동적으로 더 필요한 경우를 위한 변수



    // Start is called before the first frame update
    void Start()
    {
        poolsOfNotes = new List<List<GameObject>>();

        for(int i = 0; i < Notes.Count; i++) //Notes.Count는 4다. 4개의 라인 노트 오브젝트
        {//4번반복
            poolsOfNotes.Add(new List<GameObject>());
            // 오브젝트풀 리스트에 각 라인 노트 리스트를 대입, 
            // 리스트 안의 리스트인 이중 리스트 생성

            for(int n = 0; n < noteCount; n++) //리스트 안의 리스트에 각 노트 10개씩 생성
            {//10번반복
                GameObject obj = Instantiate(Notes[i]);
                //obj는 각 라인의 노트 1,2,3,4가 됨
                obj.SetActive(false); //처음 생성된 노트는 비활성화, 나중에 활성화해서 쓴다.
                poolsOfNotes[i].Add(obj); 
                //이중리스트인 오브젝트풀 리스트의 인덱스인 리스트에 비활성화 상태로 생성된 노트 오브젝트 10개 대입

            }
        }// 배열로 생각한다면 a[4][10]이다.
    }

    public void Judge(int noteType) //모바일 환경일 때 판정 처리 위해
    {
        //어떤 노트 라인을 클릭했는지 확인하기 위해 풀 리스트에서 모두 불러와
        foreach(GameObject obj in poolsOfNotes[noteType - 1])
        {
            if (obj.activeInHierarchy) //활성화 되어 있는 노트에만 
            {
                obj.GetComponent<NoteBehavior>().Judge(); //판정처리 호출
            }
        }
    }

    public GameObject getObject(int noteType) //오브젝트 풀에서 노트 꺼내오는 함수
    {
        foreach(GameObject obj in poolsOfNotes[noteType - 1]) //-1은 리스트는 인덱스 0부터 시작하기에 -1함
        { //obj라는 이름의 변수로 이중리스트인 오브젝트 풀 리스트에 하나씩 접근한다. 
            if (!obj.activeInHierarchy)
            {// 접근된 노트 오브젝트가 비활성화된 상태라면
                return obj; //비활성화된 오브젝트 리턴(이 오브젝트를 NoteCotroller.cs에서 활성화시켜서 사용할 것)
            }
        }

        if (more)
        {// 만약 오브젝트 풀 리스트에 비활성화된 오브젝트가 없는데 더 필요하다면
            //즉, 비활성화 된 오브젝트가 없다면 위의 foreach문은 실행되지 않는다.
            GameObject obj = Instantiate(Notes[noteType - 1]); //추가 필요할 때만 노트 동적 생성
            poolsOfNotes[noteType - 1].Add(obj); //풀 리스트에 추가하고
            return obj; //반환
        } //만약을 위한 위험요소 제거

        return null; //위의 코드가 전부 실행되지 못하면 오류 발생
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
