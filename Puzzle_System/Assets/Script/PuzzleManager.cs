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
        [SerializeField] int _draggedTarget;
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
            for(int i = 0; i < _puzzlePieces.Length; i++)
            {
                _puzzlePieces[i].PieceNumber = i;
            }
            _draggedTarget = -1;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SwapPosition(int firstTarget, Vector3 firstLastPosition)
        {
            Transform firstTransform = _puzzlePieces[firstTarget].transform;
            Transform secondTransform = _puzzlePieces[_draggedTarget].transform;

            firstTransform.position = secondTransform.position;
            secondTransform.position = firstLastPosition;
        }
    }
}
