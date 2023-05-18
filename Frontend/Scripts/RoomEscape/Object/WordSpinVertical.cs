using UnityEngine;

public class WordSpinVertical : MonoBehaviour
{
    [SerializeField] private WordlockController _wordlockController = null;

    [Header("Combination Settings")]
    public int spinnerNumber;
    private int spinnerLimit;

    [Header("Wordlock Row")]
    [SerializeField] private WordlockRow _row = WordlockRow.row1;

    [HideInInspector]
    public bool _isSelect;

    private enum WordlockRow { row1, row2, row3 }
    float tb = 0.5f;

    private void Awake()
    {
        spinnerNumber = 65;
        spinnerLimit = 70;
    }

    void OnMouseDown()
    {
        transform.Rotate(transform.rotation.x - 60, 0, 0);
        Rotate();
    }

    void Rotate()
    {
        if (spinnerNumber <= spinnerLimit - 1)
        {
            spinnerNumber++;
        }
        else
        {
            spinnerNumber = 65;
        }

        switch (_row)
        {
            case WordlockRow.row1:
                _wordlockController._charArray[0] = (char)spinnerNumber;
                _wordlockController.CheckCombination();
                break;
            case WordlockRow.row2:
                _wordlockController._charArray[1] = (char)spinnerNumber;
                _wordlockController.CheckCombination();
                break;
            case WordlockRow.row3:
                _wordlockController._charArray[2] = (char)spinnerNumber;
                _wordlockController.CheckCombination();
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
