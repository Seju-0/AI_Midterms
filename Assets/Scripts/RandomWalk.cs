using UnityEngine;
using UnityEngine.AI;

namespace Unity.AI.Navigation.Samples
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class RandomWalk : MonoBehaviour
    {
        public float m_Range = 25.0f;
        public float speed = 3.5f;
        public float followStopDistance = 1.5f; 

        [Header("Material Settings")]
        public Renderer aiRenderer; 

        private bool hasMatchedPlayer = false;
        private Transform playerTarget;
        private NavMeshAgent m_Agent;

        void Start()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            m_Agent.speed = speed;
            m_Agent.stoppingDistance = followStopDistance;

            if (aiRenderer == null)
                aiRenderer = GetComponent<Renderer>();
        }

        void Update()
        {
            if (hasMatchedPlayer && playerTarget != null)
            {
                if (m_Agent.isOnNavMesh)
                {
                    m_Agent.SetDestination(playerTarget.position);

                    if (!m_Agent.pathPending && m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                    {
                        m_Agent.isStopped = true;
                    }
                    else
                    {
                        m_Agent.isStopped = false;
                    }
                }
                return;
            }
            if (m_Agent.pathPending || !m_Agent.isOnNavMesh || m_Agent.remainingDistance > 0.1f)
                return;

            Vector3 randomPos = transform.position + new Vector3(
                Random.Range(-m_Range, m_Range),
                0,
                Random.Range(-m_Range, m_Range)
            );

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, m_Range, NavMesh.AllAreas))
            {
                m_Agent.SetDestination(hit.position);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                MatchPlayerMaterial(collision.gameObject);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                MatchPlayerMaterial(other.gameObject);
        }

        private void MatchPlayerMaterial(GameObject player)
        {
            if (hasMatchedPlayer) return;

            Renderer playerRenderer = player.GetComponent<Renderer>();
            if (playerRenderer != null && aiRenderer != null)
            {
                aiRenderer.material = new Material(playerRenderer.material);
                aiRenderer.material.color = playerRenderer.material.color;

                playerTarget = player.transform;
                hasMatchedPlayer = true;
            }
        }
    }
}
