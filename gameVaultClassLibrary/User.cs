using System.Diagnostics;

namespace gameVaultClassLibrary
{
    public class User
    {
        #region Properties
        public string Pseudo { get; set; }
        public string Password { get; set; }
        public Library Library { get; set; }


        #endregion

        #region Constructor
        public User()
        {
            Pseudo = "";
            Password = "";
            Library = new Library();
        }
        public User(string pseudo, string password, Library library)
        {
            Pseudo = pseudo;
            Password = password;
            Library = library;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"User: " +
                "{" +
                $"{Pseudo}" +
                $"{Password}" +
                $"{Library.LibraryName}" +
                "}";
        }
        #endregion
    } 
}
