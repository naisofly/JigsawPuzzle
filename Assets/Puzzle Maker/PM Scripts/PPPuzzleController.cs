using UnityEngine;
using System.Collections;
using PuzzleMaker;

public class PPPuzzleController : MonoBehaviour
{

#region "Public Variables"

    public Texture2D PuzzleImage;
    [HideInInspector]
    public bool UseFilePath = false;    //if set to true prefab will load file created by puzzle maker to create new instance of puzzle maker
    [HideInInspector]
    public string PMFilePath = "";
    [HideInInspector]
    public int _selectedFileIndex = 0;
    
    public Texture2D[] JointMaskImage;
    
    public bool ShufflePieces = true;   //Shuffle pieces on pickup place


    [Range(3,10)]
    public int PiecesInRow = 3;
    [Range(3,10)]
    public int PiecesInCol = 3;

#endregion

#region "Private Variables"

    private PuzzlePieceMaker _PuzzleMaker;
    private int _NoOfPiecesPlaced = 0;      //Keeps track of number of pieces placed inside puzzle successfully

    //Variables use to move pieces to place
    private GameObject _CurrentHoldingPiece = null;
    private Vector3 _CurrentHoldingPieceDPos = new Vector3();   //Default position of holding piece

    private GameObject[] _ChildColliders;   //Holds collider instances used to detect pieces position
    private GameObject[] _PuzzlePieces;     //Holds PuzzlePiecePrefab instances
    private Vector3[] _PuzzlePiecesOrigPos; //Holds starting position of puzzle pieces instances

    private GameObject PuzzlePiecePrefab;

#endregion



