using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dreg : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler //선지에 넣기
{
    public static Vector2 defaultposition; //드래그 후 원위치로 되돌리기 위한 변수

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnBeginDrag(PointerEventData eventData) //드래그 시작
    {
        defaultposition = gameObject.transform.position; //처음 위치 좌표 저장
        if (EventSystem.current.IsPointerOverGameObject(-1) == true) return;
        if (EventSystem.current.IsPointerOverGameObject(0) == true) return;
    }

    public void OnDrag(PointerEventData eventData) // 드래그 중
    {
        Vector2 currentPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldObjectPos = Camera.main.ScreenToWorldPoint(currentPos); //필수, 위의 변수로만 위치를 정하면 실제 유니티상 안보인다.
        gameObject.transform.position = worldObjectPos; //인풋된 마우스(터치) 위치를 저장하고 따라서 이동
    }

    public void OnEndDrag(PointerEventData eventData) // 드래그 끝
    {
        gameObject.transform.position = defaultposition; //원위치로 되돌아오기
    }
}
