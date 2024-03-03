using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFlipper : MonoBehaviour
{
    [SerializeField] private AudioSource gravitySoundEffect;
    void Start()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Physics2D.gravity *= -1;
            gravitySoundEffect.Play();
            collision.transform.Rotate(Vector3.forward, 180f);
        }
    }
}
