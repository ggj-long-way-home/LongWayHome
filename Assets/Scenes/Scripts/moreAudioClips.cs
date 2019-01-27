using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class moreAudioClips : MonoBehaviour
{
    /// <summary>
    /// MoreAudioClips: stores a list of audio clips so that they can be played from an audio source on the gameobject
    /// </summary>
    /// Attached to: a gameobject that has an audio source and you want to use multiple audio clips for
    public List<AudioClip> clips; // store the audio clips
    public List<float> volumes;

    void Start()
    {
        while (volumes.Count < clips.Count) //ensure volumes are valid for the clip indices
        {
            volumes.Add(1f); // set a default volume of 1 for clips with no specified volume
        }
    }


    public void PlayClip(int clipNum)
    { // used to play a specific clip from another script
      // GetComponent<MoreAudioClips>().PlayClip[2] would play the 3rd clip (index 2) that you set in the Inspector list for MoreAudioCLips
        if (clips.Count > 0 && clipNum >= 0 && clipNum < clips.Count)
            GetComponent<AudioSource>().PlayOneShot(clips[clipNum], volumes[clipNum]); //uses the AudioSource on the current gameObject
    }

    public void PlayRandomClip()
    { // used to play a random clip from the set of clips
        if (clips.Count > 0)
        {
            int clipNum = Random.Range(0, clips.Count - 1);
            GetComponent<AudioSource>().PlayOneShot(clips[clipNum], volumes[clipNum]);
        }
    }

}