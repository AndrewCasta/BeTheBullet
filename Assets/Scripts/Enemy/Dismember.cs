using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dismember : MonoBehaviour
{
    [SerializeField] GameObject limbPrefab;
    [SerializeField] GameObject originalLimb;

    public GameObject DismemberLimb()
    {
        Debug.Log($"Dismembering {originalLimb}");
        originalLimb.SetActive(false);
        var limb = Instantiate(limbPrefab, originalLimb.transform.position, originalLimb.transform.rotation);
        return limb;
    }
}
