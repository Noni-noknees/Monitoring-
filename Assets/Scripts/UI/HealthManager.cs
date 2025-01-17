using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HealthManager : MonoBehaviour
{
    public float health = 100f;
    public Image healthBar;

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / 100;

        if (health <= 0)
        {
            SceneManager.LoadScene(3);
            Debug.Log("Player has died!");
        }
    }
    public void HealDamage(float heal)
    {
        health += heal;
        health = Mathf.Clamp(health,0,100);
        healthBar.fillAmount = health / 100;


    }
    private void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene(3);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(20);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            HealDamage(20);
        }
        }
}