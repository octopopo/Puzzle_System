using System.Collections;
using System.Collections.Generic;
using PuzzleSystem.PuzzlePiece.V1;
using UnityEngine.UI;
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
        [SerializeField] private Vector2[] _answer;
        //[SerializeField] private int[] _solvingField;
        [SerializeField] private float _pieceGap;
        [SerializeField] private GamePhase _playerProgress;
        [SerializeField] private Vector2[] _numToPosition;

        public CameraBehavior _mainCamera;
        public VideoBehavior _videoPlayer;
        public Button _skipButton;
        //[SerializeField] private int[] _puzzleSetup;
        private int _totalPiece;
        private bool isPosSet = false;


        public int[] startedPosition;

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
            //Debug.Assert(_answer.Length == _solvingField.Length);

            _totalPiece = _puzzlePieces.Length;
            Debug.Log("totalPiece" + _totalPiece);
            _pieceIsDragging = false;

            //get the size of the pieces so that we can set up the puzzle
            _pieceWidth = _puzzlePieces[0].GetComponent<SpriteRenderer>().bounds.size.x;
            _pieceHeight = _puzzlePieces[0].GetComponent<SpriteRenderer>().bounds.size.y;

            _tounchedTarget = -1;
            _draggedTarget = -1;

            //These part might not been used
            //set up the piece in the order
            //OrderPuzzle();
            //SetPieceNumber();

            //Instantiate a dictionary
            _numToPosition = new Vector2[_totalPiece];
            _answer = new Vector2[_totalPiece];

            //Set the game Progress to very beginning
            _playerProgress = GamePhase.FirstStep;

            //Hide all the pieces
            StorePiecePosition();
            HideAllPieces();
            GameProgessTracktor();

            _skipButton.onClick.AddListener(GameProgessTracktor);

            //Test, play video at the begninning
            //We might not be using this
            //_videoPlayer.DisplayAndPlayOnPieces(0);
        }

        private void StorePiecePosition()
        {

            GameObject[] pieces = GameObject.FindGameObjectsWithTag("PuzzlePiece");
            //Debug.Log("Piece I find: " + pieces.Length);
            for(int i = 0; i < pieces.Length; i++)
            {
                _numToPosition[pieces[i].GetComponent<PuzzlePieceBehavior>().PieceNumber] = pieces[i].transform.position;
            }


            Vector2 botLeftPoint = GetBotLeftPoint();
            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    int target = i * rowCount + j;
                    //Vector2 newPosition = new Vector2(botLeftPoint.x + j * _pieceWidth, botLeftPoint.y + i * _pieceHeight);
                    Vector2 newPosition = new Vector2(botLeftPoint.x + i * (_pieceWidth + _pieceGap), botLeftPoint.y + j * (_pieceHeight + _pieceGap));
                    newPosition.x = Mathf.Round(newPosition.x * 1000) / 1000;
                    newPosition.y = Mathf.Round(newPosition.y * 1000) / 1000;
                    //Debug.Log(target);
                    _answer[target] = newPosition;
                }
            }
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
            /*Debug.Assert(_solvingField.Length == _puzzlePieces.Length);
            for(int i = 0; i < _solvingField.Length; i++)
            {
                _puzzlePieces[i].PieceNumber = _solvingField[i];
                //_puzzlePieces[i].SolvingNumber = i;
            }*/
        }

        // Update is called once per frame
        void Update()
        {
            //Add it for debug
            //GameProgessTracktor();
            if(!isPosSet)
            {
                StorePiecePosition();
                isPosSet = true;
            }
        }

        public void SwapPosition(Vector3 firstLastPosition)
        {
            //int firstPieceNum = _puzzlePieces[_draggedTarget].GetComponent<PuzzlePieceBehavior>().PieceNumber;
            //int secondPieceNum = _puzzlePieces[_tounchedTarget].GetComponent<PuzzlePieceBehavior>().PieceNumber;

            //swap the position of the real object
            _puzzlePieces[_draggedTarget].transform.position = _numToPosition[_tounchedTarget];
            _puzzlePieces[_tounchedTarget].transform.position = _numToPosition[_draggedTarget];

            //swap the positoin in the dictionary
            _numToPosition[_draggedTarget] = _puzzlePieces[_draggedTarget].transform.position;
            _numToPosition[_tounchedTarget] = _puzzlePieces[_tounchedTarget].transform.position;


            //Detect the spot and get it's new number
            _puzzlePieces[_draggedTarget].DetectSpot();
            _puzzlePieces[_tounchedTarget].DetectSpot();

            CheckAnswer();

            /*Transform firstTransform = _puzzlePieces[_draggedTarget].transform;
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
            _puzzlePieces[_tounchedTarget] = tempPPB;*/

            /*if(CheckAnswer(_solvingField, _answer))
            {
                Debug.Log("You win!");
            }*/

        }

        private void CheckAnswer()
        {
            switch(_playerProgress)
            {
                case GamePhase.FirstStep:
                    //if(_answer[0] == _numToPosition[0] && _answer[1] == _numToPosition[1])
                    if(_puzzlePieces[0].IsAnswerCorrect() && _puzzlePieces[1].IsAnswerCorrect())
                    {
                        _playerProgress = GamePhase.SecondStep;
                        _puzzlePieces[1].SetIsDraggable(false);
                        StartCoroutine(_puzzlePieces[0].playGifRoutine());
                        //_videoPlayer.DisplayAndPlay();
                        //GameProgessTracktor();
                    }
                    break;
                case GamePhase.SecondStep:
                    //if (_answer[2] == _numToPosition[2] && _answer[5] == _numToPosition[5])
                    if(_puzzlePieces[2].IsAnswerCorrect() && _puzzlePieces[5].IsAnswerCorrect())
                    {
                        _playerProgress = GamePhase.ThirdStep;
                        _puzzlePieces[2].SetIsDraggable(false);
                        _puzzlePieces[5].SetIsDraggable(false);
                        StartCoroutine(_puzzlePieces[2].playGifRoutine());
                        //_videoPlayer.DisplayAndPlay();
                        //GameProgessTracktor();
                    }
                    break;
                case GamePhase.ThirdStep:
                    //if (_answer[3] == _numToPosition[3] && _answer[4] == _numToPosition[4] && _answer[6] == _numToPosition[6] && _answer[7] == _numToPosition[7])
                    if(_puzzlePieces[3].IsAnswerCorrect() && _puzzlePieces[4].IsAnswerCorrect() && _puzzlePieces[6].IsAnswerCorrect() && _puzzlePieces[7].IsAnswerCorrect())
                    {
                        _playerProgress = GamePhase.FourthStep;
                        _puzzlePieces[3].SetIsDraggable(false);
                        _puzzlePieces[4].SetIsDraggable(false);
                        _puzzlePieces[6].SetIsDraggable(false);
                        _puzzlePieces[7].SetIsDraggable(false);
                        StartCoroutine(_puzzlePieces[4].playGifRoutine());
                        //_videoPlayer.DisplayAndPlay();
                        //GameProgessTracktor();
                    }
                    break;
                case GamePhase.FourthStep:
                    //if (_answer[8] == _numToPosition[8] && _answer[9] == _numToPosition[9] && _answer[10] == _numToPosition[10] && _answer[11] == _numToPosition[11])
                    if (_puzzlePieces[8].IsAnswerCorrect() && _puzzlePieces[9].IsAnswerCorrect() && _puzzlePieces[10].IsAnswerCorrect() && _puzzlePieces[11].IsAnswerCorrect())
                    {
                        _puzzlePieces[8].SetIsDraggable(false);
                        _puzzlePieces[9].SetIsDraggable(false);
                        _puzzlePieces[10].SetIsDraggable(false);
                        _puzzlePieces[11].SetIsDraggable(false);
                        StartCoroutine(_puzzlePieces[8].playGifRoutine());
                        //Debug.Log("You win");
                    }
                    break;
            }
        }

        private void HideAllPieces()
        {
            for (int i = 0; i < _puzzlePieces.Length; i++)
            {
                //_puzzlePieces[i].gameObject.SetActive(false);
                _puzzlePieces[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                _puzzlePieces[i].SetIsDraggable(false);
            }
        }

        private void GameProgessTracktor()
        {
            //The Camera Behavior was temporary commented out
            //each steps should disable the movement of pieces from last move
            switch(_playerProgress)
            {
                case GamePhase.FirstStep:
                    _puzzlePieces[0].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[1].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    //The starting point should not be draggable
                    _puzzlePieces[0].SetIsSwappable(false);
                    _puzzlePieces[1].SetIsDraggable(true);
                    //_mainCamera.ChangeCameraPhase(0);
                    break;
                case GamePhase.SecondStep:
                    _puzzlePieces[2].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[5].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[2].SetIsDraggable(true);
                    _puzzlePieces[5].SetIsDraggable(true);
                    _puzzlePieces[1].SetIsSwappable(false);
                    //_mainCamera.ChangeCameraPhase(1);
                    break;
                case GamePhase.ThirdStep:
                    _puzzlePieces[3].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[4].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[6].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[7].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[3].SetIsDraggable(true);
                    _puzzlePieces[4].SetIsDraggable(true);
                    _puzzlePieces[6].SetIsDraggable(true);
                    _puzzlePieces[7].SetIsDraggable(true);

                    _puzzlePieces[2].SetIsSwappable(false);
                    _puzzlePieces[5].SetIsSwappable(false);

                    //_mainCamera.ChangeCameraPhase(2);
                    break;
                case GamePhase.FourthStep:
                    _puzzlePieces[8].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[9].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[10].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[11].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    //The destination should not be draggable
                    _puzzlePieces[8].SetIsDraggable(true);
                    _puzzlePieces[10].SetIsDraggable(true);
                    _puzzlePieces[11].SetIsDraggable(true);

                    _puzzlePieces[3].SetIsSwappable(false);
                    _puzzlePieces[4].SetIsSwappable(false);
                    _puzzlePieces[6].SetIsSwappable(false);
                    _puzzlePieces[7].SetIsSwappable(false);
                    //_mainCamera.ChangeCameraPhase(3);
                    break;
                default:
                    break;
            }
        }

        public void GifPlayedHandler(int pieceNum)
        {
            switch(pieceNum)
            {
                case 0:
                    StartCoroutine(_puzzlePieces[1].playGifRoutine());
                    break;
                case 1:
                    _videoPlayer.DisplayAndPlay();
                    break;
                case 2:
                    StartCoroutine(_puzzlePieces[5].playGifRoutine());
                    break;
                case 3:
                    StartCoroutine(_puzzlePieces[6].playGifRoutine());
                    break;
                case 4:
                    StartCoroutine(_puzzlePieces[3].playGifRoutine());
                    break;
                case 5:
                    _videoPlayer.DisplayAndPlay();
                    break;
                case 6:
                    StartCoroutine(_puzzlePieces[7].playGifRoutine());
                    break;
                case 7:
                    _videoPlayer.DisplayAndPlay();
                    break;
                case 8:
                    StartCoroutine(_puzzlePieces[11].playGifRoutine());
                    break;
                case 9:
                    Debug.Log("You win!!");
                    break;
                case 10:
                    StartCoroutine(_puzzlePieces[9].playGifRoutine());
                    break;
                case 11:
                    StartCoroutine(_puzzlePieces[10].playGifRoutine());
                    break;
                default:
                    Debug.Log("Someone pass a invalid values to the function");
                    break;
            }
        }

        public bool CanSwap()
        {
            if(TouchedTarget == -1 || !_puzzlePieces[TouchedTarget].IsSwappable)
            {
                return false;
            }
            return true;
        }

        public void PutPiecesInOrder()
        {
            Vector2 calculatedResult;
            _pieceWidth = _puzzlePieces[0].GetComponent<SpriteRenderer>().bounds.size.x;
            _pieceHeight = _puzzlePieces[0].GetComponent<SpriteRenderer>().bounds.size.y;
            for (int i = 0; i < startedPosition.Length; i++)
            {
                calculatedResult = CalculatePuzzlePos(startedPosition[i]);
                _puzzlePieces[i].transform.position = calculatedResult;
            }
        }

        private Vector2 CalculatePuzzlePos(int pieceNumber)
        {
            Vector2 botLeftPoint = GetBotLeftPoint();
            Debug.Log("botLeftPoint: " + botLeftPoint);
            int i = pieceNumber % 3;
            int j = pieceNumber / 3;
            //Vector2 newPosition = new Vector2(botLeftPoint.x + j * _pieceWidth, botLeftPoint.y + i * _pieceHeight);
            Vector2 newPosition = new Vector2(botLeftPoint.x + j * (_pieceWidth + _pieceGap), botLeftPoint.y + i * (_pieceHeight + _pieceGap));

            return newPosition;
        }
    }
}
