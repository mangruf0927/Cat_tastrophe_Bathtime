using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class VirusProjectile : MonoBehaviour, IAttackable
{
    [Header("데이터")]
    public VirusProjectileData data;
    private float speed;
    private float damage;

    [Header("프리팹")]
    public GameObject projectile;
    public GameObject explosion;

    [Header("리지드바디")]
    public Rigidbody rigidBody;

    [Header("방향 정보")]
    public Vector3 directionPosition;
    private Vector3 targetDirection;

    
    private void Awake()
    {
        data.LoadDataFromPrefs();

        speed = data.speed;
        damage = data.damage;
    }

    private void OnEnable()
    {
        Invoke("Explosion", 5f);
        // 5초가 지나면 총아 ㄹ터짐
    }

    public void SetDirection(Vector3 direction)
    {
        // 총알이 가야 할 방향
        directionPosition = direction;
    }

    private void FixedUpdate()
    {
        // 실제 이동
        rigidBody.velocity = directionPosition * speed;
    }

    // 터지는 함수
    public void Explosion()
    {
        rigidBody.isKinematic = true;

        projectile.SetActive(false);
        explosion.SetActive(true);

        StartCoroutine(DestroyAfterParticles());
    }

    // 터지는 이펙트 끝났을 때 삭
    private IEnumerator DestroyAfterParticles()
    {

        ParticleSystem ps = explosion.GetComponentInChildren<ParticleSystem>();

        while (ps != null && ps.IsAlive())
        {

            yield return null;
        }

        Destroy(transform.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.GetComponentInChildren<PlayerHitScan>()!=null)
        {
            other.transform.GetComponentInChildren<PlayerHitScan>().GetDamage(GetDamage());
        }
        Explosion();
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.transform.GetComponentInChildren<PlayerHitScan>()!=null)
        {

            other.transform.GetComponentInChildren<PlayerHitScan>().GetDamage(GetDamage());
        }
        Explosion();
    }

    public float GetDamage()
    {
        return damage;
    }
}
