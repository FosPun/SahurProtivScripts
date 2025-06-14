using System;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Vector3 teleportLocation;

private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = teleportLocation;
        }
    }
}
