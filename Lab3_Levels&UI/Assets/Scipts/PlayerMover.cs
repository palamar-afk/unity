using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    private Transform _transform;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _headCheckerRadius;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private int _maxHP;
    [SerializeField] private int _maxMana;
    [SerializeField] private int _currentHP;
    [SerializeField] private int _currentMana;
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private Transform _headChecker;
    [Header("Animation")] [SerializeField] private Animator _animator;

    [SerializeField] private string _runAnimatorKey;// arrows(right,left)(a,d)
    [SerializeField] private string _jumpAnimatorKey; //space
    [SerializeField] private string _crouchAnimatorKey; // c
    [SerializeField] private string _hurtAnimatorKey; // x
    [SerializeField] private string _attackAnimatorKey; // left button mouse
    [SerializeField] private string _castAnimatorKey; // right button mouse
    [SerializeField] private Slider _hpBar;
    [SerializeField] private Slider _manaBar;

    [Header("UI")]
    [SerializeField] private TMP_Text _coinsAmountText;
    private float _direction;

    private bool _jump;

    private bool _crawl;

    private int _coinsAmount;

    public int CoinsAmount
    {
        get => _coinsAmount;
        set
        {
            _coinsAmount = value;
            _coinsAmountText.text = "Coins: " + value;
        }
    }

    public int HealthPoints
    {
        get => _currentHP;
        set
        {
            if (value >= _maxHP)
            {
                _currentHP = _maxHP;
            }else if (value < 0)
            {
                _currentHP = 0;
            }
            else
            {
                _currentHP = value;
            }

            _hpBar.value = _currentHP;
        }
    }

    public int ManaPoints
    {
        get => _currentMana;
        set
        {
            if (value >= _maxMana)
            {
                _currentMana = _maxMana;
            }
            else if (value < 0)
            {
                _currentMana = 0;
            }
            else
            {
                _currentMana = value;
            }

            _manaBar.value = _currentMana;
        }
    }

    private void Start()
    {
        _coinsAmount = 0;
        CoinsAmount = 0;
        if (_hpBar != null && _manaBar != null)
        {
            _hpBar.maxValue = _hpBar.value = _maxHP;
            _manaBar.value = 0;
            _manaBar.maxValue = _maxMana;
        }
    }

    void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");

        _animator.SetFloat(_runAnimatorKey, Mathf.Abs(_direction));

        _rigidbody2D.velocity = Vector2.right * _direction * _speed;
        FlipHero(_direction);

        _crawl = Input.GetKey(KeyCode.C);
        if (_crawl)
        {
            _headCollider.enabled = false;
        }
        else
        {
            _headCollider.enabled = true;
        }
        
        bool canStand = !Physics2D.OverlapCircle(_headChecker.position, _headCheckerRadius, _whatIsGround);

        _headCollider.enabled = !_crawl && canStand;
        
        _animator.SetBool(_crouchAnimatorKey, !_headCollider.enabled);
        _animator.SetBool(_hurtAnimatorKey, Input.GetKey(KeyCode.X));
        _animator.SetBool(_attackAnimatorKey, Input.GetKeyDown(KeyCode.Mouse0));
        _animator.SetBool(_castAnimatorKey, Input.GetKeyDown(KeyCode.Mouse1));
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
        }

        bool canJump = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);
        if (_jump && canJump)
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }
        _animator.SetBool(_jumpAnimatorKey, !canJump);
    }
    private void FlipHero(float direction)
    {
        if (direction > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if (direction < 0 && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_headChecker.position, _headCheckerRadius);
    }
    public void AddHP(int hpPoints)
    {
        int missingPoints = _maxHP - HealthPoints;
        int pointsToAdd = missingPoints > hpPoints ? hpPoints : missingPoints;
        StartCoroutine(RestoreHp(pointsToAdd));
    }

    private IEnumerator RestoreHp(int pointsToAdd)
    {
        while (pointsToAdd != 0)
        {
            --pointsToAdd;
            ++HealthPoints;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void AddCoins(int coins)
    {
        CoinsAmount += coins;
    }

    public void TakeDamage(int damage)
    {
        HealthPoints -= damage;

        if (_currentHP <= 0)
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void AddMana(int manaPoints)
    {
        ManaPoints += manaPoints;
    }
}