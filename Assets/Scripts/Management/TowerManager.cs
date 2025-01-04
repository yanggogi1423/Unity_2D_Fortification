using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : MonoBehaviour
{
    //  클릭과 관련된 인스턴스
    [Header("Click Management")] 
    private RaycastHit2D hit;
    public TowerWrapper curTowerWrapper;
    private NexusInfo nexusInfo;
    
    private void Update()
    {
        ClickProcess();
    }
    
     //  마우스 클릭 시 타워를 클릭했는지 확인
    private void ClickProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(mousePoint, Vector2.zero);

            //  UI 클릭 감지을 위한 변수
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            //  Ui 클릭 시
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UI is Clicked.");
            }
            
            //  타워 클릭 시
            else if (hit.collider != null &&
                     hit.collider.GetComponentInParent<TowerWrapper>() != null)
            {
                if (nexusInfo != null)
                {
                    nexusInfo.SetVisibleCanvas(false);
                    nexusInfo = null;
                }
                
                Debug.Log("클릭된 자리에 존재");
                //  동일 오브젝트를 다시 클릭했다면 무시
                if (curTowerWrapper == hit.collider.GetComponentInParent<TowerWrapper>())
                {
                    Debug.Log("같은 애임");
                    return;
                }
                
                //  이미 클릭된 애가 있을 때
                if (curTowerWrapper != null)
                {
                    Debug.Log("다른애 해제");
                    curTowerWrapper.SetSelect(false);
                    curTowerWrapper = null;
                }
                
                curTowerWrapper = hit.collider.GetComponentInParent<TowerWrapper>();
                curTowerWrapper.SetSelect(true);
            }
            //  Nexus
            else if (hit.collider != null &&
                     hit.collider.GetComponentInParent<NexusInfo>() != null)
            {
                if (curTowerWrapper != null)
                {
                    curTowerWrapper.SetSelect(false);
                    curTowerWrapper = null;
                }
                
                Debug.Log("클릭된 자리에 존재");
                //  동일 오브젝트를 다시 클릭했다면 무시
                if (nexusInfo == hit.collider.GetComponentInParent<NexusInfo>())
                {
                    Debug.Log("같은 애임");
                    return;
                }
                
                //  이미 클릭된 애가 있을 때
                if (nexusInfo != null)
                {
                    Debug.Log("다른애 해제");
                    nexusInfo.SetVisibleCanvas(false);
                    nexusInfo = null;
                }
                
                nexusInfo = hit.collider.GetComponentInParent<NexusInfo>();
                nexusInfo.SetVisibleCanvas(true);
            }
            
            //  타워와 UI가 클릭되지 않은 경우(메뉴를 집어 넣음)
            else
            {
                Debug.Log("클릭된 자리에 없음");
                if (curTowerWrapper != null)
                {
                    curTowerWrapper.SetSelect(false);    
                }
                if (nexusInfo != null)
                {
                    nexusInfo.SetVisibleCanvas(false);    
                }

                curTowerWrapper = null;
                nexusInfo = null;
            }
        }
    }
}
