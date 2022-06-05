using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Solitaire : MonoBehaviour
{

    [SerializeField]
    private Sprite[] cardfaces;

    [SerializeField]
    private GameObject deckButton;

    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    public GameObject[] bottomCardPossitions;
    [SerializeField]
    public GameObject[] topCardPossitions;

    [SerializeField]
    private static string[] suits = new string[] {"C", "D", "H", "S"};
    [SerializeField]
    private static string[] values = new string[] {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};

    public List<string>[] bottomRow;
    public List<string>[] topRow;

    public List<string> tripsOnDisplay = new List<string>();
    private List<List<string>> deckTrips = new List<List<string>>();


    private List<string> bottomCardRow1 = new List<string>();
    private List<string> bottomCardRow2 = new List<string>();
    private List<string> bottomCardRow3 = new List<string>();
    private List<string> bottomCardRow4 = new List<string>();
    private List<string> bottomCardRow5 = new List<string>();
    private List<string> bottomCardRow6 = new List<string>();
    private List<string> bottomCardRow7 = new List<string>();
  

    public List<string> deck;
    public List<string> discardPile = new List<string>();

    private int deckLocation;
    private int trips;
    private int tripsRemainder;

    private string CARD_TAG = "Card";

    
    // Start is called before the first frame update
    void Start()
    {
        bottomRow = new List<string>[] {bottomCardRow1, bottomCardRow2, bottomCardRow3, bottomCardRow4, bottomCardRow5, bottomCardRow6, bottomCardRow7};
        PlayCards();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCards() {

        deck = GenerateDeck();
        Shuffle(deck);
        SolitaireSort();
        StartCoroutine(SolitaireDeal());
        SortDeckIntoTrips();

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

    IEnumerator SolitaireDeal() {

        for (int counter = 0; counter < 7; counter++) {

            float yOffset = 0;
            float zOffset = 0.03f;

            foreach (string card in bottomRow[counter]) {

                yield return new WaitForSeconds(0.03f);

                GameObject newCard = Instantiate(cardPrefab, new Vector3 (bottomCardPossitions[counter].transform.position.x, 
                    bottomCardPossitions[counter].transform.position.y - yOffset, bottomCardPossitions[counter].transform.position.z - zOffset), Quaternion.identity, 
                    bottomCardPossitions[counter].transform);

                newCard.name = card;
                newCard.GetComponent<Selectable>().row = counter;

                if ( card == bottomRow[counter][bottomRow[counter].Count - 1]) {

                    newCard.GetComponent<Selectable>().faceUp = true;

                }
                

                yOffset += 0.3f;
                zOffset += 0.03f;
                discardPile.Add(card);
 
            }
            
        }

        foreach (string card in discardPile) {

            if(deck.Contains(card)) {

                deck.Remove(card);

            }
            
        }
        discardPile.Clear();


    }

    public Sprite[] GetCardFaces() {

        return this.cardfaces;

    }

    void SolitaireSort() {

        for (int outerCounter = 0; outerCounter < 7; outerCounter++) {

            for (int innerCounter = outerCounter; innerCounter < 7; innerCounter++) {

                bottomRow[innerCounter].Add(deck.Last<string>());
                deck.RemoveAt(deck.Count - 1);
                
            }
            
        }

    }

    public void SortDeckIntoTrips() {

        trips = deck.Count / 3;
        tripsRemainder = deck.Count % 3;
        deckTrips.Clear();

        int modifier = 0;
        for (int i = 0; i < trips; i++) {

            List<string> myTrips = new List<string>();
            for (int j = 0; j < 3; j++) {

                myTrips.Add(deck[j + modifier]);
                
            }

            deckTrips.Add(myTrips);
            modifier = modifier + 3;

        }

        if (tripsRemainder != 0) {

            List<string> myRemanders = new List<string>();
            modifier = 0;
            for (int k = 0; k < tripsRemainder; k++) {

                myRemanders.Add(deck[deck.Count - tripsRemainder + modifier]);
                modifier++;
                
            }

            deckTrips.Add(myRemanders);
            trips++;

        }

        deckLocation = 0;

    }

    public void DealFromDeck() {

        foreach (Transform child in deckButton.transform) {

            if(child.CompareTag(CARD_TAG)) {

                deck.Remove(child.name);
                discardPile.Add(child.name);
                Destroy(child.gameObject);

            }

        }

        if (deckLocation < trips) {

            //draw 3 cards

            tripsOnDisplay.Clear();
            float xOffset = 2.5f;
            float zOffset = -0.2f;

            if( deckLocation == trips-1 ) {

                     deckButton.GetComponent<SpriteRenderer>().enabled = false;
            }

            foreach (string card in deckTrips[deckLocation]) {

                GameObject newTopCard = Instantiate(cardPrefab, new Vector3(deckButton.transform.position.x + xOffset, deckButton.transform.position.y, deckButton.transform.position.z + zOffset), Quaternion.identity, deckButton.transform);
           
                xOffset = xOffset + 0.5f;
                zOffset = zOffset - 0.2f;

                newTopCard.name = card;
                tripsOnDisplay.Add(card);
                newTopCard.GetComponent<Selectable>().faceUp = true;
                newTopCard.GetComponent<Selectable>().inDeckPile = true;
                
            }
            deckLocation++;

        }
        else {
          
            ReStackTopDeck();

        }

    }

    void ReStackTopDeck() {

        foreach (string card in discardPile) {

            deck.Add(card);
            deckButton.GetComponent<SpriteRenderer>().enabled = true;

        }
        discardPile.Clear();
        SortDeckIntoTrips();
         
    }

    public List<string> GetTripsOnDisplay() {

        return this.tripsOnDisplay;

    }

}//class
