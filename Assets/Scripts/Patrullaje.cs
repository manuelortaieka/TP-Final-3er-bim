using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrullaje : MonoBehaviour
{
    [SerializeField] int puntoActual = 0;
    NavMeshAgent agent;
    [SerializeField] Transform sightOrigin;
    [SerializeField] float rayDistance;
    [SerializeField] Transform[] puntosPatrullaje; 
    [SerializeField] Animator anim;
    [SerializeField] float velocity;
    [SerializeField] bool patrullando = true;
    [SerializeField] Transform jugador;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.remainingDistance < 0.5f && !agent.pathPending && patrullando)
        {
            puntoActual = (puntoActual + 1) % puntosPatrullaje.Length;
            agent.SetDestination(puntosPatrullaje[puntoActual].position);
        }

        if (!patrullando)
        {
            agent.destination = jugador.position;
        }
        velocity = agent.velocity.magnitude;
        anim.SetFloat("Speed", velocity);

        // Raycast detección
        if (Physics.Raycast(sightOrigin.position, sightOrigin.forward, out RaycastHit hit, rayDistance))
        {
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                patrullando = false;
                Debug.Log("Jugador detectado!");
            }
        }
    }

    void OnDrawGizmos()
    {
        if (sightOrigin == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(sightOrigin.position, sightOrigin.position + sightOrigin.forward * rayDistance);
    }
}
