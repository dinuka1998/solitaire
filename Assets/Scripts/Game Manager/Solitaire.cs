using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Solitaire : MonoBehaviour
{

    [SerializeField]
    private Sprite[] cardfaces;

    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private GameObject[] bottomCardPossitions;
    [SerializeField]
    private GameObject[] topCardPossitions;

    [SerializeField]
    private static string[] suits = new string[] {"C", "D", "H", "S"};
    [SerializeField]
    private static string[] values = new string[] {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};

    private List<string>[] bottomRow;
    private List<string>[] topRow;


    private List<string> bottomCardRow1 = new List<string>();
    private List<string> bottomCardRow2 = new List<string>();
    private List<string> bottomCardRow3 = new List<string>();
    private List<string> bottomCardRow4 = new List<string>();
    private List<string> bottomCardRow5 = new List<string>();
    private List<string> bottomCardRow6 = new List<string>();
    private List<string> bottomCardRow7 = new List<string>();
  

    public List<string> deck;

    
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

        //test the deck
        foreach(string card in deck) {

            Debug.Log(card);

        }
        SolitaireSort();
        StartCoroutine(SolitaireDeal());

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

                if ( card == bottomRow[counter][bottomRow[counter].Count - 1]) {

                    newCard.GetComponent<Selectable>().faceUp = true;

                }
                

                yOffset += 0.3f;
                zOffset += 0.03f;
 
            }
            
        }


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

}//class
