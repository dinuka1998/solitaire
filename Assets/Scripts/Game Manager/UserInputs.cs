using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UserInputs : MonoBehaviour
{

    [SerializeField]
    private GameObject selectedCard;
    private Solitaire solitaire; 

    private string DECK_TAG = "Deck";
    private string CARD_TAG = "Card";
    private string TOP_ROW_TAG = "TopRow";
    private string BOTTOM_ROW_TAG = "BottomRow";

    // Start is called before the first frame update
    void Start()
    {
        
        solitaire = FindObjectOfType<Solitaire>();
        selectedCard = this.gameObject;

    }

    // Update is called once per frame
    void Update() {
        
        GetMouseClick();

    }


    void GetMouseClick() {

        if (Input.GetMouseButtonDown(0)) {

            Vector3 mousePositon = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit) {

                if (hit.collider.CompareTag(DECK_TAG)) {

                    Deck();

                }
                else if (hit.collider.CompareTag(CARD_TAG)) {

                    Card(hit.collider.gameObject);

                }
                else if (hit.collider.CompareTag(TOP_ROW_TAG)) {

                    TopRow();

                }
                else if (hit.collider.CompareTag(BOTTOM_ROW_TAG)) {

                    BottomRow();

                }

            }

        }

    }

    void Deck() {

        solitaire.DealFromDeck();

    }

    void Card(GameObject selected) {

        if(!selected.GetComponent<Selectable>().faceUp) {

            if(!blocked(selected)) {

                selected.GetComponent<Selectable>().faceUp = true;
                selectedCard = this.gameObject;

            }

        }
        else if(selected.GetComponent<Selectable>().inDeckPile) {

            if(!blocked(selected)) {

                selectedCard = selected;

            }  

        }


        if (selectedCard == this.gameObject) {

            selectedCard = selected;

        }
        else if ( selectedCard != selected) {

            if (Stackble(selected)) {

                Stack(selected);

            }
            else{

                selectedCard = selected;

            }

           

        }

       
        
    }

   void TopRow() {

        print("clicked on the Top Row");
        
    }

   void BottomRow() {

        print("clicked on the Botom Row");
        
    }

    public GameObject GetSelectedCard() {

        return this.selectedCard;
        
    }

    bool Stackble(GameObject selected) {

        Selectable s1 = selectedCard.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();


        if(!s2.inDeckPile){
            if(s2.top) {

                if(s1.suit == s2.suit || (s1.value == 1 && s2.suit == null)) {

                   if(s1.value == s2.value + 1) {

                      return true;

                  }

              }
              else {

                  return false;

               }

           }
            else {

                if(s1.value == s2.value -1) {

                    bool card1Red = true;
                    bool card2Red = true;        

                    if( s1.suit == "C" || s1.suit == "S") {

                        card1Red = false;

                    }  

                    if( s2.suit == "C" || s2.suit == "S") {

                        card1Red = false;

                    }  

                    if(card1Red == card2Red) {

                        return false;
                    }
                    else { 

                        return true;
                    }

                }

            }
        }

        return false;

    }

    void Stack(GameObject selected) {

        Selectable s1 = selectedCard.GetComponent<Selectable>(); 
        Selectable s2 = selected.GetComponent<Selectable>(); 
        float yOffset  = 0.3f;


        if(s2.top || !s2.top && s1.value == 13 ) {

            yOffset = 0;

        }

        selectedCard.transform.position =  new Vector3(selected.transform.position.x, selected.transform.position.y - yOffset,  selected.transform.position.z -0.01f);
        selectedCard.transform.parent = selected.transform;

        if(s1.inDeckPile) {

            solitaire.tripsOnDisplay.Remove(selectedCard.name);

        }
        else if(s1.top && s2.top && s1.value == 1) {

            solitaire.topCardPossitions[s1.row].GetComponent<Selectable>().value = 0;
            solitaire.topCardPossitions[s1.row].GetComponent<Selectable>().suit = null;

        }
        else if (s1.top) {

              solitaire.topCardPossitions[s1.row].GetComponent<Selectable>().value = s1.value -1;

        }
        else {

            solitaire.bottomRow[s1.row].Remove(selectedCard.name); 

        }
        
        s1.inDeckPile = false;
        s1.row = s2.row;

        if(s2.top)
        {

            solitaire.topCardPossitions[s1.row].GetComponent<Selectable>().value = s1.value;
            solitaire.topCardPossitions[s1.row].GetComponent<Selectable>().suit = s1.suit;
            s1.top = true;

        }
        else{

            s1.top = false;

        }

        selectedCard = this.gameObject;

    }

    bool blocked(GameObject selected) {

        Selectable s2 = selected.GetComponent<Selectable>();
        if(s2.inDeckPile == true) {
            if(s2.name == solitaire.tripsOnDisplay.Last())  {

                return false;

            }
            else {

                return true;

            }


        }
        else {

            if(s2.name == solitaire.bottomRow[s2.row].Last()) {

                return false;

            }
            else {

                return true;

            }

        }

    }

}
