using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackmanLife : MonoBehaviour
{
    public bool dead = false;
    public GameObject deathParticles;

    void Start()
    {
        GridMovementManager.Instance.onGridReset += HandleGridReset;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Ghost") && !dead)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            GetComponent<SpriteRenderer>().enabled = false;

            dead = true;

            GameObject.Find("NextLevelSelector").GetComponent<NextLevelSelector>().HandleKill();
        }
    }

    void HandleGridReset()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        dead = false;
    }

    void OnDestroy()
    {
        GridMovementManager.Instance.onGridReset -= HandleGridReset;
    }
}
