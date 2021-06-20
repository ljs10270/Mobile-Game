using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 오브젝트에 들어가는 C# 파일
//몬스터의 동작 코드
public class MonsterBehavior : MonoBehaviour
{
    private MonsterStat monsterStat; //몬스터의 스탯이 들어간 코드 조작하기 위한 변수
    private bool attacking = false; //공격 여부

    private float laskAttackTime; //마지막 공격 시간 측정, 쿨타임에 따라 공격 조작
    private CharacterStat targetStat; //몬스터가 타겟으로 삼고 있는 캐릭터 스탯 가져와서 실시간으로
    //죽었는지 검사하고 어택킹 풀기 위한 변수

    public bool died = false; //몬스터 죽었는지 검사

    // Start is called before the first frame update
    void Start()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
        monsterStat = gameObject.GetComponent<MonsterStat>();
    }

    // Update is called once per frame
    void Update()
    {
        if(died)
        {
            attacking = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false; //죽은 몬스터의 충돌감지 끄기
            //캐릭터의 사정거리 감지로 인해 캐릭터가 총알을 더 발사하는 행위 방지
        }
        else
        {
            transform.Translate(Vector2.left * monsterStat.speed * Time.deltaTime);   //1초에 1씩 왼쪽으로 이동

            if (attacking) //공격을 하고 있다면
            {
                transform.Translate(Vector2.right * monsterStat.speed * Time.deltaTime);
                //반대로 움직여 제자리에 있게 하면서 공격하기
            }

            if (attacking == true && targetStat.hp <= 0)
            {//캐릭터가 죽었으면
                targetStat = null;
                attacking = false;
            }
            //이것을 작성해 주지 않으면 앞의 몬스터가 한 캐릭터를 죽여도 뒤에 몬스터가
            //계속 공격모드가 된다. 즉 뒤에 있는 몬스터도 같은 캐릭터와 접촉했을 떄 공격모드가 되고
            //자신이 죽인게 아니기에 어택모드가 풀리지 않게 되는 것을 방지.
        }
    }

    private void OnTriggerEnter2D(Collider2D other) //몬스터는 Is Trigger로 체크되어 있어서 이 함수로 다크사이트 충돌 감지 해야함
    {
        if(other.gameObject.name == "Fence") //몬스터와 충돌한 오브젝가 태극기(울타리)인 경우
        {
            Destroy(gameObject); //자기 자신인 몬스터 삭제
            DefenseGameManager.defenseGameManager.decreaseLife(); //플레이어 목숨 감소
        }
        else if(other.gameObject.tag == "Character")
        {
            targetStat = other.gameObject.GetComponent<CharacterStat>();
            //몬스터가 공격하려는 캐릭터 스탯 담기
            attacking = true; //공격수행
            laskAttackTime = Time.time; //캐릭터를 만나면 현재시간으로
        }
    }

    private void OnTriggerStay2D(Collider2D other) //Stay는 반복적으로 계속 충돌이 발생할 떄 실행됨
    {
        if (other.gameObject.tag == "Character")
        {
            if (Time.time - laskAttackTime > monsterStat.coolTime)
            {
                //현재 시간에서 마지막공격시간을 뺸 값이 몬스터스탯의 공격시간보다 크다면

                int hp = other.gameObject.GetComponent<CharacterStat>().attacked(monsterStat.damage);
                //hp는 캐릭터의 체력이 반환됨

                if(hp <= 0) //캐릭터가 죽었다면
                {
                    attacking = false; //어택 중지
                }
                laskAttackTime = Time.time; //공격후 마지막 공격시간 초기화
                
            }
        }         
    }


}
