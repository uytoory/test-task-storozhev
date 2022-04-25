using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    void Damage(Vector3 hitForce);
    CharacterController GetCharacter();
}
