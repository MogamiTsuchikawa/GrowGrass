using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KusakariOzisan : MonoBehaviour,ITouchable
{
    [SerializeField] Transform blade;
    [SerializeField] float bladeRotateSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject destroyExplosionPrefab;
    [SerializeField] GameObject gravePrefab;
    private Rigidbody rigidbody;
    private int touchCount = 0;
    private Animator animator;
    public bool IsGrass { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.IsPlaying) 
        {
            GetComponent<AudioSource>().mute = true;
            return;
        }
        blade.Rotate(0, 0, bladeRotateSpeed);
        rigidbody.MovePosition(transform.position + transform.forward * moveSpeed);
    }
    public void OnTouch()
    {
        touchCount++;
        if(touchCount == 5)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("OnTouch");
        }

    }
    public void Die()
    {
        var explo = Instantiate(destroyExplosionPrefab);
        explo.transform.position = transform.position;
        var grave = Instantiate(gravePrefab);
        grave.transform.position = new Vector3(
            transform.position.x,
            0.3f,
            transform.position.z);
        GameManager.instance.GrassPoint += 100;
        Destroy(gameObject);
    }
}
