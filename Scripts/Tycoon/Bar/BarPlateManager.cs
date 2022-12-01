using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarPlateManager : MonoBehaviour
{
    public static BarPlateManager instance;

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

}
