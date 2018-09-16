using System.Collections;
using System.Collections.Generic;
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
        private float _clickedTime;

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
                        Debug.Log("Not click");
                        _pieceStatus = PieceStatus.Unclicked;
                        _clickedTime = 0.0f;
                    }
                }
                else
                {
                    if((_clickedTime + _holdDelta) < Time.time)
                    {
                        Debug.Log("click hold");
                        MouseHoldHandler();
                        _pieceStatus = PieceStatus.ClickHold;
                    }
                }
            }
        }

        private void OnMouseDown()
        {
            if(_pieceStatus == PieceStatus.FirstClicked && ((_clickedTime + _doubleClickDelta) >= Time.time))
            {
                Debug.Log("Double Clicked");
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
                _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }

        private void DoubleClickedHandler()
        {
            transform.Rotate(new Vector3(0, 0, 180));
        }

        private void MouseHoldHandler()
        {
            _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        }
    }
}
