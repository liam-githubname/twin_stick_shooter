using UnityEngine;
using UnityEngine.UI;
public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform target;
    void Start()
    {
        // Automatically assign the main camera at startup
        mainCamera = Camera.main; // Camera.main automatically finds the camera tagged as "Main Camera"

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found. Ensure your camera is tagged as 'Main Camera'.");
        }

        Transform parent = gameObject.transform.parent;

        if (parent != null)
        {
            target = parent.parent;
        }
    }
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
        if (slider != null && slider.value <= 0 && gameObject != null)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (target != null)
        {
            transform.rotation = mainCamera.transform.rotation;
            float upwardOffset = 1.4f;
            transform.position = target.position + Vector3.up * upwardOffset + new Vector3(0, 0, 16);
        }

        if (gameObject.transform.parent.gameObject.transform.parent.GetComponent<SpriteRenderer>().sprite.name.Contains("Fighter") || gameObject.transform.parent.gameObject.transform.parent.GetComponent<SpriteRenderer>().sprite.name.Contains("Destroyer") || gameObject.transform.parent.gameObject.transform.parent.GetComponent<SpriteRenderer>().sprite.name.Contains("Alien"))
            gameObject.transform.parent.gameObject.SetActive(true);
        else
            gameObject.transform.parent.gameObject.SetActive(false);
    }
}
