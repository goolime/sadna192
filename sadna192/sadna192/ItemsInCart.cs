using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sadna192
{
    public class ItemsInCart
    {
        public int id { get; set; }
        public int productInStoreRef { get; set; }
        [ForeignKey("productInStoreRef")]
        public ProductInStore productInStore {get; set; } 
        public int amount { get; set; }
        public int shopCartRef { get; set; }
        [ForeignKey ("shopCartRef")]
        public ShoppingCart shopping { get; set; }

        public ProductInStore First;
        public int Second; 

        public ItemsInCart(ProductInStore ps , int amount)
        {
            this.productInStore = ps;
            this.amount = amount;
            this.First = this.productInStore;
            this.Second = this.amount; 
        }
       

    }
}
