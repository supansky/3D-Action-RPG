using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    private GameObject fireball;
    private PlayerCharacter target;
    private Vector3 targetPosition;
    private Vector3 targetDirection;

    public const float baseSpeed = 3.0f;

    public float speed = 3.0f;
    public float obstacleRange = 5.0f;
    public bool alive { get; set; }

    public float visionRadius = 20.0f;

    private void Awake()
    {
        //Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    private void Start()
    {
        alive = true;
    }

    private void Update()
    {
        if (alive)
        {
            transform.Translate(0, 0, speed * Time.deltaTime);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, visionRadius);
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<PlayerCharacter>() != null)
                {
                    target = hitCollider.GetComponent<PlayerCharacter>();
                    targetPosition = target.transform.position;
                    targetPosition.y = transform.position.y;
                    targetDirection = (targetPosition - transform.position).normalized;

                }
            }

                Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(ray, 0.75f, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<PlayerCharacter>())
                {
                    if (fireball == null)
                    {
                        fireball = Instantiate(fireballPrefab) as GameObject;
                        fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                        fireball.transform.rotation = transform.rotation;
                    }
                }

                else if (hit.distance < obstacleRange && !hit.transform.gameObject.GetComponent<Fireball>())
                {
                    float angle = Random.Range(-45, 45);
                    transform.Rotate(0, angle, 0);
                }

                else if (target != null && Vector3.Dot(transform.forward, targetDirection) > 0.25f)
                {
                    transform.LookAt(targetPosition);
                }
            }
        }
    }

    public void SetAlive(bool alive)
    { this.alive = alive; }

    private void OnSpeedChanged(float value)
    {
        speed = baseSpeed * value;
    }

    private void OnDestroy()
    {
        //Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }
}
