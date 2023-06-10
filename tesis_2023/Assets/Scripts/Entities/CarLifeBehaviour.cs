using UnityEngine;
using UnityEngine.Rendering;

public class CarLifeBehaviour : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    private Rigidbody rb;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {

            CarLifeBehaviour otherCarLife = collision.gameObject.GetComponent<CarLifeBehaviour>();
            if (otherCarLife != null)
            {
                float damageA = CalculateDamage(otherCarLife.rb.velocity.magnitude, rb.velocity.magnitude);
                float damageB = CalculateDamage(rb.velocity.magnitude, otherCarLife.rb.velocity.magnitude);
                TakeDamage(damageB);
                otherCarLife.TakeDamage(damageA);
            }
        }
    }

    private float CalculateDamage(float speedA, float speedB)
    {
        float dmgMultiplier = 5f;
        float damage = dmgMultiplier * speedB / (speedA + speedB);

        return damage;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= (int)damage;
        Debug.Log(gameObject.name + " life: " + currentHealth);
        Debug.Log(gameObject.name + "velocity: " + rb.velocity);

        if (currentHealth <= 0)
        {

            Destroy(gameObject);
        }
    }
}

