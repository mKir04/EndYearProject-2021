
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public float damage = 10f;
    public float coolDown = 0.2f;    
    public float range = 100f;
    public int shoot; //must be between 0 and 49
    private System.Random ran;


    public Transform transform;
    public ParticleSystem muzzleFlash;


    void Start() {
        ran = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.deltaTime % .2f == 0){
            Shoot();
        }
    }

    void Shoot() {
        muzzleFlash.Play();
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, range)){
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if(target != null && ran.Next(0, 50) / Time.deltaTime <= shoot) {
                target.TakeDamage(damage);
            }
        }
    }
}
