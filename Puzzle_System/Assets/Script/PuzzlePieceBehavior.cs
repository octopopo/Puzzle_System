using System.Collections;
using System.Collections.Generic;
using PuzzleSystem.PuzzleManagers.V1;
using UnityEngine;
using System.Drawing;

namespace PuzzleSystem.PuzzlePiece.V1
{
    [System.Serializable]
    public struct SpriteListClass
    {
        public Sprite[] spritesList;
    }
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
        private UnityEngine.Color _lastColor;
        private float _clickedTime;
        [SerializeField] private int _pieceNumber;
        [SerializeField] private int _solvingNumber;
        [SerializeField] bool _isDraggable;
        [SerializeField] bool _isSwappable;
        [SerializeField] bool _isFlipped;
        private GifHandler _gifHandler;
        List<List<Sprite>> gifList;
        [SerializeField] public SpriteListClass[] _allSprite;
        [SerializeField] int gifCount;
        int gifLength;
        int showingPiece;
        public float playSpeed = 0.1f;
        float playCount;
        public bool playingGif;

        //The path can be change here
        private string _gifPath = "Assets/Texture/Gif/";

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

        public bool IsDraggable
        {
            get
            {
                return _isDraggable;
            }
        }

        public bool IsSwappable
        {
            get
            {
                return _isSwappable;
            }
        }

