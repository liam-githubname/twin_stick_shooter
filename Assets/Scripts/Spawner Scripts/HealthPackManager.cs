using UnityEngine;

public class HealthPackManager : MonoBehaviour
{
    public static HealthPackManager instance;
    public GameObject healthPackPrefab;
    private int currentHealthPacks = 0;
    private int maxHealthPacks = 4;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanSpawnHealthPack()
    {
        return currentHealthPacks < maxHealthPacks;
    }

    public void SpawnHealthPack(Vector3 position)
    {
        if (CanSpawnHealthPack())
        {
            GameObject healthpack = Instantiate(healthPackPrefab, position, Quaternion.identity);
            healthpack.transform.parent = GameObject.Find("HealthPackManager").transform;
            currentHealthPacks++;
        }
    }

    public void HealthPackDestroyed()
    {

        currentHealthPacks--;
        currentHealthPacks = Mathf.Max(0, currentHealthPacks);
    }
}
