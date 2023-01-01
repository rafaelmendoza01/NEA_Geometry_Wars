using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeParticleBehavior : MonoBehaviour
{
    private float speed = 10f;


    void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        StartCoroutine(ContinueMoving());
    }

    IEnumerator ContinueMoving()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
