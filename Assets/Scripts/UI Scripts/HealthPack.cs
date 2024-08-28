using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float healthAmount = 40f; // The amount of health this pack restores

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ship"))  
        {
            other.GetComponent<DamageSystem>().RecoverHealth(healthAmount);
            Destroy(gameObject);
             HealthPackManager.instance.HealthPackDestroyed();
        }
    }
}
