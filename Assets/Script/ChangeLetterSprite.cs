using System.Collections.Generic;
using UnityEngine;

public class ChangeLetterSprite : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private int                  _letterID;
    [SerializeField]
    private List<SpriteRenderer> _letterRenderer = new();
    [SerializeField]
    private LettersSO            _letterSprites;

    [SerializeField]
    private ColumnLetterMovement _letterMovement;

    [SerializeField]
    private NetworkManager       _networkManager;

    private bool _hasGetResponseData = false;

    private const int RAW_COUNT = 3;
    private const int COL_COUNT = 5;

    #endregion

    #region Unity Methods

    private void Start()
    {
        InitList();

        _letterMovement.OnReachedMidPoint += OnReachedMidPoint;
        _letterMovement.OnReachedEndPoint += OnReachedEndPoint;

        _networkManager.OnGetResponseData += OnGetResponseData;
    }

    

    private void OnDestroy()
    {
        _letterMovement.OnReachedMidPoint -= OnReachedMidPoint;
        _letterMovement.OnReachedEndPoint -= OnReachedEndPoint;

        _networkManager.OnGetResponseData -= OnGetResponseData;
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

    private Sprite GetRandomSprite()
    {
        int rand = Random.Range(0, _letterSprites.Letters.Count);

        return _letterSprites.Letters[rand];
    }

    private Sprite GetSprite(int index)
    {
        return _letterSprites.Letters[index];
    }

    #endregion

    #region Event Handler Methods

    private void OnReachedMidPoint(object sender, System.EventArgs e)
    {
        if(_hasGetResponseData)
        {
            return;
        }

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

    private void OnGetResponseData(object sender, List<char> e)
    {
        _hasGetResponseData = true;

        int rendererIndex = 0;

        for(int i = _letterID; i < e.Count; i += COL_COUNT)
        {
            char responseChar = e[i];
            _letterRenderer[rendererIndex].sprite = GetSprite(responseChar - 'a');
            rendererIndex++;
        }

        _hasGetResponseData = false;
    }

    #endregion
}