	void Start () {

        
        if (!UseFilePath)
        {
            _PuzzleMaker = new PuzzlePieceMaker(PuzzleImage, JointMaskImage, PiecesInRow, PiecesInCol);

            //Enable below code and provide a file path to save data created by _puzzlemaker class using provided image
            //_PuzzleMaker.SaveData("File Path here e.g c:\\Images\\Temp.pm");
        }
        else
        {
            
            try
            {
                _PuzzleMaker = new PuzzlePieceMaker(PMFilePath);
                PiecesInRow = _PuzzleMaker.NoOfPiecesInRow;
                PiecesInCol = _PuzzleMaker.NoOfPiecesInCol;
            }
            catch(System.Exception e)
            {
                Debug.LogError("Please check if you have provided correct file path :" + e.Message);
                Destroy(gameObject);
                return;
            }
            
        }

        //Get main instance of puzzle piece
        PuzzlePiecePrefab = gameObject.transform.FindChild("PPPiece").gameObject;

        //Seperate from cam for cam resizing to adjust whole puzzle in cam view
        Transform parentCamTransform = gameObject.transform.parent;
        Camera parentCam = parentCamTransform.GetComponent<Camera>();
        gameObject.transform.parent = null;

        //Store current transform and translate to origin for everythings placement
        Vector3 OrigionalScale = transform.localScale;
        Vector3 OrigionalPosition = transform.position;
        Vector3 OrigionalRotation = transform.localRotation.eulerAngles;


        //Set background
        GetComponent<Renderer>().material.mainTexture = _PuzzleMaker.CreatedBackgroundImage;

        //Resize gameobject to match size for pieces
        transform.localScale = new Vector3((float)PiecesInRow, (PiecesInCol * ( (float)PuzzleImage.height / (float)PuzzleImage.width ) ), 1f);

        //Translate to zero
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler( Vector3.zero );


        //Initialize and set Colliders to detect piece in place

        //Get child colider object
        GameObject _ChildCollider = gameObject.transform.FindChild("Collider").gameObject;

        //Instantiate required number of childcollider objects
        _ChildColliders = new GameObject[PiecesInCol * PiecesInRow];

        //Hold created puzzle pieces instances
        _PuzzlePieces = new GameObject[PiecesInCol * PiecesInRow];

        //Holds initial positions for puzzle pieces
        _PuzzlePiecesOrigPos = new Vector3[_PuzzlePieces.Length];


        float PieceWidthInBackground = 1;
        float PieceHeightInBackground = ((float)PuzzleImage.height / (float)PuzzleImage.width);


        for (int CCArray = 0; CCArray < PiecesInCol * PiecesInRow; CCArray++)
        {
            _ChildColliders[CCArray] = Object.Instantiate(_ChildCollider) as GameObject;

            BoxCollider CollisionDetector = _ChildColliders[CCArray].GetComponent<BoxCollider>();

            //Set colliders size
            CollisionDetector.size = new Vector3(0.2f, ((float)PuzzleImage.height / (float)PuzzleImage.width) * 0.2f, 1f);
            
            _ChildColliders[CCArray].name = "BackgroundCollider" + CCArray.ToString();


            float CalculatedX = transform.position.x - (transform.localScale.x / 2) + 
                                     PieceWidthInBackground * Mathf.Repeat(CCArray, PiecesInRow) + (PieceWidthInBackground/2);
            float CalculatedY = transform.position.y - (transform.localScale.y/2) + 
                                     (PieceHeightInBackground * (CCArray/PiecesInRow)) + (PieceHeightInBackground/2);


            _ChildColliders[CCArray].transform.position = new Vector3( CalculatedX, CalculatedY, transform.position.z - 0.1f );
            _ChildColliders[CCArray].transform.parent = transform;
            
        }

        Destroy(_ChildCollider);


#region "Pieces Initialization And Placement"

        float PieceZ = transform.position.z - (PiecesInRow * PiecesInCol) * 0.01f;
        float PieceX = transform.position.x - (transform.localScale.x/2) - PieceWidthInBackground;
        float PieceY = transform.position.y - (transform.localScale.y / 2) + PieceHeightInBackground;

        for (int RowTrav = 0; RowTrav < PiecesInCol; RowTrav++)
        {
            for (int ColTrav = 0; ColTrav < PiecesInRow; ColTrav++, PieceZ -= 0.01f)
            {
                Texture2D Img = _PuzzleMaker._CreatedImagePiecesData[RowTrav, ColTrav].PieceImage;

                float PieceScaleX = (float)Img.width / (float)_PuzzleMaker.PieceWidthWithoutJoint;
                float PieceScaleY = PieceHeightInBackground * ((float)Img.height / (float)_PuzzleMaker.PieceHeightWithoutJoint );


                //Initialize all pieces with images and resize appropriately
                GameObject Temp = Instantiate(PuzzlePiecePrefab) as GameObject;
                Temp.GetComponent<Renderer>().material.mainTexture = Img;

                Temp.transform.localScale = new Vector3(PieceScaleX, PieceScaleY, 1);

                //Place pieces
                Temp.transform.position = new Vector3(PieceX, PieceY, PieceZ);

                //Assign name
                Temp.name = "Piece" + ((RowTrav * PiecesInRow) + ColTrav).ToString();

                //Enable piece
                Temp.SetActive(true);

                _PuzzlePieces[(RowTrav * PiecesInRow) + ColTrav] = Temp;

            }
        }

#endregion


#region "Piece Shuffling"

        if (ShufflePieces)
        {
            //Shuffle pieces position using Fisher-Yates shuffle algorithm
            Vector3[] TempPosHolder = new Vector3[_PuzzlePieces.Length];

            for (int i = 0; i < _PuzzlePieces.Length; i++)
                TempPosHolder[i] = _PuzzlePieces[i].transform.position;

            Random.seed = System.DateTime.Now.Second;

            for (int i = _PuzzlePieces.Length - 1; i > 0; i--)
            {
                int RndPos = Random.Range(0, i - 1);
                
                Vector3 Swapvar = TempPosHolder[i];
                TempPosHolder[i] = TempPosHolder[RndPos];
                TempPosHolder[RndPos] = Swapvar;

                //Apply changed location to piece
                _PuzzlePieces[i].transform.position = TempPosHolder[i];
            }

        }

#endregion


        //Make all puzzle pieces child of gameobjects
        for (int i = 0; i < _PuzzlePieces.Length; i++)
        {
            _PuzzlePieces[i].transform.parent = transform;

            //Hold positions for pieces
            _PuzzlePiecesOrigPos[i] = _PuzzlePieces[i].transform.localPosition;

        }


        //Translate everything back to its origional position
        transform.position = OrigionalPosition;
        transform.rotation = Quaternion.Euler( OrigionalRotation );

        gameObject.GetComponent<Renderer>().enabled = true;


#region "Camera  Resizing And Placement"

        //Resize camera to display whole puzzle in camera view
        float widthToBeSeen = gameObject.GetComponent<Renderer>().bounds.size.x + PieceWidthInBackground * 1.35f;
        float heightToBeSeen = gameObject.GetComponent<Renderer>().bounds.size.y;

        parentCam.orthographicSize = widthToBeSeen > heightToBeSeen ? widthToBeSeen * 0.4f : heightToBeSeen * 0.4f;


        //parentCamTransform.position = new Vector3(CalcCameraX, CalcCameraY, _pieceInstances[1][0].transform.position.z - 3);
        parentCamTransform.position = new Vector3(0.28f, 0, transform.position.z - 10);


#endregion
        
	}

