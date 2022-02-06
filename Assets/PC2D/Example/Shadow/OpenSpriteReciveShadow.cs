using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OpenSpriteReciveShadow : MonoBehaviour
{
    void Awake()
    {
        transform.GetComponent<SpriteRenderer>().receiveShadows = true;
        transform.GetComponent<SpriteRenderer>().shadowCastingMode = ShadowCastingMode.TwoSided;
    }
}
