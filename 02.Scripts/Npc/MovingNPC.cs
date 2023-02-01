using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingNPC : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    private Vector3 randomDirection;
    private Vector3 finalPosition;
    [SerializeField]
    private Vector3 destination;
    private readonly int hashWalking = Animator.StringToHash("isWalking");

    private Vector3 fp;
    
    private NavMeshHit hit;
    [HideInInspector]
    public NavMeshAgent nav;
    //이동할 범위 변수
    private float walkRadius = 50f;
    private WaitForSeconds waitingForMove = new WaitForSeconds(3.0f);
    private Coroutine waitMove;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        anim.SetBool(hashWalking, true);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDestination();
        
    }
    public Vector3 RandomNavmeshLocation(float radius) {
        randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
                finalPosition = hit.position;            
        }
        return finalPosition;
    }

    void ChangeDestination()
    {
        if (nav.pathStatus == NavMeshPathStatus.PathComplete && nav.remainingDistance <= 0.04f)
        {
            destination = RandomNavmeshLocation(walkRadius);
            nav.SetDestination(destination);
        }
    }

    public void StopMove(bool mov)
    {
        anim.SetBool(hashWalking, !mov);
        //nav.speed = mov ? 0f : 1.5f;
        nav.isStopped = mov;
    }
    
}
