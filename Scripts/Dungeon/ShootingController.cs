using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShootingController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Vector3 startPos;
    Vector3 endPos;
    public float dragDistance;
    public GameObject arrow;
    public float arrowRadius;
    bool canShoot = false;
    Vector3 dir;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        startPos = Camera.main.ScreenToWorldPoint(eventData.position);
        GetComponent<Image>().raycastTarget = false;
    }

    // canShoot 슈팅 딜레이에 해당하는 동안은 꺼서 화살표 표시 안 되게끔 만들기
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        endPos = Camera.main.ScreenToWorldPoint(eventData.position);

        if (canShoot == true)
        {
            if (arrow.activeSelf == false)
            {
                arrow.SetActive(true);
            }

            dir = ((endPos - startPos).normalized) * arrowRadius;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            arrow.transform.localEulerAngles = new Vector3(0, 0, angle);

            arrow.transform.position = -dir + DungeonManager.instance.player.transform.position;
        }
        else
        {
            if (Vector3.Distance(startPos, endPos) >= dragDistance)
            {
                canShoot = true;
            }
        }

    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (canShoot == true)
        {
            GameObject bullet = DungeonManager.instance.GetObject("PlayerBullet");
            bullet.transform.SetParent(DungeonManager.instance.player.transform);
            bullet.GetComponent<Bullet>().Initialize(-dir);

            canShoot = false;
            arrow.SetActive(false);
        }

        GetComponent<Image>().raycastTarget = true;
    }
}
