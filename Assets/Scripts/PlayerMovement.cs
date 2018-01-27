using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class PlayerMovement : MonoBehaviour {
    [Header("Move animation")]
    public AnimationCurve ac;
    public float timeToMove = 0.15f;

    [Space(25)]

    [Header("Ingredients")]
    public Ingredients LightItUp;
    public Ingredients Stire;
    public Ingredients StireReverse;
    public Ingredients Shake;

    [Space(25)]

    public float cooldownMove = 0.5f;
    public GameObject otherPlayer;
    public Transform[] positions;
    public Sprite[] highPositionsSprites = new Sprite[3];
    public Sprite idleSprite;
    public string axisMove;
    public string axisHigh;
    public string validationKey;
    public string lightItUpKey;

    private int _actualPositionIndex = 0;
    private int _actualPositionHigh = 0;
    private float _sign = 0;
    private bool _canMove = true;
    private bool _canMoveHigh = true;
    private SpriteRenderer _sr;
    private int _playerId;

    private float _timestampStireBlock = 0f;
    private float _timestampShakeBlock = 0f;

    private bool _isInShakeStance = false;
    // U : Up / D : Down
    private string _shakeCombination = "UDUDUD";
    private string _actualShakeCombination = "";
    private bool _detectShakeValue = true;

    private bool _isInStireStance = false;
    private string _stireCombination = "XYBA";
    private string _stireReverseCombination = "XABY";
    private string _actualStireCombination = "";

    private bool _canLightItUp = true;

    private bool _defaultFlip;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _sign = Mathf.Sign(positions[1].position.x - positions[0].position.x);
        _playerId = _sign == -1 ? 1 : 0;

        _defaultFlip = _sr.flipX;

        LevelManager.Manager.joueur1Ended += OnRecipeEnd;
    }

    void OnRecipeEnd(Recettes r)
    {
        print(r.name);
    }

    // TODO : Block player in shake or stire stance if actual recipe is ordered and next ingredient is a stire or shake

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
        else if(Input.GetAxisRaw(axisMove) > -0.5 && Input.GetAxisRaw(axisMove) < 0.5 && !_canMove)
        {
            _canMove = true;
        }
        
        if (Input.GetAxisRaw(axisHigh) > 0.5 && _actualPositionIndex != 0 && _canMoveHigh)
        {
            StartCoroutine(MoveHigh(-1));
        }
        else if (Input.GetAxisRaw(axisHigh) < -0.5 && _actualPositionIndex != 0 && _canMoveHigh)
        {
            StartCoroutine(MoveHigh(1));
        }
        else if(Input.GetAxisRaw(axisHigh) > -0.5 && Input.GetAxisRaw(axisHigh) < 0.5 && !_canMoveHigh)
        {
            _canMoveHigh = true;
        }

        if(Input.GetButtonDown(validationKey) && _actualPositionIndex != 0)
        {
            print("Take a bottle");
            AddIngredient();
            EventManager.TriggerEvent("ServeDrink", new { });
        }
        else if(Input.GetButtonDown(validationKey) && _actualPositionIndex != 0)
        {
            print("Validation time");
            LevelManager.Manager.Valider(_playerId);
        }

        if (LevelManager.Manager.GetNextIngredient(_playerId) != null)
        {
            string ingredientName = LevelManager.Manager.GetNextIngredient(_playerId).name;
            
            if ((ingredientName == "stire" || ingredientName == "reverse_stire") && _actualPositionIndex == 0 && !_isInStireStance)
            {
                print("Go to stire stance");
                _timestampStireBlock = Time.time;
                _isInStireStance = true;
            }

            if(ingredientName == "shake" && _actualPositionIndex == 0 && !_isInShakeStance)
            {
                print("Go to shake stance");
                _timestampShakeBlock = Time.time;
                _isInShakeStance = true;
            }
        }

        // Check if we're the first to be in shake stance
        if (_timestampShakeBlock >= otherPlayer.GetComponent<PlayerMovement>()._timestampShakeBlock && _isInShakeStance)
        {
            _canMoveHigh = false;
            _canMove = false;

            // Detect shake movement, the player must go between 0.5 and -0.5 between each movement
            if (Input.GetAxisRaw("DPad_YAxis_1") > 0.5 && _detectShakeValue)
            {
                _actualShakeCombination += "U";
                _detectShakeValue = false;
            }
            else if (Input.GetAxisRaw("DPad_YAxis_1") < -0.5 && _detectShakeValue)
            {
                _actualShakeCombination += "D";
                _detectShakeValue = false;
            }
            else if (!_detectShakeValue && Input.GetAxisRaw("DPad_YAxis_1") > -0.5 && Input.GetAxisRaw("DPad_YAxis_1") < 0.5)
            {
                _detectShakeValue = true;
            }
            
            // Check if a shake movement has been detected
            if (_shakeCombination.Contains(_actualShakeCombination))
            {
                if (_shakeCombination == _actualShakeCombination)
                {
                    // Shake success
                    print("Shake is okay !");
                    EventManager.TriggerEvent("ShakeDrink", new { });
                    SuccessShake(false);
                    _actualShakeCombination = "";
                    _isInShakeStance = false;
                }
            }
            else
            {
                // Not good
                print("Shake fail");
                FailMove();
                _actualShakeCombination = "";
                _isInShakeStance = false;
            }
        }

        // Check if we're the first to be in stire stance
        if (_timestampStireBlock >= otherPlayer.GetComponent<PlayerMovement>()._timestampStireBlock && _isInStireStance)
        {
            _canMoveHigh = false;
            _canMove = false;

            // Detect stire inputs
            if (Input.GetButtonDown("A_1") )
            {
                _actualStireCombination += "A";
            }
            else if (Input.GetButtonDown("B_1"))
            {
                _actualStireCombination += "B";
            }
            else if (Input.GetButtonDown("Y_1"))
            {
                _actualStireCombination += "Y";
            }
            else if (Input.GetButtonDown("X_1"))
            {
                _actualStireCombination += "X";
            }

            // Check if a stire movement has been detected
            if (_stireCombination.StartsWith(_actualStireCombination))
            {
                if(_stireCombination == _actualStireCombination)
                {
                    // Stire success
                    print("Stire is okay !");
                    EventManager.TriggerEvent("StireDrink", new { });
                    SuccessStire(false);
                    _actualStireCombination = "";
                    _isInStireStance = false;
                }
            }
            else if (_stireReverseCombination.StartsWith(_actualStireCombination))
            {
                if (_stireReverseCombination == _actualStireCombination)
                {
                    // Reverse stire success
                    print("Reverse stire is okay !");
                    EventManager.TriggerEvent("StireDrink", new { });
                    SuccessStire(true);
                    _actualStireCombination = "";
                    _isInStireStance = false;
                }
            }
            else
            {
                // Not good
                print("Stire fail");
                FailMove();
                _actualStireCombination = "";
                _isInStireStance = false;
            }
        }

        // Detect light it up, and add ingredient
        if (Input.GetAxisRaw(lightItUpKey) > 0.5 && _actualPositionIndex == 0 && _canLightItUp)
        {
            print("LIGHT IT UP");
            EventManager.TriggerEvent("LitDrink", new { });
            LevelManager.Manager.AddIngredient(_playerId, LightItUp);
            StartCoroutine(LightCooldown());
        }
    }

    private IEnumerator LightCooldown()
    {
        _canLightItUp = false;
        yield return new WaitForSeconds(1f);
        _canLightItUp = true;
    }

    private void SuccessStire(bool reverse)
    {
        if(reverse)
        {
            LevelManager.Manager.AddIngredient(_playerId, StireReverse);
        }
        else
        {
            LevelManager.Manager.AddIngredient(_playerId, Stire);
        }
    }

    private void SuccessShake(bool reverse)
    {
        LevelManager.Manager.AddIngredient(_playerId, Shake);
    }

    private void FailMove()
    {
        LevelManager.Manager.AddIngredient(_playerId, null);
    }

    private void AddIngredient()
    {
        Ingredients ingredient = null;

        float x = positions[_actualPositionIndex].position.x;

        print(x);

        // Get bottle's Ingredient at position
        foreach (var bottle in GameObject.FindGameObjectsWithTag("Bottle"))
        {
            if(_actualPositionHigh == bottle.GetComponent<BottleBehavior>().highPosition && x == bottle.transform.position.x)
            {
                print(bottle.GetComponent<BottleBehavior>().ingredient.name);
                ingredient = bottle.GetComponent<BottleBehavior>().ingredient;
                break;
            }
        }

        // bool success = LevelManager.Manager.AddIngredient(_playerId, ingredient);
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

        yield return null;
    }

    public int GetPositionIndex()
    {
        return _actualPositionIndex;
    }

    IEnumerator Move(int direction)
    {
        _canMove = false;
        
        int otherIndex = otherPlayer.GetComponent<PlayerMovement>().GetPositionIndex();
        otherIndex = positions.Length - otherIndex;

        switch (direction)
        {
            case 1:
                if (_actualPositionIndex + 1 < positions.Length && _actualPositionIndex + 1 != otherIndex)
                {
                    _actualPositionIndex++;
                }
                break;

            case -1:
                if (_actualPositionIndex - 1 >= 0 && _actualPositionIndex - 1 != otherIndex)
                {
                    _actualPositionIndex--;
                }
                break;
        }
        
        _actualPositionIndex = Mathf.Clamp(_actualPositionIndex, 0, positions.Length - 1);
        _sr.sprite = highPositionsSprites[_actualPositionHigh];
        
        float x = positions[_actualPositionIndex].position.x;
        x = _actualPositionIndex == 0 ? x : x - (110f * _sign);
        Vector3 newPos = new Vector3(x, transform.position.y, transform.position.z);

        if (_actualPositionIndex == 0)
        {
            _sr.sprite = idleSprite;
            _sr.flipX = _defaultFlip;
        }
        else
        {
            _sr.flipX = !_defaultFlip;
        }

        float t = 0f;
        
        while (t < 1)
        {
            t += Time.unscaledDeltaTime / timeToMove;
            transform.position = Vector2.Lerp(transform.position, newPos, ac.Evaluate(t / timeToMove));
            yield return null;
        }

        /*
        yield return new WaitForSeconds(cooldownMove);
        
        _canMove = true;
        */
    }
}
