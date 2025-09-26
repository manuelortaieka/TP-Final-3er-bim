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
    bool playerVisto = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        velocity = agent.velocity.magnitude;
        anim.SetFloat("Speed", velocity);
        if (agent.remainingDistance < 0.5 && PatrullandoGlob.patrullando)
        {
            puntoActual = (puntoActual + 1) % puntosPatrullaje.Length;
            agent.SetDestination(puntosPatrullaje[puntoActual].position);
        }

        if (Physics.Raycast(sightOrigin.position, sightOrigin.forward, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerVisto = true;
                tiempo = 2f;
            }

        }
        if (playerVisto)
        {
            PatrullandoGlob.patrullando = false;
            tiempo -= Time.deltaTime;
            Debug.Log(tiempo);
        }

        if (tiempo <= 0f)
        {
            playerVisto = false;
            tiempo = 2f;
            PatrullandoGlob.patrullando = true;
        }

        if (!PatrullandoGlob.patrullando)
        {
            agent.destination = jugador.position;
            if (agent.remainingDistance < 0.5f)
            {
                SceneManager.LoadScene("EscenaDerrota");
            }
        }
    }
}
