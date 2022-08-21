using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sheep : MonoBehaviour, ITouchable
{
    [SerializeField] float moveSpeed;
    private float currentMoveSpeed;
    [SerializeField] GameObject destroyExplosionPrefab;
    private Rigidbody rigidbody;
    private int touchCount = 0;
    private Animator animator;
    private Grass target;
    private bool isEating = false;
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
        if(target == null || !target.IsActive)SetNewTarget();
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
            var explo = Instantiate(destroyExplosionPrefab);
            explo.transform.position = transform.position;
            Destroy(gameObject);
        }
        else
        {
            animator.SetTrigger("OnTouch");
            SetNewTarget();
        }
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
