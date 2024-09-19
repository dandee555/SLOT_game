using System.Collections.Generic;
using UnityEngine;

public class ChangeLetterSprite : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private List<SpriteRenderer> _letterRenderer = new();
    [SerializeField]
    private LettersSO            _letterSprites;

    [SerializeField]
    private ColumnLetterMovement _letterMovement;

    private const int RAW_COUNT = 3;

    #endregion

    #region Unity Methods

    private void Start()
    {
        InitList();

        _letterMovement.OnReachedMidPoint += OnReachedMidPoint;
        _letterMovement.OnReachedEndPoint += OnReachedEndPoint;
    }

    private void OnDestroy()
    {
        _letterMovement.OnReachedMidPoint -= OnReachedMidPoint;
        _letterMovement.OnReachedEndPoint -= OnReachedEndPoint;
    }

    #endregion

    #region Private Helper Methods

    private void InitList()
    {
        foreach(Transform child in transform)
        {
            if(child.TryGetComponent(out SpriteRenderer renderer))
            {
                _letterRenderer.Add(renderer);
            }
            else
            {
                Debug.Log("Missing Sprite Renderer");
            }
        }
    }

    private void OnReachedMidPoint(object sender, System.EventArgs e)
    {
        for(int i = 0; i < RAW_COUNT; i++)
        {
            _letterRenderer[i].sprite = GetRandomSprite();
        }

        int j = 0;
        for(int i = _letterRenderer.Count - RAW_COUNT; i < _letterRenderer.Count; i++)
        {
            _letterRenderer[i].sprite = _letterRenderer[j].sprite;
            j++;
        }
    }

    private void OnReachedEndPoint(object sender, System.EventArgs e)
    {
        for(int i = RAW_COUNT; i < _letterRenderer.Count - RAW_COUNT; i++)
        {
            _letterRenderer[i].sprite = GetRandomSprite();
        }
    }

    private Sprite GetRandomSprite()
    {
        int rand = Random.Range(0, _letterSprites.Letters.Count);

        return _letterSprites.Letters[rand];
    }

    #endregion
}
