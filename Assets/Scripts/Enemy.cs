using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IHealth
{
    public int health;
    public Slider healthBar;
    public Material mat;
    public Gradient color;
    public GameObject playerPos;
    public float enemySpeed;
    public List<SkinnedMeshRenderer> skinnedMeshes = new List<SkinnedMeshRenderer>();
    // Enemy bullet
    bool isShooting = false;
    public float bulletSpeed;
    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        healthBar.value = health;
        //mat.SetColor("_Color", color.Evaluate(health * .01f));
        SetColorOnMat();
        playerPos = GameObject.FindWithTag("Player");
        isShooting = false;
    }

    void SetColorOnMat()
    {
        foreach (var mesh in skinnedMeshes)
        {
            mesh.material = mat;
            mesh.material.SetColor("_Color", color.Evaluate(health * .01f));
        }
    }

   

    // Update is called once per frame
    void Update()
    {
        if (playerPos != null)
        {
            if (playerPos.activeInHierarchy)
            {
                if (enemySpeed > 0)
                    transform.position = Vector3.MoveTowards(transform.position, playerPos.transform.position, enemySpeed * Time.deltaTime);
                else
                {
                    if (!isShooting)
                        ShootTowardPlayer(playerPos.transform);
                }
                transform.transform.LookAt(playerPos.transform.position);

            }

        }

    }

    void ShootTowardPlayer(Transform target)
    {
        isShooting = true;
        GameObject bullet =GameManager.Instance.GetEnmeyBullet();
        bullet.GetComponent<EnmeyBullet>().enmeyBody = transform;
        bullet.transform.position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        Vector3 shootDirection = (target.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = shootDirection * bulletSpeed;
        StartCoroutine(ResetShoot());
    }

    IEnumerator ResetShoot()
    {
        yield return new WaitForSeconds(4f);
        isShooting = false;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            collision.collider.gameObject.SetActive(false);
            HealthDamage(34);
        }

    }


    public void HealthDamage(int value)
    {
        health -= value;
        healthBar.value = health;
        //mat.SetColor("_Color", color.Evaluate(health * .01f));
        SetColorOnMat();
        if (health <= 0)
        {
            gameObject.SetActive(false);
            // drop coin or healthbox when enemy dies 
            DropCoinOrHealthBox();
        }
    }

    void DropCoinOrHealthBox()
    {
        // drop coin or health box at enemy at random postion around dead raduis 
        Vector2 randomPoint = Random.insideUnitCircle * 1f;
        Vector3 coinDropPos = new Vector3(randomPoint.x, .2f, randomPoint.y);
        int randomNo = Random.Range(0, 6);
        if (randomNo == 0)
        {
            GameObject coins = GameManager.Instance.GetGoldCoin();
            coins.transform.position = transform.position + coinDropPos;
        }
        else if (randomNo == 1)
        {
            GameObject coins = GameManager.Instance.GetGoldCoin();
            coins.transform.position = transform.position + coinDropPos;
        }
        else if (randomNo == 2)
        {
            GameObject coins = GameManager.Instance.GetGoldCoin();
            coins.transform.position = transform.position + coinDropPos;

        }
        else if (randomNo == 3)
        {

            GameObject healthBox = GameManager.Instance.GetHealthBoxCoin();
            healthBox.transform.position = transform.position + coinDropPos;
        }
        else if (randomNo == 4)
        {
            GameObject coins = GameManager.Instance.GetGoldCoin();
            coins.transform.position = transform.position + coinDropPos;
        }
        else if (randomNo == 5)
        {
            GameObject coins = GameManager.Instance.GetGoldCoin();
            coins.transform.position = transform.position + coinDropPos;
        }
    }

    public void HealthGen(int value)
    {
        throw new System.NotImplementedException();
    }
}
