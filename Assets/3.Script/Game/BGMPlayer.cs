using System.Collections;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] float playTerm;
    [SerializeField] AudioClip music;
    void Start()
    {
        StartCoroutine(PlayMusic());
    }

     IEnumerator PlayMusic()
    {
        yield return new WaitForSeconds(playTerm);
        SoundManager.Instance.PlayBGM(music, true);
    }

}
