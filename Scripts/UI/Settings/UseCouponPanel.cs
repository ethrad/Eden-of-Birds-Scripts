using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseCouponPanel : MonoBehaviour
{
    public InputField couponInputField;
    
    public void OnCompleteButtonClicked()
    {
        // couponInputField.text �޾Ƽ� ������ ���
        BackendManager.Instance.CouponUse(couponInputField.text);
    }
}
