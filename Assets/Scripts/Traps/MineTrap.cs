using KenneyJam2025;
using System.Collections;
using UnityEngine;

public class MineTrap : TrapBase
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _damage = 50f;
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private float _delayBeforeExplosion = 0.5f;

    [Header("Visual Flash")]
    [SerializeField] private Color _flashColor = Color.red;
    [SerializeField] private float _flashInterval = 0.1f;

    [SerializeField] private float _pushForce = 10f;
    [SerializeField] private float _upwardModifier = 1.0f;


    private Renderer _rend;
    private Color _originalColor;
    private Animator _animator;

    private void Awake()
    {
        _rend = GetComponent<Renderer>();
        _originalColor = _rend.material.color;
        _animator = GetComponent<Animator>();    
    }

    protected override void Activate(GameObject target)
    {
        _animator.SetTrigger("Explode");
        StartCoroutine(FlashAndExplode());
    }

    private IEnumerator FlashAndExplode()
    {
        float elapsed = 0f;
        bool flashing = false;

        while (elapsed < _delayBeforeExplosion)
        {
            _rend.material.color = flashing ? _flashColor : _originalColor;
            flashing = !flashing;

            yield return new WaitForSeconds(_flashInterval);
            elapsed += _flashInterval;
        }

        _rend.material.color = _originalColor;

        if (_explosionEffect != null)
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                var healt = hit.GetComponent<IDamageable>();
                healt?.OnDamage(_damage, null);

                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(_pushForce, transform.position, _explosionRadius, _upwardModifier, ForceMode.Impulse);
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
