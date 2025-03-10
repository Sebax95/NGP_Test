using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private bool _canskip = false;
    private IEnumerator Start()
    {
        UpdateManager.SetPause(true);
        yield return new WaitForSeconds(5);
        _canskip = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _canskip)
        {
            UpdateManager.SetPause(false);
            Destroy(gameObject);
        }
    }
}
