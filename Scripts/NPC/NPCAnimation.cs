using UnityEngine;
using UnityEngine.AI;

public class NPCAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;

    void Start()
    {
        //animator = animator.GetComponent<Animator>();
        //agent = agent.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}