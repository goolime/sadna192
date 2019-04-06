namespace sadna192
{
    class ImmediatePurchase : ProductPurchaseType
    {
        private I_PaymentSystem paymentSystem;
        private Store store;
        private ShopingBasket shopingBasket;

        public override bool purchaseImplementation()
        {

            return true;
        }

        public override bool purchaseImplementation()
        {
            throw new NotImplementedException();
        }
    }
}
