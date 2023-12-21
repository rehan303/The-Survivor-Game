using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PlayerControler : MonoBehaviour, IHealth, Ixp
{
    public static PlayerControler Instance;
    public OnScreenJoystickInput playerInput;

    public float playerXp;
    public Slider xpBar;
    public TMP_Text levelShow;
    public int health;
    public Slider healthBar;
    public Gradient color;
    public Material mat;
    public Transform camraMove;
    public GameObject body;
    public Vector3 cameraOffset;
    public float speed;
    public Animator anim;
    public Transform circle;
    public float DefencRadius = 5f;

    public float bulletSpeed = 10f;
    bool isShooting;

    // player jump
    bool isJump = false;
    public float jumpForce =5;
    Transform nearestEnemy;
    Transform veryCloseEnemy;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        playerInput = new OnScreenJoystickInput();
    }
    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// set init values to the player
    /// </summary>
    public void Init()
    {
        healthBar.value = health;
        xpBar.value = playerXp;
        levelShow.text = GameManager.Instance.level.ToString();
        mat.SetColor("_Color", color.Evaluate(health * .01f));
        isShooting = false;
        isJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = playerInput.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        if (move.magnitude >= 0.1f)
        {
            // move to direction
            Vector3 moveDirection = move;
            moveDirection.y = 0;

            // Rotate player face to the direction
            body.transform.LookAt(transform.position + moveDirection);

            // Move player toward direction
            transform.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);
            // move camera wiht player
            camraMove.transform.position = new Vector3(transform.position.x, 0, transform.position.z) + cameraOffset;
        }

        // Defence radius around the player
        circle.localScale = new Vector3(DefencRadius * 2, DefencRadius * 2, 0);

        // Find and shoot enemy
        nearestEnemy = FindEnemyUnderTheRaduis();
        
        if (nearestEnemy != null && !isShooting)
        {
            Shoot(nearestEnemy);
        }
        // check if enemy is very close
        //veryCloseEnemy = FindEnemyVeryColse();
        //if (veryCloseEnemy != null && !isJump)
        //    Jump(veryCloseEnemy);

    }


    Transform FindEnemyUnderTheRaduis()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, DefencRadius, LayerMask.GetMask("Enemy"));

        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = collider.transform;
            }
        }

        return nearestEnemy;
    }


    void Shoot(Transform target)
    {
        isShooting = true;
        //GameObject bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
        GameObject bullet = GameManager.Instance.GetBullet();
        bullet.transform.position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        Vector3 shootDirection = (target.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = shootDirection * bulletSpeed;
        StartCoroutine(ResetShoot());
    }

    IEnumerator ResetShoot()
    {
        yield return new WaitForSeconds(.5f);
        isShooting = false;
    }


    public void HealthGen(int value)
    {
        health += value;
        healthBar.value = health;
        mat.SetColor("_Color", color.Evaluate(health * .01f));
        if (health >= 100)
        {
            health = 100;
        }
    }

    Transform FindEnemyVeryColse()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, DefencRadius / 2, LayerMask.GetMask("Enemy"));

        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = collider.transform;
            }
        }

        return nearestEnemy;
    }

    void Jump(Transform target)
    {
        // Calculate the direction toward enemy
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;
        direction.Normalize();
       GetComponent<Rigidbody>().AddForce(direction * jumpForce, ForceMode.Impulse);


    }

    public void HealthDamage(int value)
    {
        health -= value;
        healthBar.value = health;
        mat.SetColor("_Color", color.Evaluate(health * .01f));
        if (health <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.GameOverScreen();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            HealthDamage(5);
        }
        if (collision.collider.CompareTag("EnemyBullet"))
        {
            collision.collider.gameObject.SetActive(false);
            HealthDamage(10);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            // get xp
            PlayerXp(15f);

        }

        if (other.CompareTag("HealthBox"))
        {
            other.gameObject.SetActive(false);
            // get xp
            PlayerXp(5f);
            // get health gen
            HealthGen(15);



        }

    }

    public void PlayerXp(float value)
    {

        playerXp += value;
        if (playerXp >= 100)
        {
            // Increase level in xp is >= 100
            GameManager.Instance.level++;
            // show level on screen
            levelShow.text = GameManager.Instance.level.ToString();
            playerXp = 0;
        }
        // show xp bar
        xpBar.value = playerXp;


    }
}
