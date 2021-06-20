using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//총알 오브젝트에 들어가는 코드다. 또한 이 코드가 적용된 총알 오브젝트는 캐릭터에 적용된
// CharacterBehavior 매개변수인 불렛에 넣어야 함
public class BulletBehavior : MonoBehaviour
{//총알 동작 코드

    public BulletStat bulletStat { get; set; }
    public GameObject character; //캐릭터 오브젝트 유니티에서 드래그 해서 넣기

    public float activeTime = 3.0f; //총알 오브젝트가 화면에 존재할 수 있는 시간
    
    public BulletBehavior() //생성자
    {
        bulletStat = new BulletStat(0, 0);// 총알 스피드와 데미지
    }

    public void Spawn() //오브젝트풀러에서 꺼내진 비활성화 된 총알 오브젝트를 활성화 시킴
    {
        gameObject.SetActive(true);
    }

    private void OnEnable() //오브젝트가 활성화되면 자동호출됨(이벤트함수임)
    {
        StartCoroutine(BulletInactive(activeTime));   
    }

    IEnumerator BulletInactive(float activeTime)
    {
        yield return new WaitForSeconds(activeTime); //3초뒤 호출됨
        gameObject.SetActive(false); //비활성화
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * bulletStat.speed * Time.deltaTime);        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Monster")
        {
            gameObject.SetActive(false); //비활성화
            other.GetComponent<MonsterStat>().attacked(bulletStat.damage); //몬스터에게 데미지 주기
        }
    }
}
