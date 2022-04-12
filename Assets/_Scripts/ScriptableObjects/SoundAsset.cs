using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundObject", menuName = "ScriptableObjects/SoundObject", order = 1)]
public class SoundAsset : ScriptableObject
{
    public List<SoundVariant> soundAssets = new List<SoundVariant>();
}

[System.Serializable]
public class SoundVariant
{
    public int terrainTextureOrder;
    public string tag;
    [Range(0, 1)] public float maxVolume = 1;
    [Range(0, 1)] public float minVolume = 0.3f;
    public AudioClip[] audioClips;

    public void Play(AudioSource audioPlayer)
    {
        audioPlayer.volume = Random.Range(minVolume, maxVolume);
        audioPlayer.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioPlayer.Play();
    }
}