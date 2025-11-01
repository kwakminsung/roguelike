using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive = true;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
        coll = GetComponent<Collider2D>();
    }


    void FixedUpdate()
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.linearVelocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!isLive || target == null)
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        if (Gamemanager.instance == null || Gamemanager.instance.player == null)
        {
            Debug.LogWarning("Enemy.OnEnable: Gamemanager or Player not assigned");
            return;
        }
        target = Gamemanager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;

        if(animCon == null || animCon.Length == 0)
        {
            Debug.LogError("Enemy: animCon 배열이 비어 있습니다. spriteType={data.spriteType}");
            return;
        }

        int idx = Mathf.Clamp(data.spriteType, 0, animCon.Length - 1);
        if (idx != data.spriteType)
            Debug.LogError($"Enemy: 잘못된 spriteType {data.spriteType} 이 animcon 범위를 벗어남. {idx}로 보정함. animCon.Length={animCon.Length}");
        anim.runtimeAnimatorController = animCon[idx];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(knockBack());


        if ((health > 0))
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
        }
    }

    IEnumerator knockBack()
    {
        yield return wait; // 다음 하나의 물리 프레임 대기
        Vector3 PlayerPos = Gamemanager.instance.player.transform.position;
        Vector3 dirVec = transform.position - PlayerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
