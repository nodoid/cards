using System;

namespace cards
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var cards = new Deck();
            Console.WriteLine("Shuffle cards");
            cards.Shuffle();
            Console.WriteLine("Draw 0");
            var drawzero = cards.Draw();
            Console.WriteLine("Draw 5");
            var drawfive = cards.Draw(5);
            if (drawfive.Count != 0)
            {
                for (int n = 0; n < 5; ++n)
                    Console.WriteLine("Card {0} : {1} of {2}", n, drawfive[n].cardValue, drawfive[n].cardSuit);
            }
            Console.WriteLine("Draw 10 sorted");
            var drawtensorted = cards.DrawSorted(10);
            if (drawtensorted.Count != 0)
            {
                for (int n = 0; n < 10; ++n)
                    Console.WriteLine("Card {0} : {1} of {2}", n, drawtensorted[n].cardValue, drawtensorted[n].cardSuit);
            }
        }
    }
}
