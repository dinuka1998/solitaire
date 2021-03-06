using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{

    [SerializeField]
    private Sprite cardFace;
    [SerializeField]
    private Sprite cardBack;

    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private Solitaire solitaire;
    private UserInputs userInput;


    // Start is called before the first frame update
    void Start() {

        List<string> deck = Solitaire.GenerateDeck();
        solitaire = FindObjectOfType<Solitaire>();
        userInput = FindObjectOfType<UserInputs>();

        int deckCount = 0;
        foreach (string card in deck) {

            if (this.name == card) {

                cardFace = solitaire.GetCardFaces()[deckCount];
                break;
                
            }
            deckCount ++;
            
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();
        
    }

    // Update is called once per frame
    void Update() {

        if (selectable.faceUp == true) {

            spriteRenderer.sprite = cardFace;

        }
        else {

            spriteRenderer.sprite = cardBack;

        }

        if(userInput.GetSelectedCard()) {

            if (name == userInput.GetSelectedCard().name) {

                spriteRenderer.color = Color.yellow;

            }
            else {

                spriteRenderer.color = Color.white;

            }

        }


       
        
    }
}
