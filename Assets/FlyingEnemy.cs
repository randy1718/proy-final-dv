using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float life;
    [SerializeField] AudioClip audioDeath;
    Rigidbody2D myBody;
    bool isDead = false;
    Animator myAnim;
    AIPath MyPath;
    // Start is called before the first frame update
    void Start()
    {
        MyPath = GetComponent<AIPath>();
        myAnim = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
    }

    IEnumerator destroyEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (isDead) {  
                Debug.Log("muertoooo!!!!!!!!!!!!!!!!!!!");
                Destroy(gameObject);
            }
        }

    }

    void ChasePlayer()
    {
        Debug.DrawRay(transform.position,player.transform.position,Color.red);
        Collider2D col = Physics2D.OverlapCircle(transform.position,8f,LayerMask.GetMask("Player"));
        if (col != null)
        {
            MyPath.isStopped = false;
        }
        else
        {
            MyPath.isStopped = true;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,6f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            if (life == 0)
            {
                MyPath.isStopped = true;
                myAnim.SetBool("isDead", true);
                isDead = true;
                AudioSource.PlayClipAtPoint(audioDeath, transform.position);
                StartCoroutine(destroyEnemy());
            }
            else
            {
                life--;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Megaman")
        {
            myBody.bodyType = RigidbodyType2D.Static;
        }
    }
}
