﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    GameObject lastTarget = null;
    ChargerPhase phase;
    float timer;
    public float attackDelay = 0.5f;
    float attackTimer;
    Animator anim;
    GameObject selection;
    public int Damage = 7;

    class RaycastHitDistanceComparer : IComparer<RaycastHit>
    {
        public int Compare(RaycastHit x, RaycastHit y)
        {
            return Mathf.RoundToInt(x.distance - y.distance);
        }
    }

	void Start()
	{
        attrs = GetComponent<EnemyAttrs>();
        phase = ChargerPhase.PHASE_TARGETING;
        anim = GetComponent<Animator>();
        timer = 0f;
        attackTimer = 0f;
	}
	
	void Update()
	{
        if (phase == ChargerPhase.PHASE_TARGETING)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

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
            Vector3 dist = lastTarget.transform.position + new Vector3(0, 1, 0) - transform.position + new Vector3(0, 1, 0);
            Vector3 rot = Vector3.RotateTowards(transform.forward, dist, 0.1f, 0);

            // check to make sure we don't collide with the scene
            RaycastHit[] hits;
            Ray ray = new Ray(transform.position + new Vector3(0, 1, 0), rot);
            hits = Physics.RaycastAll(ray, dist.magnitude);
            Array.Sort<RaycastHit>(hits, new RaycastHitDistanceComparer());
            foreach (var hit in hits)
            {
                if (hit.transform == transform)
                {
                    continue;
                }
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    continue;
                }
                if (hit.transform.gameObject.tag != "Player")
                {
                    // raycast collided
                    phase = ChargerPhase.PHASE_TARGETING;
                    rigidbody.velocity = Vector3.zero;
                    return;
                }
            }
                
            transform.rotation = Quaternion.LookRotation(rot);
            rigidbody.velocity = Vector3.ClampMagnitude(dist, 0.1f) * 250;
            if (dist.magnitude < 2)
            {
                rigidbody.velocity = Vector3.zero;
                phase = ChargerPhase.PHASE_TARGETING;
            }
        }

        attackTimer += Time.deltaTime;

        if (lastTarget != null && 
            Vector3.Distance(lastTarget.transform.position, transform.position) < 2 &&
            attackTimer >= attackDelay)
        {
            lastTarget.GetComponent<PlayerAttrs>().TakeDamage(Damage);
            attackTimer = 0f;
        }
	}

    public GameObject randomOf(GameObject[] objs)
    {
        return objs[(int)(UnityEngine.Random.value * objs.Length)];
    }
}
