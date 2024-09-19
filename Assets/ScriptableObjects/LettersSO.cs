using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LetterSO", menuName = "LettersSO")]
public class LettersSO : ScriptableObject
{
    public List<Sprite> Letters = new();
}
