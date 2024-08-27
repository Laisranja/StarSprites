using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLifetime = 2f;

    // Update is called once per frame
    private void Awake ()
    {
        Destroy(gameObject, bulletLifetime);
    }
}

