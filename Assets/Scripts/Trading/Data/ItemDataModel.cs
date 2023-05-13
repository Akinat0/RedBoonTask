namespace Trading.Data
{
    public class ItemDataModel
    {
        public ItemDataModel(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }
        
        public string Name;
        public int Cost;
    }
}