using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace gameVaultClassLibrary
{
    public class GameViewModel : INotifyPropertyChanged
    {
        #region Properties
        private Game _currentGame;

        public Game CurrentGame
        {
            get => _currentGame;
            set
            {
                _currentGame = value;
                OnPropertyChanged(nameof(CurrentGame)); // Note : Correct ?
            }
        }
        #endregion

        #region Constructor
        public GameViewModel()
        {
            // Note : Temporary
            CurrentGame = new Game();
        }
        #endregion

        #region Methods
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); 
        #endregion
    }
}
