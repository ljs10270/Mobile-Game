using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터 오브젝트에 들어가는 코드다
public class CharacterStat : MonoBehaviour
{ //캐릭터에 적용되는 체력, 레벨 등 정의하는 코드 
 //(실제 동작은 CharacterBehavior.cs에서 이 파일을
//GetConponent로 불러와 여기에 있는 함수들을 호출하여 동작하게 함

    public int level = 1; //캐릭터의 레벨
    public int hp = 30; //싸우는 도중 캐릭터의 체력 값
    public int maxHp = 30; //캐릭터의 전체 체력 값, 레벨업하면 올라감
    public int damage = 5; //캐릭터의 공격력
    public int cost = 130; //캐릭터를 생성하는 비용
    public int upgradeCost = 200; //캐릭터 레벨업 비용
    public float coolTime = 2.0f; //총알을 발사하는 쿨타임

    public int attacked(int damage) //몬스터로부터 공격을 받아 처리하는 함수
    {
        hp = hp - damage;

        if(hp <= 0) //죽으면
        {
            Destroy(gameObject); //삭제
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //죽으면 몬스터가 캐릭터의 충돌을 비활성화해서 바로 앞으로 나아감
        }
        return hp; //남은 hp 반환, MonsterBehavior에서 호출할 것
    }

    // 캐릭터를 추가로 생성 할 수 있는지 여부를 반환하는 함수
    public bool canCreate(int seed) //매개변수는 현재 캐릭터가 가지고 있는 골드가 들어감
    {
        if(cost <= seed)
        {
            return true;
        }
        return false;
    }

    // 레벨업이 가능한지 여부를 반환
    public bool canLavelUp(int seed)
    {
        if(level < 3)
        {
            if(upgradeCost <= seed)
            {
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    // 레벨업을 수행하는 함수
    public void increaseLevel()
    {
        if(level == 1)
        {
            level = 2;
            maxHp += 25;
            hp = maxHp; //레벨업하면 체력 풀충전
            damage += 5;
            transform.localScale += new Vector3(0.03f, 0.03f, 0);
            //레벨 업하면 캐릭터 크기 증가
        }
        else if(level == 2)
        {
            level = 3;
            maxHp += 50;
            hp = maxHp; //레벨업하면 체력 풀충전
            damage += 5;
            transform.localScale += new Vector3(0.03f, 0.03f, 0);
            //레벨 업하면 캐릭터 크기 증가
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
