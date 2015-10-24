using UnityEngine;
using System.Collections;
using PuzzleMaker;
using System.IO;

public class JPPuzzleController : MonoBehaviour
{

    public Texture2D PuzzleImage;
    [HideInInspector]
    public bool UseFilePath = false;        //if set to true prefab will load file created by puzzle maker to create new instance of puzzle maker
    [HideInInspector]
    public string PMFilePath = "";
    [HideInInspector]
    public int _selectedFileIndex = 0;


    public Texture2D[] JointMaskImage;

    public bool DisplayCompletedImage = true;

    [Range(3, 10)]
    public int PiecesInRow = 3;
    [Range(3, 10)]
    public int PiecesInCol = 3;



    private PuzzlePieceMaker _PuzzleMaker;
    private int _NoOfPiecesJoined = 0;      //Keeps track of number of pieces placed inside puzzle successfully

    private GameObject[] _PuzzlePieces;     //Holds PuzzlePiecePrefab instances

    //Variables use to move pieces to place
    private GameObject _CurrentHoldingPiece = null;

    //Holds main instance of puzzle piece gameobject
    private GameObject _jpPuzzlePieceInst;


    public int PuzzleMakerPieceWidthWithoutJoint
    {
        get { return _PuzzleMaker.PieceWidthWithoutJoint; }
    }

    public int PuzzleMakerPieceHeightWithoutJoint
    {
        get { return _PuzzleMaker.PieceHeightWithoutJoint; }
    }

    public float PieceWidthInWorld
    {
        get { return 1f; }
    }

    public float PieceHeightInWorld
    {
        get { return ((float)PuzzleImage.height / (float)PuzzleImage.width); }
    }

    // Get path for game data
    private static string getPath()
    {
        return Application.dataPath;
    }

    void Start()
    {

        if (!UseFilePath)
        {
            //random image selection
            PuzzleImage = new Texture2D(1, 1);
            PuzzleImage.LoadImage(File.ReadAllBytes(getPath() + "/dewa/" + Random.Range(0, 6) + ".jpg"));
            //Debug.Log(getPath()+ "/Puzzle Maker/PM Free Images/dewa/" + "1" + ".jpg");
            //Random.Range(0, 3) //KIDS PUZZLE IMAGES
            //Random.Range(3,6)  //ADULTS PUZZLE IMAGES
            _PuzzleMaker = new PuzzlePieceMaker(PuzzleImage, JointMaskImage, PiecesInRow, PiecesInCol);

            //Enable below code and provide a file path to save data created by _puzzlemaker class using provided image
            //_PuzzleMaker.SaveData("File Path here e.g c:\\Images\\Temp.pm");
        }
        else
        {

            try
            {
                _PuzzleMaker = new PuzzlePieceMaker(PMFilePath);
                PiecesInCol = _PuzzleMaker.NoOfPiecesInCol;
                PiecesInRow = _PuzzleMaker.NoOfPiecesInRow;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Please check if you have provided correct file path : " + e.Message);
                Destroy(gameObject);
                return;
            }

        }


        //Get main instance of puzzle piece
        _jpPuzzlePieceInst = gameObject.transform.FindChild("JPPiece").gameObject;

        //Seperate from cam for cam resizing to adjust whole puzzle in cam view
        Transform parentCamTransform = gameObject.transform.parent;
        Camera parentCam = parentCamTransform.GetComponent<Camera>();
        gameObject.transform.parent = null;

        //Store current transform and translate to origin for everythings placement
        Vector3 OrigionalScale = transform.localScale;
        Vector3 OrigionalPosition = transform.position;
        Vector3 OrigionalRotation = transform.localRotation.eulerAngles;


        //Set background
        if (DisplayCompletedImage)
        {
            GetComponent<Renderer>().enabled = true;
            GetComponent<Renderer>().material.mainTexture = _PuzzleMaker.CreatedBackgroundImage;
        }

        //Resize gameobject to match size for pieces
        transform.localScale = new Vector3((float)PiecesInRow, (PiecesInCol * ((float)PuzzleImage.height / (float)PuzzleImage.width)), 1f);

        //Translate to zero
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(Vector3.zero);


        #region "Pieces Initialization And Placement"

        //Initialize all pieces resize them and place them on screen randomly
        _PuzzlePieces = new GameObject[PiecesInRow * PiecesInCol];

        Random.seed = System.DateTime.Now.Millisecond;

        for (int RowTrav = 0; RowTrav < PiecesInCol; RowTrav++)
        {
            for (int ColTrav = 0; ColTrav < PiecesInRow; ColTrav++)
            {
                GameObject Temp = Object.Instantiate(_jpPuzzlePieceInst) as GameObject;

                //AudioSettings name for this piece
                Temp.name = "Piece" + ((RowTrav * PiecesInRow) + ColTrav).ToString();


                Texture2D Img = _PuzzleMaker._CreatedImagePiecesData[RowTrav, ColTrav].PieceImage;

                float PieceScaleX = (float)Img.width / (float)_PuzzleMaker.PieceWidthWithoutJoint;
                float PieceScaleY = this.PieceHeightInWorld * ((float)Img.height / (float)_PuzzleMaker.PieceHeightWithoutJoint);


                JPPieceController TempPieceControllerInst = Temp.GetComponent<JPPieceController>();
                TempPieceControllerInst.JpPuzzleControllerInstance = this;


                //Get this piece information
                SPieceInfo ThisPieceData = _PuzzleMaker._CreatedImagePiecesData[RowTrav, ColTrav].PieceMetaData.MakeCopy();
                TempPieceControllerInst.ThisPieceData = ThisPieceData;

                //Assign image to piece
                Temp.GetComponent<Renderer>().material.mainTexture = Img;

                //Resize piece in world
                Temp.transform.localScale = new Vector3(PieceScaleX, PieceScaleY, 1);


                //Position pieces randomly on screen
                Vector3 CalcPosition = Camera.main.ScreenToWorldPoint(
                    new Vector3(Random.Range(_PuzzleMaker.PieceWidthWithoutJoint, Screen.width - _PuzzleMaker.PieceWidthWithoutJoint),
                                Random.Range(_PuzzleMaker.PieceHeightWithoutJoint, Screen.height - _PuzzleMaker.PieceHeightWithoutJoint),
                                0));
                CalcPosition.z = transform.position.z - 0.4f;

                Temp.transform.position = CalcPosition;


                //Enable collider for this piece
                Temp.GetComponent<BoxCollider>().enabled = true;
                TempPieceControllerInst.enabled = true;

                //Enable piece
                Temp.SetActive(true);

                _PuzzlePieces[(RowTrav * PiecesInRow) + ColTrav] = Temp;
            }
        }

        #endregion


        #region "Camera Resizing And Placement"

        //Resize camera to display whole puzzle in camera view
        float widthToBeSeen = PiecesInRow * PieceWidthInWorld;
        float heightToBeSeen = PiecesInCol * PieceHeightInWorld;

        parentCam.orthographicSize = widthToBeSeen > heightToBeSeen ? widthToBeSeen * 0.4f : heightToBeSeen * 0.4f;

        //Position camera in centre of puzzle
        //float CalcCameraX = ;
        //float CalcCameraY = ;

        //parentCamTransform.position = new Vector3(CalcCameraX, CalcCameraY, _pieceInstances[1][0].transform.position.z - 3);
        parentCamTransform.position = new Vector3(0, 0, transform.position.z - 10);

        #endregion

    }

