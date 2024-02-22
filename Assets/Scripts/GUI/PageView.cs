using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageView : ScrollRect
{
    public class OnObjChangeEvent : UnityEvent<int> { };

    public int CurrentObjIndex = 0;

    public OnObjChangeEvent OnObjChange = new OnObjChangeEvent();
    private GridLayoutGroup grid = null;
    private float finalPosY = 0;
    private Tweener tweenerMove = null;

    protected override void Start()
    {
        base.Start();
        grid = content.GetComponent<GridLayoutGroup>();
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (tweenerMove != null)
        {
            tweenerMove.Kill();
            tweenerMove = null;
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        int index = ProcessCurrntObjIndex();

        if (CurrentObjIndex != index)
        {
            CurrentObjIndex = index;
            OnObjChange.Invoke(CurrentObjIndex);
        }

    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        float cellSizeY = grid.cellSize.y;
        CurrentObjIndex = ProcessCurrntObjIndex();
        finalPosY = cellSizeY / 2 + CurrentObjIndex * cellSizeY;
        //content.localPosition = new Vector2(content.localPosition.x, -finalPosY);
        tweenerMove = content.DOLocalMoveY(-finalPosY, 0.3f);
        tweenerMove.Play();
    }

    private int ProcessCurrntObjIndex()
    {
        int contentChildCount = content.childCount;
        float contentEndPosY = content.localPosition.y;
        float cellSizeY = grid.cellSize.y;

        int resultIndex = Mathf.Abs((int)(contentEndPosY / cellSizeY));
        if (resultIndex < 0) resultIndex = 0;
        if (resultIndex > contentChildCount - 1) resultIndex = contentChildCount - 1;
        return resultIndex;
    }
}
