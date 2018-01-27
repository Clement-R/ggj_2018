using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float cooldownMove = 0.5f;
    public GameObject otherPlayer;
    public Transform[] positions;
    public Sprite[] highPositionsSprites = new Sprite[3];
    public Sprite idleSprite;
    public string axisMove;
    public string axisHigh;
    public string validationKey;

    private int _actualPositionIndex = 0;
    private int _actualPositionHigh = 0;
    private float _sign = 0;
    private bool _canMove = true;
    private bool _canMoveHigh = true;
    private SpriteRenderer _sr;
    private int _playerId;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _sign = Mathf.Sign(positions[1].position.x - positions[0].position.x);
        _playerId = _sign == -1 ? 0 : 1;

        LevelManager.Manager.joueur1Ended += OnRecipeEnd;
    }

    void OnRecipeEnd(Recettes r)
    {
        print(r.name);
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

        if(Input.GetButtonDown(validationKey) && _actualPositionIndex != 0)
        {
            AddIngredient();
        }
    }

    private void AddIngredient()
    {
        Ingredients ingredient = null;

        // Get bottle's Ingredient at position
        foreach (var bottle in GameObject.FindGameObjectsWithTag("Bottle"))
        {
            if(_actualPositionHigh == bottle.GetComponent<BottleBehavior>().highPosition && transform.position.x == bottle.transform.position.x)
            {
                print(bottle.GetComponent<BottleBehavior>().name);
                ingredient = bottle.GetComponent<BottleBehavior>().ingredient;
                break;
            }
        }

        bool success = LevelManager.Manager.AddIngredient(_playerId, ingredient);
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

        yield return new WaitForSeconds(cooldownMove);

        _canMoveHigh = true;
    }

    IEnumerator Move(int direction)
    {
        _canMove = false;
        
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
        
        if (_actualPositionIndex == 0)
        {
            _sr.sprite = idleSprite;
        }

        yield return new WaitForSeconds(cooldownMove);
        
        _canMove = true;
    }
}
