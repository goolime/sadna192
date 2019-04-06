using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sadna192
{
    class ImmediatePurchase : ProductPurchaseType
    {
        private I_PaymentSystem paymentSystem;
        private Store store;
        private ShopingBasket shopingBasket;

        public bool purchaseImplementation(shoppingBasket s)
        {
            throw new NotImplementedException(); 
        }

        public override bool purchaseImplementation()
        {
            throw new NotImplementedException();
        }
    }
}
