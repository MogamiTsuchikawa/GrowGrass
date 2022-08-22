using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sheep : MonoBehaviour, ITouchable
{
    [SerializeField] float moveSpeed;
    private float currentMoveSpeed;
    [SerializeField] GameObject destroyExplosionPrefab;
    [SerializeField] GameObject bonePrefab;
    private Rigidbody rigidbody;
    private int touchCount = 0;
    private Animator animator;
    private Grass target;
    private bool isEating = false;
    public bool IsGrass { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        target = GrassManager.instance.grasss
            .Where(g => g.IsActive)
            .OrderBy(
            g => Vector3.Distance(
                transform.position, g.transform.position)
            ).FirstOrDefault();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.IsPlaying)
        {
            GetComponent<AudioSource>().mute = true;
            return;
        }
        if (target == null || !target.IsActive)SetNewTarget();
        rigidbody.MovePosition(
            Vector3.MoveTowards(
                transform.position,
                target.transform.position,
                currentMoveSpeed));
        if (currentMoveSpeed == 0) return;
        transform.LookAt(
            new Vector3(
                target.transform.position.x,
                transform.position.y,
                target.transform.position.z));
    }
    public void OnTouch()
    {
        touchCount++;
        if (touchCount == 5)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("OnTouch");
            SetNewTarget();
        }
    }
    public void Die()
    {
        var explo = Instantiate(destroyExplosionPrefab);
        explo.transform.position = transform.position;
        var bone = Instantiate(bonePrefab);
        bone.transform.position = new Vector3(
            transform.position.x,
            0.31f,
            transform.position.z);
        GameManager.instance.GrassPoint += 100;
        Destroy(gameObject);
    }
    public void OnEat()
    {
        target.Kill();
    }
    public void SetNewTarget()
    {
        target = GrassManager.instance.grasss
            .Where(g => g.IsActive)
            .OrderBy(
            g => Vector3.Distance(
                transform.position, g.transform.position)
            ).FirstOrDefault();
        currentMoveSpeed = moveSpeed;
        isEating = false;
    }
    public void OnTriggerStay(Collider collider)
    {
        if (isEating) return;
        if (collider.CompareTag("Grass"))
        {
            target = collider.gameObject.GetComponent<Grass>();
            animator.SetTrigger("Eat");
            currentMoveSpeed = 0;
            isEating = true;
        }
    }
}
