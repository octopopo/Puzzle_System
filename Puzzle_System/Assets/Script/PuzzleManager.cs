﻿using System.Collections;
using System.Collections.Generic;
using PuzzleSystem.PuzzlePiece.V1;
using UnityEngine;

namespace PuzzleSystem.PuzzleManagers.V1
{
    public enum GamePhase
    {
        FirstStep,
        SecondStep,
        ThirdStep,
        FourthStep
    }
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
        [SerializeField] private int[] _solvingField;
        [SerializeField] private float _pieceGap;
        [SerializeField] private GamePhase _playerProgress;
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
            Debug.Assert(_answer.Length == _solvingField.Length);

            _pieceIsDragging = false;

            //get the size of the pieces so that we can set up the puzzle
            _pieceWidth = _puzzlePieces[0].GetComponent<SpriteRenderer>().bounds.size.x;
            _pieceHeight = _puzzlePieces[0].GetComponent<SpriteRenderer>().bounds.size.y;

            _tounchedTarget = -1;
            _draggedTarget = -1;

            //These part might not been used
            //set up the piece in the order
            OrderPuzzle();
            //SetPieceNumber();

            //Set the game Progress to very beginning
            _playerProgress = GamePhase.FirstStep;
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
                    //Vector2 newPosition = new Vector2(botLeftPoint.x + j * _pieceWidth, botLeftPoint.y + i * _pieceHeight);
                    Vector2 newPosition = new Vector2(botLeftPoint.x + j * (_pieceWidth + _pieceGap), botLeftPoint.y + i * (_pieceHeight + _pieceGap));
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
                //The solution below is without the gap
                //leftPoint = -_pieceWidth * ((colCount / 2) - 1 + 0.5f);

                //The solution with the gap
                leftPoint = -(_pieceWidth + _pieceGap) * ((colCount / 2) - 1 + 0.5f);
            }
            else
            {
                //leftPoint = -_pieceWidth * ((colCount / 2));
                leftPoint = -(_pieceWidth + _pieceGap) * ((colCount / 2));
            }

            if (rowCount % 2 == 0)
            {
                //botPoint = -_pieceHeight * ((rowCount / 2) - 1 + 0.5f);
                botPoint = -(_pieceHeight + _pieceGap) * ((rowCount / 2) - 1 + 0.5f);
            }
            else
            {
                //botPoint = -_pieceHeight * ((rowCount / 2));
                botPoint = -(_pieceHeight + _pieceGap) * ((rowCount / 2));
            }

            return new Vector2(leftPoint, botPoint);
        }

        public void SetPieceNumber()
        {
            Debug.Assert(_solvingField.Length == _puzzlePieces.Length);
            for(int i = 0; i < _solvingField.Length; i++)
            {
                _puzzlePieces[i].PieceNumber = _solvingField[i];
                _puzzlePieces[i].SolvingNumber = i;
            }
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

            int tempSwap = _solvingField[_draggedTarget];
            _solvingField[_draggedTarget] = _solvingField[_tounchedTarget];
            _solvingField[_tounchedTarget] = tempSwap;

            _puzzlePieces[_draggedTarget].SolvingNumber = _tounchedTarget;
            _puzzlePieces[_tounchedTarget].SolvingNumber = _draggedTarget;

            PuzzlePieceBehavior tempPPB = _puzzlePieces[_draggedTarget];
            _puzzlePieces[_draggedTarget] = _puzzlePieces[_tounchedTarget];
            _puzzlePieces[_tounchedTarget] = tempPPB;

            if(CheckAnswer(_solvingField, _answer))
            {
                Debug.Log("You win!");
            }

        }

        private bool CheckAnswer(int[] arr1, int[] arr2)
        {
            for(int i = 0; i < arr1.Length; i++)
            {
                if(arr1[i] != arr2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
