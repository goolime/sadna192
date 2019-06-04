using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using sadna192;

namespace WebApplication1.Models
{
    public class ProductInStoreViewModel
    {
        public string Name { get; set; }
        public string StoreName { get; set; }
        public EditProductViewModel EditProduct { get; set; }
        public AddToCartViewModel AddToCart { get; set; }
    }

    public class EditProductViewModel
    {
        public string ProductCategory { get; set; }
        public double ProductPrice { get; set; } = -1;
        public int ProductAmount { get; set; } = -1;
        public string NewName { get; set; }
    }

    public class AddToCartViewModel
    {
        public int Amount { get; set; }
    }

    public class StoreViewModel
    {
        public string StoreName { get; set; }

    }

    public class OwnerViewModel
    {
        public string Name { get; set; }

        public void SetName(string name)
        {
            Name = name;
        }

    }

    public class AddManagerViewModel
    {
        [Required]
        public string Name { get; set; }
        public bool AddPermission { get; set; }
        public bool RemovePermission { get; set; }
        public bool UpdatePermission { get; set; }
    }

    public class AddProductViewModel
    {
        public string ProductCategory { get; set; }
        public double ProductPrice { get; set; } = -1;
        public Discount Discount;
        public Policy ProductPolicy;

        [Required]
        public string ProductName { get; set; }
        public int ProductAmount { get; set; } = -1;
    }

    public class Store_AddManagerViewModel
    {
        public OwnerViewModel O { get; set; }
        public StoreViewModel S { get; set; }
        public AddManagerViewModel AM { get; set; }
        public AddProductViewModel AP { get; set; }
        public AddDiscountViewModel AD { get; set; }
        public AddPolicyViewModel APolicy { get; set; }


        public string DeleteOwnerConfig(string owner, string store)
        {
            O = new OwnerViewModel() { Name = owner };
            S = new StoreViewModel() { StoreName = store };
            return "";
        }

        public string DeleteManagerConfig(string owner, string store)
        {
            return DeleteOwnerConfig(owner, store);
        }
    }

    public class AddDiscountViewModel
    {
        public bool IsProductsDiscount { get; set; }
        public string LogicConnection { get; set; }
        public List<DiscountViewModel> Discounts { get; set; }

        public TimeSpanViewModel  TimeSpan { get; set; }

        public bool IsStoreDiscount { get; set; }
        public int NumberOfDiscounts { get; set; }
        public bool DiscountVisible { get; set; }
    }

    public class DiscountViewModel
    {
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public int DiscountPercent { get; set; }
    }

    public class AddPolicyViewModel
    {
        public bool IsPolicyVisible { get; set; }

        public bool Immidiate { get; set; }
        public bool Memebr { get; set; }
        public bool IncludeStorePolicy { get; set; }
        public bool IsProductsPolicy { get; set; }

        public bool TotalPolicy { get; set; }
        public string TotalConstraint { get; set; }//Max, Min etc
        public int TotalConstraintValue { get; set; }

        public bool TotalInCartPolicy { get; set; }
        public string TotalInCartConstraint { get; set; }//Max, Min etc
        public int TotalInCartConstraintValue { get; set; }

        public TimeSpanViewModel TimeSpan { get; set; }

        public string LogicConnection { get; set; }
        public List<ProductPolicyViewModel> Policies { get; set; }
        public int NumberOfPolicies { get; set; }

    }

    public class TimeSpanViewModel
    {
        public bool IsTimeLimited { get; set; }
        [DisplayName("Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Start { get; set; }
        [DisplayName("Finish Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Finish { get; set; }
    }

    public class ProductPolicyViewModel
    {
        public string Name { get; set; }
        public string Constraint { get; set; }
        public int Amount { get; set; }

    }


}
