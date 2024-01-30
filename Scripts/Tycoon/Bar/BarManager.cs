using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tycoon
{
    public class BarManager : MonoBehaviour
    {
        public static BarManager instance;

        public GameObject normalSeatPrefab;
        public GameObject specialSeatPrefab;

        public GameObject[] seats;
        public Text leftOrderText;

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

                GameObject tempSeat;

                if (tempOrder.isSpecial)
                {
                    tempSeat = Instantiate(specialSeatPrefab);
                }
                else
                {
                    tempSeat = Instantiate(normalSeatPrefab);
                }

                tempSeat.transform.SetParent(seats[seatID].transform);
                tempSeat.transform.localPosition = Vector3.zero;
                tempSeat.GetComponent<SeatController>().UpdateSeat(tempOrder);
                tempSeat.GetComponent<SeatController>().seatID = seatID;

                TycoonManager.instance.orderList.RemoveAt(orderID);
                leftOrderText.text = TycoonManager.instance.orderList.Count + " Έν";
            }
        }

        #region Manage Plates

        public GameObject foodPrefab;
        public GameObject[] plates;
        public string[] foodNames = new string[5];

        public bool GetServed(string recipeName)
        {
            bool fullFlag = false;

            for (int i = 0; i < 5; i++)
            {
                if (foodNames[i] == "")
                {
                    fullFlag = false;
                    foodNames[i] = recipeName;
                    StartCoroutine(Plating(i));
                    break;
                }

                fullFlag = true;
            }

            if (fullFlag == true)
            {
                return false;
            }

            return true;
        }

        IEnumerator Plating(int plateIndex)
        {
            yield return new WaitForSeconds(2.5f);

            GameObject tempFood = Instantiate(foodPrefab, plates[plateIndex].transform);
            tempFood.GetComponent<FoodController>().UpdateFood(plateIndex, foodNames[plateIndex]);
            tempFood.transform.SetParent(plates[plateIndex].transform);
        }

        public void ResetPlate(int plateIndex)
        {
            foodNames[plateIndex] = "";
        }

        #endregion


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
            GetComponent<AudioSource>().volume = GameManager.instance.settings.backgroundMusicVolume;
            
            for (int i = 0; i < 3; i++)
            {
                UpdateSeat(i);
            }
        }
    }
}
