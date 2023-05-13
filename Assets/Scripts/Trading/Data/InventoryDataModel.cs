using System.Collections.Generic;

namespace Trading.Data
{
    public class InventoryDataModel
    {
        public float CostModifier = 1;
        
        public List<ItemDataModel> ItemDataModels = new List<ItemDataModel>();
    }
}