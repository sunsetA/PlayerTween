﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FlowerBloom : MonoBehaviour
{
    // declaring the Animator component (which you need to add to this gameobject, and assign it into the "flowerAnim" field in the Inspector)
    public Animator flowerAnim;
    // declaring the boolean condition to check whether the flower should bloom, or not
    public bool bloom;
    
    private float yyy;
    // Start is called before the first frame update
    void Start()
    {
        yyy =transform.localScale.x;
        transform.localScale=new Vector3(0,0,0);
        transform.DOScale(yyy,30);
        flowerAnim=GetComponentInChildren<Animator>();
        // setting the boolean condition "bloom" to false at the Start of your game (meaning: at the first press of "LeftShift", key used in this example, the flower will bloom/open) 
        // *you can erase this line, if you wish to manually set the boolean state at the Start of oyur game (checking or unchecking "bloom" in the Animator's Parameters tab)*
        //bloom = false;
        Invoke("Flower",30);
    }
    private void Flower(){
        flowerAnim.speed=0.2f;
          flowerAnim.SetBool("bloom", true);
    }

    // Update is called once per frame
    void Updates()
    {
        // if pressing "SPACE" (you can change this to any other key -->)
        if (Input.GetKeyDown(KeyCode.Space)) //(<-- change here the KeyCode. from "Space" to whichever key/button you wish)
        {
            // change the boolean condition (if true, set it to false, or if false, set it to true; this way, pressing the same button will shift between true and false states of the "bloom" boolean condition)
            bloom = !bloom;

            // if not "bloom"/flower is closed
            if (!bloom)
            {
                // set the Animator's "bloom" condition to true / animate the flower: opening
                flowerAnim.SetBool("bloom", true);
            }
            // if "bloom"/flower is open
            else if (bloom)
            {
                // set the Animator's "bloom" condition to false / animate the flower: closing
                flowerAnim.SetBool("bloom", false);
            }
        }
    }
}
