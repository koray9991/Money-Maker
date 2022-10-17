using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform character;
    public float y, zIndex;
    
    void FixedUpdate()
    {
        transform.position = new Vector3(character.position.x, y, character.position.z+zIndex);
    }
}
