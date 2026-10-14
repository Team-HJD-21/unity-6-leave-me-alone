using UnityEngine;

public class FenceCtrl : MonoBehaviour
{
    public float explosionForce = 800f;
    public float explosionRadius = 3f;
    public float upwardsModifier = 0.5f;

    public GameObject fence; // 프리팹
    private Rigidbody _rb;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        // ✅ 초기 위치 저장
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyCar")||other.CompareTag("PlayerCar"))
        {
            Vector3 explosionPos = other.transform.position;

            // ✅ Constraints 해제 후 폭발
            _rb.constraints = RigidbodyConstraints.None;
            _rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, upwardsModifier, ForceMode.Impulse);
            SoundManager.Instance.Play3DSfx(SoundManager.Sfx.FenceBreak,transform,0.5f);
            // Debug.Log("🚗 Fence hit by car → flying!");

            // ✅ 5초 후 재생성
            Invoke(nameof(Regen), 5f);
        }
    }

    void Regen()
    {
        // ✅ 초기 위치에 새로 생성
        Instantiate(fence, initialPosition, initialRotation);
        Destroy(gameObject);
    }
}