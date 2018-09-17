﻿using System.Collections;
using System.Collections.Generic;
using PuzzleSystem.PuzzlePiece.V1;
using UnityEngine;

namespace PuzzleSystem.PuzzleManagers.V1
{
    public class PuzzleManager : MonoBehaviour
    {
        [SerializeField] private PuzzlePieceBehavior[] _puzzlePieces;
        [SerializeField] private bool _pieceIsDragging;
        [SerializeField] private float _pieceWidth;
        [SerializeField] private float _pieceHeight;
        [SerializeField] private int rowCount;
        [SerializeField] private int colCount;
        [SerializeField] private int _tounchedTarget;
        [SerializeField] private int _draggedTarget;
        [SerializeField] private int[] _answer;
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
            //make sure we have enough puzzle pieces
            Debug.Assert((rowCount * colCount) == _puzzlePieces.Length);
            _pieceIsDragging = false;
            _pieceWidth = _puzzlePieces[0].GetComponent<SpriteRenderer>().bounds.size.x;
            _pieceHeight = _puzzlePieces[0].GetComponent<SpriteRenderer>().bounds.size.y;
            _tounchedTarget = -1;
            _draggedTarget = -1;
            OrderPuzzle();
        }

        private void OrderPuzzle()
        {
            Vector2 botLeftPoint = GetBotLeftPoint();
            Debug.Log("botLeftPoint: " + botLeftPoint);
            for(int i = 0; i < rowCount; i++)
            {
                for(int j = 0; j < colCount; j++)
                {
                    int target = i * colCount + j;
                    Vector2 newPosition = new Vector2(botLeftPoint.x + j * _pieceWidth, botLeftPoint.y + i * _pieceHeight);
                    _puzzlePieces[target].transform.position = newPosition;
                }
            }
            return;
        }

        private Vector2 GetBotLeftPoint()
        {
            float leftPoint, botPoint;
            int totalPuzzle = _puzzlePieces.Length;
            if (colCount % 2 == 0)
            {
                //is minus because is left;
                leftPoint = -_pieceWidth * ((colCount / 2) - 1 + 0.5f);
            }
            else
            {
                leftPoint = -_pieceWidth * ((colCount / 2));
            }

            if (rowCount % 2 == 0)
            {
                botPoint = -_pieceHeight * ((rowCount / 2) - 1 + 0.5f);
            }
            else
            {
                botPoint = -_pieceHeight * ((rowCount / 2));
            }

            return new Vector2(leftPoint, botPoint);
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
