using System.Collections.Generic;
using System.Linq;
using List;

namespace Player
{
    [System.Serializable]
    public class PlayerStorage
    {
        public int tuna, treats;
        
        public List<Item> collection = new List<Item>();
        public List<Item> deck = new List<Item>();
        public List<Item> inGameDeck = new List<Item>();
        
        /// <summary>
        /// Updates in game deck with the player's deck of the debug deck
        /// </summary>
        public void SwitchToInGameDeck()
        {
            inGameDeck = deck.Copy();
        }

        /// <summary>
        /// Transfers an entity from the "from" list to the "to" list.
        /// If the entity doesn't exists, exits the function.
        /// </summary>
        public void Transfer(List<Item> from, List<Item> to, int entityIndex)
        {
            // exit, if the from list haven't the entity
            if (from.FirstOrDefault(item => item.entityIndex == entityIndex) == null) return;
            
            // exit, if the from list have zero entity of the given index
            if (from.FirstOrDefault(item => item.entityIndex == entityIndex)!.cats.Count == 0) return;

            if (to.FirstOrDefault(item => item.entityIndex == entityIndex) != null)
            {
                var newData = new CatData(entityIndex, Registry.entitiesConfig.cats[entityIndex].health);
                to.FirstOrDefault(item => item.entityIndex == entityIndex)!.Add(newData);
            }
            else
            {
                to.Add(new Item(entityIndex));
            }
            from.FirstOrDefault(item => item.entityIndex == entityIndex)!.Remove();
        }

        /// <summary>
        /// Add the given entity into the in-game deck
        /// </summary>
        public void AddToInGameDeck(int entityIndex)
        {
            if (inGameDeck.FirstOrDefault(item => item.entityIndex == entityIndex) != null)
            {
                var newData = new CatData(entityIndex, Registry.entitiesConfig.cats[entityIndex].health);
                inGameDeck.FirstOrDefault(item => item.entityIndex == entityIndex)!.Add(newData);
            }
            else
            {
                inGameDeck.Add(new Item(entityIndex));
            }
        }

        /// <summary>
        /// Synchronize battle cat's data into the player's in-game deck
        /// </summary>
        public void SynchronizeCatData(List<Cat> cats)
        {
            foreach (var item in inGameDeck)
            {
                var index = 0;
                foreach (var cat in cats)
                {
                    // continue, if types aren't the same
                    if (cat.catType != item.entityIndex) continue;

                    // continue, if the cat is dead
                    if (cat.state == CatState.IN_GRAVEYARD)
                    {
                        item.cats[index].isDead = true;
                        continue;
                    }
                
                    // synchronize data
                    item.cats[index].health = cat.health;
                    index++;
                }
            }
        }

        /// <summary>
        /// Resets all cats' data from collection, deck and in-game deck
        /// </summary>
        public void ResetAllData()
        {
            ResetData(collection);
            ResetData(deck);
            ResetData(inGameDeck);
        }

        private void ResetData(List<Item> items)
        {
            foreach (var item in items)
            {
                foreach (var data in item.cats)
                {
                    data.health = Registry.entitiesConfig.cats[item.entityIndex].health;
                    data.isDead = false;
                }
            }
        }
    }
}