using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Card = System.Object;//******************WRONG. FIX EVENTUALLY*******************************************************

public class CardDeck {

    private List<Card> _deck = new List<Card>(); //List that will store cards

    public List<Card> getCards() //returns the list as a whole
    {
        return _deck;
    }

    public void AddCard(Card card) //adds input card to the "bottom" of the deck
    {
        _deck.Add(card);
    }
    public void AddCards(List<Card> cards) //adds multiple cards to the "bottom" of the deck
    {
        _deck.AddRange(cards);
    }
    public void AddCards(CardDeck deck) //adds two decks together. Input is put on the "bottom" of the original deck. (For shuffling, add discard pile to original deck)
    {
        _deck.AddRange(deck.getCards());
    }

    public void Shuffle() //creates a shuffled deck and copied deck, takes random cards from copied and adds them to shuffled. copied is then copied to _deck
    {
        List<Card> shuffled = new List<Card>(_deck);
        List<Card> copy = new List<Card>(_deck);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int j=0;
            int size = copy.Count;
            j = Random.Range(0, size);
            shuffled[i] = copy[j];
            copy.RemoveAt(j);
        }
        _deck = shuffled;
            

    }

    public Card TakeCard(Card card) //Retrieves a specific card from a deck.
    {
        if (_deck.Count == 0)
            return null; // the deck is depleted

        // 
		if (_deck.Remove(card)) { //Only retrieves the card if it is in the deck. 
			return card;
		} else {
			return null;
		}
    }
    public Card TakeCard() //take the top card off the deck
    {
        if (_deck.Count == 0) {
            Debug.Log("drawing more cards than there are in deck");
            return null; // the deck is depleted
        }

        //creates a temporary card that is the card, then removes the card from the deck. Returns temporary card
        Card tempCard = _deck[0];
        _deck.RemoveAt(0);
        return tempCard;
    }


    public List<Card> Look(int amount) //peek at the "top" cards of a deck
    {
        return _deck.GetRange(0, amount - 1);
    }

    public List<Card> TakeCards(int amount) // take mulitple cards from the "top" of a deck as a list
    {
        if (_deck.Count < amount)
        {
            Debug.Log("drawing more cards than there are in deck");
            return null;
        }

        // ditto to TakeCard()
        List<Card> tempList = _deck.GetRange(0, amount - 1);
        _deck.RemoveRange(0,amount-1);
        return tempList;
    }
    public List<Card> InsertCard(Card card, int index) //places a card in a specific place in the deck
    {
        _deck.Insert(index, card);
        return _deck;
    }
    public List<Card> InsertCards(List<Card> cards, int index) //places multiple cards in a specific place in th deck
    {
        _deck.InsertRange(index, cards);
        return _deck;
    }

        /* ...etc... */

}
