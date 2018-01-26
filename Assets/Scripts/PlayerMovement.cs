using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public GameObject otherPlayer;
    public Transform[] positions;
    public Sprite[] highPositionsSprites = new Sprite[3];
    public string axisMove;
    public string axisHigh;

    private int _actualPositionIndex = 0;
    private int _actualPositionHigh = 0;
    private float _sign = 0;
    private bool _canMove = true;
    private bool _canMoveHigh = true;
    private SpriteRenderer _sr;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _sign = Mathf.Sign(positions[1].position.x - positions[0].position.x);
    }

    void Update ()
    {
        if(Input.GetAxisRaw(axisMove) > 0.5 && _canMove)
        {
            StartCoroutine(Move(1 * (int) _sign));
        }
        else if (Input.GetAxisRaw(axisMove) < -0.5 && _canMove)
        {
            StartCoroutine(Move(-1 * (int)_sign));
        }
        
        if (Input.GetAxisRaw(axisHigh) > 0.5 && _canMoveHigh && _actualPositionIndex != 0)
        {
            StartCoroutine(MoveHigh(-1));
        }
        else if (Input.GetAxisRaw(axisHigh) < -0.5 && _canMoveHigh && _actualPositionIndex != 0)
        {
            StartCoroutine(MoveHigh(1));
        }
    }

    IEnumerator MoveHigh(int direction)
    {
        _canMoveHigh = false;

        switch (direction)
        {
            case 1:
                _actualPositionHigh++;
                break;

            case -1:
                _actualPositionHigh--;
                break;
        }
        
        _actualPositionHigh = Mathf.Clamp(_actualPositionHigh, 0, 2);
        _sr.sprite = highPositionsSprites[_actualPositionHigh];

        print(_actualPositionHigh);

        yield return new WaitForSeconds(1f);

        _canMoveHigh = true;
    }

    IEnumerator Move(int direction)
    {
        _canMove = false;
        
        // TODO : Check if can move to this direction, not blocked by other player

        switch (direction)
        {
            case 1:
                if (_actualPositionIndex + 1 < positions.Length && positions[_actualPositionIndex + 1].position.x != otherPlayer.transform.position.x)
                {
                    _actualPositionIndex++;
                }
                break;

            case -1:
                if (_actualPositionIndex - 1 >= 0 && positions[_actualPositionIndex - 1].position.x != otherPlayer.transform.position.x)
                {
                    _actualPositionIndex--;
                }
                break;
        }
        
        _actualPositionIndex = Mathf.Clamp(_actualPositionIndex, 0, positions.Length - 1);
        transform.position = new Vector3(positions[_actualPositionIndex].position.x, transform.position.y, transform.position.z);

        // TODO : Change for idle sprite
        if (_actualPositionIndex == 0)
        {

        }

        yield return new WaitForSeconds(1f);
        
        _canMove = true;
    }
}
