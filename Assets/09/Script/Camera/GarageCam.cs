using UnityEngine;
using UnityEngine.EventSystems;

public class GarageCam : MonoBehaviour
{
    public float orbitingTimer;
    public float distance = 7.5f;
    public float xSpeed = 150f;
    public float ySpeed = 100f;
    [Range(0f, 1f)] public float orbitSpeed = 0.05f;
    public float yMinLimit = 10f;
    public float yMaxLimit = 45f;
    public Vector3 offset = new Vector3(0f, -.45f, 0f);
    public Transform spawnPoint;

    private float _x, _y;
    private float _xSmoothed, _ySmoothed;
    private Vector2 _lastTouchPos;
    private bool _isDragging;
    private bool _orbiting;
    
    private void Start()
    {
        var angles = transform.eulerAngles;
        _x = angles.y;
        _y = angles.x;
    }

    private void LateUpdate()
    {
        
        HandleTouchInput();

        if (_orbiting)
            _x += orbitSpeed * xSpeed * Time.deltaTime;

        _y = Mathf.Clamp(_y, yMinLimit, yMaxLimit);
        _xSmoothed = Mathf.Lerp(_xSmoothed, _x, Time.deltaTime * 10f);
        _ySmoothed = Mathf.Lerp(_ySmoothed, _y, Time.deltaTime * 10f);

        if (spawnPoint == null) return;
        var rotation = Quaternion.Euler(_ySmoothed, _xSmoothed, 0);
        var position = rotation * new Vector3(0, 0, -distance) + spawnPoint.position + offset;
        transform.SetPositionAndRotation(position, rotation);
    }
    
    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            var touch = Input.GetTouch(0);

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                Debug.Log(touch.fingerId);
                return;
            }

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _lastTouchPos = touch.position;
                    _isDragging = true;
                    _orbiting = false;
                    orbitingTimer = 1.5f;
                    break;
                case TouchPhase.Moved when _isDragging:
                {
                    var delta = touch.position - _lastTouchPos;
                    _x += delta.x * xSpeed * 0.002f;
                    _y -= delta.y * ySpeed * 0.002f;
                    _lastTouchPos = touch.position;
                    _orbiting = false;
                    orbitingTimer = 1.5f;
                    break;
                }
                case TouchPhase.Ended or TouchPhase.Canceled:
                    _isDragging = false;
                    break;
            }
        }

        if (_isDragging) return;
        if (orbitingTimer > 0f)
            orbitingTimer -= Time.deltaTime;
        else
        {
            orbitingTimer = 0f;
            _orbiting = true;
        }
    }
}
