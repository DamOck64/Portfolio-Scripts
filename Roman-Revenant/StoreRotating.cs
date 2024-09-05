using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoreRotating : MonoBehaviour
{
    private int id;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClickRight()
    {

    }

    private void OnClickLeft()
    {

    }

    private IEnumerator ChangeShownWeapon()
    {
        animator.SetInteger("position", id);
        yield break;
    }
}
