using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosterInitGame : MonoBehaviour
{
    private void Start()
    {
        GameManager.OnFinish += Activate;
    }

    private void OnDestroy()
    {
        GameManager.OnFinish -= Activate;
    }

    public void Activate()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }
}
