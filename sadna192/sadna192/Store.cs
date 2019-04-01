using System;

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

        internal void addManager(UserState userState, Member other_user, bool permision_add, bool permission_remove, bool permission_update)
        {
            throw new NotImplementedException();
        }

        internal void addOwner(UserState userState, Member other_user)
        {
            throw new NotImplementedException();
        }
    }
}