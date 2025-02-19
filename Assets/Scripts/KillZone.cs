﻿using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage();
            other.GetComponent<PlayerHealth>().Respawn();
        }
    }
}
