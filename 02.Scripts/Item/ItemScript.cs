using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    //private Item item = ScriptableObject.CreateInstance<Item>();
    public Item item;
    private float magnetSpeed = 0.1f; // 자석의 세기
    protected int getItemSuccess = -1;
    private string sd_itemPickUp = "ItemPickUp";
    SphereCollider getItemCollider;

    Transform targetTransform;

    private void Start()
    {
        getItemCollider = gameObject.AddComponent<SphereCollider>();
        getItemCollider.isTrigger = true;
        getItemCollider.radius = 2.5f * (1 / transform.localScale.x);
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }
    // Start is called before the first frame update
    void FixedUpdate()
    {
        MagnetMove();
        transform.Rotate(new Vector3(0f, 20f, 0f) * Time.deltaTime);

    }
    public void PickUp(Transform Target)
    {
        if(targetTransform == null)
        {
            getItemSuccess = Inventory.instance.Add(item);
            targetTransform = Target;
        }
    }

    void MagnetMove()
    {
        if (getItemSuccess != -1)
        {
            transform.position = Vector3.Lerp(transform.position, targetTransform.position, magnetSpeed);
            StartCoroutine(GetItem());
            
        }
    }
    IEnumerator GetItem()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
        AudioManager.instance.SFXPlay(sd_itemPickUp);
    }

}
