using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public GameObject model;

   [Range(1,10)] public int shootDamage;
   [Range(5,1500)] public int shootDistance;
   [Range(0.1f,3)] public float shootRate;

   public int ammoCur;

   [Range(5,60)] public int ammoMax;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSounds;
   [Range(0,1)] public float shootVol;
    internal int damage;
    internal float fireRate;
    internal int range;
}
