using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpecialCommands : MonoBehaviour
{
    private PlayerInventory _playerInv;
    [SerializeField] private List<Interactive> _cans = new List<Interactive>();
    [SerializeField] private Interactive _uvLight;
    private ChalkDrawingPoint[] _chalkPoints;
    [SerializeField] private GameObject _chalkPrefab;
    private SlushieCup _slushieCup;
    [SerializeField] private Interactive _cupObject;
    [SerializeField] private Interactive _cent;
    [SerializeField] private Animator _animFrezeerDoor;
    [SerializeField] private Interactive _femur;

    private void Start()
    {
        _playerInv = FindFirstObjectByType<PlayerInventory>();
        _chalkPoints = FindObjectsByType<ChalkDrawingPoint>(0);
        _slushieCup = FindFirstObjectByType<SlushieCup>();
    }
    #if UNITY_EDITOR
    private void Update()
    {
        Cheats();
    }
    #endif
    private void Cheats()
    {   
        if (Input.GetKey(KeyCode.Z))
        {
            if (Input.GetKeyDown(KeyCode.C)) CansPuzzle();
            if (Input.GetKeyDown(KeyCode.U)) UVLight();
            if (Input.GetKeyDown(KeyCode.M)) SummonDemon();
            if (Input.GetKeyDown(KeyCode.L)) CorrectSlushie();
            if (Input.GetKeyDown(KeyCode.T)) ThermostatCent();
            if (Input.GetKeyDown(KeyCode.O)) OpenFrezeerDoor();
            if (Input.GetKeyDown(KeyCode.F)) FemurPuzzle();
            if (Input.GetKeyDown(KeyCode.R)) ResetGame();
        }
    }
    private void CheatsGive(Interactive interactive) => _playerInv.Add(interactive);

    private void CansPuzzle()
    {
        _playerInv.Clear();
        foreach (Interactive can in _cans) CheatsGive(can);
    }
    private void UVLight() => CheatsGive(_uvLight);
    private void SummonDemon()
    {
        foreach (ChalkDrawingPoint point in _chalkPoints)
        {
            GameObject chalk = Instantiate(_chalkPrefab);
            chalk.transform.position = point.transform.position;
        }
    }
    private void CorrectSlushie()
    {
        CheatsGive(_cupObject);
        _slushieCup.AddFlavour(Flavours.Red);
        _slushieCup.AddFlavour(Flavours.Green);
        _slushieCup.AddFlavour(Flavours.Yellow);
        _slushieCup.AddFlavour(Flavours.Blue);
    }
    private void ThermostatCent() => CheatsGive(_cent);
    private void OpenFrezeerDoor()
    {
        _animFrezeerDoor.SetTrigger("Open");
    }
    private void FemurPuzzle() => CheatsGive(_femur);
    private void ResetGame() => 
    SceneManager.LoadScene("MainSceneTests");
}
