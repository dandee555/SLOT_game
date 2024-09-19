using UnityEngine;

public class ColumnLetterMovement : MonoBehaviour
{
    #region Private Fields

    [SerializeField] private SpriteRenderer _letter;
    [SerializeField] private Transform      _standardPoint;
    [SerializeField] private float          _scrollSpeed;
    [SerializeField] private bool           _canMove;

    private Vector3 _origPos;
    private Vector3 _endPointPos;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _origPos     = _standardPoint.position;
        _endPointPos = CalculateEndPointPos();
        Debug.Log($"{_letter.bounds.size}");
        Debug.Log($"{_letter.bounds.extents}");
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

    private Vector3 CalculateEndPointPos()
    {
        float moveDistance = (transform.childCount - 3) * _letter.bounds.size.y;

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

        if(HasReachedEndPoint())
        {
            _standardPoint.position = _origPos;
        }
    }

    private bool HasReachedEndPoint()
    {
        return _standardPoint.position.y <= _endPointPos.y;
    }

    #endregion
}
