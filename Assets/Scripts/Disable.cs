using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(DisableObject());
    }

  public IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
