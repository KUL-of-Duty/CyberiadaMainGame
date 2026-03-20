using UnityEngine;

using UnityEngine.AI;

public class PlayerNavMesh : MonoBehaviour

{

    [SerializeField, HideInInspector] private Transform movePositionTransform;

    [SerializeField] private Transform objectToFollow;

    [Header("Targeting")]
    [SerializeField] private bool followPlayerTag = true;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float targetRefreshInterval = 0.25f;
    [SerializeField] private float destinationSampleRadius = 1.0f;
    [SerializeField] private int destinationAreaMask = NavMesh.AllAreas;
    [SerializeField] private float warpSampleRadius = 1.5f;

    private NavMeshAgent navMeshAgent;

    private Transform currentTarget;
    private float nextTargetRefreshTime;



    void Awake()

    {

        navMeshAgent = GetComponent<NavMeshAgent>();

    }



    private void Update()

    {
        if (navMeshAgent == null) return;
        if (!navMeshAgent.enabled) return;
        if (!gameObject.activeInHierarchy) return;

        Transform target = null;

        if (followPlayerTag)
        {
            if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy || Time.time >= nextTargetRefreshTime)
            {
                currentTarget = FindClosestPlayerTarget();
                nextTargetRefreshTime = Time.time + targetRefreshInterval;
            }

            target = currentTarget;
        }
        else
        {
            target = objectToFollow;
        }

        if (!navMeshAgent.isOnNavMesh)
        {
            Vector3 currentPos = transform.position;
            if (NavMesh.SamplePosition(currentPos, out var selfHit, warpSampleRadius, destinationAreaMask))
                navMeshAgent.Warp(selfHit.position);
        }

        if (!navMeshAgent.isOnNavMesh)
            return;

        if (target != null)
        {
            Vector3 desired = target.position;
            if (NavMesh.SamplePosition(desired, out var hit, destinationSampleRadius, destinationAreaMask))
                desired = hit.position;

            navMeshAgent.destination = desired;
        }

    }


    private Transform FindClosestPlayerTarget()
    {
        GameObject[] players;
        try
        {
            players = GameObject.FindGameObjectsWithTag(playerTag);
        }
        catch
        {
        
            players = null;
        }

        if (players == null || players.Length == 0)
            return objectToFollow;

        Vector3 myPos = transform.position;
        Transform best = null;
        float bestSqrDist = float.PositiveInfinity;

        for (int i = 0; i < players.Length; i++)
        {
            var p = players[i];
            if (p == null) continue;

            float sqrDist = (p.transform.position - myPos).sqrMagnitude;
            if (sqrDist < bestSqrDist)
            {
                bestSqrDist = sqrDist;
                best = p.transform;
            }
        }

        return best != null ? best : objectToFollow;
    }

}