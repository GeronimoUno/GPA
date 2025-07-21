using UnityEngine;

public class Exploder : MonoBehaviour
{
    [Tooltip("Сила разлёта")]
    public float force = 500f;
    [Tooltip("Радиус действия")]
    public float radius = 5f;

    // Запрос извне — взорвать дочерние Rigidbody
    public void Explode()
    {
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
            rb.AddExplosionForce(force, transform.position, radius);
    }
}
