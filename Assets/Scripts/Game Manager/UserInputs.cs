using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputs : MonoBehaviour
{

    private Solitaire solitaire; 

    private string DECK_TAG = "Deck";
    private string CARD_TAG = "Card";
    private string TOP_ROW_TAG = "TopRow";
    private string BOTTOM_ROW_TAG = "BottomRow";

    // Start is called before the first frame update
    void Start()
    {
        
        solitaire = FindObjectOfType<Solitaire>();

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

                    Card();

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

    void Card() {

        print("clicked on the Card");
        
    }

   void TopRow() {

        print("clicked on the Top Row");
        
    }

   void BottomRow() {

        print("clicked on the Botom Row");
        
    }

}
