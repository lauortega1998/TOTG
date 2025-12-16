using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    public float countdownTime = 3f;
    private bool timerStarted = false;
    private float timer;
    public int damageAmount = 10; 
    private PlayerHealth playerHealth;
    private int lastPrintedTime = -1; // For printing only when seconds change

    public GameObject heavyEnemy;
    public GameObject enemy;

    public void Start()
    {
        // Prefer inspector assignment; fall back to fastest, safest lookups with clear warnings.
        if (playerHealth == null)
        {
            // 1) Try finding the player by tag (recommended pattern if your player GameObject has the "Player" tag)
            var playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerHealth = playerObj.GetComponent<PlayerHealth>();
            }

            // 2) Fallback to FindObjectOfType only if necessary
            if (playerHealth == null)
            {
                playerHealth = FindObjectOfType<PlayerHealth>();
            }

            // 3) If still null, log an explicit error so the issue is obvious in Editor
            if (playerHealth == null)
            {
                Debug.LogError($"{gameObject.name}: PlayerHealth not found. Assign a PlayerHealth in the inspector or ensure a GameObject tagged \"Player\" has a PlayerHealth component.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovementStopper") && !timerStarted)
        {
            EnemyManager.Instance.isAnyEnemyAttacking = true;

            StartTimer();
        }
    } 
    private void OnEnable()
    {
        EnemyManager.Instance.isAnyEnemyAttacking = true;

        ResetTimerState(); // Ensure fresh start
    }
    
    private void Update()
    {
        if (timerStarted)
        {
            timer -= Time.deltaTime;

            int currentSeconds = Mathf.CeilToInt(timer);

            if (currentSeconds != lastPrintedTime && currentSeconds >= 0)
            {
                Debug.Log($"{gameObject.name} Timer: {currentSeconds} seconds remaining");
                lastPrintedTime = currentSeconds;
            }

            if (timer <= 0f)
            {
                PerformAction();
                StartTimer(); // Optional: keep looping
            }
        }
    }
    public void StartTimer()
    {
        timerStarted = true;
        timer = countdownTime;
        lastPrintedTime = -1;
        Debug.Log($"{gameObject.name} Timer started!");
    }

    public void StopTimer()
    {
        timerStarted = false;
        timer = 0;
        Debug.Log($"{gameObject.name} Timer stopped/reset.");
    }

    public void ResetTimerState()
    {
        timerStarted = false;
        timer = 0;
        lastPrintedTime = -1;
    }
    private void PerformAction() //Appling the methods when the timer end 
    {
            playerHealth.TakeDamage(15);
            Destroy(this.gameObject);
    }
    private void PerformActionHeavy() //Appling the methods when the timer end 
    {
        playerHealth.TakeDamage(20);
        Destroy(this.gameObject);
    }
}
