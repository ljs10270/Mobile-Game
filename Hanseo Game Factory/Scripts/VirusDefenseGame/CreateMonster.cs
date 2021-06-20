using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Respawn Spots 오브젝트에 들어가는 파일
public class CreateMonster : MonoBehaviour
{// 몬스터를 리스폰하는 코드

    public List<GameObject> respawnSpotList; //리스폰 스팟 리스트, 유니티에서 리스폰 스팟 오브젝트의
    // List 사이즈를 4로 설정하고 요소들 드래그 하기

    public GameObject Monster1Prefab; //몬스터 1 프리팹 변수, 유니티에서 몬스터 오브젝트 드래그 해서 넣기
    public GameObject Monster2Prefab; //유니티에서 몬스터 공격력과 체력, 스피드 올렸음,코드는 인스펙터가 우선이기에 그대로 적용되어 있음

    private GameObject monsterPrefab; //몬스터 1,2중 하나가 이 변수에 담겨 생성됨

    private int spawnCount = 0; //현재 라운드에서 몇마리 리스폰 되었는지 변수

    public Button menuButton; //게임 클리어 or 패배시 메뉴버튼 비활성화, 유니티에서 메뉴 버튼 넣기

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        monsterPrefab = Monster1Prefab;

        coroutine = process();
        StartCoroutine(coroutine);
    }

    void Create()
    {
        int index = Random.Range(0, 4); //0~3(4개)까지 랜덤으로 얻어옴
        GameObject respawnSpot = respawnSpotList[index];
        Instantiate(monsterPrefab, respawnSpot.transform.position, Quaternion.identity);
        //선택된 몬스터 오브젝트를 랜덤으로 선택된 리스폰스팟의 기존 위치에 회전변경없이 생성
        //DefenseGameManager.defenseGameManager.monsterAddCount++;
        spawnCount += 1; //몬스터가 추가되었으니 1추가
    }

    IEnumerator process()
    {
        while (true) // 반복수행되야 하는 코루틴은 while(true)하기 
        {
            if(DefenseGameManager.defenseGameManager.life == 0) // 패배
            {
                StopCoroutine(coroutine);
                DefenseGameManager.defenseGameManager.gameClear();
                StartCoroutine("Exit");
            }

            if (DefenseGameManager.defenseGameManager.round > DefenseGameManager.defenseGameManager.totalRound) //전체게임 클리어
            {
                StopCoroutine(coroutine);
                menuButton.interactable = false; //메뉴버튼 비활성화
                StartCoroutine("Exit");
            }

            if(spawnCount < DefenseGameManager.defenseGameManager.spawnNumber) //몬스터가 덜 출몰했다면
            {
                Create(); //몬스터 출몰
            }

            if(spawnCount == DefenseGameManager.defenseGameManager.spawnNumber &&
                GameObject.FindGameObjectWithTag("Monster") == null) //해당 라운드 클리어 했을 때
            {
                if(DefenseGameManager.defenseGameManager.totalRound == DefenseGameManager.defenseGameManager.round)
                { //마지막 라운드 클리어
                    DefenseGameManager.defenseGameManager.gameClear();
                    DefenseGameManager.defenseGameManager.round += 1;
                }
                else
                {
                    DefenseGameManager.defenseGameManager.ClearRound();
                    spawnCount = 0; //몬스터 출몰 횟수 초기화

                    if (DefenseGameManager.defenseGameManager.round == 4) //강한 몬스터로 바꾸기
                    {
                        monsterPrefab = Monster2Prefab;
                        DefenseGameManager.defenseGameManager.spawnTime = 2.0f;
                        DefenseGameManager.defenseGameManager.spawnNumber = 10;
                    }
                }
            }
            if (spawnCount == 0) //새로운 라운드가 시작되었다면(출현 몬스터 0)
                yield return new WaitForSeconds(DefenseGameManager.defenseGameManager.roundReadyTime);
            else
                yield return new WaitForSeconds(DefenseGameManager.defenseGameManager.spawnTime);
        }
    }

    private IEnumerator Exit()
    {
        yield return new WaitForSeconds(3.0f);

        if(PlayerInformation.defenseGameClear == false || PlayerInformation.fullTipUser)
        {
            SceneManager.LoadScene("StageSelectScene");
        }
        else
        {
            SceneManager.LoadScene("TipViewScene");
        }
        StopCoroutine("Exit");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
