using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour {

    public enum SuitEnum
    {
        Item = 1,
        Event = 2,
        Omen = 3,
    }

    public class Card
    {
        private SuitEnum _suit;
        private int _rank;

        public SuitEnum Suit { get { return _suit; } }
        public int Rank { get { return _rank; } }

        private GameObject _card;

        public Card(SuitEnum suit, int rank, Vector3 position, Quaternion rotation)
        {
            // to do: validate rank, position, and rotation
            string assetName = string.Format("Card_{0}_{1}", suit, rank);  // Example:  "Card_1_10" would be the Jack of Hearts.
            GameObject asset = GameObject.Find(assetName);
            if (asset == null)
            {
                Debug.LogError("Asset '" + assetName + "' could not be found.");
            }
            else
            {
                _card = Instantiate(asset, position, rotation);
                _suit = suit;
                _rank = rank;
            }
        }
    }

    public class Deck
        
    {
        private List<Card> _deck = new List<Card>();
        private List<Card> _hand = new List<Card>();
        private List<Card> _discardPile = new List<Card>();

        public void Shuffle() //creates a shuffled deck and copied deck, takes random cards from copied and adds them to shuffled. copied is then copied to _deck
        {
            _deck.AddRange(_discardPile);
            int size =_deck.Count;
            List<Card> shuffled = new List<Card>(_deck);
            List<Card> copy = new List<Card>(_deck);
            for (int i = 0; i < size; i++)
            {
                int j=0;
                size = copy.Count;
                j = Random.Range(0, size);
                shuffled[i] = copy[j];
                copy.RemoveAt(j);
            }
            _deck = shuffled;
            

        }

        public Card TakeCard(Card card)
        {
            if (_deck.Count == 0)
                return null; // the deck is depleted

            // take the first card off the deck and add it to the discard pile
            _deck.Remove(card);
            _discardPile.Add(card);
            return card;

        }


        /* ...etc... */
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
