using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterClass : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer characterSkin = null;
    [SerializeField] private MeshRenderer characterHair = null;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;

    public enum Type
    {
        Player,
        Enemy
    }
    public Type type = Type.Enemy;

    [SerializeField] private RuntimeAnimatorController playerAnimatorCtrl = null;
    [SerializeField] private RuntimeAnimatorController enemyAnimatorCtrl = null;

    public GameObject gun;
    public GameObject sword;

    public void SetupClass(NavMeshAgent Agent, Animator Animator)
    {
        agent = Agent;
        animator = Animator;
        SetupCharacter();
    }

    private void SetupCharacter()
    {
        switch (type)
        {
            case Type.Player :
                SetupPlayer();
                break;
            case Type.Enemy :
                SetupEnemy();
                break;
        }
    }

    private void SetupPlayer()
    {
        agent.speed = 7;
        animator.runtimeAnimatorController = playerAnimatorCtrl;
        gun.SetActive(true);
        ChangeSkinColor(new Color(1,1,1,1));
    }

    private void SetupEnemy()
    {
        agent.speed = 0.6f;
        animator.runtimeAnimatorController = enemyAnimatorCtrl;
        sword.SetActive(true);
        ChangeSkinColor(new Color(1,0,0,1));
    }

    public void ChangeSkinColor(Color color)
    {
        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

        characterSkin.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", color);
        characterSkin.SetPropertyBlock(propBlock);

        characterHair.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", color);
        characterHair.SetPropertyBlock(propBlock);
    }
}
