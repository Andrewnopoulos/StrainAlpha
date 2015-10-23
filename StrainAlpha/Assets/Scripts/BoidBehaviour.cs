//
// Boids - Flocking behavior simulation.
//
// Copyright (C) 2014 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using UnityEngine;
using System.Collections;

public class BoidBehaviour : MonoBehaviour
{
    // Reference to the controller.
    public BoidController controller;

    // Options for animation playback.
    public float animationSpeedVariation = 0.2f;

    private float rotationOffset = 0.0f;

    private bool dying = false;

    private Vector3 deathVelocity;
    private Vector3 deathRotation;
    private float deathSpeed;
    private float deathAngularVelocity;
    private float deathTimer;

    private Renderer renderer;

    // Random seed.
    float noiseOffset;

    // Caluculates the separation vector with a target.
    Vector3 GetSeparationVector(Transform target)
    {
        var diff = transform.position - target.transform.position;
        var diffLen = diff.magnitude;
        var scaler = Mathf.Clamp01(1.0f - diffLen / controller.neighborDist);
        return diff * (scaler / diffLen);
    }

    void Start()
    {
        noiseOffset = Random.value * 10.0f;

        var animator = GetComponent<Animator>();
        if (animator)
            animator.speed = Random.Range(-1.0f, 1.0f) * animationSpeedVariation + 1.0f;

        rotationOffset = Random.Range(0.0f, 360.0f);

        renderer = GetComponent<Renderer>();
    }

    void LaserBehaviour()
    {

    }

    void ChargeBehaviour()
    {

    }

    void ShieldBehaviour()
    {
        var rotationVector = controller.transform.up;

        transform.position = controller.transform.position + new Vector3(Mathf.Cos(Time.time + rotationOffset), 0, Mathf.Sin(Time.time + rotationOffset)) * 2;

    }

    void BombBehaviour()
    {

    }

    public void Kill()
    {
        dying = true;
        deathRotation = new Vector3(Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f));
        deathVelocity = (transform.position - controller.transform.position).normalized;
        deathSpeed = Random.Range(1.0f, 5.0f);
        deathAngularVelocity = Random.Range(1.0f, 5.0f);
        deathTimer = Random.Range(2.0f, 3.0f);
    }

    void DyingBehaviour()
    {
        transform.position += deathVelocity * deathSpeed * Time.deltaTime;
        transform.Rotate(deathRotation, deathAngularVelocity * Time.deltaTime);

        deathTimer -= Time.deltaTime;

        renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, deathTimer);

        if (deathTimer < 0)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (dying)
        {
            DyingBehaviour();
            return;
        }

        ShieldBehaviour();
        return;

        var currentPosition = transform.position;
        var currentRotation = transform.rotation;

        // Current velocity randomized with noise.
        var noise = Mathf.PerlinNoise(Time.time, noiseOffset) * 2.0f - 1.0f;
        var velocity = controller.velocity * (1.0f + noise * controller.velocityVariation);

        // Initializes the vectors.
        var separation = Vector3.zero;
        var alignment = controller.transform.forward * 0.5f;
        var cohesion = controller.transform.position;

        // Looks up nearby boids.
        var nearbyBoids = Physics.OverlapSphere(currentPosition, controller.neighborDist, controller.searchLayer);

        // Accumulates the vectors.
        foreach (var boid in nearbyBoids)
        {
            if (boid.gameObject == gameObject) continue;
            var t = boid.transform;
            separation += GetSeparationVector(t);
            alignment += t.forward;
            cohesion += t.position;
        }

        var avg = 1.0f / nearbyBoids.Length;
        alignment *= avg;
        cohesion *= avg;
        cohesion = (cohesion - currentPosition).normalized;

        // Calculates a rotation from the vectors.
        var direction = separation + alignment + cohesion;
        var rotation = Quaternion.FromToRotation(Vector3.forward, direction.normalized);

        // Applys the rotation with interpolation.
        if (rotation != currentRotation)
        {
            var ip = Mathf.Exp(-controller.rotationCoeff * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(rotation, currentRotation, ip);
        }

        // Moves forawrd.
        transform.position = currentPosition + transform.forward * (velocity * Time.deltaTime);
    }
}
