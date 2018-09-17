using System.Collections;
using System.Collections.Generic;
using PuzzleSystem.PuzzleManagers.V1;
using UnityEngine;

namespace PuzzleSystem.PuzzlePiece.V1
{
    public enum PieceStatus
    {
        Unclicked,
        FirstClicked,
        DoubleClicked,
        ClickHold
    }
    public class PuzzlePieceBehavior : MonoBehaviour
    {
        [SerializeField] private float _doubleClickDelta;
        [SerializeField] private float _holdDelta;
        [SerializeField] private PieceStatus _pieceStatus;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private PuzzleManager _puzzleManager;
        [SerializeField] private BoxCollider2D _boxCollider2D;
        private Vector3 _lastPosition;
        private Color _lastColor;
        private float _clickedTime;
        [SerializeField] private int _pieceNumber;
        [SerializeField] private int _solvingNumber;

        public int PieceNumber
        {
            set
            {
                _pieceNumber = value;
            }
            get
            {
                return _pieceNumber;
            }
        }

        public int SolvingNumber
        {
            set
            {
                _solvingNumber = value;
            }
            get
            {
                return _solvingNumber;
            }
        }

        // Use this for initialization
        void Start()
        {
            _pieceStatus = PieceStatus.Unclicked;
        }

        // Update is called once per frame
        void Update()
        {
            if (_pieceStatus == PieceStatus.FirstClicked)
            {
                if (!Input.GetMouseButton(0))
                {
                    if ((_clickedTime + _doubleClickDelta) < Time.time)
                    {
                        //Debug.Log("Not click");
                        _pieceStatus = PieceStatus.Unclicked;
                        _clickedTime = 0.0f;
                    }
                }
                else
                {
                    if((_clickedTime + _holdDelta) < Time.time)
                    {
                        //Debug.Log("click hold");
                        MouseHoldHandler();
                    }
                }
            }
            if(_pieceStatus == PieceStatus.ClickHold)
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 newMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
                transform.position = new Vector3(newMousePos.x, newMousePos.y, 0);
            }
        }

        private void OnMouseDown()
        {
            if(_pieceStatus == PieceStatus.FirstClicked && ((_clickedTime + _doubleClickDelta) >= Time.time))
            {
                //Debug.Log("Double Clicked");
                DoubleClickedHandler();
                _pieceStatus = PieceStatus.Unclicked;
                _clickedTime = 0.0f;
            }
            else if(_pieceStatus == PieceStatus.Unclicked)
            {
                _clickedTime = Time.time;
                _pieceStatus = PieceStatus.FirstClicked;
            }
        }

        private void OnMouseUp()
        {
            if(_pieceStatus == PieceStatus.ClickHold)
            {
                _pieceStatus = PieceStatus.Unclicked;
                _clickedTime = 0.0f;
                _spriteRenderer.color = new Color(_lastColor.r, _lastColor.g, _lastColor.b, 1.0f);
                _puzzleManager.PieceIsDragging = false;
                _boxCollider2D.enabled = true;
                if(_puzzleManager.TouchedTarget == -1)
                {
                    transform.position = _lastPosition;
                }
                else
                {
                    _puzzleManager.SwapPosition(_lastPosition);
                }
                _puzzleManager.TouchedTarget = -1;
                _puzzleManager.DraggedTarget = -1;
            }
        }

        private void OnMouseEnter()
        {
            if(_puzzleManager.PieceIsDragging)
            {
                _puzzleManager.TouchedTarget = SolvingNumber;
            }
        }

        private void DoubleClickedHandler()
        {
            transform.Rotate(new Vector3(0, 0, 180));
        }

        private void MouseHoldHandler()
        {
            if(!_puzzleManager.PieceIsDragging)
            {
                _pieceStatus = PieceStatus.ClickHold;
                _boxCollider2D.enabled = false;
                _puzzleManager.PieceIsDragging = true;
                _puzzleManager.DraggedTarget = SolvingNumber;
                _lastPosition = transform.position;
                _lastColor = _spriteRenderer.color;
                _spriteRenderer.color = new Color(_lastColor.r, _lastColor.g, _lastColor.b, 0.3f);
            }
        }
    }
}
