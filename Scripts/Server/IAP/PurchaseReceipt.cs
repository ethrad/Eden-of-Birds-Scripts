using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseReceipt : MonoBehaviour
{
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
    {
        /*
        뒤끝 영수증 검증 처리
        */
        BackendReturnObject validation = Backend.Receipt.IsValidateGooglePurchase ( args.purchasedProduct.receipt , "receiptDescription", false);
        string id = string.Empty;
        string token = string.Empty;
        Param param = new Param();

        #if UNITY_IOS
        // ios의 경우 PurchaseEventArgs에 포함되어 있는 영수증 토큰을 그대로 뒤끝 펑션으로 송신하면 됩니다.
        param.Add("token", args.purchasedProduct.receipt);

        #elif UNITY_ANDROID
        // android의 경우 PurchaseEventArgs에 포함되어 있는 영수증 토큰을
        // BackEnd.Game.Payment.GoogleReceiptData.FromJson 함수를 이용하여 파싱하여 id와 token 값을 추출한 뒤
        // 이를 뒤끝 펑션으로 송신해야 합니다.
        BackEnd.Game.Payment.GoogleReceiptData.FromJson(args.purchasedProduct.receipt, out id, out token);
        param.Add("productID", id);
        param.Add("token", token);
        #endif

        // 뒤끝 펑션 호출
        Backend.BFunc.InvokeFunction("receiptVaildate", param, callback => {
            // TODO: 뒤끝 펑션 진행 후 결과 확인
        });

        /*
        // 영수증 검증에 성공한 경우
        
        if(validation.IsSuccess())
        {
            // 구매 성공한 제품에 대한 id 체크하여 그에 맞는 보상 
            // A consumable product has been purchased by this user.
            if (String.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                
                ScoreManager.score += 100;
            }

        }
        // 영수증 검증에 실패한 경우 
        else 
        {
            // Or ... an unknown product has been purchased by this user. Fill in additional products here....
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 

        */
        return PurchaseProcessingResult.Complete;
    }
}