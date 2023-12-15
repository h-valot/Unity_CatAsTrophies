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
            inGameDeck = deck.ToList();
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
            if (GetCount(entityIndex, from) == 0) return;

            if (to.FirstOrDefault(item => item.entityIndex == entityIndex) != null)
            {
                to.FirstOrDefault(item => item.entityIndex == entityIndex)!.amount++;
            }
            else
            {
                var newItem = new Item(entityIndex);
                newItem.Add(Registry.entitiesConfig.cats[entityIndex].health);
                to.Add(newItem);
            }
            
            to.Add(new Item(entityIndex, Registry.entitiesConfig.cats[entityIndex].health));
            from.Remove(from.FirstOrDefault(item => item.entityIndex == entityIndex));
        }

        /// <summary>
        /// Add the given entity into the in-game deck
        /// </summary>
        public void AddToInGameDeck(int newEntityIndex)
        {
            inGameDeck.Add(new Item(newEntityIndex, Registry.entitiesConfig.cats[newEntityIndex].health));
        }

        /// <summary>
        /// Set the given entity from the in-game deck to dead
        /// </summary>
        public void SetDead(int newEntityIndex)
        {
            // exit, if there is no item with this id
            if (inGameDeck.FirstOrDefault(item => item.entityIndex == newEntityIndex) == null) return;
            
            // exit, if the current cat is already dead
            if (inGameDeck.FirstOrDefault(item => item.entityIndex == newEntityIndex)!.isDead) return;

            inGameDeck.FirstOrDefault(item => item.entityIndex == newEntityIndex)!.isDead = true;
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
                        SetDead(cat.catType);
                        continue;
                    }
                    
                    // synchronize data
                    item.health = cat.health;
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
                item.health = Registry.entitiesConfig.cats[item.entityIndex].health;
                item.isDead = false;
            }
        }

        public int GetCount(int entityIndex, List<Item> items)
        {
            var output = 0;
            foreach (var item in items)
            {
                // continue, if the index isn't the same
                if (item.entityIndex != entityIndex) continue;

                output++;
            }
            return output;
        }
    }
}