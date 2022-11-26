using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class BarManager : MonoBehaviour
{
    public static BarManager instance;

    public GameObject[] seats;
    //public GameObject[] orderBalloons;
    //public Order[] orders = new Order[3];

    public float resetDelay;

    public void UpdateSeat(int seatID)
    {
        if (TycoonManager.instance.orderList.Count == 0)
        {
            TycoonManager.instance.EndTycoon();
        }
        else
        {
            int orderID = Random.Range(0, TycoonManager.instance.orderList.Count);
            Order tempOrder = TycoonManager.instance.orderList[orderID];

            seats[seatID].GetComponent<SeatController>().UpdateSeat(tempOrder);
            seats[seatID].GetComponent<SeatController>().seatID = seatID;

            TycoonManager.instance.orderList.RemoveAt(orderID);
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            UpdateSeat(i);
        }
    }
}
