using System.Collections.Generic;
using System.Linq;

namespace Player
{
    [System.Serializable]
    public class Collection
    {
        public List<Item> collection = new List<Item>();
        public List<Item> deck = new List<Item>();
        public List<Item> inGameDeck = new List<Item>();

        /// <summary>
        /// Adds one entity to the player's deck if the collection 
        /// </summary>
        public void AddToDeck(EntityConfig newEntity)
        {
            foreach (var item in collection)
            {
                // continue, if there is no more cat of this type in the collection
                if (item.entity == newEntity && item.count > 0) continue;

                if (deck.FirstOrDefault(item => item.entity == newEntity) != null)
                {
                    deck.FirstOrDefault(item => item.entity == newEntity).count++;
                    item.count--;
                }
                else
                {
                    deck.Add(new Item(newEntity, 1));
                    item.count--;
                }
            }
        }

        /// <summary>
        /// Adds one entity from the deck to the collection
        /// </summary>
        public void RemoveFromDeck(EntityConfig newEntity)
        {
            foreach (var item in deck)
            {
                // continue, if there is no more cat of this type in the collection
                if (item.entity == newEntity && item.count > 0) continue;

                if (collection.FirstOrDefault(item => item.entity == newEntity) != null)
                {
                    collection.FirstOrDefault(item => item.entity == newEntity).count++;
                    item.count--;
                }
                else
                {
                    collection.Add(new Item(newEntity, 1));
                    item.count--;
                }
            }
        }

        /// <summary>
        /// Updates in game deck with the player's deck of the debug deck
        /// </summary>
        public void SwitchToInGameDeck()
        {
            inGameDeck.Clear();
            inGameDeck = Registry.gameSettings.playerDeckDebugMode 
                ? Registry.playerConfig.deck 
                : deck;
        }

        /// <summary>
        /// Add the given entity to the in-game deck
        /// </summary>
        public void AddToInGameDeck(EntityConfig newEntity)
        {
            if (inGameDeck.FirstOrDefault(item => item.entity == newEntity) != null)
            {
                inGameDeck.FirstOrDefault(item => item.entity == newEntity)!.count++;
            }
            else
            {
                inGameDeck.Add(new Item(newEntity, 1));
            }
        }
    }
}