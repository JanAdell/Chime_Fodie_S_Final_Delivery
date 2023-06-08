using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Footsteps : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip gravelFootsteps;
    public AudioClip snowFootsteps;
    public AudioClip grassFootsteps;
    public AudioClip dryGrassFootsteps;
    public AudioClip stoneFootsteps;
    public AudioClip waterFootsteps;
    public AudioClip woodFootsteps;

    private AudioSource audioSource;
    private TerrainDetector terrain;
    private InputActionReference move, sprint;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        AudioClip clip = GetClip();

        if (move == true || sprint == true)
            audioSource.PlayOneShot(clip);
    }

    private AudioClip GetClip()
    {
        int terrainTextureIndex = terrain.GetTerrainAtPosition(transform.position);

        switch (terrainTextureIndex)
        {
            case 0:
                return stoneFootsteps;
            case 1:
                return dryGrassFootsteps;
            case 2:
                return grassFootsteps;
            case 3:
                return gravelFootsteps;
            case 4:
                return stoneFootsteps;
            case 5:
                return snowFootsteps;
            case 6:
                return waterFootsteps;
            case 7:
                return woodFootsteps;
            default:
                return gravelFootsteps;
                
        }

    }


}
