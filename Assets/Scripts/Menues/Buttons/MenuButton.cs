using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class MenuButton : MonoBehaviour
{
    public MenuButton next;
    public MenuButton prev;
    public abstract void Invoke();
    public abstract void OnSelect();
    public abstract void OnDeselect();

    [SerializeField]
    protected Image visualRepresentation;
    [SerializeField]
    protected CanvasGroup menuGroup;
}
