using System;
using UnityEditor.VersionControl;
using UnityEngine;

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
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private Transform _headChecker;


    private float _direction;

    private bool _jump;

    private bool _crawl;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");
        
        //1 - MoveAddForce(_direction, _speed);
        //2 - MoveVelocity(_direction, _speed);
        //3 - MoveTransform(_direction, _speed);
        //4 - MoveTransformTranslate(_direction, _speed);

        _rigidbody2D.velocity = Vector2.right * _direction * _speed;
        FlipHero(_direction);

        if (Input.GetKey(KeyCode.C))
        {
            _headCollider.enabled = false;
        }
        else
        {
            _headCollider.enabled = true;
        }

        _crawl = Input.GetKey(KeyCode.C);
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
        }

        if (_jump && Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround))
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }

        _headCollider.enabled =
            !_crawl && !Physics2D.OverlapCircle(_headChecker.position, _headCheckerRadius, _whatIsGround);
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

    public void MoveAddForce(float direction, float speed)
    {
        if (direction < 0)
        {
            _rigidbody2D.AddForce(Vector2.left * speed);
        }
        else if (direction > 0)
        {
            _rigidbody2D.AddForce(Vector2.right * speed);
        }
    }

    public void MoveVelocity(float direction, float speed)
    {
        _rigidbody2D.velocity = new Vector2(direction * speed, _rigidbody2D.velocity.y);
    }

    public void MoveTransform(float direction, float speed)
    {
        Vector2 position = _transform.position;
        if (direction < 0)
        {
            position = new Vector2(position.x - speed, position.y);
        }
        else if (direction > 0)
        {
            position = new Vector2(position.x + speed, position.y);
        }
        _transform.position = position;
    }
    
    public void MoveTransformTranslate(float direction, float speed)
    {
        if (direction < 0)
        {
            _transform.Translate(Vector3.left * Time.deltaTime);
        }
        else if (direction > 0)
        {
            _transform.Translate(Vector3.right * Time.deltaTime);
        }
    }
}