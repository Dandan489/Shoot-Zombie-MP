using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blood : MonoBehaviour
{
    private IEnumerator coroutine;
    void Start()
    {
        coroutine = Wait();
        StartCoroutine(coroutine);
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
