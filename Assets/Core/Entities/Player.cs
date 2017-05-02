using UnityEngine;

namespace Core.Entities
{

    public class Player : Entity
    {

        #region Fields & Properties
        private ulong playerId;
        public string playerName;
        public ulong PlayerId { get { return playerId; } set { playerId = value; } }
        public string PlayerName { get { return playerName; } set { playerName = value; } }
        #endregion
    }
}
