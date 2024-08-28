using UnityEngine;
using TMPro;

public class ResponsiveFontSize : MonoBehaviour
{
    public TMP_Text textComponent;
    public float baseScreenWidth = 1920f; // Base screen width to match the design
    [SerializeField] float baseFontSize; // Base font size at the base screen width

    void Start()
    {
        UpdateFontSize();
    }

    void Update()
    {
        UpdateFontSize();
    }

    private void UpdateFontSize()
    {
        // Calculate the current screen width as a proportion of the base width
        float screenWidthRatio = Screen.width / baseScreenWidth;
        // Adjust font size based on the screen width ratio
        textComponent.fontSize = baseFontSize * screenWidthRatio;
    }
}
