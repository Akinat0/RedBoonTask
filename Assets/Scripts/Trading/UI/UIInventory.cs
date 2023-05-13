using System;
using System.Collections.Generic;
using Trading.Controllers;
using Trading.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] GridLayoutGroup inventoryGrid;
    [SerializeField] UIDropZone dropZone;

    InventoryController Inventory { get; set; }
    RectTransform inventoryGridRectTransform;

    readonly List<UIItemView> itemViews = new List<UIItemView>();

    Action OnBeginItemDrag { get; set; }
    Action OnEndItemDrag { get; set; }
    Action<GameObject> OnDropObject { get; set; }
    
    bool isDirtyItems;


    public void Initialize(InventoryController inventory, Action onBeginItemDrag, Action onEndItemDrag, Action<GameObject> onDropObject)
    {
        Inventory = inventory;

        Inventory.OnItemsChanged += OnItemsChangedHandler;
        dropZone.OnDropGameObject += OnDropGameObjectHandler;

        OnBeginItemDrag = onBeginItemDrag;
        OnEndItemDrag = onEndItemDrag;
        OnDropObject = onDropObject;
        
        isDirtyItems = true;
        UpdateGrid();
    }

    void OnDestroy()
    {
        Inventory.OnItemsChanged -= OnItemsChangedHandler;
        dropZone.OnDropGameObject -= OnDropGameObjectHandler;
    }

    void LateUpdate()
    {
        if (isDirtyItems)
        {
            isDirtyItems = false;
            UpdateItems();
        }
    }

    void OnRectTransformDimensionsChange()
    {
        UpdateGrid();
    }

    public void SetItemDropZoneActive(bool isActive)
    {
        dropZone.gameObject.SetActive(isActive);
    }
    
    void UpdateItems()
    {
        //clear items
        foreach (UIItemView itemView in itemViews)
            UIItemView.Release(itemView);

        itemViews.Clear();
        
        //create items        
        foreach (ItemController item in Inventory.Items)
            itemViews.Add(UIItemView.Create(item, inventoryGrid.transform, OnBeginItemDrag, OnEndItemDrag));
    }

    void UpdateGrid()
    {
        if (inventoryGridRectTransform == null)
            inventoryGridRectTransform = (RectTransform)inventoryGrid.transform;

        const int columnCount = 5;
        
        inventoryGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        inventoryGrid.constraintCount = columnCount;

        float availableWidth = inventoryGridRectTransform.rect.width - inventoryGrid.spacing.x * (columnCount - 1) - inventoryGrid.padding.horizontal;
        float cellSize = availableWidth / columnCount;
        
        inventoryGrid.cellSize = new Vector2(cellSize, cellSize);
    }

    void OnItemsChangedHandler()
    {
        isDirtyItems = true;
    }
    
    void OnDropGameObjectHandler(GameObject droppedObject)
    {
        OnDropObject?.Invoke(droppedObject);
    }
}
