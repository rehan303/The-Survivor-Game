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

    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        healthBar.value = health;
        mat.SetColor("_Color", color.Evaluate(health * .01f));
        playerPos = GameObject.FindWithTag("Player");
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

                transform.transform.LookAt(playerPos.transform.position);

            }

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            collision.collider.gameObject.SetActive(false);
            HealthDamage(50);
        }

    }


    public void HealthDamage(int value)
    {
        health -= value;
        healthBar.value = health;
        mat.SetColor("_Color", color.Evaluate(health * .01f));

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
