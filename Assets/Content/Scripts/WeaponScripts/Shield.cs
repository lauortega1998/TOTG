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

    [Header("Phases")]
    [Tooltip("0=100hp, 1=80hp, 2=60hp, 3=40hp, 4=20hp, 5=0hp")]
    public GameObject[] phases = new GameObject[7];


    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (shieldHealthBar != null)
        {
            shieldHealthBar.maxValue = shieldHealth;
            shieldHealthBar.value = shieldHealth;
        }

        UpdatePhaseVisual();



    }
    private void Start()
    {
        UpdateShieldUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsActive()) return; // <-- only take damage when held

        if (other.CompareTag("AttackCollider"))
        {
            lastFrameTriggered = Time.frameCount;

            Debug.Log("Shield (Trigger) detected AttackCollider while held");

            shieldHealth = Mathf.Max(0f, shieldHealth - 10);
            UpdateShieldUI();
            UpdatePhaseVisual();

            if (shieldHealth <= 0f)
            {
                Destroy(gameObject, 1);
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
    private void UpdatePhaseVisual()
    {
        if (phases.Length != 7)
        {
            Debug.LogWarning("Assign exactly 6 shield phase objects!");
            return;
        }

        int index;
        if (shieldHealth > 90f) index = 0;
        else if (shieldHealth > 80f) index = 1;
        else if (shieldHealth > 70f) index = 2;
        else if (shieldHealth > 50f) index = 3;
        else if (shieldHealth > 30f) index = 4;
        else if (shieldHealth > 1f) index = 5;
        else index = 6;

        // Enable only the correct phase
        for (int i = 0; i < phases.Length; i++)
        {
            if (phases[i] != null)
                phases[i].SetActive(i == index);
        }
    }

    public bool IsActive()
    {
        return grabInteractable != null
               && grabInteractable.isSelected
               && shieldHealth > 0;
    }
}
