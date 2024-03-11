using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target object the camera should follow
    public float smoothSpeed = 0.125f; // Adjust for smoother camera movement
    public Vector2 backgroundScale = new Vector2(240, 120); // The scale of your background
    private float verticalLimit; // Limit for vertical movement
    private float horizontalLimit; // Limit for horizontal movement

    void Start()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera not found. Ensure your camera is tagged as 'MainCamera'.");
            return; // Exit the Start method to avoid accessing Camera.main properties
        }

        // Assuming the camera is centered on the background and using orthographic size to determine the limits
        float camHeight = 2f * Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;
        
        // Calculate limits based on background scale and camera size
        verticalLimit = (backgroundScale.y - camHeight) / 2f;
        horizontalLimit = (backgroundScale.x - camWidth) / 2f;
    }


    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position;
            desiredPosition.z = transform.position.z; // Keep the camera's original Z position

            // Clamp the desired position to ensure the camera does not go beyond the background bounds
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, -horizontalLimit, horizontalLimit);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, -verticalLimit, verticalLimit);

            // Smoothly interpolate between the camera's current position and the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
