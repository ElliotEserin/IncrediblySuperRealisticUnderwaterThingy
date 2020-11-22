using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bait : MonoBehaviour
{
    ParticleBoids closestShoal;
    GoalObject shoalGoal;
    public float strength = 100f;

    private void Start()
    {
        var boids = FindObjectsOfType<ParticleBoids>();
        var shortestDistance = strength;

        foreach(ParticleBoids boid in boids)
        {
            var distance = Vector3.Distance(transform.position, boid.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestShoal = boid;
            }
        }

        shoalGoal = closestShoal.GetComponentInChildren<GoalObject>();
        shoalGoal.transform.position = transform.position;
        shoalGoal.bait = transform;
        Debug.Log(closestShoal);
    }
}
