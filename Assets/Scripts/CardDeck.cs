using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck {

    public class Card
    {
        private List<CSL.Token> tokens = new List<CSL.Token>();

        public Card (List<CSL.Token> tokens)
        {
            this.tokens = tokens;
        }
        public List<CSL.Token> GetTokens()
        {
            return tokens;

        }
    }


    private List<Card> _deck = new List<Card>();

    public List<Card> getCards()
    {
        return _deck;
    }

    public void AddCard(Card card)
    {
        _deck.Add(card);
    }
    public void AddCards(List<Card> cards)
    {
        _deck.AddRange(cards);
    }
    public void AddCards(CardDeck deck)
    {
        _deck.AddRange(deck.getCards());
    }

    public void Shuffle() //creates a shuffled deck and copied deck, takes random cards from copied and adds them to shuffled. copied is then copied to _deck
    {
        int size = _deck.Count;
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
        return card;

    }
    public Card TakeCard()
    {
        if (_deck.Count == 0) {
            Debug.Log("drawing more cards than there are in deck");
        return null; // the deck is depleted
        }

        // take the first card off the deck and add it to the discard pile
        Card tempCard = _deck[0];
        _deck.RemoveAt(0);
        return tempCard;
    }
    public List<Card> Look(int amount)
    {
        return _deck.GetRange(0, amount - 1);
    }
    public List<Card> TakeCards(int amount)
    {
        if (_deck.Count < amount)
        {
            Debug.Log("drawing more cards than there are in deck");
            return null;
        }

        // take the first card off the deck and add it to the discard pile
        List<Card> tempList = _deck.GetRange(0, amount - 1);
        _deck.RemoveRange(0,amount-1);
        return tempList;
    }

        /* ...etc... */

}
