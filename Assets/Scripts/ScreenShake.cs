using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{

    public static ScreenShake instance;
    private float shakeTimeRemaining, shakePower, shakeFade, shakeRotation;

    public float rotation = 10;



    public void Start()
    {
        instance = this;
    }

    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;
            float x = Random.Range(-1, 1) * shakePower;
            float y = Random.Range(-1, 1) * shakePower;


            transform.position += new Vector3(0, 0, 0);

            shakePower = Mathf.MoveTowards(shakePower, 0, shakeFade * Time.deltaTime);

            shakeRotation = Mathf.MoveTowards(shakeRotation, 0, shakeFade * rotation * Time.deltaTime);
        }

        transform.rotation = Quaternion.Euler(0, 0, shakeRotation * Random.Range(-1, 1));
    }


    public void StartShake(float lenght, float power)
    {
        shakeTimeRemaining = lenght;
        shakePower = power;

        shakeFade = power / lenght;

        shakeRotation = power * rotation;

    }





}
