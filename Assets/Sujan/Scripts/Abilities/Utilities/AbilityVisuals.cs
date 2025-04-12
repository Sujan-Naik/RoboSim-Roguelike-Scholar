using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Serialization;

// Handles visuals, should not be used for anything else
// Only ability effects should interact
public class AbilityVisuals : MonoBehaviour
{
    
    public ParticleSystem[] kStartParticles;
    public ParticleSystem[] kEndParticles;
    public ParticleSystem[] kUserParticles;
    public ParticleSystem[] kEnemyParticles;
    public ParticleSystem[] kEnvironmentParticles;

	private List<ParticleSystem> _mActivatedParticles = new List<ParticleSystem>();

	public void Remove()
	{
		foreach (var particle in _mActivatedParticles)
        {
			if (!particle)
			{
				Destroy(particle.gameObject);
			}
        }
	}

    private void PlayParticleArray(ParticleSystem[] particles)
    {
        foreach (var particle in particles)
        {
			var clonedParticles = Instantiate(particle, transform);
			_mActivatedParticles.Add(clonedParticles);
            clonedParticles.Play();
        }
    }
    
    private void PlayParticleArray(ParticleSystem[] particles, Vector3 position)
    {
        foreach (var particle in particles)
        {
            var clonedParticles = Instantiate(particle, position, Quaternion.identity);
            _mActivatedParticles.Add(clonedParticles);
            clonedParticles.Play();
        }
    }

    public void PlayStartParticles()
    {
		PlayParticleArray(kStartParticles);

    }
    
    public void PlayEndParticles()
    {
        PlayParticleArray(kEndParticles);
    }
    
    public void PlayUserParticles()
    {
        PlayParticleArray(kUserParticles);
    }
    
    public void PlayEnemyParticles(Vector3 position)
    {
        PlayParticleArray(kEnemyParticles, position);
    }
    
    public void PlayEnvironmentParticles(Vector3 position)
    {
        PlayParticleArray(kEnvironmentParticles, position);
    }
    

}
