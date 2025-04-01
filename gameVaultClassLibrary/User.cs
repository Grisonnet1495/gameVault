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
        public User(int id = 0, string username = "", string name = "", string pseudo = "")
        {
            Id = id;
            Username = username;
            Name = name;
            Pseudo = pseudo;

            //create a library for the user with the user's pseudo
            string LibraryName = $"Librairie de {Pseudo}";
            UserLibrary = new Library(LibraryName);  // a user has a library
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
