using Trading.Data;

namespace Trading.Controllers
{
    public class GameController
    {
        public GameController(GameDataModel model)
        {
            Model = model;

            PlayerInventory = new InventoryController(model.PlayerInventory);
            TraderInventory = new InventoryController(model.TraderInventory);
            PlayerWallet = new WalletController(model.PlayerWallet);
        }

        GameDataModel Model { get; set; }
        
        public InventoryController PlayerInventory { get; }
        public InventoryController TraderInventory { get; }
        public WalletController PlayerWallet { get; }
    }
}