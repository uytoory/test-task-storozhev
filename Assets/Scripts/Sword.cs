using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Rigidbody _rb;
    [HideInInspector] public bool hitting = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        IHitable hitableObject = collision.gameObject.GetComponent<IHitable>();
        if (hitableObject != null)
        {
            if (hitableObject.GetCharacter().characterClass.type == CharacterClass.Type.Enemy)
                return;
            hitableObject.Damage(_rb.velocity * 3);
        }
    }
}
