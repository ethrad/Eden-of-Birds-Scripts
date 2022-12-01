using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OrderBalloonController : MonoBehaviour
{
    public GameObject[] foodImages; // 자식에 x 표시 된 것 만들기
    public Image timeBar;

    float timeLimit;
    float currentTime;

    public void UpdateBalloon(Order order)
    {
        for (int i = 0; i < order.menuList.Count; i++)
        {
            foodImages[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Tycoon/" + order.menuList[i]);
        }

        timeLimit = order.waitingTime;
        currentTime = timeLimit;
        StartCoroutine(UpdateTimeBar());
    }

    IEnumerator UpdateTimeBar()
    {
        while (currentTime > 0)
        {
            currentTime -= 0.1f;
            timeBar.fillAmount = currentTime / timeLimit;

            yield return new WaitForSeconds(0.1f);
        }

        transform.parent.gameObject.GetComponent<SeatController>().FailedOrder();

        yield return null;
    }

    public void MarkServedFood(int foodIndex)
    {
        foodImages[foodIndex].transform.GetChild(0).gameObject.SetActive(true);
    }

    public void StopBalloon()
    {
        StopCoroutine(UpdateTimeBar());
    }

    public void ResetBalloon()
    {
        for (int i = 0; i < 3; i++)
        {
            foodImages[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Dots/Tycoon/transparent");
            foodImages[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
