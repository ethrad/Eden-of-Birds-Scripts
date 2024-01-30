using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Town
{
    public class NewspaperPanel : ObjectInteractionPanel
    {
        public Text title;
        public RectTransform background;
        public GameObject emptyStars;
        public Text leftHeadline;
        public Text leftBody;
        public Text rightHeadline;
        public Text rightBody;

        bool isInitialized;

        public void SetNewsPaperPanelSize()
        {
            if (GameManager.instance.gameData.purchasedAdFree == false)//광고제거권 없으면 사이즈 변경해주기
            {
                background.offsetMax = new Vector2(-70, 0);
                background.offsetMin = new Vector2(70, GameManager.instance.gameObject.GetComponent<LoadMobileAD>().DebugBannerViewHeight() + 30);
            }
        }

        protected override void Initialize()
        {
            isInitialized = true;
            ReputationData tempReputationData = null;

            foreach (var reputationData in GameManager.instance.reputationData)
            {
                if (GameManager.instance.gameData.reputation <= int.Parse(reputationData.Key))
                {
                    tempReputationData = reputationData.Value;
                    break;
                }
            }

            for (int i = 0; i < tempReputationData.star; i++)
            {
                emptyStars.transform.GetChild(i).gameObject.SetActive(false);
            }

            title.text = tempReputationData.title;

            int randomIndex = Random.Range(0, tempReputationData.leftHeadline.Count);
            leftHeadline.text = tempReputationData.leftHeadline[randomIndex];
            leftBody.text = tempReputationData.leftBody[randomIndex];

            randomIndex = Random.Range(0, tempReputationData.rightHeadline.Count);
            rightHeadline.text = tempReputationData.rightHeadline[randomIndex];
            rightBody.text = tempReputationData.rightBody[randomIndex];
        }

        public override void ExitPanel()
        {
            base.ExitPanel();
            LoadMobileAD.Instance.DestroyBannerView();
            GetComponent<UIPanelEffect>().OffPanel(1);
        }

        public override void Interact()
        {
            if (!isInitialized)
            {
                Initialize();
            }

            if (!GameManager.instance.gameData.purchasedAdFree)
            {
                LoadMobileAD.Instance.LoadBanner();
            }

            gameObject.SetActive(true);
            SetNewsPaperPanelSize();
        }
    }
}