    void Update()
    {

        //Move holding piece w.r.t mouse position
        if (_CurrentHoldingPiece != null){

            float MousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float MousePositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

            _CurrentHoldingPiece.transform.position = new Vector3(MousePositionX, MousePositionY, 
                                                _CurrentHoldingPiece.transform.position.z) ;
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
                    _CurrentHoldingPieceDPos = _CurrentHoldingPiece.transform.position;

                    _CurrentHoldingPiece.transform.position = new Vector3(_CurrentHoldingPieceDPos.x, _CurrentHoldingPieceDPos.y,
                                                                            transform.position.z);
                }

            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_CurrentHoldingPiece != null)
            {
                _CurrentHoldingPiece.transform.position = _CurrentHoldingPieceDPos;
                _CurrentHoldingPiece = null;
            }
        }

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 30), "Reset"))
            Reset();
    }



    /// <summary>
    /// This method is called throw SendMessage from piece when it collides with appropriate collider for its placement.
    /// </summary>
    /// <param name="ObjNum">Number of collider this piece found its place with</param>
    private void PiecePlaceFound(int ObjNum)
    {

        if (_CurrentHoldingPiece != null)
        {
            
            int PieceRow = 0;
            int PieceCol = 0;

            if (ArrayPosToRC(ObjNum, PiecesInCol, PiecesInRow, out PieceRow, out PieceCol))
            {
                //Get this piece joints information for placement position calculation
                SPieceInfo TempPieceInfo = _PuzzleMaker._CreatedImagePiecesData[PieceRow, PieceCol].PieceMetaData;

                SJointInfo[] TempJointsInfo = TempPieceInfo.GetJoints();

                Vector3 PieceColliderPosition = _ChildColliders[ObjNum].transform.position;

                Vector3 CalculatedPosition = new Vector3();


#region "Calculate Adjusted X Position For Piece Placement"

                // Calculate X Position
                // Default scaleX of piece is 1 in world
                bool LeftJointPresent = false;
                bool RightJointPresent = false;

                SJointInfo TempLeftJointInfo = TempPieceInfo.GetJoint(EJointPosition.Left, out LeftJointPresent);
                SJointInfo TempRightJointInfo = TempPieceInfo.GetJoint(EJointPosition.Right, out RightJointPresent);

                EJointType TempLeftJointType = TempLeftJointInfo.JointType;
                EJointType TempRightJointType = TempRightJointInfo.JointType;

                float LeftJointWorldScale = (float)TempLeftJointInfo.JointWidth / (float)_PuzzleMaker.PieceWidthWithoutJoint;
                float RightJointWorldScale = (float)TempRightJointInfo.JointWidth / (float)_PuzzleMaker.PieceWidthWithoutJoint;

                if (LeftJointPresent && RightJointPresent && TempLeftJointType == EJointType.Male && TempRightJointType == EJointType.Male)
                {
                    CalculatedPosition.x = _ChildColliders[ObjNum].transform.position.x + (RightJointWorldScale/2) - (LeftJointWorldScale/2);
                }
                else if (LeftJointPresent && TempLeftJointType == EJointType.Male)
                {
                    CalculatedPosition.x = _ChildColliders[ObjNum].transform.position.x - (LeftJointWorldScale/2f);
                }
                else if (RightJointPresent && TempRightJointType == EJointType.Male)
                {
                    CalculatedPosition.x = _ChildColliders[ObjNum].transform.position.x + (RightJointWorldScale / 2f);
                }
                else
                {
                    CalculatedPosition.x = _ChildColliders[ObjNum].transform.position.x;
                }

#endregion

#region "Calculate Adjusted Y Position For Piece Placement"

                // Calculate Y Position
                // Default scaleY of piece is calculated with aspect ratio of image and ScaleX = 1

                bool TopJointPresent = false;
                bool BottomJointPresent = false;

                SJointInfo TempTopJointInfo = TempPieceInfo.GetJoint(EJointPosition.Top, out TopJointPresent);
                SJointInfo TempBottomJointInfo = TempPieceInfo.GetJoint(EJointPosition.Bottom, out BottomJointPresent);

                EJointType TempTopJointType = TempTopJointInfo.JointType;
                EJointType TempBottomJointType = TempBottomJointInfo.JointType;

                float TopJointWorldScale = (float)TempTopJointInfo.JointHeight / (float)_PuzzleMaker.PieceWidthWithoutJoint;
                float BottomJointWorldScale = (float)TempBottomJointInfo.JointHeight / (float)_PuzzleMaker.PieceWidthWithoutJoint;


                if (TopJointPresent && BottomJointPresent && TempTopJointType == EJointType.Male && TempBottomJointType == EJointType.Male)
                {
                    CalculatedPosition.y = _ChildColliders[ObjNum].transform.position.y + (TopJointWorldScale / 2) - (BottomJointWorldScale / 2);
                }
                else if (TopJointPresent && TempTopJointType == EJointType.Male)
                {
                    CalculatedPosition.y = _ChildColliders[ObjNum].transform.position.y + (TopJointWorldScale / 2);
                }
                else if (BottomJointPresent && TempBottomJointType == EJointType.Male)
                {
                    CalculatedPosition.y = _ChildColliders[ObjNum].transform.position.y - (BottomJointWorldScale / 2);
                }
                else
                {
                    CalculatedPosition.y = _ChildColliders[ObjNum].transform.position.y;
                }

#endregion


                // Z position
                CalculatedPosition.z = _ChildColliders[ObjNum].transform.position.z;


                // _CurrentHoldingPiece.transform.position = _ChildColliders[ObjNum].transform.position;
                _CurrentHoldingPiece.transform.position = CalculatedPosition;

                //Disable this piece
                _CurrentHoldingPiece.GetComponent<CharacterController>().enabled = false;

                //Call event function
                OnPiecePlaced(_CurrentHoldingPiece, ObjNum);

                _CurrentHoldingPiece = null;

                //Update trackers
                _NoOfPiecesPlaced++;

                //Check for Puzzle complete
                if (_NoOfPiecesPlaced >= _PuzzlePieces.Length)
                    OnPuzzleComplete();

            }


        }

    }

    /// <summary>
    /// Convert position of element in single dimensional array to Row & Col in multidimensional array
    /// </summary>
    /// <param name="ElementPosInArray">Position of element inside one dimensional array ,minimum value is 0</param>
    /// <param name="TotalRows">Total rows in multidimensional array ,minimum value is 1</param>
    /// <param name="TotalCols">Total cols in multidimensional array ,minimum value is 1</param>
    /// <param name="ElementRow">Output calculated Row position in multidimensional array ,minimum value is 0</param>
    /// <param name="ElementCol">Output calculated Col position in multidimensional array ,minimum value is 0</param>
    /// <returns>Whether calculation is successfull or not</returns>
    public static bool ArrayPosToRC(int ElementPosInArray, int TotalRows, 
                                        int TotalCols, out int ElementRow, out int ElementCol)
    {
        ElementRow = -1;
        ElementCol = -1;

        if (TotalRows < 1 || TotalCols < 1 || ElementPosInArray < 0 || ElementPosInArray > TotalRows * TotalCols - 1)
            return false;

        ElementRow = ElementPosInArray / TotalRows;

        ElementCol = ElementPosInArray % TotalCols;

        return true;
    }



    /// <summary>
    /// Trigered when user places all the pieces in correct order inside puzzle.
    /// </summary>
    public void OnPuzzleComplete()
    {
        Debug.Log("Puzzle Completed");
    }

    /// <summary>
    /// Triggered when piece has been placed successfully in its place inside puzzle
    /// </summary>
    /// <param name="PieceInstance">Gameobject instance of piece placed in puzzle</param>
    /// <param name="PieceNum">Number of this piece in sequential array</param>
    public void OnPiecePlaced(GameObject PieceInstance, int PieceNum)
    {
        Debug.Log("Piece number " + PieceNum + " placed");
    }

    /// <summary>
    /// Resets puzzle to start
    /// </summary>
    public void Reset()
    {
        _NoOfPiecesPlaced = 0;

        //Reset all puzzle pieces
        for (int i = 0; i < _PuzzlePieces.Length; i++)
        {
            _PuzzlePieces[i].transform.localPosition = _PuzzlePiecesOrigPos[i];

            _PuzzlePieces[i].GetComponent<CharacterController>().enabled = true;
        }

    }


}
