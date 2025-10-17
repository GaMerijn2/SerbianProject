using Unity.VisualScripting;
using UnityEngine;

public partial class ProjectileSpellStrategy
{
    public class ProjectileBuilder
    {
        private GameObject projectilePrefab;
        private float speed = 10f;
        private float duration = 5f;
        private Vector3 offset = new Vector3(0, 0, 0);

        public ProjectileBuilder WithProjectilePrefab(GameObject projectilePrefab)
        {
            this.projectilePrefab = projectilePrefab;
            return this;
        }

        public ProjectileBuilder WithSpeed(float speed)
        {
            this.speed = speed;
            return this;
        }

        public ProjectileBuilder WithDuration(float duration)
        {
            this.duration = duration;
            return this;
        }

        public ProjectileBuilder WithOffset(Vector3 offset)
        {
            this.offset = offset;
            return this;
        }

        public GameObject Build(Transform origin)
        {
            Vector3 instantiatePosition = (origin.position + offset) + origin.forward * 2;

            GameObject projectile = Instantiate(projectilePrefab, instantiatePosition, origin.rotation);
            Rigidbody rigidbody = projectile.GetOrAddComponent<Rigidbody>();
            DestroyObject destroyObject = projectile.GetOrAddComponent<DestroyObject>();

            rigidbody.linearVelocity = origin.forward * speed;
            destroyObject.duration = duration;

            return projectile;
        }
    }
}
