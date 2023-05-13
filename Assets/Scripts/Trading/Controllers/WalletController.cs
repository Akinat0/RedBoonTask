using System;
using Trading.Data;

namespace Trading.Controllers
{
    public class WalletController
    {
        public WalletController(WalletDataModel model)
        {
            Model = model;
        }

        public event Action OnAmountChanged;
        
        public int Amount
        {
            get => Model.Amount;
            set
            {
                if(Model.Amount == value)
                    return;

                Model.Amount = value;
                OnAmountChanged?.Invoke();
            }
        }
        
        WalletDataModel Model { get; }

        public bool TrySpend(int amount)
        {
            if (Amount - amount >= 0)
            {
                Amount -= amount;
                return true;
            }

            return false;
        }
    }
}