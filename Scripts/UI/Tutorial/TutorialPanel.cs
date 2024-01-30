using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class NotebookTutorial
{
    public Dictionary<string, List<string>> subhead;
}

namespace TutorialDiary
{
    public class TutorialPanel : MonoBehaviour
    {
        public Image tutorialImage;
        public Text tutorialText;
        
        int pageIndex = 0;

        public GameObject rightPageButton;
        public GameObject leftPageButton;
        public GameObject titleButtons;
        public GameObject subheadButtons;
        Dictionary<string, List<GameObject>> subheadButtonList = new Dictionary<string, List<GameObject>>();


        private GameObject selectTitleButton;
        private GameObject selectSubheadButton;

        public Sprite selectTitleButtonImage;
        public Sprite normalTitleButtonImage;
        public Sprite selectSubheadButtonImage;
        public Sprite normalSubheadButtonImage;

        public Dictionary<string, NotebookTutorial> notebookTutorialList;

        bool isInitialized = false;
        void ReadTutorialData()
        {
            notebookTutorialList = IOManager.instance.ReadJsonFromResources<Dictionary<string, NotebookTutorial>>("NotebookTutorial");
        }
        
        public void OnTutorialButtonClicked()//튜토리얼 버튼 클릭
        {
            if (!isInitialized)
            {
                isInitialized = true;
                ReadTutorialData();
            }
            
            for (int i = 0; i < titleButtons.transform.childCount; i++)
            {
                List<GameObject> tempSubheadButtonList = new List<GameObject>();
                for (int j = 0; j < subheadButtons.transform.GetChild(i).transform.childCount; j++)
                {
                    //소제목 버튼들 임시로 리스트에 추가
                    tempSubheadButtonList.Add(subheadButtons.transform.GetChild(i).transform.GetChild(j).gameObject);
                }
                //버튼 리스트 들에는 키값을 카테고리 이름으로, 값들을 소제목 버튼들로
                subheadButtonList.Add(titleButtons.transform.GetChild(i).gameObject.name, tempSubheadButtonList);
            }
            //첫 번째 타이틀 버튼이랑 소제목 버튼을 각각 할당
            SetFirstButtonSetting(0, 0);
            SelectedButtonSpriteChange(selectTitleButton, selectSubheadButton);
            SetPage(selectTitleButton, selectSubheadButton, 0);
            pageIndex = 0;
        }
        
        public void OnTitleButtonClicked() //카테고리 바꿈
        {
            //선택한 타이틀 버튼에 맞춰 소제목 버튼 변경
            subheadButtonList[selectTitleButton.name][0].transform.parent.gameObject.SetActive(false);
            DisabledButtonSpriteChange(selectTitleButton, selectSubheadButton);
            
            //선택한 대제목 버튼
            selectTitleButton = EventSystem.current.currentSelectedGameObject;
            subheadButtonList[selectTitleButton.name][0].transform.parent.gameObject.SetActive(true);
            Debug.Log(selectTitleButton.name);
            //선택한 대제목 버튼에 해당하는 소제목 버튼들 중 첫번째 소제목 버튼
            selectSubheadButton = subheadButtonList[selectTitleButton.name][0];
            
            SelectedButtonSpriteChange(selectTitleButton, selectSubheadButton);
            pageIndex = 0;
            SetPage(selectTitleButton, selectSubheadButton, 0);
        }

        public void OnSubheadButtonClicked() //카테고리 내에서 다른 내용으로 바꿈
        {
            selectSubheadButton.GetComponent<Image>().sprite = normalSubheadButtonImage;
            selectSubheadButton = EventSystem.current.currentSelectedGameObject;
            selectSubheadButton.GetComponent<Image>().sprite = selectSubheadButtonImage;
            pageIndex = 0;
            SetPage(selectTitleButton, selectSubheadButton, 0);
        }

        void SelectedButtonSpriteChange(GameObject title, GameObject subhead)
        {
            title.GetComponent<Image>().sprite = selectTitleButtonImage;
            subhead.GetComponent<Image>().sprite = selectSubheadButtonImage;
        }
        void DisabledButtonSpriteChange(GameObject title, GameObject subhead)
        {
            title.GetComponent<Image>().sprite = normalTitleButtonImage;
            subhead.GetComponent<Image>().sprite = normalSubheadButtonImage;
        }
        

        //이미지, 텍스트 바꿔주는 함수
        void SetPage(GameObject title, GameObject subhead, int pageNum)
        {
            //페이지가 1개일때
            if (notebookTutorialList[selectTitleButton.name].subhead[selectSubheadButton.name].Count == 1)
            {
                //버튼 다 끔
                DisabledButton();
            }
            else //페이지가 2개 이상일때
            {
                //페이지가 1페이지일때
                if (pageNum == 0)
                {
                    leftPageButton.SetActive(false);
                    rightPageButton.SetActive(true);
                }
                //페이지가 마지막 페이지일때
                else if (pageNum == notebookTutorialList[title.name].subhead[subhead.name].Count - 1)
                {
                    leftPageButton.SetActive(true);
                    rightPageButton.SetActive(false);
                }
                //페이지가 중간 페이지일때
                else
                {
                    leftPageButton.SetActive(true);
                    rightPageButton.SetActive(true);
                }
            }
            string titleName = title.name;
            string subheadName = subhead.name;
            tutorialText.text = notebookTutorialList[titleName].subhead[subheadName][pageNum];
            tutorialImage.sprite = Resources.Load<Sprite>("Diary/Tutorial/" + titleName + "/" + subheadName + "/" + subheadName +pageNum);
        }
     
        public void OnExitButtonClicked()
        {
           // tutorialPages[pageIndex].SetActive(false);
            gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        void SetFirstButtonSetting(int titleButtonNum, int subheadButtonNum)
        {
            selectTitleButton = titleButtons.transform.GetChild(titleButtonNum).gameObject;
            selectSubheadButton = subheadButtonList[titleButtons.transform.GetChild(subheadButtonNum).gameObject.name][0];
        }

        void DisabledButton()
        {
            leftPageButton.SetActive(false);
            rightPageButton.SetActive(false);
        }
        
        public void OnClickedLeftButton()
        {
            //현재 페이지가 0보다 클때
            if (0 < pageIndex )
            {
                pageIndex--;
                SetPage(selectTitleButton, selectSubheadButton, pageIndex);
            }
            else // 현재 페이지가 0보다 작거나 같을때
            {
                //페이지 넘으면 버튼 비활
                pageIndex = 0;
            }
        }

        public void OnClickedRightButton()
        {
            //현재 페이지가 0보다는 크고 전체 페이지의 수보다 작을 때
            if (0 <= pageIndex && pageIndex < notebookTutorialList[selectTitleButton.name].subhead[selectSubheadButton.name].Count-1)
            {
                pageIndex++;
                SetPage(selectTitleButton, selectSubheadButton, pageIndex);
            }
            else
            {
                //페이지 넘으면 버튼 비활
                pageIndex = notebookTutorialList[selectTitleButton.name].subhead[selectSubheadButton.name].Count - 1;
            }
        }
    }
}