        public bool IsFlipped
        {
            get { return _isFlipped; }
        }
        // Use this for initialization
        void Start()
        {
            _pieceStatus = PieceStatus.Unclicked;
            _isFlipped = false;
            //_gifHandler = new GifHandler();

            //0103, read the image with System.Drawing
            /*if (gifCount > 0)
            {
                string mGif = _gifPath + _pieceNumber + ".gif";
                Image fileImage = Image.FromFile(mGif);
                gifList = new List<List<Sprite>>();
                gifList.Add(_gifHandler.GifToSpriteList(fileImage));
                if(gifCount > 1)
                {
                    mGif = _gifPath + _pieceNumber + "-2.gif";
                    fileImage = Image.FromFile(mGif);
                    gifList.Add(_gifHandler.GifToSpriteList(fileImage));
                }
            }*/

            //This part is for initailize the animation of the gif
            showingPiece = 0;
            playCount = 0;
            playingGif = false;
            _isSwappable = true;
            _solvingNumber = -1;
            //DetectSpot();
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
                    if ((_clickedTime + _holdDelta) < Time.time)
                    {
                        //Debug.Log("click hold");
                        MouseHoldHandler();
                    }
                }
            }
            if (_pieceStatus == PieceStatus.ClickHold)
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 newMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
                transform.position = new Vector3(newMousePos.x, newMousePos.y, 0);
            }
        }

        private void OnMouseDown()
        {
            if (_isDraggable)
            {
                if (_pieceStatus == PieceStatus.FirstClicked && ((_clickedTime + _doubleClickDelta) >= Time.time))
                {
                    DoubleClickedHandler();
                    _pieceStatus = PieceStatus.Unclicked;
                    _clickedTime = 0.0f;
                }
                else if (_pieceStatus == PieceStatus.Unclicked)
                {
                    _clickedTime = Time.time;
                    _pieceStatus = PieceStatus.FirstClicked;
                }
            }
        }

        private void OnMouseUp()
        {
            if (_pieceStatus == PieceStatus.ClickHold)
            {
                _pieceStatus = PieceStatus.Unclicked;
                _clickedTime = 0.0f;
                _spriteRenderer.color = new UnityEngine.Color(_lastColor.r, _lastColor.g, _lastColor.b, 1.0f);
                _puzzleManager.PieceIsDragging = false;
                _boxCollider2D.enabled = true;
                /*if (!_puzzleManager.CanSwap())
                {
                    transform.position = _lastPosition;
                }
                else
                {
                    _puzzleManager.SwapPosition(_lastPosition);
                }*/
                if(FindSpot())
                {
                    Debug.Log("Move");
                    _puzzleManager.CheckAnswer();
                    _puzzleManager.DeductOneMove();
                }
                else
                {
                    if (_puzzleManager.CanSwap())
                    {
                        Debug.Log("Swap");
                        _puzzleManager.SwapPosition(_lastPosition);
                        //_puzzleManager.CheckAnswer();
                        _puzzleManager.DeductOneMove();
                    }
                    else
                    {
                        transform.position = _lastPosition;
                    }
                }
                _puzzleManager.TouchedTarget = -1;
                _puzzleManager.DraggedTarget = -1;
            }
        }

        private void OnMouseEnter()
        {
            if (_puzzleManager.PieceIsDragging)
            {
                _puzzleManager.TouchedTarget = _pieceNumber;
            }
        }

        private void DoubleClickedHandler()
        {
            transform.Rotate(new Vector3(0, 0, 180));
            _isFlipped = !_isFlipped;
            _puzzleManager.CheckAnswer();
        }

        public void SetIsFlipped(bool flipped)
        {
            if(flipped)
            {
                transform.rotation = Quaternion.Euler(0,0,180);
                _isFlipped = flipped;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                _isFlipped = flipped;
            }
        }

        private void MouseHoldHandler()
        {
            if (!_puzzleManager.PieceIsDragging)
            {
                _pieceStatus = PieceStatus.ClickHold;
                _boxCollider2D.enabled = false;
                _puzzleManager.PieceIsDragging = true;
                _puzzleManager.DraggedTarget = _pieceNumber;
                _lastPosition = transform.position;
                _lastColor = _spriteRenderer.color;
                _spriteRenderer.color = new UnityEngine.Color(_lastColor.r, _lastColor.g, _lastColor.b, 0.3f);
            }
        }

        public void SetIsDraggable(bool draggable)
        {
            Debug.Log(gameObject.name + " has set draggable as: " + draggable);
            _isDraggable = draggable;
        }

        public void SetIsSwappable(bool swappable)
        {
            _isSwappable = swappable;
        }

        public IEnumerator playGifRoutine(int gifNum = 0)
        {
            if (gifNum >= gifCount)
            {
                Debug.LogError("You called the wrong gif");
            }
            else
            {
                //gifLength = gifList[gifNum].Count;
                gifLength = _allSprite[gifNum].spritesList.Length;
                for (int i = 0; i < gifLength; i++)
                {
                    yield return new WaitForSeconds(playSpeed);
                    showingPiece = i;
                    //_spriteRenderer.sprite = gifList[gifNum][showingPiece];
                    _spriteRenderer.sprite = _allSprite[gifNum].spritesList[showingPiece];
                }
                _puzzleManager.GifPlayedHandler(_pieceNumber, gifNum);
            }
        }

        public void DetectSpot()
        {
            ContactFilter2D contactFilt = new ContactFilter2D();
            Collider2D[] overlappedCollider = new Collider2D[1];
            _boxCollider2D.OverlapCollider(contactFilt, overlappedCollider);
            //Debug.Log("This is " + name + " I am overlapping with " + overlappedCollider[0].name);
            PuzzleSpotBehavior targetSpot = overlappedCollider[0].GetComponent<PuzzleSpotBehavior>();
            if(targetSpot != null)
            {
                _solvingNumber = targetSpot.SpotNum;
            }
            transform.position = new Vector3(overlappedCollider[0].transform.position.x, overlappedCollider[0].transform.position.y, 0);
        }

        public bool FindSpot()
        {
            ContactFilter2D contactFilt = new ContactFilter2D();
            Collider2D[] overlappedCollider = new Collider2D[1];
            _boxCollider2D.OverlapCollider(contactFilt, overlappedCollider);
            PuzzleSpotBehavior targetSpot = overlappedCollider[0].GetComponent<PuzzleSpotBehavior>();
            if(targetSpot == null)
            {
                return false;
            }
            else
            {
                _solvingNumber = targetSpot.SpotNum;
            }
            transform.position = new Vector3(overlappedCollider[0].transform.position.x, overlappedCollider[0].transform.position.y, 0);
            return true;
        }

        public bool IsAnswerCorrect()
        {
            return _pieceNumber == _solvingNumber;
        }
    }
}
