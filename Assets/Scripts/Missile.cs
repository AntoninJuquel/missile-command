using System;
using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour, IHit
{
    public static event Action OnMissileHit;
    public event Action OnDetonation;
    [SerializeField] private Sprite detonationSprite;
    [SerializeField] private float detonationDuration, detonationScale;
    [SerializeField] private GameObject cross;
    private LayerMask _layerToHit;

    private float _speed;

    private Transform _transform;
    private SpriteRenderer _sr;
    private TrailRenderer _tr;
    private Collider2D _col, _hit;

    private void Awake()
    {
        _transform = transform;
        _sr = GetComponent<SpriteRenderer>();
        _tr = GetComponent<TrailRenderer>();
        _col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        _hit = Physics2D.OverlapCircle(_transform.position, _transform.localScale.x / 2f, _layerToHit);
        if (_speed == 0 || !_hit) return;
        _hit.GetComponent<IHit>().TakeHit();
        Detonate();
    }

    private void FixedUpdate()
    {
        _transform.position += _transform.up * (_speed * Time.fixedDeltaTime);
    }

    private IEnumerator DetonationRoutine()
    {
        var elapsedTime = 0f;
        var currentScale = _transform.localScale;
        var gotToScale = Vector3.one * detonationScale;

        while (elapsedTime < detonationDuration)
        {
            _transform.localScale = Vector3.Lerp(currentScale, gotToScale, (elapsedTime / detonationDuration));
            elapsedTime += Time.deltaTime;
            if (_hit)
                _hit.GetComponent<IHit>().TakeHit();
            yield return null;
        }

        _transform.localScale = gotToScale;
        Destroy(gameObject);
        yield return null;
    }

    public void Setup(Vector3 targetPosition, float speed, LayerMask layerToHit)
    {
        var position = _transform.position;
        var direction = ((Vector2) targetPosition - (Vector2) position).normalized;
        _transform.up = direction;
        _speed = speed;
        cross = Instantiate(cross, targetPosition, Quaternion.identity);
        _layerToHit = layerToHit;

        var time = Vector2.Distance(position, targetPosition) / _speed;
        Invoke(nameof(Detonate), time);
    }

    public void Detonate()
    {
        CancelInvoke();
        _speed = 0;
        _tr.time = 0.25f;
        _sr.sprite = detonationSprite;
        _col.enabled = false;
        Destroy(cross);
        OnDetonation?.Invoke();
        StartCoroutine(DetonationRoutine());
    }

    public void TakeHit()
    {
        OnMissileHit?.Invoke();
        Detonate();
    }
}