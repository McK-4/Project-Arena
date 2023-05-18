using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Animator anim;
    AudioSource source;
    [SerializeField]AudioClip clip;

    float fireballTimer;
    float fireballTime = 1.5f;
    bool exploding;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fireballTimer < fireballTime)
        {
            fireballTimer += Time.deltaTime;
        }
        else if (fireballTimer >= fireballTime && !exploding)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            StartCoroutine(fireballExplotion());
            exploding = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (name.Substring(0, 8) != collision.gameObject.transform.name && collision.gameObject.tag == "Player")
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            StartCoroutine(fireballExplotion());
        }    
    }

    IEnumerator fireballExplotion()
    {
        anim.SetTrigger("Explode");
        source.clip = clip;
        source.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
