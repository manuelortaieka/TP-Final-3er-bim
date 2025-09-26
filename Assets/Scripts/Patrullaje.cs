using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Patrullaje : MonoBehaviour
{
    [SerializeField] int puntoActual = 0;
    NavMeshAgent agent;
    [SerializeField] Transform sightOrigin;
    [SerializeField] float rayDistance;
    [SerializeField] Transform[] puntosPatrullaje;
    [SerializeField] Animator anim;
    [SerializeField] float velocity;
    [SerializeField] Transform jugador;
    [SerializeField] float tiempo = 2f;
    bool playerEnSight = false;
    float tiempoAct = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.remainingDistance < 0.5f && !agent.pathPending && PatrullandoGlob.patrullando)
        {
            puntoActual = (puntoActual + 1) % puntosPatrullaje.Length;
            agent.SetDestination(puntosPatrullaje[puntoActual].position);
        }

        if (!PatrullandoGlob.patrullando)
        {
            agent.destination = jugador.position;
            if (agent.remainingDistance < 0.5f)
            {
                //  SceneManager.LoadScene("EscenaDerrota");
            }

            velocity = agent.velocity.magnitude;
            anim.SetFloat("Speed", velocity);

            if (Physics.Raycast(sightOrigin.position, sightOrigin.forward, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    playerEnSight = true;
                }

                if (playerEnSight)
                {
                    PatrullandoGlob.patrullando = false;
                    tiempoAct = 0f;
                    playerEnSight = false;
                }
                else
                {
                    tiempoAct += Time.deltaTime;
                    if (tiempoAct > tiempo)
                    {
                        PatrullandoGlob.patrullando = true;
                    }
                }

            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(sightOrigin.position, sightOrigin.position + sightOrigin.forward * rayDistance);
        }
    }
}
