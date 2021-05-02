using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemSizeComponent : MonoBehaviour
{

    public void SetSize(float size)
    {
        var particleSystem = GetComponent<ParticleSystem>();

        particleSystem.Stop();
        
        var shape = particleSystem.shape;
        shape.scale *= size;

        particleSystem.Play();
    }
}
