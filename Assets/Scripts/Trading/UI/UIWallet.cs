using TMPro;
using Trading.Controllers;
using UnityEngine;

namespace Trading.UI
{
    public class UIWallet : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;

        public WalletController Wallet { get; private set; }
        
        public void Initialize(WalletController wallet)
        {
            Wallet = wallet;
            Wallet.OnAmountChanged += UpdateText;
            
            UpdateText();
        }

        void OnDestroy()
        {
            Wallet.OnAmountChanged -= UpdateText;
        }

        void UpdateText()
        {
            text.text = Wallet.Amount.ToString();
        }
    }
}