using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.ReadOnly;

public class RespawnPoint : MonoBehaviour
{
    [ReadOnly] public Vector2 respawnCoordinates;

    private void Start()
    {
        Transform transform = GetComponent<Transform>();
        respawnCoordinates = transform.localPosition;
    }
}
