using UnityEngine;
using UnityEngine.AI;

// Prosty AI przeciwnika: NavMesh pogoń + obrażenia przy kontakcie.
[RequireComponent(typeof(NavMeshAgent))]
public sealed class enemyai : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Transform player;

    [Header("Chase")]
    [SerializeField] private float repathInterval = 0.15f;
    [SerializeField] private float stopDistance = 1.2f;

    [Header("Damage")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float damageCooldown = 0.75f;

    private NavMeshAgent _agent;
    private float _nextRepath;
    private float _nextDamage;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = stopDistance;
    }

    private void Start()
    {
        if (player == null)
        {
            var go = GameObject.FindGameObjectWithTag(playerTag);
            if (go != null) player = go.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;
        if (Time.time < _nextRepath) return;

        _nextRepath = Time.time + repathInterval;
        _agent.SetDestination(player.position);
    }

    private void DealDamageIfPlayer(GameObject other)
    {
        if (Time.time < _nextDamage) return;
        if (!other.CompareTag(playerTag)) return;

        _nextDamage = Time.time + damageCooldown;

        // Twoje HP (singleplayer)
        var hp = other.GetComponent<Health>();
        if (hp != null)
        {
            hp.TakeDamage(Mathf.RoundToInt(damage));
            return;
        }

        // Twoje HP (netcode)
        var target = other.GetComponent<Target>();
        if (target != null) target.TakeDamage(damage);
    }

    private void OnCollisionStay(Collision collision) => DealDamageIfPlayer(collision.gameObject);
    private void OnTriggerStay(Collider other) => DealDamageIfPlayer(other.gameObject);
}

