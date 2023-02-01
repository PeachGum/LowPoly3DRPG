using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class DamagePopUpAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;
    private Camera cam;

    private TextMeshProUGUI tmp;
    private float time = 0;
    public Transform target;
    public Vector3 origin;
    private float disableTime = 5f;
    WaitForSeconds waitForSeconds;
    private Vector3 movePosition = new Vector3(0, 1f, 0);
    private void OnEnable()
    {
        StartCoroutine(DisableObject());
    }
    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(disableTime);
        cam = Camera.main;
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
    }
    private void Update()
    {
        
        if(target != null)
        {
            transform.forward = cam.transform.forward;
            tmp.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
            transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
            transform.position = target.position + new Vector3(0, 1 * heightCurve.Evaluate(time), 0);
        }
        
        //transform.position = origin + new Vector3(0, 1 * heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }

    IEnumerator DisableObject()
    {
        yield return waitForSeconds;
        target = null;
        gameObject.SetActive(false);
    }
}
