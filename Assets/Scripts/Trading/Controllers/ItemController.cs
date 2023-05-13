using Trading.Data;
using UnityEngine;

namespace Trading.Controllers
{
    public class ItemController
    {
        public ItemController(ItemDataModel model)
        {
            Model = model;
        }

        public float CostModifier { get; set; } = 1;
        public string Name => Model.Name;
        public int Cost => Mathf.CeilToInt(Model.Cost * CostModifier);
        
        ItemDataModel Model { get; }
    }
}