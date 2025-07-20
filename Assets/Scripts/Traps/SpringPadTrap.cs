using UnityEngine;

public class SpringPadTrap : TrapBase
{
    [Header("Explosion-style Launch Settings")]
    [SerializeField] private float _pushForce = 20f;
    [SerializeField] private float _upwardModifier = 1.5f;
    [SerializeField] private float _randomOffsetAngle = 30f;

    [Header("Optional Feedback")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggerName = "Bounce";

    protected override void Activate(GameObject target)
    {
        if (!target.CompareTag("Player")) return;

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;

            Vector3 explosionDir = GetRandomizedDirection();
            rb.AddForce(explosionDir * _pushForce, ForceMode.Impulse);
        }

        if (_animator != null && !string.IsNullOrEmpty(_triggerName))
        {
            _animator.SetTrigger(_triggerName);
        }
    }

    private Vector3 GetRandomizedDirection()
    {
        Vector3 baseDir = Vector3.up + Random.insideUnitSphere;

        if (baseDir.y < 0.3f)
            baseDir.y = 0.3f;

        return baseDir.normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Vector3.up * 3f);
    }
}
