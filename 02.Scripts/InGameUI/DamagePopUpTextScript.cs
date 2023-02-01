using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpTextScript : MonoBehaviour
{
    public float destoryTime = 0.5f;
    public Vector3 offset = new Vector3(0, 2f, 0);
    public Vector3 randomxyz;
    Camera cam;
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(WaitForActive());

        transform.localPosition += offset;
        transform.localPosition += new Vector3(Random.Range(-randomxyz.x, randomxyz.x), Random.Range(-randomxyz.y, randomxyz.y), Random.Range(-randomxyz.z, randomxyz.z));
    }
    private void Awake()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        transform.forward = cam.transform.forward;
    }

    IEnumerator WaitForActive()
    {
        yield return new WaitForSeconds(destoryTime);
        gameObject.SetActive(false);
    }

}
