using UnityEngine;
public class Equipable : MonoBehaviour
{
    
    private bool _mEquipped = false;
    private Collider _mCollider;
    private Vector3 _mCenter;
    private Vector3 _mOriginalPosition;
    private void Start()
    {
        _mCollider = GetComponent<Collider>();
        _mCenter = _mCollider.bounds.center;
        _mOriginalPosition = transform.position;
    }

    private void Update()
    {
        if (!_mEquipped)
        {
            transform.RotateAround(_mCenter, Vector3.up, 60f * Time.deltaTime);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (_mEquipped || !other.gameObject.CompareTag("Player")) return;
        transform.position = _mOriginalPosition;
        transform.rotation = Quaternion.identity;
        _mEquipped = true;
        var holdable = GetComponentInParent<Holdable>();
        holdable.SetHolding(true);
    }
    
}