using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CustomVerticalLayout : MonoBehaviour
{
    #region Private Fields

    [SerializeField, OnValueChanged("ApplyLayout")]
    private float _spacing;

    [SerializeField, OnValueChanged("ApplyLayout")]
    private Transform _standardPoint;

    private List<Transform> _prevChilds = new();

    #endregion

    #region Unity Methods

    private void Start()
    {
        _prevChilds = GetAllChilds();
        ApplyLayout();
    }

    private void Update()
    {
        if(HasHierarchyChanged())
        {
            ApplyLayout();
            _prevChilds = GetAllChilds();
        }
    }

    #endregion

    #region Public Helper Methods

    [Button("Apply Layout")]
    public void ApplyLayout()
    {
        if(!_standardPoint)
        {
            return;
        }

        bool isFirstChild   = true;
        float prevChildYPos = _standardPoint.position.y;

        foreach(Transform child in transform)
        {
            if(isFirstChild)
            {
                child.position = _standardPoint.position;
                isFirstChild   = false;
            }
            else
            {
                float spriteHeight = GetSpriteHeight(child);
                child.position = new Vector3
                (
                    _standardPoint.position.x,
                    prevChildYPos - spriteHeight - _spacing,
                    _standardPoint.position.z
                );

                prevChildYPos = child.position.y;
            }
        }
    }

    #endregion

    #region Private Helper Methods

    private List<Transform> GetAllChilds()
    {
        List<Transform> childs = new();

        for (int i = 0; i < transform.childCount; i++)
        {
            childs.Add(transform.GetChild(i));
        }

        return childs;
    }

    private float GetSpriteHeight(Transform sprite)
    {
        if(sprite.TryGetComponent(out SpriteRenderer renderer))
        {
            return renderer.bounds.size.y;
        }
        else
        {
            return 0;
        }
    }
    
    private bool HasHierarchyChanged()
    {
        List<Transform> currChilds = GetAllChilds();

        if(currChilds.Count != _prevChilds.Count)
        {
            return true;
        }

        for (int i = 0; i < currChilds.Count; i++)
        {
            if(currChilds[i] != _prevChilds[i])
            {
                return true;
            }
        }

        return false;
    }

    #endregion
}
