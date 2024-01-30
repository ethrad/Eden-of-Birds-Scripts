using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed;
    public int damage;
    public string targetTag;
    public string poolingObjectName;
    public float destroyTime;
    
    [HideInInspector]
    public bool canShoot;

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Object"))
        {
            DungeonManager.instance.ReturnObject(poolingObjectName, gameObject);
        }

        if (col.gameObject.CompareTag(targetTag))
        {
            col.gameObject.GetComponent<MonsterController>().Damaged(gameObject);
        }
    }

    protected virtual IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(destroyTime);
        DungeonManager.instance.ReturnObject(poolingObjectName, gameObject);
        
        yield break;
    }

    [HideInInspector]
    public Vector3 dir;
    
    public virtual void Initialize(Vector3 pos)
    {
        transform.position = pos;
        StartCoroutine(DestroySelf());
    }
    
    protected virtual void FixedUpdate()
    {
        transform.Translate(dir * (moveSpeed * Time.fixedDeltaTime));
    }
}
