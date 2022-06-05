using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solitaire : MonoBehaviour
{

    [SerializeField]
    private static string[] suits = new string[] {"C", "D", "H", "S"};
    [SerializeField]
    private static string[] values = new string[] {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};

    public List<string> deck;

    
    // Start is called before the first frame update
    void Start()
    {

         PlayCards();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCards() {

        deck = GenerateDeck();
        Shuffle(deck);

        //test the deck
        foreach(string card in deck) {

            Debug.Log(card);

        }

    }


    public static List<string> GenerateDeck() {

        List<string> newDeck = new List<string>();

        foreach (string suit in suits) {

            foreach (string value in values) {

                newDeck.Add(suit + value);

            }

        }

        return newDeck;

    }


    void Shuffle<T>(List<T> list)  {

        System.Random random = new System.Random();
        int lsitCount = list.Count;
        
        while (lsitCount > 1) {

            int randomNextCount = random.Next(lsitCount);
            lsitCount--;
            T temp = list[randomNextCount];
            list[randomNextCount] = list[lsitCount];
            list[lsitCount] = temp;
            
        }

    }

}//class
