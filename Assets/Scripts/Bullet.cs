using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolObject
{
    private Rigidbody _rb;

    public void OnSpawn()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;
        _rb.AddForce(transform.forward * 25, ForceMode.Impulse);

        Invoke("Erase", 3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IHitable hitableObject = collision.gameObject.GetComponent<IHitable>();
        if (hitableObject != null)
        {
            hitableObject.Damage(_rb.velocity * 3);
        }

        Erase();
    }

    private void Erase()
    {
        gameObject.SetActive(false);
    }
}
