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
        string specialThanks = "����\n������\n��ī\n���\nƮ����������\nlawming\n��ȸ��\n�ڹ�ö\n����\n���\n����\n���������Ƴ�\n" +
                               "����\n�Ҽ���\n���� �����\n�ø�\n�޻�(���)\n�ϼ���\nYJL\n����\n�������䳩\n���\n����\nSilent401\n" +
                               "Alien\n���ϳ�\n�̴�\n�̸���\n�ط�\nȣ��S2\n����\n����ȭ\n����\n����\n��ÿ�\nƽ����\n����\n���\n����\n������\n" +
                               "piyoppi\n����\n�����Ķ���\nǪ���ڸ�\n�뱸���� sook\u2606\n����\n������ �丮�� J\n����\n����\nSorb\n�����ڳ���ģ��\n" +
                               "������\nĿ��Ĺ\n��Ƽ�ÿ�\n��������08\n�ֳ�\n����ź����\n���������̳���\n����ȯ\n�Źμ�\n��ũƼŸ��\n�̱׳뽺Ʈ\n��\n���\n" +
                               "����Ű��\n����������������\n������\n���Ͼ\n�弭��\nº\n���\n������\n�ֺ���\nSIW SIS\n��ȫ���Ǹ޷г���\n�ɾ��\n" +
                               "chester\n�����ָԹ����\n���ұ�\nû������\n�����\n��������\n���ϸ�\n����\n����\n���������̳���\nk-jin";

        foreach (string name in specialThanks.Split('\n'))
        {
            var tempText = Instantiate(specialThanksTextPrefab, textGroupPanel.transform);
            tempText.GetComponent<Text>().text = name;
        }
    }
}
