using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    AndroidInputs inputs;
    Transform player;
    [SerializeField] private float speed;
    [SerializeField] private float checkDistance = 1f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Vector3 boxSize = new Vector3(0.4f, 0.4f, 0.4f);

    private Vector3 pendingDirection = Vector3.zero; // Направление ожидающего поворота
    private bool hasPendingTurn = false;
    

    private void Start()
    {
        inputs = FindFirstObjectByType<AndroidInputs>();
        player = GetComponent<Transform>();
    }

    void Update()
    {
        Move();
        HandleInput(); // теперь отдельно обрабатываем нажатия
    }

    private void Move()
    {
        player.Translate(Vector3.forward * (speed * Time.deltaTime));

        if (hasPendingTurn && CanMove(pendingDirection))
        {
            RotateToDirection(pendingDirection);
            hasPendingTurn = false; // сбрасываем после поворота
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || inputs.moveUp)
        {
            pendingDirection = Vector3.forward;
            hasPendingTurn = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || inputs.moveDown)
        {
            pendingDirection = Vector3.back;
            hasPendingTurn = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || inputs.moveLeft)
        {
            pendingDirection = Vector3.left;
            hasPendingTurn = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || inputs.moveRight)
        {
            pendingDirection = Vector3.right;
            hasPendingTurn = true;
        }
    }

    private void RotateToDirection(Vector3 dir)
    {
        if (dir == Vector3.forward)
            player.rotation = Quaternion.Euler(0, 0, 0);
        else if (dir == Vector3.back)
            player.rotation = Quaternion.Euler(0, 180, 0);
        else if (dir == Vector3.left)
            player.rotation = Quaternion.Euler(0, 270, 0);
        else if (dir == Vector3.right)
            player.rotation = Quaternion.Euler(0, 90, 0);
    }

    private bool CanMove(Vector3 direction)
    {
        Vector3 checkPosition = player.position + direction.normalized * checkDistance;

        Collider[] hitColliders = Physics.OverlapBox(checkPosition, boxSize / 2f, Quaternion.identity, wallLayer);

        return hitColliders.Length == 0;
    }

}
