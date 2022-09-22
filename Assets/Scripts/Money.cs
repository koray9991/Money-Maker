using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    
   public void ChangeTag()
    {
        transform.tag = "Money";
    }
    public IEnumerator Tag()
    {
        yield return new WaitForSeconds(0.5f);
        ChangeTag();
        transform.position = new Vector3(transform.position.x, 0.35f, transform.position.z);

        
    }
}
