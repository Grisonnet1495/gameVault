namespace gameVaultClassLibrary
{
    public class User
    {
        private int _id { get; set; }
        private string _username { get; set; }
        private string _name { get; set; }
        private string _pseudo { get; set; }

        public User()
        {
            _id = 0;
            _username = "";
            _name = "";
            _pseudo = "";
        }

        public User(int id, string username,string name, string pseudo)
        {
            _id = id;
            _username = username;
            _name = name;
            _pseudo = pseudo;
        }
    }
}
