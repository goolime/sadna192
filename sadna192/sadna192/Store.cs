using System;
using System.Collections.Generic;

namespace sadna192
{
    public class Store
    {
        private string name;

        public Store(string name)
        {
            this.name = name;
        }

        internal bool isMe(string name)
        {
            return this.name == name;
        }

        internal void addOwner(Owner owner)
        {
            throw new NotImplementedException();
        }

        internal void addManager(UserState userState, Member other_user, bool permision_add, bool permission_remove, bool permission_update)
        {
            throw new NotImplementedException();
        }

        internal void addOwner(UserState userState, Member other_user)
        {
            throw new NotImplementedException();
        }

        internal IEnumerable<ProductInStore> Search(string name, string category, List<string> keywords, double price_min, double price_max, double store_rank, double product_rank)
        {
            throw new NotImplementedException();
        }
    }
}