using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using LitJson;
using BackEnd;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager Instance;

    void Awake(){
        if (Instance == null){
            Instance = this;
        }
    }

    
    //클래스 데이터 넣기
    public class GameData {
        public int month = 0;
        public int year = 0;
        public int tycoonEnteredCounti = 0;
        public int dungeonRemainCount = 0;
        public int reputation = 0;
        public bool canEnterTycoon = false;
        public bool canEnterDungeon = false;
        public int gold = 0;
        public int feather = 0;
    }

    public class ResidentFriendship {
        public int friendship = 0;
        public int level = 0;
        public bool isInteractedInThisMonth = false;
        public bool isGivingPresent = false;
    }

    public class OngoingQuestGoal {
        public int test = 0;
    }

    public void InsertPlayerData() { //초기값 데이터 삽입

        GameData GameData = new GameData();
        ResidentFriendship ResidentFriendship = new ResidentFriendship();
        OngoingQuestGoal OngoingQuestGoal = new OngoingQuestGoal();

        List<int> list_i = new List<int>();
        List<String> list_s = new List<String>();
        List<OngoingQuestGoal> OngoingQuestGoal_list = new List<OngoingQuestGoal>();

        Dictionary<string, List<string>> MenuOfTheMonth = new Dictionary<string, List<string>>
        {};

        Dictionary<string, List<string>> OwnRecipes = new Dictionary<string, List<string>>
        {};

        Dictionary<string, ResidentFriendship> residentFriendships = new Dictionary<string,  ResidentFriendship>
        {};

        Dictionary<string, int> inventory = new Dictionary<string, int>
        {};

        Dictionary<string, List<int>> clearedQuests = new Dictionary<string, List<int>>
        {};

        Dictionary<string, List<OngoingQuestGoal>> ongoingQuests = new Dictionary<string, List<OngoingQuestGoal>>
        {};

        
        Param param = new Param();
        param.Add("GameData", GameData);
        param.Add("MenuOfTheMonth", MenuOfTheMonth);
        param.Add("OwnRecipes", OwnRecipes);
        param.Add("residentFriendships", residentFriendships);
        param.Add("inventory", inventory);
        param.Add("clearedQuests", clearedQuests);
        param.Add("ongoingQuests", ongoingQuests);

        Backend.GameData.Insert("Player_Info", param);
    }

    public void UpdatePlayerData<T>(string tableName, string columnName, T updateData) { //플레이어 데이터 수정 (테이블명, 열머리, 수정데이터)
        BackendReturnObject bro = Backend.BMember.GetUserInfo ();
        string owner_inDate = Backend.UserInDate; //유저 키값 inDate
        Debug.Log("게이머 inDate : " + owner_inDate);

        Param updateParam = new Param();
        updateParam.Add(columnName, updateData);

        Where where = new Where();
        where.Equal("owner_inDate", owner_inDate);

        Backend.GameData.Update(tableName, where, updateParam);
    }

    public void getMyData() { //유저 정보 조회
        var bro = Backend.GameData.GetMyData("Player_Info", new Where(), 10);
        Debug.Log("내 정보 조회: " + bro);
    }

    public T readData<T>(string tableName, string attribute) where T : new() {
        Where where = new Where();
        string[] select = {attribute};

        var bro = Backend.GameData.GetMyData(tableName, where, select);
        var json = BackendReturnObject.Flatten(bro.Rows());
        var data = JsonMapper.ToObject<T>(json[0][attribute].ToJson());

        if(bro.IsSuccess() == false) { //불러오기 실패
            Debug.Log("데이터 불러오기 실패");
            return new T();
        }
        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0) { //데이터가 0개
            Debug.Log("데이터 없음 : " + bro);
            return new T();
        }
        return data;
    }


/* 백업 테이블 만들기?
    PlayerInfo playerInfo = new PlayerInfo();
    JsonData loadData;
    string playerInfo_inDate;

    private void GetPlayerData() {


        SendQueue.Enqueue(Backend.GameData.GetMyData, "Player_Info", new Where(), AsyncCallback => {
            Debug.Log("Player_GetMyData" + callback.ToString());

            if(callback.IsSuccesss()){
                if(AsyncCallback.FlattenRows().Count > 0){
                    Debug.Log("데이터가 1개이상 존재합니다. 최신 데이터로 적용합니다.");
                    loadData = callback.FlattenRows();
                    playerInfo.SetData(AsyncCallback.FlattenRows()[0]);
                    playerInfo_inDate = callback.FlattenRows()[0]["inDate"].Tostring();

                    //현재 시간 불러오기
                    CherckNewInDateNeed();
                }
                else {
                    Debug.LogWarning("데이터가 존재하지 않습니다. 새로 삽입합니다.");
                    playerInfo.Init();
                    //LoadFinish(); //데이터를 불러오고 난 후 게임을 시작하기 위함
                }
            }
            else {
                Debug.LogError("에러가 발생하였습니다. 다시 시도해주세요." + callback.ToString());
            }

        });
    }*/

   /*LoadFinish(){
        SceneManager.LoadScene("InGame");
    }*/
}
