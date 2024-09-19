using System;
using UnityEngine;

public class ColumnLetterMovement : MonoBehaviour
{
    #region Private Fields

    [SerializeField] private SpriteRenderer _letter;
    [SerializeField] private Transform      _standardPoint;
    [SerializeField] private float          _scrollSpeed;
    [SerializeField] private bool           _canMove;

    private Vector3 _origPointPos;
    private Vector3 _midPointPos;
    private Vector3 _endPointPos;

    private bool    _canFireEvent = true;

    private const int ROW_COUNT = 3;

    #endregion

    #region Public Events

    public event EventHandler OnReachedMidPoint;
    public event EventHandler OnReachedEndPoint;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _origPointPos = _standardPoint.position;
        _midPointPos  = CalculateMidPointPos();
        _endPointPos  = CalculateEndPointPos();
    }

    private void Update()
    {
        if(_canMove)
        {
            Scrolling();
        }
    }

    #endregion

    #region Private Helper Methods

    private Vector3 CalculateMidPointPos()
    {
        float moveDistance = ROW_COUNT * _letter.bounds.size.y;

        Vector3 midPointPos = new
        (
            _standardPoint.position.x,
            _standardPoint.position.y - moveDistance,
            _standardPoint.position.z
        );

        return midPointPos;
    }

    private Vector3 CalculateEndPointPos()
    {
        float moveDistance = (transform.childCount - ROW_COUNT) * _letter.bounds.size.y;

        Vector3 endPointPos = new
        (
            _standardPoint.position.x,
            _standardPoint.position.y - moveDistance,
            _standardPoint.position.z
        );

        return endPointPos;
    }

    private void Scrolling()
    {
        Vector3 stdPointPos = _standardPoint.position;
        stdPointPos = new
        (
            stdPointPos.x,
            stdPointPos.y - _scrollSpeed * Time.deltaTime,
            stdPointPos.z
        );

        _standardPoint.position = stdPointPos;

        if(HasReachedMidPoint() && _canFireEvent)
        {
            OnReachedMidPoint?.Invoke(this, EventArgs.Empty);
            _canFireEvent = false;
        }

        if(HasReachedEndPoint())
        {
            OnReachedEndPoint?.Invoke(this, EventArgs.Empty);
            ResetToOrigPoint();
        }
    }

    private bool HasReachedMidPoint()
    {
        return _standardPoint.position.y <= _midPointPos.y;
    }

    private bool HasReachedEndPoint()
    {
        return _standardPoint.position.y <= _endPointPos.y;
    }

    private void ResetToOrigPoint()
    {
        _standardPoint.position = _origPointPos;
        _canFireEvent = true;
    }

    #endregion
}
