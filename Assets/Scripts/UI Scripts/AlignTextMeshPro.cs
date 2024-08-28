using TMPro;
using UnityEngine;

public class AlignTextMeshPro : MonoBehaviour
{
    public RectTransform referenceText; // Assign the RectTransform of the Reference TMP Text object
    public RectTransform text1; // Assign the RectTransform of Text1
    public RectTransform text2; // Assign the RectTransform of Text2
    public float baseScreenHeight = 1080f; // Set this to your design screen height
    public float baseOffsetY = -25f; // Base vertical offset for referenceText at design resolution

    void Start()
    {
        // Calculate the screen height ratio
        float screenHeightRatio = Screen.height / baseScreenHeight;

        // Update vertical positions based on screen height ratio
        float offsetY = baseOffsetY * screenHeightRatio;
        referenceText.position = new Vector2(referenceText.position.x, referenceText.position.y + offsetY);
    }

    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {

        if (referenceText != null && text1 != null && text2 != null)
        {
            // Update Text1 and Text2 position based on referenceText's new position
            text1.position = new Vector2(
                referenceText.position.x - referenceText.rect.width / 4,
                referenceText.position.y + referenceText.rect.height / 2 + text1.rect.height / 2);

            text2.position = new Vector2(
                referenceText.position.x + referenceText.rect.width / 4,
                referenceText.position.y + referenceText.rect.height / 2 + text2.rect.height / 2);
        }
    }
}
