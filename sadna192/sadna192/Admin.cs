namespace sadna192
{
    internal class Admin : Member
    {
        public Admin() { } 
        public Admin(string name, string password) : base(name, password)
        {
        }

        public override bool isAdmin()
        {
            return true;
        }

        public override string ToString()
        {
            return base.ToString() + "-Admin";
        }
    }
}