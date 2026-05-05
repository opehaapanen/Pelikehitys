/// <summary>
/// Health vastaa terveydestä.
/// </summary>
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// positiivinen arvo --> parantaa
    /// negatiivinen arvo --> tekee vahinkoa
    /// </summary>
    public void Modify(int amount)
    {
        // Kasvattaa terveyttä
        currentHealth += amount;

        // Mathf.Clamp estää arvon menemisen alle 0 tai yli maksimin
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Testitulostus
        Debug.Log("Health: " + currentHealth);
    }

}
