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

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Object")
        {
            Destroy(this.gameObject);
        }

        if (col.gameObject.tag == targetTag)
        {
            col.gameObject.GetComponent<MonsterController>().Damaged(damage);
        }
    }

    public IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(destroyTime);
        DungeonManager.instance.ReturnObject(poolingObjectName, this.gameObject);
        
        yield return null;
    }


    protected Vector3 dir;

    // Start is called before the first frame update
    public virtual void Initialize(Vector3 dir)
    {
        this.dir = DungeonManager.instance.player.transform.position - transform.position;
        this.dir = this.dir.normalized;
        StartCoroutine(DestroySelf());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }
}