    void Update()
    {
        if (IsHoldingPiece())
        {

            float MousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float MousePositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

            _CurrentHoldingPiece.transform.position = new Vector3(MousePositionX, MousePositionY,
                                                _CurrentHoldingPiece.transform.position.z);
        }

        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.collider.transform.name.Contains("Piece"))
                {

                    _CurrentHoldingPiece = hit.collider.transform.gameObject;
                    Vector3 _CurrentHoldingPieceDPos = _CurrentHoldingPiece.transform.position;

                    _CurrentHoldingPiece.transform.position = new Vector3(_CurrentHoldingPieceDPos.x, _CurrentHoldingPieceDPos.y,
                                                                            transform.position.z - 0.4f);


                    Transform Temp = _CurrentHoldingPiece.transform.root;

                    if (Temp != null)
                    {
                        _CurrentHoldingPiece.transform.parent = null;

                        if (Temp.name != _CurrentHoldingPiece.name)
                            Temp.parent = _CurrentHoldingPiece.transform;
                    }


                }
            }


        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (IsHoldingPiece())
                UnholdPiece();
        }

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 150, 80), "Reset"))
            Reset();
    }



    void Reset()
    {

        for (int i = 0; i < _PuzzlePieces.Length; i++)
        {
            _PuzzlePieces[i].GetComponent<BoxCollider>().enabled = false;

            _PuzzlePieces[i].transform.parent = null;

            //Position pieces randomly on screen
            Vector3 CalcPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(Random.Range(_PuzzleMaker.PieceWidthWithoutJoint, Screen.width - _PuzzleMaker.PieceWidthWithoutJoint),
                            Random.Range(_PuzzleMaker.PieceHeightWithoutJoint, Screen.height - _PuzzleMaker.PieceHeightWithoutJoint),
                            0));

            CalcPosition.z = transform.position.z - 0.01f;

            _PuzzlePieces[i].transform.position = CalcPosition;

            _PuzzlePieces[i].GetComponent<BoxCollider>().enabled = true;
        }

    }

    public void UnholdPiece()
    {
        _CurrentHoldingPiece = null;
    }

    public bool IsHoldingPiece()
    {
        return _CurrentHoldingPiece != null;
    }

    public int HoldingPieceID()
    {
        if (_CurrentHoldingPiece != null)
        {
            return _CurrentHoldingPiece.GetComponent<JPPieceController>().ThisPieceData.ID;
        }

        return -1;

    }

}
