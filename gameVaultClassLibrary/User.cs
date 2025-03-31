namespace gameVaultClassLibrary
{
    public class User
    {
        #region Properties
        private int _id { get; set; }
        private string _username { get; set; }
        private string _name { get; set; }
        private string _pseudo { get; set; }
        #endregion

        #region constructor
        public User()
        {
            _id = 0;
            _username = "";
            _name = "";
            _pseudo = "";
        }

        public User(int id, string username, string name, string pseudo)
        {
            _id = id;
            _username = username;
            _name = name;
            _pseudo = pseudo;
        }
        #endregion

        #region Getters and Setters
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Pseudo
        {
            get { return _pseudo; }
            set { _pseudo = value; }
        }
        #endregion
    }
}
