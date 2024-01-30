using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PixelCrushers.DialogueSystem;
using Town;
using UnityEngine.Serialization;
using ResidentLevelTitle;

public class TownManager : MonoBehaviour
{
    public static TownManager instance;

    [Header("Player and NPCs")]
    public GameObject player;
    public GameObject NPCs;
    public GameObject incaTern;

    [Header("Camera and Bounds")]
    public new Camera camera;
    public GameObject virtualCamera;
    public List<GameObject> floorBounds;

    [Header("Portal and Position")]
    public List<GameObject> portals;
    public GameObject playerIncaTernPos;

    [Header("Buttons")]
    public GameObject joystick;
    public GameObject NPCInteractButton;
    public GameObject ObjectInteractButton;
    public GameObject NormalResidentInteractButton;

    [Header("Panels")]
    public GameObject basePanel;
    public GameObject minimapHoldingPanel;
    public Text date;
    public GameObject MOTMPanel;
    public GameObject ResidentFriendshipTitlePanel;
    public GameObject questPanel;
    public GameObject questTutorialPanel;

    [Header("Sounds")]
    public AudioSource backgroundMusic;

    public Dictionary<string, ResidentController> residentDic = new Dictionary<string, ResidentController>();

    #region Interact

    bool canClick = false;
    [HideInInspector]
    public string residentName;

    [HideInInspector]
    public GameObject selectedNPC;
    [HideInInspector]
    public GameObject selectedObject;




    public void UpdateInteractionState(GameObject go, string residentName)
    {
        canClick = true;
        selectedNPC = go;
        this.residentName = residentName;

        NPCInteractButton.SetActive(true);
    }


    public void ResetInteractionState()
    {
        canClick = false;
        residentName = "";

        NPCInteractButton.SetActive(false);
    }

    public void UpdateInteractedObjectState(GameObject obj, bool isNormalResident)
    {
        selectedObject = obj;
        if (isNormalResident)
        {
            NormalResidentInteractButton.SetActive(true);
        }
        else
        {
            ObjectInteractButton.SetActive(true);
        }
    }

    public void ResetInteractedObjectState(bool isNormalResident)
    {
        selectedObject = null;
        if (isNormalResident)
        {
            NormalResidentInteractButton.SetActive(false);
        }
        else
        {
            ObjectInteractButton.SetActive(false);
        }
    }


    public GameObject interactionPanel;

    public void OnNPCInteractButtonClicked()
    {
        if (canClick)
        {
            OffBasePanel();
            interactionPanel.GetComponent<InteractionPanel>().UpdatePanel();
        }
    }

    public void OnObjectInteractButtonClicked()
    {
        OffBasePanel();
        selectedObject.GetComponent<ObjectController>().Interact();
    }

    #endregion

    #region Quest and Conversation

    private void ReloadQuests()
    {
        for (int i = 0; i < NPCs.transform.childCount; i++)
        {
            NPCs.transform.GetChild(i).GetComponent<ResidentController>().CheckQuest();
        }
    }


    public Quest selectedQuest;
    [HideInInspector]
    public bool isProgressQuest;

    public void OnConversationStart()
    {
        questPanel.SetActive(false);
        basePanel.SetActive(false);
    }

    public void OnConversationEnd()
    {
        if (selectedNPC == null)
        {
            basePanel.SetActive(true);
            return;
        }

        if (selectedQuest != null)
        {
            if (isProgressQuest)
            {
                GameManager.instance.AcceptQuest(selectedQuest);
            }
            else
            {
                GameManager.instance.ClearQuest(selectedQuest);
            }

            selectedQuest = null;
        }

        questPanel.GetComponent<QuestPanel>().ResetQuestPanel();
        selectedNPC.GetComponent<ResidentController>().questionMark.SetActive(false);
        selectedNPC.GetComponent<ResidentController>().questionMarkMinimap.SetActive(false);
        selectedNPC.GetComponent<ResidentController>().exclamationMark.SetActive(false);
        selectedNPC.GetComponent<ResidentController>().exclamationMarkMinimap.SetActive(false);

        basePanel.SetActive(true);

        ReloadQuests();
    }

