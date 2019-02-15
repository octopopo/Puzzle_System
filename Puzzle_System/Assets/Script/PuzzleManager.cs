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
        FourthStep,
        Win,
        Lose,
    }
    public class PuzzleManager : MonoBehaviour
    {
        [SerializeField] private PuzzlePieceBehavior[] _puzzlePieces;
        [SerializeField] private PuzzleSpotBehavior[] _puzzleSpot;
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
        [SerializeField] private Text _winText;
        [SerializeField] private Text _moveText;
        public int TotalMove = 9;

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

            _winText.gameObject.SetActive(false);

            _moveText.text = "Remaining: " + TotalMove;

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
                    Vector2 newPosition = new Vector2(botLeftPoint.x + i * (_pieceWidth + _pieceGap), botLeftPoint.y + j * (_pieceHeight + _pieceGap));
                    newPosition.x = Mathf.Round(newPosition.x * 1000) / 1000;
                    newPosition.y = Mathf.Round(newPosition.y * 1000) / 1000;
                    _answer[target] = newPosition;
                }
            }
        }

        private void OrderPuzzle()
        {
            Vector2 botLeftPoint = GetBotLeftPoint();
            //Debug.Log("botLeftPoint: " + botLeftPoint);
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
                //The solution with the gap
                leftPoint = -(_pieceWidth + _pieceGap) * ((colCount / 2) - 1 + 0.5f);
            }
            else
            {
                leftPoint = -(_pieceWidth + _pieceGap) * ((colCount / 2));
            }

            if (rowCount % 2 == 0)
            {
                botPoint = -(_pieceHeight + _pieceGap) * ((rowCount / 2) - 1 + 0.5f);
            }
            else
            {
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
            //swap the position of the real object
            //Vector3 tempPos;
            _puzzlePieces[_draggedTarget].transform.position = _puzzlePieces[_tounchedTarget].transform.position;
            _puzzlePieces[_tounchedTarget].transform.position = firstLastPosition;

            //swap the positoin in the dictionary
            /*_numToPosition[_draggedTarget] = _puzzlePieces[_draggedTarget].transform.position;
            _numToPosition[_tounchedTarget] = _puzzlePieces[_tounchedTarget].transform.position;*/


            //Detect the spot and get it's new number
            _puzzlePieces[_draggedTarget].DetectSpot();
            _puzzlePieces[_tounchedTarget].DetectSpot();

            CheckAnswer();

        }

        public void CheckAnswer()
        {
            switch (_playerProgress)
            {
                case GamePhase.FirstStep:
                    if(_puzzlePieces[0].IsAnswerCorrect() && _puzzlePieces[1].IsAnswerCorrect() 
                        && !_puzzlePieces[0].IsFlipped && !_puzzlePieces[1].IsFlipped)
                    {
                        _playerProgress = GamePhase.SecondStep;
                        _puzzlePieces[1].SetIsDraggable(false);
                        StartCoroutine(_puzzlePieces[0].playGifRoutine(0));
                    }
                    break;
                case GamePhase.SecondStep:
                    if(_puzzlePieces[2].IsAnswerCorrect() && _puzzlePieces[5].IsAnswerCorrect()
                        && !_puzzlePieces[2].IsFlipped && !_puzzlePieces[5].IsFlipped)
                    {
                        _playerProgress = GamePhase.ThirdStep;
                        _puzzlePieces[2].SetIsDraggable(false);
                        _puzzlePieces[5].SetIsDraggable(false);
                        StartCoroutine(_puzzlePieces[2].playGifRoutine(0));
                    }
                    break;
                case GamePhase.ThirdStep:
                    if(_puzzlePieces[3].IsAnswerCorrect() && _puzzlePieces[4].IsAnswerCorrect() && _puzzlePieces[8].IsAnswerCorrect()
                        && !_puzzlePieces[3].IsFlipped && !_puzzlePieces[4].IsFlipped && !_puzzlePieces[8].IsFlipped)
                    {
                        _playerProgress = GamePhase.FourthStep;
                        _puzzlePieces[3].SetIsDraggable(false);
                        _puzzlePieces[4].SetIsDraggable(false);
                        //_puzzlePieces[6].SetIsDraggable(false);
                        _puzzlePieces[8].SetIsDraggable(false);
                        StartCoroutine(_puzzlePieces[4].playGifRoutine(0));
                    }
                    break;
                case GamePhase.FourthStep:
                    if (_puzzlePieces[7].IsAnswerCorrect() && _puzzlePieces[9].IsAnswerCorrect() && _puzzlePieces[10].IsAnswerCorrect() && _puzzlePieces[11].IsAnswerCorrect() &&_puzzlePieces[6].IsAnswerCorrect()
                        && !_puzzlePieces[7].IsFlipped && !_puzzlePieces[9].IsFlipped && !_puzzlePieces[10].IsFlipped && !_puzzlePieces[11].IsFlipped && !_puzzlePieces[6].IsFlipped)
                    {
                        _puzzlePieces[6].SetIsDraggable(false);
                        _puzzlePieces[7].SetIsDraggable(false);
                        _puzzlePieces[9].SetIsDraggable(false);
                        _puzzlePieces[10].SetIsDraggable(false);
                        _puzzlePieces[11].SetIsDraggable(false);

                        Debug.Log("The fourth answer is correct");
                        _playerProgress = GamePhase.Win;
                        StartCoroutine(_puzzlePieces[7].playGifRoutine(0));
                        //Debug.Log("You win");
                    }
                    break;
            }
        }

        private void HideAllPieces()
        {
            for (int i = 0; i < _puzzlePieces.Length; i++)
            {
                _puzzlePieces[i].gameObject.SetActive(false);
                //_puzzlePieces[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                //_puzzlePieces[i].SetIsDraggable(false);
            }
        }

        public void GameProgessTracktor()
        {
            //The Camera Behavior was temporary commented out
            //each steps should disable the movement of pieces from last move
            switch(_playerProgress)
            {
                case GamePhase.FirstStep:
                    //_puzzlePieces[0].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    //_puzzlePieces[1].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[0].gameObject.SetActive(true);
                    _puzzlePieces[1].gameObject.SetActive(true);

                    _puzzlePieces[0].transform.position = new Vector3(_puzzleSpot[startedPosition[0]].transform.position.x, _puzzleSpot[startedPosition[0]].transform.position.y, 0);
                    _puzzlePieces[1].transform.position = new Vector3(_puzzleSpot[startedPosition[1]].transform.position.x, _puzzleSpot[startedPosition[1]].transform.position.y, 0);

                    _puzzlePieces[0].SolvingNumber = _puzzleSpot[startedPosition[0]].SpotNum;
                    _puzzlePieces[1].SolvingNumber = _puzzleSpot[startedPosition[1]].SpotNum;

                    //The starting point should not be draggable
                    _puzzlePieces[0].SetIsSwappable(false);
                    _puzzlePieces[1].SetIsDraggable(true);
                    _puzzlePieces[1].SetIsFlipped(true);
                    _mainCamera.ChangeCameraPhase(0);
                    break;
                case GamePhase.SecondStep:
                    //_puzzlePieces[2].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    //_puzzlePieces[5].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[2].gameObject.SetActive(true);
                    _puzzlePieces[5].gameObject.SetActive(true);

                    _puzzlePieces[2].transform.position = new Vector3(_puzzleSpot[startedPosition[2]].transform.position.x, _puzzleSpot[startedPosition[2]].transform.position.y, 0);
                    _puzzlePieces[5].transform.position = new Vector3(_puzzleSpot[startedPosition[5]].transform.position.x, _puzzleSpot[startedPosition[5]].transform.position.y, 0);

                    _puzzlePieces[2].SolvingNumber = _puzzleSpot[startedPosition[2]].SpotNum;
                    _puzzlePieces[5].SolvingNumber = _puzzleSpot[startedPosition[5]].SpotNum;

                    _puzzlePieces[2].SetIsDraggable(true);
                    _puzzlePieces[5].SetIsDraggable(true);
                    _puzzlePieces[1].SetIsSwappable(false);
                    _mainCamera.ChangeCameraPhase(1);
                    break;
                case GamePhase.ThirdStep:
                    //_puzzlePieces[3].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    //_puzzlePieces[4].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    //_puzzlePieces[8].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[3].gameObject.SetActive(true);
                    _puzzlePieces[4].gameObject.SetActive(true);
                    _puzzlePieces[8].gameObject.SetActive(true);

                    _puzzlePieces[3].transform.position = new Vector3(_puzzleSpot[startedPosition[3]].transform.position.x, _puzzleSpot[startedPosition[3]].transform.position.y, 0);
                    _puzzlePieces[4].transform.position = new Vector3(_puzzleSpot[startedPosition[4]].transform.position.x, _puzzleSpot[startedPosition[4]].transform.position.y, 0);
                    _puzzlePieces[8].transform.position = new Vector3(_puzzleSpot[startedPosition[8]].transform.position.x, _puzzleSpot[startedPosition[8]].transform.position.y, 0);

                    _puzzlePieces[3].SolvingNumber = _puzzleSpot[startedPosition[3]].SpotNum;
                    _puzzlePieces[4].SolvingNumber = _puzzleSpot[startedPosition[4]].SpotNum;
                    _puzzlePieces[8].SolvingNumber = _puzzleSpot[startedPosition[8]].SpotNum;

                    _puzzlePieces[3].SetIsDraggable(true);
                    _puzzlePieces[4].SetIsDraggable(true);
                    //_puzzlePieces[6].SetIsDraggable(true);
                    _puzzlePieces[8].SetIsDraggable(true);

                    _puzzlePieces[2].SetIsSwappable(false);
                    _puzzlePieces[5].SetIsSwappable(false);

                    _mainCamera.ChangeCameraPhase(2);
                    break;
                case GamePhase.FourthStep:
                    //_puzzlePieces[7].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    //_puzzlePieces[9].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    //_puzzlePieces[10].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    //_puzzlePieces[11].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[6].gameObject.SetActive(true);
                    _puzzlePieces[7].gameObject.SetActive(true);
                    _puzzlePieces[9].gameObject.SetActive(true);
                    _puzzlePieces[10].gameObject.SetActive(true);
                    _puzzlePieces[11].gameObject.SetActive(true);
                    _puzzlePieces[6].transform.position = new Vector3(_puzzleSpot[startedPosition[6]].transform.position.x, _puzzleSpot[startedPosition[6]].transform.position.y, 0);
                    _puzzlePieces[7].transform.position = new Vector3(_puzzleSpot[startedPosition[7]].transform.position.x, _puzzleSpot[startedPosition[7]].transform.position.y, 0);
                    _puzzlePieces[9].transform.position = new Vector3(_puzzleSpot[startedPosition[9]].transform.position.x, _puzzleSpot[startedPosition[9]].transform.position.y, 0);
                    _puzzlePieces[10].transform.position = new Vector3(_puzzleSpot[startedPosition[10]].transform.position.x, _puzzleSpot[startedPosition[10]].transform.position.y, 0);
                    _puzzlePieces[11].transform.position = new Vector3(_puzzleSpot[startedPosition[11]].transform.position.x, _puzzleSpot[startedPosition[11]].transform.position.y, 0);

                    _puzzlePieces[7].SolvingNumber = _puzzleSpot[startedPosition[7]].SpotNum;
                    _puzzlePieces[9].SolvingNumber = _puzzleSpot[startedPosition[9]].SpotNum;
                    _puzzlePieces[10].SolvingNumber = _puzzleSpot[startedPosition[10]].SpotNum;
                    _puzzlePieces[11].SolvingNumber = _puzzleSpot[startedPosition[11]].SpotNum;

                    //_puzzlePieces[6].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _puzzlePieces[6].SetIsDraggable(true);
                    //The destination should not be draggable
                    _puzzlePieces[7].SetIsDraggable(true);
                    _puzzlePieces[10].SetIsDraggable(true);
                    _puzzlePieces[11].SetIsDraggable(true);

                    _puzzlePieces[3].SetIsSwappable(false);
                    _puzzlePieces[4].SetIsSwappable(false);
                    _puzzlePieces[8].SetIsSwappable(false);
                    _mainCamera.ChangeCameraPhase(3);
                    break;
                default:
                    break;
            }
        }

        public void GifPlayedHandler(int pieceNum, int gifNum = 0)
        {
            switch(pieceNum)
            {
                case 0:
                    StartCoroutine(_puzzlePieces[1].playGifRoutine(0));
                    break;
                case 1:
                    _videoPlayer.DisplayAndPlay();
                    break;
                case 2:
                    StartCoroutine(_puzzlePieces[5].playGifRoutine(0));
                    break;
                case 3:
                    StartCoroutine(_puzzlePieces[4].playGifRoutine(1));
                    break;
                case 4:
                    if (gifNum == 0)
                    {
                        StartCoroutine(_puzzlePieces[3].playGifRoutine(0));
                    }
                    else if(gifNum == 1)
                    {
                        StartCoroutine(_puzzlePieces[5].playGifRoutine(1));
                    }
                    break;
                case 5:
                    if (gifNum == 0)
                    {
                        _videoPlayer.DisplayAndPlay();
                    }
                    else if(gifNum == 1)
                    {
                        _videoPlayer.DisplayAndPlay();
                        //StartCoroutine(_puzzlePieces[7].playGifRoutine(0));
                    }
                    break;
                case 6:
                    break;
                case 7:
                    if (gifNum == 0)
                    {
                        StartCoroutine(_puzzlePieces[10].playGifRoutine(0));
                    }
                    /*else if(gifNum == 1)
                    {
                        StartCoroutine(_puzzlePieces[10].playGifRoutine(0));
                    }*/
                    break;
                case 8:
                    break;
                case 9:
                    Debug.Log("You win!!");
                    _winText.text = "Win";
                    _winText.gameObject.SetActive(true);
                    break;
                case 10:
                    StartCoroutine(_puzzlePieces[9].playGifRoutine(0));
                    break;
                case 11:
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

        public void DeductOneMove()
        {
            TotalMove--;
            _moveText.text = "Remaining: " + TotalMove;
            if (TotalMove <= 0 && _playerProgress != GamePhase.Win)
            {
                _playerProgress = GamePhase.Lose;
                _winText.gameObject.SetActive(true);
                _winText.text = "Lose";
            }
        }
    }
}
