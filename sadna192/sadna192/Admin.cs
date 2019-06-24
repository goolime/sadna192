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

        public static Admin GetAdmin(string name, string password)
        {
            Admin admin = DBAccess.getAdminFromDB(name);
            if (admin == null)
            {
                admin = new Admin(name, password);
                if (!DBAccess.SaveToDB(admin))
                    DBAccess.DBerror("could not save admin to DB");      
            }    
            return admin; 
        }
    }
}