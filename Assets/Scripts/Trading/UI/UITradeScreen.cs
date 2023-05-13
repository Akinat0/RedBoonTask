using Trading.Controllers;
using UnityEngine;

namespace Trading.UI
{
    public class UITradeScreen : MonoBehaviour
    {
        [SerializeField] UIInventory playerInventoryView;
        [SerializeField] UIInventory traderInventoryView;
        [SerializeField] UIWallet playerWalletView;

        InventoryController PlayerInventory { get; set; }
        InventoryController TraderInventory { get; set; }
        WalletController PlayerWallet { get; set; }

        public void Initialize(InventoryController playerInventory, InventoryController traderInventory, WalletController playerWallet)
        {
            PlayerInventory = playerInventory;
            TraderInventory = traderInventory;
            PlayerWallet = playerWallet;

            playerWalletView.Initialize(playerWallet);
            
            playerInventoryView.Initialize(playerInventory, 
                () => traderInventoryView.SetItemDropZoneActive(true),
                () => traderInventoryView.SetItemDropZoneActive(false), 
                TryBuyItem
                );
            
            traderInventoryView.Initialize(traderInventory, 
                () => playerInventoryView.SetItemDropZoneActive(true),
                () => playerInventoryView.SetItemDropZoneActive(false),
                TrySellItem);
        }

        void TrySellItem(GameObject dropObject)
        {
            if(!dropObject.TryGetComponent(out UIItemView itemView))
                return;

            ItemController item = itemView.ItemController;
            
            //trader doesn't have wallet so we can sell items as mush as we want
            PlayerWallet.Amount += item.Cost;
            PlayerInventory.RemoveItem(item);
            TraderInventory.AddItem(item);
        }
        
        void TryBuyItem(GameObject dropObject)
        {
            if(!dropObject.TryGetComponent(out UIItemView itemView))
                return;

            if (PlayerWallet.TrySpend(itemView.ItemController.Cost))
            {
                PlayerInventory.AddItem(itemView.ItemController);
                TraderInventory.RemoveItem(itemView.ItemController);
            }
        }
    }
}