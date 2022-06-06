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

    private float timer;
    private float doubleClickTime = 0.3f;
    private int clickCount;

    // Start is called before the first frame update
    void Start() {
        
        solitaire = FindObjectOfType<Solitaire>();
        selectedCard = this.gameObject;

    }

    // Update is called once per frame
    void Update() {

        if(clickCount == 1) {

            timer += Time.deltaTime;

        }   
        if(clickCount == 3) {

            timer = 0;
            clickCount = 1;

        }
        if(timer > doubleClickTime) {

            timer = 0;
            clickCount = 0;

        }

        
        GetMouseClick();

    }


    void GetMouseClick() {

        if (Input.GetMouseButtonDown(0)) {

            clickCount++;

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

                    TopRow(hit.collider.gameObject);

                }
                else if (hit.collider.CompareTag(BOTTOM_ROW_TAG)) {

                    BottomRow(hit.collider.gameObject);

                }

            }

        }

    }

    void Deck() {

        solitaire.DealFromDeck();
        selectedCard = this.gameObject;

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

                if(selectedCard == selected) {

                    if(DoubleClick()) {

                        //auto stack

                    }

                }
                selectedCard = selected;

            }  

        }
        else {

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

    }

   void TopRow(GameObject selected) {

       if(selectedCard.CompareTag(CARD_TAG)) {

           if(selectedCard.GetComponent<Selectable>().value == 1) {

               Stack(selected);

           }

       }
        
    }

   void BottomRow(GameObject selected) {

        if(selected.CompareTag(CARD_TAG)) {

            if(selectedCard.GetComponent<Selectable>().value == 13) {

                Stack(selected);

            }

        }
        
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

                        card2Red = false;

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

    bool DoubleClick() {

        if(timer < doubleClickTime && clickCount == 2) {

            print("double Click");
            return true;

        }
        else {

            return false;

        }   

    }

}
