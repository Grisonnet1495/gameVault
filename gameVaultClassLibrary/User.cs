using System.Diagnostics;

namespace gameVaultClassLibrary
{
    public class User
    {
        #region Properties
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Pseudo { get; set; }
        public Library UserLibrary { get; set; }
        #endregion

        #region Constructor
        public User()
        {
            Id = 0;
            Username = "";
            Name = "";
            Pseudo = "";
            UserLibrary = new Library();
        }
        public User(int id, string username, string name, string pseudo)
        {
            Id = id;
            Username = username;
            Name = name;
            Pseudo = pseudo;
            UserLibrary = new Library();
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"User: {Id} - {Username} - {Name} - {Pseudo}";
        }
        #endregion
    } 
}
