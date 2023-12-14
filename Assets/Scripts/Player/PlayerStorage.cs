using System.Collections.Generic;
using System.Linq;
using Data;
using List;

namespace Player
{
    [System.Serializable]
    public class PlayerStorage
    {
        public List<Item> collection = new List<Item>();
        public List<Item> deck = new List<Item>();
        public List<Item> inGameDeck = new List<Item>();

        /// <summary>
        /// Updates in game deck with the player's deck of the debug deck
        /// </summary>
        public void SwitchToInGameDeck()
        {
            inGameDeck.FillWith(deck);
        }

        /// <summary>
        /// Transfers an entity from the "from" list to the "to" list.
        /// If the entity doesn't exists, exits the function.
        /// </summary>
        public void Transfer(List<Item> from, List<Item> to, int entityIndex)
        {
            // exit if the from list haven't the entity
            if (from.FirstOrDefault(item => item.entityIndex == entityIndex) == null) return;
            
            // exit if the from list have zero entity of the given index
            if (from.FirstOrDefault(item => item.entityIndex == entityIndex)!.Count == 0) return;
            
            if (to.FirstOrDefault(item => item.entityIndex == entityIndex) != null)
            {
                to.FirstOrDefault(item => item.entityIndex == entityIndex)!.Count++;
            }
            else
            {
                collection.Add(new Item(entityIndex, 1));
            }
            from.FirstOrDefault(item => item.entityIndex == entityIndex)!.Count--;
        }

        public void AddToInGameDeck(int newEntityIndex)
        {
            if (inGameDeck.FirstOrDefault(item => item.entityIndex == newEntityIndex) != null)
            {
                inGameDeck.FirstOrDefault(item => item.entityIndex == newEntityIndex)!.Count++;
            }
            else
            {
                collection.Add(new Item(newEntityIndex, 1));
            }
        }
    }
}