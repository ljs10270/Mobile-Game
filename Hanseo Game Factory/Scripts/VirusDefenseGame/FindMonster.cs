using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터 오브젝트의 자식인 Attack Range 오브젝트 들어가는 코드
//몬스터 오브젝트 감지
public class FindMonster : MonoBehaviour
{
    public GameObject character; //유니티에서 캐릭터 오브젝트 드래기 해서 넣기
    private CharacterBehavior characterBehavior;
    private float coolTime;
    private float laskAttackTime; //마지막 공격 시간

    // Start is called before the first frame update
    void Start()
    {
        characterBehavior = character.GetComponent<CharacterBehavior>();
        coolTime = character.GetComponent<CharacterStat>().coolTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Monster")
        {
            if(Time.time - laskAttackTime > coolTime) //쿨타임을 넘어섰다면
            {
                int damage = character.GetComponent<CharacterStat>().damage; //캐릭터 스탯의 공격력을 가져와서
                characterBehavior.attack(damage); //공격력을 매개변수로 총알 발사 공격 함수 실행
                laskAttackTime = Time.time; //최근 공격 시간을 현재 타임으로 저장하여 측정 할 수 있도록 함
            }

        }
    }
}
