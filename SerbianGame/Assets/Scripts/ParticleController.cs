using NUnit.Framework;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem groundImpactParticle;

    public void PlayGroundImpactParticle()
    {
        if (groundImpactParticle != null)
            groundImpactParticle.Play();
    }
}
