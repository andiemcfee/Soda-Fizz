using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    Transform target;
    Vector3 initialPos;

    public float shakeDuration;
    public bool isShaking = false;
    public float timeInShakeLeft;

    void Start() //grabbing components
    {
        target = GameObject.Find("Static Main Camera").GetComponent<Transform>();
        initialPos = target.localPosition;
    }

    void Update() //checks if isShaking is true
    {
        /*if (timeInShakeLeft > 0 && !isShaking)
        {
            StartCoroutine("doShake");
        }*/
    }

    public void Shake(float shakeDuration)
    {
        if (shakeDuration > 0)
        {
            timeInShakeLeft = shakeDuration;
        }
        StartCoroutine("doShake");
    }

    IEnumerator doShake()
    {
        isShaking = true;
        var startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < startTime + timeInShakeLeft)
        {
            var newCameraPosition = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0);
            target.position = initialPos + newCameraPosition;
            yield return null;
        }

        timeInShakeLeft = 0f;
        target.localPosition = initialPos;
        isShaking = false;

    }
}
