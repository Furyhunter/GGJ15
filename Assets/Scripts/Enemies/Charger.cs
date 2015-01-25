using UnityEngine;
using System.Collections;
using System;

public class Charger : MonoBehaviour
{

    enum ChargerPhase
    {
        PHASE_CHARGING,
        PHASE_TARGETING,
        PHASE_TURNING, 
        PHASE_ANGRY
    };

    EnemyAttrs attrs;
    public CapsuleCollider frontCollider;
    GameObject lastTarget = null;
    ChargerPhase phase;
    float timer;
    Animator anim;

	void Start()
	{
        attrs = GetComponent<EnemyAttrs>();
        phase = ChargerPhase.PHASE_TARGETING;
        anim = GetComponent<Animator>();
        timer = 0f;
	}
	
	void Update()
	{
        if (phase == ChargerPhase.PHASE_TARGETING)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            GameObject selection;

            if (players.Length == 0)
                return;
            else if (players.Length == 1)
                selection = players[0];
            else
                while ((selection = randomOf(players)) == lastTarget) ;

            lastTarget = selection;

            phase = ChargerPhase.PHASE_TURNING;
        }
        else if (phase == ChargerPhase.PHASE_TURNING)
        {
            if (lastTarget == null)
            {
                phase = ChargerPhase.PHASE_TARGETING;
                return;
            }
            Vector3 dist = lastTarget.transform.position - transform.position;
            Vector3 rot = Vector3.RotateTowards(transform.forward, dist, 0.1f, 0);
            transform.rotation = Quaternion.LookRotation(rot);
            
            double angle = Math.Abs(Vector3.Angle(transform.forward, dist));
            if (angle < 0.3)
                phase = ChargerPhase.PHASE_ANGRY;
        }
        else if (phase == ChargerPhase.PHASE_ANGRY)
        {
            timer += Time.deltaTime;
            if (timer >= 1.0)
            {
                timer = 0f;
                phase = ChargerPhase.PHASE_CHARGING;
            }
        }
        else if (phase == ChargerPhase.PHASE_CHARGING)
        {
            if (lastTarget == null)
            {
                phase = ChargerPhase.PHASE_TARGETING;
                rigidbody.velocity = Vector3.zero;
                return;
            }
                
            Vector3 dist = lastTarget.transform.position - transform.position;
            Vector3 rot = Vector3.RotateTowards(transform.forward, dist, 0.1f, 0);
            transform.rotation = Quaternion.LookRotation(rot);
            rigidbody.velocity = Vector3.ClampMagnitude(dist, 0.1f) * 500;
            if (dist.magnitude < 2)
            {
                rigidbody.velocity = Vector3.zero;
                phase = ChargerPhase.PHASE_TARGETING;
            }
        }
	}

    public GameObject randomOf(GameObject[] objs)
    {
        return objs[(int)(UnityEngine.Random.value * objs.Length)];
    }
}
