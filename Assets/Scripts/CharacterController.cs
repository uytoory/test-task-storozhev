using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private GameObject ragdoll = null;
    private Collider[] ragdollParts;
    private Vector3[] ragdollPosition;
    private Quaternion[] ragdollRotation;
    [SerializeField] private Collider headCollider = null;
    private bool ragdollActive = false;

    private NavMeshAgent agent;
    private Animator animator;

    public CharacterClass characterClass { get; private set; }

    public bool alive { get; private set; } = true;
    [SerializeField] private float health = 3;
    [SerializeField] private HealthBar healthBar = null;

    private bool isMoving = false;
    private bool moveNextTo = false;

    private Transform moveDestination;
    private Vector3 target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterClass = GetComponent<CharacterClass>();

        SetupRagdoll();
        characterClass.SetupClass(agent, animator);

        healthBar.SetUp(health);
    }

    private void SetupRagdoll()
    {
        ragdollParts = ragdoll.GetComponentsInChildren<Collider>(false);

        ragdollPosition = new Vector3[ragdollParts.Length];
        ragdollRotation = new Quaternion[ragdollParts.Length];

        for (int i = 0; i < ragdollParts.Length; i++)
        {
            ragdollParts[i].attachedRigidbody.isKinematic = true;
            ragdollParts[i].gameObject.AddComponent<HitReceiver>().character = this;
            
            ragdollPosition[i] = ragdollParts[i].transform.localPosition;
            ragdollRotation[i] = ragdollParts[i].transform.localRotation;
        }
    }

    private void Update()
    {
        if (!alive)
            return;

        if (isMoving && ragdollActive == false)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);

            if(agent.pathPending)
                return;

            float distnace = agent.remainingDistance;
            float reqDistance = moveNextTo ? 1.8f : 0;
            
            if (distnace != Mathf.Infinity && agent.remainingDistance <= reqDistance)
            {
                agent.isStopped = true;
                animator.SetFloat("Speed", 0);
                isMoving = false;
                animator.SetBool("Aim", true);

                if (moveNextTo)
                    StartCoroutine(TurnBody(Quaternion.LookRotation(moveDestination.position - transform.position)));
                else
                    StartCoroutine(TurnBody(moveDestination.rotation));

                if (characterClass.type == CharacterClass.Type.Player && GameManager.OnStageReached != null)
                    GameManager.OnStageReached();
            }
        }
    }

    public void MoveToPoint(Transform point, bool nextTo)
    {
        if (!alive)
            return;

        moveDestination = point;
        animator.SetBool("Aim", false);
        agent.isStopped = false;
        agent.SetDestination(point.position);
        moveNextTo = nextTo;
        isMoving = true;
    }

    public void Damage(Collider hitCollider, Vector3 hitForce)
    {
        health -= (hitCollider == headCollider) ? 3 : 1;
        healthBar.SetHealth(health);

        if (health <= 0)
            Die();
        else if (ragdollActive == false)
            ActivateRagdoll(true);
            
        hitCollider.attachedRigidbody.AddForce(hitForce, ForceMode.Impulse);
    }

    public void Die()
    {
        alive = false;
        characterClass.ChangeSkinColor(new Color(0,0,0,1));
        ActivateRagdoll(false);

        if (characterClass.type == CharacterClass.Type.Enemy)
        {
            if (GameManager.OnEnemyKilled != null)
                GameManager.OnEnemyKilled();
        }
        else
        {
            if (GameManager.OnPlayerKilled != null)
                GameManager.OnPlayerKilled();
        }
    }

    private void ActivateRagdoll(bool temporary)
    {
        ragdollActive = true;
        animator.enabled = false;
        agent.enabled = false;
        foreach (Collider part in ragdollParts)
        {
            part.attachedRigidbody.isKinematic = false;
        }
        if (temporary)
            StartCoroutine(ResetPosture());
    }

    IEnumerator ResetPosture()
    {
        yield return new WaitForSeconds(3);

        Vector3[] startPositions = new Vector3[ragdollParts.Length];
        Quaternion[] startRotations = new Quaternion[ragdollParts.Length];
        for (int i = 0; i < ragdollParts.Length; i++)
        {
            startPositions[i] = ragdollParts[i].transform.localPosition;
            startRotations[i] = ragdollParts[i].transform.localRotation;
        }

        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * 2;
            for (int i = 0; i < startPositions.Length; i++)
            {
                ragdollParts[i].transform.localPosition = Vector3.Lerp(startPositions[i], ragdollPosition[i], time);
                ragdollParts[i].transform.localRotation = Quaternion.Slerp(startRotations[i], ragdollRotation[i], time);
            }
            yield return null;

            if (!alive)
                yield break;
        }
        DeactivateRagdoll();
        MoveToPoint(moveDestination, true);
    }
    private void DeactivateRagdoll()
    {
        ragdollActive = false;
        animator.enabled = true;
        agent.enabled = true;
        foreach (Collider part in ragdollParts)
        {
            part.attachedRigidbody.isKinematic = true;
        }
    }

    IEnumerator TurnBody(Quaternion destination)
    {
        Quaternion start = transform.rotation;
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime * 3;
            transform.rotation = Quaternion.Slerp(start, destination, time);
            yield return null;
        }
    }

    private void Hit()
    {
        animator.SetTrigger("Attack");
        characterClass.sword.GetComponent<Sword>().hitting = true;
    }
    private void ResetHit()
    {
        characterClass.sword.GetComponent<Sword>().hitting = false;
    }

    public void AimAndShoot(Vector3 Target)
    {
        target = Target;
        animator.SetTrigger("Attack");
    }

    [SerializeField] private Transform pistolTip = null;
    private void PistolShoot()
    {
        Quaternion rotation = Quaternion.LookRotation(target - pistolTip.position);
        BulletPool.Instance.SpawnBullet(pistolTip.position, rotation);
    }
    private void ResetTrigger()
    {
        animator.ResetTrigger("Attack");
    }
}
