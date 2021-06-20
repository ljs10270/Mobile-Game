using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//hp바 이미지에 들어가는 코드(캐릭터, 몬스터 오브젝트의 자식임)
public class HPBar : MonoBehaviour
{//HP 바 UI 조작 코드

    public GameObject image; //hp바 이미지 담을 변수,유니티에서 이코드가 담기는 HP바 자기자신 이미지 드래그
    public GameObject character; //체력바 이미지가 자식으로 적용된 캐릭터 오브젝트 유니티에서 넣기

    public GameObject parent; //hp바의 부모 오브젝트가 캐릭터인지 몬스터인지 처리하기 위한 변수
    //유니티에서 캐릭터 오브젝트는 캐릭터 오브젝트를 넣고 몬스터는 몬스터 오브젝트 넣기
    private CharacterStat characterStat; //캐릭터 스텟파일을 가져와 레벨에 맞게 hp 변동
    private MonsterStat monsterStat;

    public float max = 100;
    public float current = 100; //비율
    private float scale; //체력 바 이미지의 x좌표를 가져오고 조작할 변수

    private int maxHp = 100;
    private int hp = 100;

    // Start is called before the first frame update
    void Start()
    {
        scale = image.transform.localScale.x; //체력바의 가로 크기 가져옴

        if(parent.name.Contains("Character")) //""글자를 포함한다면(일치 아님)
        {
            characterStat = parent.GetComponent<CharacterStat>();
        }
        else if(parent.name.Contains("Monster"))
        {
            monsterStat = parent.GetComponent<MonsterStat>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(characterStat != null)
        {
            maxHp = characterStat.maxHp; //캐릭터 스텟의 최대 체력을 받아옴
            hp = characterStat.hp; //캐릭터의 현재 hp를 받아옴
        }
        else if (monsterStat != null)
        {
            maxHp = monsterStat.maxHp; //몬스터 스텟의 최대 체력을 받아옴
            hp = monsterStat.hp; //몬스터의 현재 hp를 받아옴
        }

        current = (float)hp / (float)maxHp * 100; //전체 hp에서 현재 hp를 나눈 비율

        if(current < 0)
        {
            current = 0; //이미지 안보이게 하기
        }

        Vector2 temp = image.transform.localScale; //hp바 조작을 위해 벡터 변수에 스케일 전체를 담음
        temp.x = current / max * scale; //x값 조작
        image.transform.localScale = temp; //이미지의 스케일에 적용
    }
}
