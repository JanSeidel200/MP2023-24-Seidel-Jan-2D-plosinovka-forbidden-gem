using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement pm;
    private bool isDead = false;
    private Vector2 originalGravity;
    [SerializeField] AudioSource deathSoundEffect;

    private void Start()
    {
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();

        originalGravity = Physics2D.gravity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead && collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        pm.enabled = false;
        anim.SetTrigger("death");
        deathSoundEffect.Play();
        Physics2D.gravity = originalGravity;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}