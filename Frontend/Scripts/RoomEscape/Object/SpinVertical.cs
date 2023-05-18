using UnityEngine;

public class SpinVertical : MonoBehaviour
{
    [SerializeField] private NumberlockController _numberlockController = null;
     
    [Header("Combination Settings")]
    public int spinnerNumber;
    private int spinnerLimit;
     
    [Header("Padlock Row")]
    [SerializeField] private PadlockRow _row = PadlockRow.row1;

    [HideInInspector]
    public bool _isSelect;

    private enum PadlockRow { row1, row2, row3, row4 }
    float tb = 0.5f;

    private void Awake()
    {
        spinnerNumber = 0;
        spinnerLimit = 9;
    }

    void OnMouseDown()
    {
    }

    void Rotate()
    {
        if (spinnerNumber <= spinnerLimit - 1)
        {
            spinnerNumber++;
        }
        else
        {
            spinnerNumber = 0;
        }

        switch (_row)
        {
            case PadlockRow.row1:
                _numberlockController._numberArray[0] = spinnerNumber;
                _numberlockController.CheckCombination();
                break;
            case PadlockRow.row2:
                _numberlockController._numberArray[1] = spinnerNumber;
                _numberlockController.CheckCombination();
                break;
            case PadlockRow.row3:
                _numberlockController._numberArray[2] = spinnerNumber;
                _numberlockController.CheckCombination();
                break;
            case PadlockRow.row4:
                _numberlockController._numberArray[3] = spinnerNumber;
                _numberlockController.CheckCombination();
                break;
        }
    }

    public void BlinkingMaterial()
    {
        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        
        if (_isSelect)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(Color.clear, Color.yellow, Mathf.PingPong(Time.time, tb)));
        }
        if (_isSelect == false)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.clear);
        }

    }
}
