using Trading.Controllers;
using Trading.Data;
using Trading.UI;
using UnityEngine;

namespace Trading
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] UITradeScreen tradeScreen;
        
        GameDataModel gameDataModel;
        GameController gameController;
        
        
        void Start()
        {
            InitializeDataModels();
            InitializeControllers();
            InitializeTradeScreen();
        }

        void InitializeDataModels()
        {
            gameDataModel = new GameDataModel();
            
            InventoryDataModel playerInventory = new InventoryDataModel();
            playerInventory.ItemDataModels.Add(new ItemDataModel("Armor", 100));
            playerInventory.ItemDataModels.Add(new ItemDataModel("Log", 1));
            playerInventory.ItemDataModels.Add(new ItemDataModel("Statue", 75));
            playerInventory.CostModifier = 0.8f;

            InventoryDataModel traderInventory = new InventoryDataModel();
            traderInventory.ItemDataModels.Add(new ItemDataModel("Sword", 80));
            traderInventory.ItemDataModels.Add(new ItemDataModel("BuffPoison", 40));
            traderInventory.ItemDataModels.Add(new ItemDataModel("HealPoison", 40));
            traderInventory.CostModifier = 1;

            WalletDataModel wallet = new WalletDataModel { Amount = 101 };

            gameDataModel.PlayerInventory = playerInventory;
            gameDataModel.TraderInventory = traderInventory;
            gameDataModel.PlayerWallet = wallet;
        }

        void InitializeControllers()
        {
            gameController = new GameController(gameDataModel);
        }

        void InitializeTradeScreen()
        {
            tradeScreen.Initialize(gameController.PlayerInventory, gameController.TraderInventory, gameController.PlayerWallet);
        }
    }
}