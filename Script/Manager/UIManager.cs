using System.Collections.Generic;
using UnityEngine;
using Tool;

public class UIManager : Singleton<UIManager>
{
    // canvas of scene
    public Canvas Canvas;

    public enum View : int
    {
        Lobby = Tool.BuilderUI.Type.LobbyView,
        ShipSelection = Tool.BuilderUI.Type.ShipSelectionView,
        None
    }

    [Tooltip("List of available menu")]
    public List<View> AvailableView = new List<View>();

    [Tooltip("First view displayed")]
    public View First;

    // all ui screen
    private Dictionary<View, GameObject> _views = new Dictionary<View, GameObject>();
    // current view display
    private View _current = View.None;

    public void Init()
    {
        _views.Clear();
        // add all screen needed
        foreach (View view in AvailableView)
        {
            _views.Add(view, Builder.Instance.Build(Builder.FactoryType.UI, (int)view, Vector3.zero, Quaternion.identity, Canvas.transform));
            _views[view].SetActive(false);
        }
        GoToView(First);
    }

    public void GoToView(View value)
    {
        ActiveView(_current, false);
        ActiveView(value, true);
        _current = value;
    }

    private void ActiveView(View value, bool active)
    {
        if (_views.ContainsKey(value))
        {
            _views[value].SetActive(active);
        }
    }
}
