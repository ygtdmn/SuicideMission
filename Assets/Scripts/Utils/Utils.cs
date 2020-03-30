using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SuicideMission
{
    public static class Utils
    {
        public static void PlayParticle(GameObject particle, Vector3 position)
        {
            if (particle == null) throw new Exception("Particle does not exist.");
            if (position == null) throw new Exception("Position does not exist.");

            var explosion = Object.Instantiate(particle, position, Quaternion.identity);
            Object.Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        }

        public static void PlayParticle(GameObject particle, Vector3 position, Transform parent)
        {
            if (particle == null) throw new Exception("Particle does not exist.");
            if (position == null) throw new Exception("Position does not exist.");

            var explosion = Object.Instantiate(particle, position, Quaternion.identity, parent);
            Object.Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        }
    }
}