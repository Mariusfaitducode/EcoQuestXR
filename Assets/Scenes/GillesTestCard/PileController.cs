using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class PileController : MonoBehaviour
{
    public List<Card> CardsPile = new List<Card>();

    public float nb_attribute_card = 2;

    public string nomFichierCSV = "cards.csv";


    public GameObject displayPlace;

    public int nb_card_to_draw = 5; 
    
    //public List<> area_list = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        // ReadDataCSV();
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     // V�rifie si le bouton de la souris droit est enfonc�
    //     if (Input.GetMouseButtonDown(1))
    //     {
    //         displayPlace.GetComponent<DisplayCardController>().DisplayCards(Draw(nb_card_to_draw));
    //     }
    // }
    //
    //
    //
    // // M�thode pour instancier une carte avec les donn�es fournies
    // public Card InstanciateCard(int id, float price)
    // {
    //     Card newCard = new Card();
    //     newCard.id = id; // D�finissez l'ID de la carte
    //     // newCard.price = price; // D�finissez le prix de la carte
    //
    //     // Vous pouvez ajouter d'autres initialisations ici selon les besoins de votre jeu
    //
    //     return newCard;
    // }


    // public void ReadDataCSV()
    // {
    //
    //     string cheminFichierCSV = Path.Combine(Application.dataPath, nomFichierCSV);
    //
    //     // V�rifiez si le chemin du fichier CSV est valide
    //     if (!File.Exists(cheminFichierCSV))
    //     {
    //         Debug.LogError("Le fichier CSV n'existe pas � l'emplacement sp�cifi� : " + cheminFichierCSV);
    //         return;
    //     }
    //
    //     // Lire le fichier CSV ligne par ligne
    //     string[] lignes = File.ReadAllLines(cheminFichierCSV);
    //
    //     // Parcourez chaque ligne du fichier CSV (en sautant la premi�re ligne si elle contient des en-t�tes)
    //     for (int i = 1; i < lignes.Length; i++)
    //     {
    //         string ligne = lignes[i];
    //         string[] colonnes = ligne.Split(';');
    //
    //         // V�rifiez si le nombre de colonnes est correct (vous pouvez ajuster cela en fonction de la structure de votre CSV)
    //         if (colonnes.Length != nb_attribute_card)
    //         {
    //             Debug.LogWarning("La ligne " + (i + 1) + " du fichier CSV ne contient pas le bon nombre de colonnes.");
    //             continue; // Passez � la prochaine ligne
    //         }
    //
    //         // Convertissez les donn�es en types appropri�s
    //         int id;
    //         float price;
    //         if (!int.TryParse(colonnes[0], out id) || !float.TryParse(colonnes[1], out price))
    //         {
    //             Debug.LogWarning("Impossible de convertir les donn�es sur la ligne " + (i + 1) + " du fichier CSV.");
    //             continue; // Passez � la prochaine ligne
    //         }
    //
    //         // Cr�ez un objet CarteData et ajoutez-le � la liste des donn�es de carte
    //         Card card = InstanciateCard(id, price);
    //         CardsPile.Add(card);
    //     }
    //
    // }
    //
    //
    //
    // // fonction qui tire des cartes dans la liste
    // private List<Card> Draw(int quantity)
    // {
    //
    //     List<Card> cartesTirees = new List<Card>();
    //
    //     // Tire le nombre sp�cifi� de cartes
    //     for (int i = 0; i < quantity; i++)
    //     {
    //         // V�rifie si la pile de cartes est vide
    //         if (CardsPile.Count == 0)
    //         {
    //             Debug.LogWarning("La pile de cartes est vide !");
    //             break; // Sort de la boucle si la pile est vide
    //         }
    //
    //         // G�n�re un index al�atoire dans la plage des indices de la liste
    //         int randomIndex = Random.Range(0, CardsPile.Count);
    //
    //         // Ajoute la carte pioch�e � la liste de cartes tir�es
    //         cartesTirees.Add(CardsPile[randomIndex]);
    //
    //         // Retire la carte pioch�e de la pile de cartes
    //         CardsPile.RemoveAt(randomIndex);
    //     }
    //
    //     return cartesTirees;
    //
    // }



}
