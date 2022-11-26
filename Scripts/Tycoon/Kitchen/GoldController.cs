using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldController : MonoBehaviour
{
    public float upLength;
    public float moveSpeed;
    public float rotationSpeed;

    IEnumerator coinUp()
    {
        for (float a = -26f; a <= upLength; a += Time.deltaTime * moveSpeed)
        {
            transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetComponent<RectTransform>().anchoredPosition.x, a);
            //transform.Rotate(new Vector2(0, a * rotationSpeed));
            yield return null;
        }
        Destroy(this.gameObject);
    }

    void Start()
    {
        StartCoroutine(coinUp());
    }
}
