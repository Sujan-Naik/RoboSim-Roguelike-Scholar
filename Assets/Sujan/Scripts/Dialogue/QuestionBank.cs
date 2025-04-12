using UnityEngine;

[CreateAssetMenu(fileName = "QuestionBank", menuName = "Scriptable Objects/QuestionBank")]
public class QuestionBank : ScriptableObject
{
    [TextArea]
    public string[] questions;
}
