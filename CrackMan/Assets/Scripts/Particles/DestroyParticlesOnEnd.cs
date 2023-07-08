using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticlesOnEnd : MonoBehaviour
{
    ParticleSystem _particleSystem;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!_particleSystem.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
