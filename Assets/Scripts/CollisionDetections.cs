using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionDetections : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("collision start");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dead")){
            transform.gameObject.SetActive(false);
            Destroy(transform.gameObject);

        }
        else if (transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            transform.GetComponent<Animator>().SetBool("isAttacking", false);

        }else if(transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle") && transform.tag != "Enemy")
        {
            transform.GetComponent<Collider>().enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("col:"+ other.gameObject.tag);

       
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (other.gameObject.tag == "Enemy" && other.GetComponent<ChessPiece>().team!= transform.GetComponent<ChessPiece>().team)
        {
            transform.GetComponent<Animator>().SetBool("isWalking", false);
            transform.GetComponent<Animator>().SetBool("isAttacking", true);
            other.gameObject.GetComponent<Animator>().SetBool("isDead", true);
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("Do something here");
        }
        else if (other.gameObject.tag == "Weapon")
        {
            Debug.Log("take damage");
            
        }
    }

    public void playSound(AudioClip clip)
    {
        Debug.Log("playing sound");
        AudioSource source = transform.GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
    }
}
