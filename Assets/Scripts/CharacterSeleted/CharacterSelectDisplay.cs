using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectDisplay : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private Transform charactersHolder;
    [SerializeField] private CharacterSelectButton selectButtonPrefab;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private Transform introSpawnPoint;

    private GameObject introInstance;
    private List<CharacterSelectButton> characterButtons = new List<CharacterSelectButton>();

/*    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {


        }
    }*/

    private void Start()
    {
        Character[] allCharacters = characterDatabase.GetAllCharacters();

        foreach (var character in allCharacters)
        {
            var selectbuttonInstance = Instantiate(selectButtonPrefab, charactersHolder);
            selectbuttonInstance.SetCharacter(this, character);
            characterButtons.Add(selectbuttonInstance);
        }

    }

    public void Select(Character character)
    {

        characterNameText.text = character.DisplayName;

        if (introInstance != null)
        {
            Destroy(introInstance);
        }

        introInstance = Instantiate(character.IntroPrefab, introSpawnPoint);

    }

}
