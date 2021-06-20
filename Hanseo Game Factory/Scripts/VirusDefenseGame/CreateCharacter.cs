using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //UI, 오브젝트 구분을 위해

//바닥 맵 프리팹 오브젝트에 들어가는 코드다
public class CreateCharacter : MonoBehaviour
{ //바닥 맵을 클릭하면 캐릭터 프리팹 오브젝트가 생성된다.
    
    private GameObject characterPrefab; //하나의 프리팹을 이용해 특정 지점 위에 캐릭터를 지을 수 있도록 하는 변수
    // 바닥 맵 오브젝트에 유니티로 이 파일이 적용된 바닥 맵의 위의 변수 값으로 캐릭터 오브젝트를 넣어야 함

    public GameObject characterPrefab1; //캐릭터 1이 들어갈 변수, 유니티에서 프리팹 폴더에서 바닥 오브젝트에 캐릭터1 오브젝트 드래그하기
    public GameObject characterPrefab2;

    private GameObject character; //특정 지점 위에 만들어질 캐릭터 변수
    private AudioSource audioSource; //바닥 오브젝트를 클릭하면 캐릭터 오브젝트가
    //생성되는데 이 때 효과음을 조작하기 위한 변수
    
    private CharacterStat characterStat;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() //마우스 이벤트
    {
        if (EventSystem.current.IsPointerOverGameObject(-1) == true) return;
        if (EventSystem.current.IsPointerOverGameObject(0) == true) return;
        //마우스 이벤트가 UI에 대해서는 true, 오브젝트에 대해서는 false를 가지게 한다.
        // 즉 현재 마우스가 UI 위에 있는지 검사 가능
        // 메뉴버튼을 눌러도 메뉴 캔버스인데 캐릭터가 생성되는 것을 막기 위함
        // 이것은 마우스를 기본으로 삼아서 모바일의 터치는 적용이 안될 수 있음
        // 마우스에 대한 이벤트는 -1, 모바일에 대한 이벤트는 0 이상이 매개변수로 들어감

        if (DefenseGameManager.defenseGameManager.nowSelect == 1) //선택된 캐릭터가 캐릭터 1이라면 
        {
            characterPrefab = characterPrefab1;
            characterStat = characterPrefab.GetComponent<CharacterStat>();
        }
        else if(DefenseGameManager.defenseGameManager.nowSelect == 2)
        {
            characterPrefab = characterPrefab2;
            characterStat = characterPrefab.GetComponent<CharacterStat>();
        }

        if(character == null)
        {
            CharacterStat characterStat = characterPrefab.GetComponent<CharacterStat>();
            //캐릭터프리팹은 캐릭터 오브젝트를 의미, 여기에 적용된 스탯.cs파일을 가져와서
            if(characterStat.canCreate(DefenseGameManager.defenseGameManager.seed)) //스탯cs파일의 canCreate 함수 호출하여 true값을 받으면
            {
                character = (GameObject)Instantiate(characterPrefab, transform.position, Quaternion.identity);
                //캐릭터 초기화
                //마우스이벤트로 클릭된 건설지점에 프리팹 오브젝트를 해당 오브젝트가 가지고 있는 기본 회전값
                //(클릭된 위치 그대로)으로 오브젝트 생성되도록 캐릭터 변수에 대입
                //DefenseGameManager.defenseGameManager.characterAddCount++;
                audioSource.PlayOneShot(audioSource.clip);
                //클릭 이벤트가 바닥 맵에 발생하면 위의 코드로 캐릭터 오브젝트가 생성되고
                // 바닥 맵에 적용된 오디오 소스의 클립에 있는 오디오(효과음)를 한번 재생
                // 오디오소스의 플레이 투 어웨이 체크해제 해야함

                DefenseGameManager.defenseGameManager.seed -= character.GetComponent<CharacterStat>().cost; 
                DefenseGameManager.defenseGameManager.updateText(); //차감된 (골드)를 화면에 표시하는 함수 다시 호출
            }
        }
    }
}
