using System.Collections.Generic;
using System.Linq;
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
            if (from.FirstOrDefault(item => item.entityIndex == entityIndex)!.data.Count == 0) return;
            
            if (to.FirstOrDefault(item => item.entityIndex == entityIndex) != null)
            {
                var data = new ItemData(Registry.entitiesConfig.cats[entityIndex].health);
                to.FirstOrDefault(item => item.entityIndex == entityIndex)!.Add(data);
            }
            else
            {
                to.Add(new Item(entityIndex));
            }
            from.FirstOrDefault(item => item.entityIndex == entityIndex)!.Remove();
        }

        public void AddToInGameDeck(int newEntityIndex)
        {
            if (inGameDeck.FirstOrDefault(item => item.entityIndex == newEntityIndex) != null)
            {
                var data = new ItemData(Registry.entitiesConfig.cats[newEntityIndex].health);
                inGameDeck.FirstOrDefault(item => item.entityIndex == newEntityIndex)!.Add(data);
            }
            else
            {
                inGameDeck.Add(new Item(newEntityIndex));
            }
        }

        public void RemoveFromInGameDeck(int newEntityIndex)
        {
            if (inGameDeck.FirstOrDefault(item => item.entityIndex == newEntityIndex) != null)
            {
                inGameDeck.FirstOrDefault(item => item.entityIndex == newEntityIndex)!.Remove();
            }
        }
    }
}