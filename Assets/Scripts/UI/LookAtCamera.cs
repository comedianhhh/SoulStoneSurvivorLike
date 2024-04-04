using UnityEngine;

/// <summary>
/// This class works as a component that can be added to any object (usually a 2D image) to be oriented toward the Camera.
/// </summary>
public class LookAtCamera : MonoBehaviour
{
    /// <summary>
    /// CAM_POS: camera position
    /// CAM_PLANE: camera plane
    /// </summary>
    public enum LookAtTarget
    {
        CAM_POS,
        CAM_PLANE
    }

    /// <summary>
    /// Expose a property to let the designer choose the mode in the inspector.
    /// </summary>
    [SerializeField] private LookAtTarget mode = LookAtTarget.CAM_PLANE;

    /// <summary>
    /// LateUpdate happens last in the Unity life cycle; this guarantees that the object's orientation will be given after the Camera and the object have moved. Other modes can be added.
    /// </summary>
    public void LateUpdate()
    {
        switch (mode)
        {
            case LookAtTarget.CAM_POS:
                Vector3 direction = (transform.position - Camera.main.transform.position).normalized;   
                transform.LookAt(transform.position + direction);
            break;

            case LookAtTarget.CAM_PLANE:
                transform.forward = Camera.main.transform.forward;
            break;
        }
    }
}
