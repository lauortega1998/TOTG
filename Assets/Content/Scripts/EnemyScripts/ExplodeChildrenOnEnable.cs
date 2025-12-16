using UnityEngine;

public class ExplodeChildrenOnEnable : MonoBehaviour
{
    [Header("Force Settings")]
    public float forceStrength = 5f;
    public float torqueStrength = 5f;

    void OnEnable()
    {
        ExplodeChildren();
    }

    public void ExplodeChildren()
    {
        // Find all rigidbodies in children
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rbs)
        {
            // skip parent if it also has an RB
            if (rb.gameObject == this.gameObject)
                continue;

            rb.isKinematic = false; // ensure physics is active

            // random outward force
            Vector3 dir = Random.onUnitSphere;
            rb.AddForce(dir * forceStrength, ForceMode.Impulse);

            // random spin
            rb.AddTorque(Random.insideUnitSphere * torqueStrength, ForceMode.Impulse);
        }
    }
}