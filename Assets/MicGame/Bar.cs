using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bar : MonoBehaviour
{
    public Animator anim;

    private bool isGameStarted = false;
    private bool isGameOver = false;

    public GameObject textBg;
    public GameObject[] textLines;

    public Vector2 randomTauntFreq = new Vector2(20, 30);
    private float tauntTime;

    public KeyCode tauntKey = KeyCode.T;

    public float welcomeDelay = 1f;
    public AudioClip soundWelcome;
    public AudioClip soundWelcome2;
    public float welcome2Delay = 3f;
    public AudioClip[] soundTaunt;
    private int soundTauntIndex = 0;
    public float winDelay = 5;
    public AudioClip soundWin;
    public AudioSource baronVoice;

    public void Say(AudioClip clip)
    {
        baronVoice.PlayOneShot(clip);
    }

    public void Start()
    {
        tauntTime = Time.time + Random.Range(randomTauntFreq.x, randomTauntFreq.y);
        anim.SetTrigger("Welcome");

        // randomize
        soundTaunt = soundTaunt.OrderBy(x => Random.value).ToArray();

        StartCoroutine(pTween.Wait(welcomeDelay, () => { Say(soundWelcome); }));
        StartCoroutine(pTween.Wait(welcome2Delay, () => { Say(soundWelcome2); }));
        StartCoroutine(pTween.Wait(5, () => { isGameStarted = true; }));
    }

    private void Update()
    {
        // handle text background
        {
            textBg.SetActive(textLines.Any(tl => tl.activeSelf));
        }

        if (Input.GetKeyDown(tauntKey))
        {
            Taunt();
        }

        if (isGameStarted && !isGameOver)
        {
            if (Time.time > tauntTime)
            {
                tauntTime = Time.time + Random.Range(randomTauntFreq.x, randomTauntFreq.y);
                Taunt();
            }
        }

        if (!isGameOver)
        {
            if (Cas.instance.isWin)
            {
                Win();
            }
        }
    }

    public void Taunt()
    {
        anim.SetTrigger("Taunt");
        soundTauntIndex++;
        if (soundTauntIndex >= soundTaunt.Length)
        {
            soundTauntIndex = 0;
            soundTaunt = soundTaunt.OrderBy(x => Random.value).ToArray();
        }

        Say(soundTaunt[soundTauntIndex]);
    }

    public void Win()
    {
        isGameOver = true;
        StartCoroutine(pTween.Wait(winDelay, () =>
        {
            Say(soundWin);
            anim.SetTrigger("Win");
        }));
    }
}