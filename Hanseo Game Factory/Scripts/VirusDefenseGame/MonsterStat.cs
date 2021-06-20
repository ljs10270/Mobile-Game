using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 스탯 정보 정의, 몬스터 오브젝트에 들어감
public class MonsterStat : MonoBehaviour
{
    public float speed = 1.0f; //이동속도
    public int damage;
    public float coolTime = 2.0f; //공격속도
    public int hp = 20;
    public int maxHp = 20;

    public int attacked(int damage) //공격을 받는 함수
    {
        hp = hp - damage;
        if(hp <= 0)
        {
            Destroy(gameObject);
            gameObject.GetComponent<MonsterBehavior>().died = true;
        }
        return hp;
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
