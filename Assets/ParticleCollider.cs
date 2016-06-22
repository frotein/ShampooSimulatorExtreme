using UnityEngine;
using System.Collections;

public class ParticleCollider : MonoBehaviour {

    public ParticleSystem ps;
    public float radius;
    public float scale;
    float radiusSqr;
    // Use this for initialization
	void Start () {
        radiusSqr = radius * radius;
	}
	
	// Update is called once per frame
	void Update ()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        ps.GetParticles(particles); 

        for(int i = 0; i < particles.Length; i++)
        {
            ParticleSystem.Particle p1 = particles[i];
            for(int j = i; j < particles.Length; j++)
            {
                ParticleSystem.Particle p2 = particles[j];
                Vector2 lnth = new Vector2(p1.position.x - p2.position.x, p1.position.y - p2.position.y);
                if(lnth.x * lnth.x + lnth.y * lnth.y < radiusSqr)
                {
                    Vector2 mid = (p1.position + p2.position) / 2f;
                    p1.velocity += ((p1.position.XY() - mid) * scale).XYZ(0);
                    p1.velocity += ((p2.position.XY() - mid) * scale).XYZ(0);
                }
                particles[j] = p2;
            }
            particles[i] = p1;
        }

        ps.SetParticles(particles, particles.Length);
	}
}
