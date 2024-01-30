using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialThanksPanel : MonoBehaviour
{
    public GameObject specialThanksTextPrefab;
    public GameObject textGroupPanel;
    
    private void Start()
    {
        string specialThanks = "유진\n정시윤\n시카\n요안\n트롤중퇴장인\nlawming\n김회인\n박민철\n유생\n디디\n혠유\n내얼굴전국훈남\n" +
                               "서연\n소서로\n국밥 가즈아\n시리\n달새(까마귀)\n하선로\nYJL\n서서\n곰돌이토낑\n향로\n설꿈\nSilent401\n" +
                               "Alien\n장하나\n미닝\n미르스\n해령\n호랑S2\n동동\n설월화\n예빈\n유림\n김시영\n틱텍톡\n엘라\n까만밤\n히잉\n최혜원\n" +
                               "piyoppi\n렌테\n김계란후라이\n푸른자몽\n대구엄마 sook\u2606\n으랭\n전설의 요리사 J\n늉늉\n개락\nSorb\n개발자남자친구\n" +
                               "류은향\n커피캣\n레티시엘\n딱따구리08\n주녕\n가연탄배상곰\n작은멋쟁이나비\n현재환\n신민성\n밀크티타임\n이그노스트\n졍\n기멘\n" +
                               "레르키노\n오성민프롬제이피\n로투스\n지니어스\n장서현\n쨘\n비닙\n양정제\n최병희\nSIW SIS\n재홍이의메론농장\n심양오\n" +
                               "chester\n매콤주먹무라딘\n맛소금\n청월적일\n수쟈뿅\n가이코츠\n김하르\n인훈\n현서\n작은멋쟁이나비\nk-jin";

        foreach (string name in specialThanks.Split('\n'))
        {
            var tempText = Instantiate(specialThanksTextPrefab, textGroupPanel.transform);
            tempText.GetComponent<Text>().text = name;
        }
    }
}
