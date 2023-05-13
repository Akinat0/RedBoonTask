using System;
using System.Collections.Generic;
using Trading.Data;

namespace Trading.Controllers
{
    public class InventoryController
    {
        public InventoryController(InventoryDataModel model)
        {
            Model = model;
            
            foreach (ItemDataModel itemModel in model.ItemDataModels)
                AddItem(new ItemController(itemModel));
        }

        public event Action OnItemsChanged; 
        public IReadOnlyCollection<ItemController> Items => items;
        public float CostModifier => Model.CostModifier;

        InventoryDataModel Model { get; }

        readonly HashSet<ItemController> items = new HashSet<ItemController>();

        public void AddItem(ItemController item)
        {
            item.CostModifier = CostModifier; 
            items.Add(item);
            OnItemsChanged?.Invoke();
        }

        public void RemoveItem(ItemController item)
        {
            items.Remove(item);
            OnItemsChanged?.Invoke();
        }
    }
}