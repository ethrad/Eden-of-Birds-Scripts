    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Diary
{
    public class FoodPage : Page
    {
        public GameObject content;

        // page 2
        public Image image;
        public Text foodName;
        public Text price;
        public Text description;

        public GameObject foodSlotPre;
        [FormerlySerializedAs("cunsumeSlotPre")] public GameObject consumeSlotPre;
        public GameObject ingredientPanel;

        public GameObject nonePanel;
        public GameObject foodGrade;
        public GameObject starIcon;
        public Scrollbar scrollbar;
        
        Dictionary<string, Food> foods;
        List<FoodSlotButton> foodSlotPreList = new List<FoodSlotButton>();
        
        public void SetFoodSlotDetail(string recipeName)
        {
            if(foods != null)
            {
                scrollbar.value = 1;
                detailPanel.SetActive(true);
                nonePanel.SetActive(false);
                image.sprite = Resources.Load<Sprite>("Dots/Tycoon/Foods/" + recipeName);

                if (foodGrade.transform.childCount > 0)
                {
                    for (int i = 0; i < foodGrade.transform.childCount; i++)
                    {
                        Destroy(foodGrade.transform.GetChild(i).gameObject);
                    }
                }

                for (int i = 0; i < foods[recipeName].grade; i++)
                {
                    Instantiate(starIcon, foodGrade.transform);
                }

                foodName.text = foods[recipeName].koreanName;
                price.text = foods[recipeName].price.ToString();
                description.text = foods[recipeName].description;

                ClearCunsumeItem();
                foreach (var item in foods[recipeName].ingredients)
                {
                    CreateCunsumeSlotPanel(item.Key, item.Value.ToString());
                }

                ShowNonePanel(0);
            }
        }

        //필요한 재료 지우기
        void ClearCunsumeItem() 
        {
            if (ingredientPanel.transform.childCount > 0)
            {
                foreach (Transform child in ingredientPanel.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        //필요한 재료 생성함수
        public void CreateCunsumeSlotPanel(string wantName, string wantCount)
        {
            GameObject tempCunsumeSlot = Instantiate(consumeSlotPre);
            tempCunsumeSlot.GetComponent<CunsumeSlot>().UpdateCunsumePanel(wantName, wantCount);
            tempCunsumeSlot.transform.SetParent(ingredientPanel.transform);
        }

        public void Initialize(Dictionary<string, Food> foodList)
        {
            this.foods = foodList;

            if (content.transform.childCount > 0)
            {
                foodSlotPreList.Clear();
                for (int i = 0; i < content.transform.childCount; i++)
                {
                    Object.Destroy(content.transform.GetChild(i).gameObject);
                }
            }

            List<string> ownRecipesList = new List<string>();
            foreach (KeyValuePair<string, List<string>> item in GameManager.instance.ownRecipes)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    ownRecipesList.Add(item.Value[i]);
                }
            }

            foreach (KeyValuePair<string, Menu> item in GameManager.instance.menus)
            {
                GameObject tempFoodSlot = Instantiate(foodSlotPre, content.transform);
                foodSlotPreList.Add(tempFoodSlot.GetComponent<FoodSlotButton>());
                
                FoodSlotButton tempSlotbutton = tempFoodSlot.GetComponent<FoodSlotButton>();
                tempSlotbutton.UpdateFoodInfo(item.Key);
                tempSlotbutton.menu = item.Value;
                if (ownRecipesList.Count <= 0)
                {
                    tempSlotbutton.GetComponent<FoodSlotButton>().questionMark.SetActive(true);
                }
                else
                {
                    for (int i = 0; i < ownRecipesList.Count; i++)
                    {
                        if (item.Key == ownRecipesList[i])
                        {
                            tempSlotbutton.GetComponent<FoodSlotButton>().isDiscovery = true;
                            tempSlotbutton.GetComponent<FoodSlotButton>().questionMark.SetActive(false);
                            break;
                        }
                        else
                        {
                            tempSlotbutton.GetComponent<FoodSlotButton>().questionMark.SetActive(true);
                        }
                    }
                }
            }
            SetFoodSlotDetail(foodSlotPreList[0].engName);
        }

        // int FindCurrentNum()
        // {
        //     for (int i = 0; i < foodSlotPreList.Count; i++)
        //     {
        //         if (curFoodName == foodSlotPreList[i].engName) return i;
        //     }
        //     return 0;
        // }

        void ShowNonePanel(int idx)
        {
            if (foodSlotPreList[idx].GetComponent<FoodSlotButton>().isDiscovery == false)
            {
                nonePanel.SetActive(true);
            }
            else nonePanel.SetActive(false);
        }

        // public void OnClickedPagePlipButton(string dir)
        // {
        //     int temp = FindCurrentNum();
        //     if(dir == "Left")
        //     {
        //         if (temp == 0)
        //         {
        //             SetFoodSlotDetail(foodSlotPreList[temp].engName);
        //         }
        //         else if(temp > 0)
        //         {
        //             temp--;
        //             SetFoodSlotDetail(foodSlotPreList[temp].engName);
        //         }
        //     }
        //
        //     if (dir == "Right")
        //     {
        //         if (temp == foodSlotPreList.Count - 1)
        //         {
        //             SetFoodSlotDetail(foodSlotPreList[temp].engName);
        //         }
        //         else if(temp < foodSlotPreList.Count)
        //         {
        //             temp++;
        //             SetFoodSlotDetail(foodSlotPreList[temp].engName);
        //         }
        //     }
        //     ShowNonePanel(temp);
        // }
    }
}