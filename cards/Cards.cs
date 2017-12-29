using System;
using System.Collections.Generic;
using System.Linq;

namespace cards
{
    /// <summary>
    /// Card - a simple container class for the cards
    /// </summary>
    public class Card
    {
        public string cardSuit { get; set; }

        public string cardValue { get; set; }
    }

    /// <summary>
    /// Deck - public methods for dealing with the deck of cards
    /// </summary>
    public class Deck
    {
        List<Dictionary<int, Card>> cardHandMaster;
        List<Dictionary<int, Card>> handInUse;

        /// <summary>
        /// Initializes a new instance of the Deck class.
        /// Creates the top level deck of cards. This is not the deck that the hand is dealt from, but a master hand
        /// </summary>
        public Deck()
        {
            cardHandMaster = new List<Dictionary<int, Card>>();
            var suits = new List<string> { "Clubs", "Diamonds", "Spades", "Hearts" };
            var values = new List<string> { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            int s = 0, v = 0;
            for (int n = 0; n < 52; ++n)
            {
                var card = new Dictionary<int, Card>();
                card.Add(n, new Card { cardValue = values[v], cardSuit = suits[s] });
                cardHandMaster.Add(card);
                v++;
                if (v % 13 == 0 && n != 0)
                {
                    s++;
                    v = 0;
                }
            }
            #if DEBUG
            var cards = new Card();
            for (int n = 0; n < cardHandMaster.Count; ++n)
            {
                cardHandMaster[n].TryGetValue(n, out cards);
                Console.WriteLine("{0} : {1}{2}\n", n, cards.cardSuit, cards.cardValue);
            }
            #endif
        }

        /// <summary>
        /// Shuffle cards - using this method creates a new deck of cards to be shuffled
        /// In - nothing
        /// Out - nothing directly. Creates a new list used for dealing cards from. Master list untouched.
        ///
        /// A similar method could be used to shuffle the existing hand by checking if the handInUse.Count > 0
        /// if it is, use that hand over the master hand, but ensure that the values for the random generator goes
        /// between 0 and handInUse.Count rather than 52.
        /// 
        /// The handInUse.Count != 0 check would also need to be altered.
        /// 
        /// This method uses the standard .NET Random class. While this is fine here, there are many others available
        /// 
        /// </summary>
        public void Shuffle()
        {
            int c = 0;
            var numbers = new List<int>();
            while (c < 52)
            {
                var r = new Random();
                var t = r.Next(0, 52);
                if (!numbers.Contains(t))
                {
                    numbers.Add(t);
                    c++;
                }
            }
            #if DEBUG
            for (int n = 0; n < numbers.Count; ++n)
                Console.WriteLine("{0} : {1}", n, numbers[n]);
            #endif

            if (handInUse == null)
                handInUse = new List<Dictionary<int, Card>>();
            if (handInUse.Count != 0)
                handInUse.Clear();

            var cards = new Card();
            for (int n = 0; n < numbers.Count; ++n)
            {
                var card = new Dictionary<int, Card>();
                if (cardHandMaster[numbers[n]].TryGetValue(numbers[n], out cards))
                {
                    card.Add(n, cards);
                    handInUse.Add(card);
                }
            }

            #if DEBUG
            var cardi = new Card();
            for (int n = 0; n < handInUse.Count; ++n)
            {
                handInUse[n].TryGetValue(n, out cardi);
                Console.WriteLine("{0} : {1}{2}\n", n, cardi.cardSuit, cardi.cardValue);
            }
            #endif
        }

        /// <summary>
        /// Draw the specified howMany number of cards. This is an unsorted list and counts as the master draw method for this
        /// and DrawSorted
        /// 
        /// in - number of cards to draw
        /// out - empty list (error)
        ///     - List<Card> containing the first howMany from the stack
        /// 
        /// Error checks - requesting less than 0 cards (0 == pass) or > 12 cards (should never be needed)
        ///              - number of cards available - requested number < 0
        /// 
        /// Once the number of cards for the list has been created, the handInUse has the 0 -> howMany removed
        /// 
        /// </summary>
        /// <returns>Empty list (error) or List of cards</returns>
        /// <param name="howMany">How many cards to draw.</param>

        public List<Card> Draw(int howMany = 0)
        {
            if (howMany < 0 || howMany > 12)
            {
                if (howMany < 0)
                    Console.WriteLine("You cannot draw a negative value");
                else
                    Console.WriteLine("You cannot draw more than 12 cards");
                return new List<Card>();
            }

            if (handInUse.Count - howMany < 0)
            {
                Console.WriteLine("You cannot draw that many cards");
                return new List<Card>();
            }



            var cardsList = new List<Card>();
            var card = new Card();

            for (int n = 0; n < howMany; ++n)
            {
                var key = handInUse[n].Keys.FirstOrDefault();
                if (handInUse[n].TryGetValue(key, out card))
                {
                    cardsList.Add(card);
                }
            }
            handInUse.RemoveRange(0, howMany);

            return cardsList;
        }

        /// <summary>
        /// Draws the cards and sorts.
        /// Calls the Draw method to grab the cards, sorts using LINQ
        /// Returns empty list if the Draw method generates an error
        /// </summary>
        /// 
        /// <returns>Empty List (error) or the sorted card List</returns>
        /// <param name="howMany">How many cards to draw.</param>
        /// 
        public List<Card> DrawSorted(int howMany)
        {
            var cardsList = new List<Card>();
            cardsList = Draw(howMany);

            if (cardsList.Count == 0)
                return new List<Card>();

            cardsList = cardsList.OrderBy(t => t.cardSuit).ThenBy(t => t.cardValue).ToList();

            #if DEBUG
            for (int n = 0; n < cardsList.Count; ++n)
                Console.WriteLine("{0} {1}", cardsList[n].cardValue, cardsList[n].cardSuit);
            #endif

            return cardsList;
        }
    }
}

