using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionUp : MonoBehaviour
{
    [SerializeField] PlayerController player;
    void OnTriggerEnter2D(Collider2D collision2D)
    {
        if (collision2D.gameObject.tag == "Player")
        {
            player
        }
    }
}
