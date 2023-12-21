using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnmeyBullet : MonoBehaviour
{

  public  Transform enmeyBody;
   
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, enmeyBody.position) > 20f)
        {
            gameObject.SetActive(false);
        }
    }
}
