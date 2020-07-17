using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    [SerializeField] Transform respawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.transform.position = respawnPoint.position;
    }
}
