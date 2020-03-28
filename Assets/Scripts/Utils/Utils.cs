using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SuicideMission
{
    public static class Utils
    {
        public static void PlayParticle(GameObject particle, Transform transform)
        {
            if (particle == null) throw new Exception("Particle does not exist.");
            if (transform == null) throw new Exception("Transform does not exist.");

            var explosion = Object.Instantiate(particle, transform.position, Quaternion.identity);
            Object.Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        }

        public static void PlayParticle(GameObject particle, Transform transform, Transform parent)
        {
            if (particle == null) throw new Exception("Particle does not exist.");
            if (transform == null) throw new Exception("Transform does not exist.");

            var explosion = Object.Instantiate(particle, transform.position, Quaternion.identity, parent);
            Object.Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        }
    }
}