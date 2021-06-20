using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //맨 밑의 UI, 오브젝트 구분을 위해

//캐릭터 오브젝트에 적용되는 코드
public class CharacterBehavior : MonoBehaviour
{// 캐릭터의 레벨을 동작하게 하는 코드

    private CharacterStat characterStat;
    
    public GameObject bullet; //유니티에서 총알 오브젝트 드래그해서 넣기
    private AudioSource audioSource;

    private GameObject bulletObjectPool; //캐릭터마다 총알 오브젝트 풀 지정
    private ObjectPooler bulletObjectPooler; //위의 변수의 오브젝트풀러에 접근해서 총알을 하나씩 꺼내올 변수

    // Start is called before the first frame update
    void Start()
    {
        characterStat = gameObject.GetComponent<CharacterStat>();
        //캐릭터 스탯과 레벨을 올리는 C# 파일을 대입
        audioSource = gameObject.GetComponent<AudioSource>();

        if(gameObject.name.Contains("Character_1")) //오브젝트풀러를 싱글톤 기법으로 적용하면 코드가 늘어날 수 있음(디펜스에서)
        {
            bulletObjectPool = GameObject.Find("Bullet1 Object Pool");
        }
        else if (gameObject.name.Contains("Character 2"))
        {
            bulletObjectPool = GameObject.Find("Bullet2 Object Pool");
        }
        bulletObjectPooler = bulletObjectPool.GetComponent<ObjectPooler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attack(int damage) //공격. 즉 총알 발사 함수
    {
        audioSource.PlayOneShot(audioSource.clip);

        GameObject bullet = bulletObjectPooler.GetObject();
        if (bullet == null)
            return;

        bullet.transform.position = gameObject.transform.position;
        //GameObject curentBullet = Instantiate(bullet, transform.position, Quaternion.identity); 오브젝트 풀 사용 전 코드
        bullet.GetComponent<BulletBehavior>().bulletStat = 
            new BulletStat(10 + characterStat.level * 3, characterStat.damage);
        //현재 캐릭터의 스텟을 매개변수로 총알의 속도, 데미지 설정
        bullet.GetComponent<BulletBehavior>().Spawn();
    }

    private void OnMouseDown() //캐릭터를 클릭하면 레벨업을 할 수 있도록 하는 함수
    {
        if (EventSystem.current.IsPointerOverGameObject(-1) == true) return;
        if (EventSystem.current.IsPointerOverGameObject(0) == true) return;
        //마우스 이벤트가 UI에 대해서는 true, 오브젝트에 대해서는 false를 가지게 한다.
        // 즉 현재 마우스가 UI 위에 있는지 검사 가능
        // 메뉴버튼을 눌러도 메뉴 캔버스인데 캐릭터가 업그레이드되는 것을 막기 위함
        // 이것은 마우스를 기본으로 삼아서 모바일의 터치는 적용이 안될 수 있음
        // 마우스에 대한 이벤트는 -1, 모바일에 대한 이벤트는 0 이상이 매개변수로 들어감

        if (characterStat.canLavelUp(DefenseGameManager.defenseGameManager.seed)) //해당 함수를 불러와 참이면
        {
            characterStat.increaseLevel(); //레벨업 수행

            DefenseGameManager.defenseGameManager.seed -= characterStat.upgradeCost;
            DefenseGameManager.defenseGameManager.updateText();
        }
    }
}
