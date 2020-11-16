
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBoids : MonoBehaviour
{
    public float fishMaxSpeed = 5f;
    public float NeighborDistance = 0.8f;
    [Header("Cohesion")]
    public float CohesionStep;            // 100
    public float CohesionWeight;          // 0.05
    [Header("Separation")]
    public float SeparationWeight;        // 0.01`
    [Header("Alignment")]
    public float AlignmentWeight;         // 0.01
    [Header("Seek")]
    public float SeekWeight;              // 0
    [Header("Socialize")]
    public float SocializeWeight;         // 0
    [Header("Arrival")]
    public float ArrivalSlowingDistance;  // 2
    public float ArrivalMaxSpeed;         // 0.2

    ParticleSystem system;
    ParticleSystem.Particle[] fish;
    GoalObject target;

    List<ParticleSystem.Particle> neighbors = new List<ParticleSystem.Particle>();
    ParticleSystem.Particle particle;

    // Start is called before the first frame update
    void Start()
    {
        system = GetComponent<ParticleSystem>();
        target = GetComponentInChildren<GoalObject>();

        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if(system.particleCount < 1 || fish.Length < 1)
        {
            SetUp();
        }

        for (int f = 0; f < fish.Length; f++)
        {
            particle = fish[f];
            neighbors = UpdateNeighbors(fish, NeighborDistance);
            // Steering Behaviors
            var cohesionVector = Cohesion(CohesionStep, CohesionWeight);
            var separationVector = Separation(SeparationWeight);
            var alignmentVector = Alignment(AlignmentWeight);
            var seekVector = Seek(target.transform, SeekWeight);
            var socializeVector = Socialize(fish, SocializeWeight);
            var arrivalVector = Arrival(target.transform, ArrivalSlowingDistance, ArrivalMaxSpeed);
            // Update Boid's Position and Velocity
            var velocity = particle.velocity + cohesionVector + separationVector + alignmentVector + seekVector + socializeVector + arrivalVector;
            velocity = LimitVelocity(velocity, fishMaxSpeed);
            var position = particle.position + velocity;
            particle = UpdateBoid(position, velocity);
            fish[f] = particle;
        }

        system.SetParticles(fish);
    }

    public List<ParticleSystem.Particle> UpdateNeighbors(ParticleSystem.Particle[] boids, float distance)
    {
        var neighbors = new List<ParticleSystem.Particle>();

        for (var i = 0; i < boids.Length; ++i)
        {
            if (particle.position != boids[i].position)
            {
                if (Vector3.Distance(boids[i].position, particle.position) < distance)
                {
                    neighbors.Add(boids[i]);
                }
            }
        }
        return neighbors;
    }

    public Vector3 Cohesion(float steps, float weight)
    {
        var pc = Vector3.zero;    // Perceived Center of Neighbors

        if (neighbors.Count == 0 || steps < 1) return pc;

        // Add up the positions of the neighbors
        for (var i = 0; i < neighbors.Count; ++i)
        {
            var neighbor = neighbors[i];
            if (pc == Vector3.zero)
            {
                pc = neighbor.position;
            }
            else
            {
                pc = pc + neighbor.position;
            }
        }
        // Average the neighbor's positions
        pc = pc / neighbors.Count;
        // Return the offset vector, divide by steps (100 would mean 1% towards center) and multiply by weight
        return (pc - particle.position) / steps * weight;
    }

    public Vector3 Separation(float weight)
    {
        var c = Vector3.zero;    // Center point of a move away from close neighbors

        for (var i = 0; i < neighbors.Count; ++i)
        {
            if (fish[i].Equals(particle))
                continue;

            var neighbor = neighbors[i];
            var distance = Vector3.Distance(particle.position, neighbor.position);

            Vector3 vector = Vector3.Normalize(particle.position - neighbor.position) / Mathf.Pow(distance, 2);

            c = c + vector;
        }
        
        return c * weight;
    }

    public Vector3 Alignment(float weight)
    {
        Vector3 pv = Vector3.zero;    // Perceived Velocity of Neighbors

        if (neighbors.Count == 0) return pv;

        for (var i = 0; i < neighbors.Count; ++i)
        {
            var neighbor = neighbors[i];
            pv = pv + neighbor.velocity;
        }
        // Average the velocities
        if (neighbors.Count > 1)
        {
            pv = pv / (neighbors.Count);
        }
        // Return the offset vector multiplied by weight
        return (pv - particle.velocity) * weight;
    }

    public Vector3 Seek( Transform target, float weight)
    {
        if (weight < 0.0001f) return Vector3.zero;

        var desiredVelocity = (target.position - particle.position) * weight;
        return desiredVelocity - particle.velocity;
    }

    public Vector3 Socialize(ParticleSystem.Particle[] boids, float weight)
    {
        var pc = Vector3.zero;    // Perceived Center of the rest of the flock

        if (neighbors.Count != 0) return pc;

        // Add up the positions of all other boids
        for (var i = 0; i < boids.Length; ++i)
        {
            var boid = boids[i];
            if (particle.position != boid.position)
            {
                if (pc == Vector3.zero)
                {
                    pc = boid.position;
                }
                else
                {
                    pc = pc + boid.position;
                }
            }
        }
        // Average the positions
        if (boids.Length > 1)
        {
            pc = pc / (boids.Length - 1);
        }
        // Normalize the offset vector, divide by steps (100 would mean 1% towards center) and multiply by weight
        return Vector3.Normalize(pc - particle.position) * weight;
    }

    public Vector3 Arrival(Transform target, float slowingDistance, float maxSpeed)
    {
        var desiredVelocity = Vector3.zero;
        if (slowingDistance < 0.0001f) return desiredVelocity;

        var targetOffset = target.position - particle.position;
        var distance = Vector3.Distance(target.position, particle.position);
        var rampedSpeed = maxSpeed * (distance / slowingDistance);
        var clippedSpeed = Mathf.Min(rampedSpeed, maxSpeed);
        if (distance > 0)
        {
            desiredVelocity = (clippedSpeed / distance) * targetOffset;
        }
        return desiredVelocity - particle.velocity;
    }

    public ParticleSystem.Particle UpdateBoid(Vector3 position, Vector3 velocity)
    {
        particle.position = position;
        particle.velocity = velocity;

        return particle;
    }

    public Vector3 LimitVelocity(Vector3 v, float limit)
    {
        if (v.magnitude > limit)
        {
            v = v / v.magnitude * limit;
        }
        return v;
    }

    void SetUp()
    {
        fish = new ParticleSystem.Particle[system.particleCount];
        system.GetParticles(fish);    
    }
}
