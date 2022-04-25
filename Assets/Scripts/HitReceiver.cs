using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReceiver : MonoBehaviour, IHitable
{
    [HideInInspector] public CharacterController character;
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public void Damage(Vector3 hitForce)
    {
        if (character.alive)
            character.Damage(_collider, hitForce);
    }
    public CharacterController GetCharacter()
    {
        return character;
    }
}
