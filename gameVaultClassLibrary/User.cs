using System.Diagnostics;

namespace gameVaultClassLibrary
{
    public class User
    {
        #region Properties
        public string Pseudo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Library Library { get; set; }


        #endregion

        #region Constructor
        public User()
        {
            FirstName = "";
            LastName = "";
            Pseudo = "";
            Library = new Library();
        }
        public User(string pseudo, string firstName, string lastName, Library library)
        {
            Pseudo = pseudo;
            FirstName = firstName;
            LastName = lastName;
            Library = library;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"User: {Pseudo} - {FirstName} - {LastName}";
        }
        #endregion
    } 
}