    #endregion

    public Dictionary<string, ResidentFriendshipLevelTite> resLevelTitleDic;

    void SetResidentDic()
    {
        for (int i = 0; i < NPCs.transform.childCount; i++)
        {
            residentDic.Add(NPCs.transform.GetChild(i).GetComponent<ResidentController>().residentName, NPCs.transform.GetChild(i).GetComponent<ResidentController>());
        }
    }

    private void ReadResidentFriendshipLevelTitle()
    {
        resLevelTitleDic = IOManager.instance.ReadJsonFromResources<Dictionary<string, ResidentFriendshipLevelTite>>("ResidentFriendshipLevelTitle");
    }

    public void OnBasePanel()
    {
        basePanel.SetActive(true);
    }

    public void OffBasePanel()
    {
        player.GetComponent<TownPlayerController>().Brake();
        basePanel.SetActive(false);
    }

    public void RegisterResidentFriendship()
    {
        GameManager.instance.InitializedResidentFriendshipList(residentName);
        GameManager.instance.WriteResidentFriendShip();
        ResidentFriendshipTitlePanel.GetComponent<InteractionPanelResidentFriendship>().UpdatePanel();
    }

    public void ApplySettings()
    {
        var settings = GameManager.instance.settings;

        // 배경음악 크기 설정
        backgroundMusic.volume = settings.backgroundMusicVolume;

        // 효과음 크기 설정
        player.GetComponent<AudioSource>().volume = settings.soundEffectsVolume;

        // 미니맵 홀딩 방식 바꾸기
        minimapHoldingPanel.GetComponent<MinimapHoldingPanel>().isHoldSetting = settings.minimapHolding;

        joystick.GetComponent<Joystick>().SetJoystick(settings.joystickValues);
    }

    private void UpdateDate()
    {
        date.GetComponent<Text>().text = GameManager.instance.gameData.year + "년째 " + GameManager.instance.gameData.month + "월";
    }

    private void ChangeMenuOfTheMonth()
    {
        GameManager.instance.isMonthChanged = false;

        MOTMPanel.GetComponent<MOTMPanel>().Initialize();
    }

    private void SetPlayerAndIncaTernPosition()
    {
        incaTern.SetActive(true);
        player.transform.position = playerIncaTernPos.transform.position;
    }

    private void ApplySkills()
    {
        foreach (var skillList in GameManager.instance.earnedSkills)
        {
            if (skillList.Key == "all")
            {
                foreach (var skill in skillList.Value)
                {
                    switch (skill.skillType)
                    {
                        case "speed":
                            player.GetComponent<TownPlayerController>().moveSpeed +=
                                player.GetComponent<TownPlayerController>().moveSpeed * skill.skillValue;
                            break;
                    }
                }
            }
        }
    }

    private void Awake()
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

    private void Start()
    {
        UpdateDate();
        ApplySettings();
        ApplySkills();
        ReadResidentFriendshipLevelTitle();
        GameManager.instance.FadeInEffect();
        GameManager.instance.ResetFadeFlags();
        if (GameManager.instance.isMonthChanged)
        {
            ChangeMenuOfTheMonth();
        }

        if (GameManager.instance.gameData.isIncaTernInTown)
        {
            SetPlayerAndIncaTernPosition();
        }

        SetResidentDic();
        questTutorialPanel.GetComponent<QuestTutorial>().StartQuestTutorial();
        purchasedAdFree = GameManager.instance.gameData.purchasedAdFree;
    }
    
    
    private bool purchasedAdFree;
    private float adTimer;
    [Header("For Ads")]
    [SerializeField]
    private float adDelay;

    public void OnAdFree()
    {
        purchasedAdFree = true;
    }

    private void Update()
    {
        if (purchasedAdFree) return;

        if (adTimer >= adDelay)
        {
            LoadMobileAD.Instance.LoadInterstitialAd();
            adTimer = 0;
        }

        adTimer += Time.deltaTime;
    }
}
