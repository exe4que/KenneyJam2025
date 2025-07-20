using System.Diagnostics.Contracts;
using UnityEngine;

public abstract class TrapBase : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Activate(other.gameObject);
        }
    }

    protected abstract void Activate(GameObject target);
}
