using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpawnPoint : MonoBehaviour
{
    bool spawnDisabled;

    public void FlashWrapper()
    {
        if (!spawnDisabled)
            StartCoroutine("SpawnDisableTimer");
    }


    IEnumerator SpawnDisableTimer()
    {

        spawnDisabled = true;
        Debug.Log("spawn disabled");
        yield
        return new WaitForSeconds(0.1f);
        Debug.Log("spawn enabled");
        yield
        return new WaitForSeconds(0.1f);
        spawnDisabled = false;
        Debug.Log("spawn enabled");
    }

    public void SpawnDisable()
    {
        FlashWrapper();
    }
}
