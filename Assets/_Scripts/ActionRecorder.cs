using System.Collections.Generic;
using UnityEngine;

public class ActionRecorder : MonoBehaviour
{

    public static ActionRecorder Instance = null;

    private readonly Stack<ActionBase> _actions = new Stack<ActionBase>();

    private void Awake()
    {
        if (Instance == null)
        {

            Instance = this;
            return;
        }
        Destroy(this.gameObject);
    }

    public void Record(ActionBase action)
    {
        _actions.Push(action);
        action.Execute();
    }

    public void Rewind()
    {
        if (_actions.Count > 0)
        {
            var action = _actions.Pop();
            action.Undo();
        }
    }

    public void ResetACtions()
    {
        _actions.Clear();
    }
}
