using UnityEngine;

/// <summary>
/// Class used to destroy an object after receiving damage.
/// It should be expanded to encopass health amount and special effect,
/// and also the use of an interface.
/// </summary>
public class Destroyable : MonoBehaviour
{
    /// <summary>
    /// Check if the collision was happened with a bullet. It should be updated
    /// to check for IDoDamage interface to make it more generic.
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<Bullet>(out Bullet bullet))
        {
            Destroy(this.gameObject);
        }
    }
}
