using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    [SerializeField] List<GameObject> respawnPoints;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int index = Random.Range(0, respawnPoints.Count);
        collision.gameObject.transform.position = respawnPoints[index].GetComponent<RespawnPoint>().respawnCoordinates;
    }
}
