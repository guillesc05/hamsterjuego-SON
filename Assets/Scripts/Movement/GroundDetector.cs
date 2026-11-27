using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GroundDetector : MonoBehaviour
{
    [SerializeField] float _distance = 0;
    [SerializeField] int _rays = 1;
    [SerializeField] float _width = 1f;

    private LayerMask _groundMask;
    private SurfaceType _currentSurface;
    private bool _isGrounded = false;
    private int _lastCheckFrame = -1;

    private void Start()
    {
        _groundMask = LayerMask.GetMask("Ground");
    }

    private void Refresh()
    {
        if (_lastCheckFrame == Time.frameCount) return;
        _lastCheckFrame = Time.frameCount;

        _isGrounded = false;
        _currentSurface = SurfaceType.Invalid;

        float unit = _rays > 0 ? _width / _rays : 0f;
        for (int i = 0; i <= _rays; ++i)
        {
            Vector2 origin = new Vector2(transform.position.x - _width / 2 + unit * i, transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, _distance, _groundMask);
            if (!hit) continue;

            _isGrounded = true;
            switch(hit.collider.tag)
            {
                case "Wooden":    _currentSurface = SurfaceType.Wood;      break;
                case "Metallic":  _currentSurface = SurfaceType.Metal;     break;
                case "Plastic":   _currentSurface = SurfaceType.Plastic;   break;
                case "Cardboard": _currentSurface = SurfaceType.Cardboard; break;
                default:          _currentSurface = SurfaceType.Invalid;   break;
            }
            return;
        }
    }

    public bool IsGrounded()
    {
        Refresh();
        return _isGrounded;
    }

    public SurfaceType GetGroundSurface()
    {
        Refresh();
        return _currentSurface;
    }

    private void OnDrawGizmos()
    {
        float unit = _width / _rays;
        for(int i = 0; i <= _rays; ++i) Debug.DrawRay(new Vector2(transform.position.x - (_width / 2) + unit * i, transform.position.y), Vector2.down * _distance, Color.red);
    }
}

public enum SurfaceType
{
    Wood,
    Metal,
    Plastic,
    Cardboard,
    Invalid,
}