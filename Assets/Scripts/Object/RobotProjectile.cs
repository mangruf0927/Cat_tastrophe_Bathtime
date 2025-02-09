using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotProjectile : MonoBehaviour
{
    private Vector3 startPos, endPos;
    //땅에 닫기까지 걸리는 시간
    protected float timer;
    protected float timeToFloor;

    private bool isCollision = false;

    public float height = 1.5f;
    private float moveSpeed = 0.5f; // 이동 속도
    public Vector3 rotationAxis = Vector3.left; // 회전 축

    Vector3 Direction;
    Quaternion targetRotation;

    public GameObject explosion;
    public GameObject modeling;

    private void Start()
    {
        isCollision = true;
        transform.position = startPos;

        if (isCollision)
        {
            Direction = endPos - transform.position;
            targetRotation = Quaternion.LookRotation(Direction);
            transform.rotation = targetRotation;

            StartCoroutine("BulletMove");

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (null != other.transform.GetComponentInChildren<PlayerHitScan>())
            {
                other.transform.GetComponentInChildren<PlayerHitScan>().GetDamage();
            }
        }

        Explosion();
    }

    public void Explosion()
    {
        modeling.SetActive(false);
        explosion.SetActive(true);
        StartCoroutine(DestroyAfterParticles());
    }

    protected static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);


        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    protected IEnumerator BulletMove()
    {
        isCollision = false;
        timer = 0;
        while (transform.position.y >= endPos.y)
        {
            transform.rotation = Quaternion.Euler(Time.time * 90, 0, 0);

            timer += Time.deltaTime * moveSpeed;
            Vector3 tempPos = Parabola(startPos, endPos, height, timer);
            transform.position = tempPos;

            yield return new WaitForEndOfFrame();

        }

        Explosion();

    }

    private IEnumerator DestroyAfterParticles()
    {
        ParticleSystem ps = explosion.GetComponent<ParticleSystem>();

        while (ps != null && ps.IsAlive())
        {

            yield return null;
        }
        Destroy(transform.gameObject);

    }

    public float GetDamage()
    {
        return 5;
    }

    public void SetEndPos(Vector3 start, Vector3 end)
    {
        startPos = start;
        endPos = end;
        endPos.y = 0;
    }
}
