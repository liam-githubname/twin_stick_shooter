using UnityEngine;

public class Withinborder : MonoBehaviour
{
    private Vector2 horizontalLimits = new Vector2(-119f, 119f); // Left and Right limits
    private Vector2 verticalLimits = new Vector2(-59f, 58f);    // Bottom and Top limits

    void Update()
    {
        Vector3 position = transform.position;

        // Check horizontal boundaries
        if (position.x > horizontalLimits.y)
        {
            position.x = horizontalLimits.x;
            
        }
        else if (position.x < horizontalLimits.x)
        {
            position.x = horizontalLimits.y;
        }

        // Check vertical boundaries
        if (position.y > verticalLimits.y)
        {
            position.y = verticalLimits.x;
        }
        else if (position.y < verticalLimits.x)
        {
            position.y = verticalLimits.y;
        }

        transform.position = position;
    }
}
