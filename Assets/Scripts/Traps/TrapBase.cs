using System.Diagnostics.Contracts;
using KenneyJam2025;
using UnityEngine;

public abstract class TrapBase : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            Activate(other.gameObject);
        }
    }

    protected abstract void Activate(GameObject target);
}
