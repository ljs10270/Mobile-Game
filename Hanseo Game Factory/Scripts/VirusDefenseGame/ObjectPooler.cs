using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour //오브젝트 풀 오브젝트에 적용
{
    public GameObject pooledObject; //프리팹에서 총알 오브젝트 드래그 해서 넣기
    public int poolCount = 28;
    public bool more = true;

    private List<GameObject> poolList;

    // Start is called before the first frame update
    void Start()
    {
        poolList = new List<GameObject>();

        while(poolCount > 0) //게임 시작과 동시에 28개 리스트에 들어갈 오브젝트 생성
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive(false);
            poolList.Add(obj);
            poolCount = poolCount - 1;
            //DefenseGameManager.defenseGameManager.bulletAddCount++;// 디버깅용
        }
    }

    public GameObject GetObject()
    {
        foreach(GameObject obj in poolList) //풀리스트를 obj라는 변수로 하나씩 접근
        {
            if (!obj.activeInHierarchy) //유니티 하이어뷰에 오브젝트가 활성화 되지 않았다면
            {//즉 비활성화 된 오브젝트를 발견하면
                return obj;
            }
        }

        if (more) //풀리스트에 오브젝트가 초과로 더 필요하면(비활성화 된 오브젝트가 없어 더 필요한데 사용 못하면)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            poolList.Add(obj);
            //DefenseGameManager.defenseGameManager.bulletAddCount++; 디버깅용
            return obj;
        }
        return null; //오류발생
    }


}
