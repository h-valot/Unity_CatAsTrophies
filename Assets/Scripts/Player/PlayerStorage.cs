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
        /// Adds one entity to the player's deck if the collection 
        /// </summary>
        public void AddToDeck(int newEntityIndex)
        {
            foreach (var item in collection)
            {
                // continue, if there is no more cat of this type in the collection
                if (item.entityIndex == newEntityIndex && item.count > 0) continue;

                if (deck.FirstOrDefault(item => item.entityIndex == newEntityIndex) != null)
                {
                    deck.FirstOrDefault(item => item.entityIndex == newEntityIndex).count++;
                    item.count--;
                }
                else
                {
                    deck.Add(new Item(newEntityIndex, 1));
                    item.count--;
                }
            }
        }

        /// <summary>
        /// Adds one entity from the deck to the collection
        /// </summary>
        public void RemoveFromDeck(int newEntityIndex)
        {
            foreach (var item in deck)
            {
                // continue, if there is no more cat of this type in the collection
                if (item.entityIndex == newEntityIndex && item.count > 0) continue;

                if (collection.FirstOrDefault(item => item.entityIndex == newEntityIndex) != null)
                {
                    collection.FirstOrDefault(item => item.entityIndex == newEntityIndex).count++;
                    item.count--;
                }
                else
                {
                    collection.Add(new Item(newEntityIndex, 1));
                    item.count--;
                }
            }
        }

        /// <summary>
        /// Updates in game deck with the player's deck of the debug deck
        /// </summary>
        public void SwitchToInGameDeck()
        {
            inGameDeck.FillWith(Registry.gameSettings.playerDeckDebugMode ? Registry.playerConfig.deck : deck);
        }

        /// <summary>
        /// Add the given entity to the in-game deck
        /// </summary>
        public void AddToInGameDeck(int newEntityIndex)
        {
            if (inGameDeck.FirstOrDefault(item => item.entityIndex == newEntityIndex) != null)
            {
                inGameDeck.FirstOrDefault(item => item.entityIndex == newEntityIndex)!.count++;
            }
            else
            {
                inGameDeck.Add(new Item(newEntityIndex, 1));
            }
        }
    }
}