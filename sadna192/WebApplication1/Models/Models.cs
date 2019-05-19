using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
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

    public class Store_AddManagerViewModel
    {
        public OwnerViewModel O { get; set; }
        public StoreViewModel S { get; set; }
        public AddManagerViewModel AM { get; set; }
        //public AddManagerViewModel AM { get; set; }

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
}
