using System.Collections;
using System.Collections.Generic;
using PuzzleSystem.PuzzlePiece.V1;
using UnityEngine;

namespace PuzzleSystem.PuzzleManagers.V1
{
    public class PuzzleManager : MonoBehaviour
    {
        [SerializeField] PuzzlePieceBehavior[] _puzzlePieces;
        [SerializeField] bool _pieceIsDragging;
        [SerializeField] float _pieceWidth;
        [SerializeField] float _pieceHeight;
        [SerializeField] int _tounchedTarget;
        [SerializeField] int _draggedTarget;
        [SerializeField] int[] _answer;
        public bool PieceIsDragging
        {
            set
            {
                _pieceIsDragging = value;
            }
            get
            {
                return _pieceIsDragging;
            }
        }

        public int TouchedTarget
        {
            set
            {
                _tounchedTarget = value;
            }
            get
            {
                return _tounchedTarget;
            }
        }

        public int DraggedTarget
        {
            set
            {
                _draggedTarget = value;
            }
            get
            {
                return _draggedTarget;
            }
        }
        // Use this for initialization
        void Start()
        {
            _pieceIsDragging = false;
            _pieceWidth = _puzzlePieces[0].GetComponent<SpriteRenderer>().bounds.size.x;
            _pieceHeight = _puzzlePieces[0].GetComponent<SpriteRenderer>().bounds.size.y;
            _tounchedTarget = -1;
            _draggedTarget = -1;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SwapPosition(Vector3 firstLastPosition)
        {
            Transform firstTransform = _puzzlePieces[_draggedTarget].transform;
            Transform secondTransform = _puzzlePieces[_tounchedTarget].transform;

            firstTransform.position = secondTransform.position;
            secondTransform.position = firstLastPosition;
        }
    }
}
