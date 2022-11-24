using UnityEngine;

public abstract class BaseEnemyController : MonoBehaviour, IDamageable
{
    [Header("Enemy Properties")]
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;

    [Header("Effects")]
    [SerializeField] ParticleSystem damageVFX;
    [SerializeField] AudioClip damageSFX;
    [SerializeField] AudioClip deathSFX;

    // Internal vars
    AudioSource audioSource;
    Rigidbody rb;

    // Start is called before the first frame update
    public virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDamage(float damage, float damageForce, RaycastHit hit)
    {
        Debug.Log($"{name} took {damage} damage.");
        currentHP--;
        if (damageSFX != null) audioSource.PlayOneShot(damageSFX);
        if (currentHP < 1) OnDie();
        // Add forece after death, so it applies once physics are enabled
        rb.AddForceAtPosition(damageForce * -hit.normal, hit.point, ForceMode.Impulse);
    }

    public void OnDie()
    {
        Debug.Log($"{name} died.");
        if (deathSFX != null) audioSource.PlayOneShot(deathSFX);
        SetAlivePhysics(false);
    }

    public void SetAlivePhysics(bool state)
    {
        rb.freezeRotation = state;
    }
}
