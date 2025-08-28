using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Shield : MonoBehaviour
{
    [Header("Shield")]
    public float shieldHealth = 100f;
    public Slider shieldHealthBar;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    private int lastFrameTriggered = -1; // track which physics frame we last triggered


    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (shieldHealthBar != null)
            shieldHealthBar.maxValue = shieldHealth;

    }
    private void Start()
    {
        UpdateShieldUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackCollider"))
        {
            lastFrameTriggered = Time.frameCount;

            Debug.Log("Shield (Trigger) detected AttackCollider");

            shieldHealth -= 10;
            UpdateShieldUI();


            if (shieldHealth == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void UpdateShieldUI()
    {
        if (shieldHealthBar != null)
            shieldHealthBar.value = shieldHealth;
    }

    void DestroyShield()
    {
        { }
    }

    public bool IsActive()
    {
        return grabInteractable != null
               && grabInteractable.isSelected
               && shieldHealth > 0;
    }
}
