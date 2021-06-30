using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IControlEventHandler:
	IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{}

//클릭된 ListBox에서 전송된 onClick 이벤트를 전달하기 위한 콜백입니다.
//int 매개 변수는 누른 ListBox가 보관하는 내용의 ID가 됩니다.
[System.Serializable]
public class ListBoxClickEvent : UnityEvent<int>
{}

//목록의 이벤트에 대한 콜백입니다.
//ListPositionCtrl 파라미터는 이벤트를 발생시키는 목록입니다.
[System.Serializable]
public class ListEvent : UnityEvent<ListPositionCtrl>
{}

public class ListPositionCtrl : MonoBehaviour, IControlEventHandler
{
	public enum ListType
	{
		Circular,
		Linear
	};

	public enum ControlMode
	{
		Year,
		Drag,       // 마우스 포인터 or 손가락 터치
		Function  // MoveOneUnitUp/MoveOneUnitDown 기능을 호출했을때
	};

	public enum Direction
	{
		Vertical,
		Horizontal
	};

	public enum PositionState
	{
		Top,	
		Middle,	
		Bottom	
	};

	/*========== Settings ==========*/
	public ListType listType = ListType.Circular; //리스트 타입(Circular, Linear)
	public ControlMode controlMode = ControlMode.Drag; //목록의 제어 모드(Drag, Function)
	public bool alignMiddle = false; //슬라이딩 후 목록 가운데에 상자가 정렬되어야 하는지
	public Direction direction = Direction.Vertical; //목록의 주요 이동 방향(Vertical, Horizontal)
	public BaseListBank listBank; //목록에 대한 content bank를 보유하는 게임 개체(BaseList Bank의 파생 클래스가 될 것임)
	public int centeredContentID = 0; //가운데 상자의 초기 내용 ID를 지정
	public ListBox[] listBoxes; //리스트에 속하는 박스들
	public float boxDensity = 2.0f; //각 상자 사이의 거리
	public AnimationCurve boxPositionCurve = new AnimationCurve(
		new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 0.0f));
	public AnimationCurve boxScaleCurve = new AnimationCurve(
		new Keyframe(0.0f, 1.0f), new Keyframe(1.0f, 1.0f));
	public AnimationCurve boxMovementCurve = new AnimationCurve(
		new Keyframe(0.0f, 1.0f, 0.0f, -2.5f),
		new Keyframe(1.0f, 0.0f, 0.0f, 0.0f));
	public ListBoxClickEvent onBoxClick;
	// 목록이 이동 중일 때 콜백이 호출
	public ListEvent onListMove;

	/*===============================*/
	private Canvas _parentCanvas; //스크롤 목록이 있는 캔버스 평면.
	private float _canvasMaxPos; //현재 캔버스 평면 공간에 위치 제약 조건.
	public float unitPos { get; private set; }
	public float lowerBoundPos { get; private set; }
	public float upperBoundPos { get; private set; }

	// 위임 함수
	private Action<PointerEventData, TouchPhase> _inputPositionHandler;
	private Action<Vector2> _scrollHandler;

	// listBoxes 이동 변수
	private IMovementCtrl _movementCtrl;
	// 현재 목록 공간의 마우스/손가락 위치를 입력
	private float _deltaInputPos;
	private float _deltaDistanceToCenter = 0.0f;

	// linear mode에 대한 변수
	private PositionState _positionState = PositionState.Middle;
	[HideInInspector]
	public int numOfUpperDisabledBoxes = 0;
	[HideInInspector]
	public int numOfLowerDisabledBoxes = 0;
	private int _maxNumOfDisabledBoxes = 0;


	//ListBox는 여기서 변수를 초기화하므로 ListPositionCtrl은 ListBox보다 먼저 실행되어야 함 -> inspector에서 실행 순서를 설정해야 함
	private void Start()
	{
		Application.targetFrameRate = 60;
		InitializePositionVars();
		InitializeInputFunction();
		InitializeBoxDependency();
		_maxNumOfDisabledBoxes = listBoxes.Length / 2;
		foreach (ListBox listBox in listBoxes)
			listBox.Initialize(this);
		SetUnitMove(25);
	}

	private void InitializePositionVars()
	{
		//캔버스 평면의 reference of
		_parentCanvas = GetComponentInParent<Canvas>();

		//캔버스 공간에서 캔버스 평면의 최대 위치를 구함, 캔버스 공간의 원점이 캔버스 평면의 중심에 있다고 가정
		RectTransform rectTransform = _parentCanvas.GetComponent<RectTransform>();

		switch (direction) {
			case Direction.Vertical:
				_canvasMaxPos = rectTransform.rect.height / 2;
				break;
			case Direction.Horizontal:
				_canvasMaxPos = rectTransform.rect.width / 2;
				break;
		}

		unitPos = _canvasMaxPos / boxDensity;
		lowerBoundPos = unitPos * (-1 * listBoxes.Length / 2 - 1);
		upperBoundPos = unitPos * (listBoxes.Length / 2 + 1);

		// ListBox 수가 짝수인 경우 1 UnitPos의 경계를 좁힙니다.
		if ((listBoxes.Length & 0x1) == 0) {
			lowerBoundPos += unitPos / 2;
			upperBoundPos -= unitPos / 2;
		}
	}

	private void InitializeBoxDependency()
	{
		// 컨테이너 'listBox'의 순서에 따라 상자 ID 설정
		for (int i = 0; i < listBoxes.Length; ++i)
			listBoxes[i].listBoxID = i;

		// 인접 상자 설정
		for (int i = 0; i < listBoxes.Length; ++i) {
			listBoxes[i].lastListBox = listBoxes[(i - 1 >= 0) ? i - 1 : listBoxes.Length - 1];
			listBoxes[i].nextListBox = listBoxes[(i + 1 < listBoxes.Length) ? i + 1 : 0];
		}
	}

	//선택한 제어 모드에 해당하는 핸들러를 초기화
	//사용되지 않은 핸들러에는 이벤트 처리를 방지하기 위한 더미 기능이 할당됨
	private void InitializeInputFunction()
	{
		Func<float> getAligningDistance = () => _deltaDistanceToCenter;
		Func<PositionState> getPositionState = () => _positionState;
		var overGoingThreshold = unitPos * 0.3f;

		switch (controlMode) {
			case ControlMode.Year:
				_movementCtrl = new FreeMovementCtrl(
					boxMovementCurve, alignMiddle, overGoingThreshold,
					getAligningDistance, getPositionState);
				_inputPositionHandler = Dragchoice;
				_scrollHandler = (Vector2 v) => { };
				break;

			case ControlMode.Drag:
				_movementCtrl = new FreeMovementCtrl(
					boxMovementCurve, alignMiddle, overGoingThreshold,
					getAligningDistance, getPositionState);
				_inputPositionHandler = DragPositionHandler;
				_scrollHandler = (Vector2 v) => { };
				break;

			case ControlMode.Function:
				_movementCtrl = new UnitMovementCtrl(
					boxMovementCurve, overGoingThreshold,
					getAligningDistance, getPositionState);
				_inputPositionHandler =
					(PointerEventData pointer, TouchPhase phase) => { };
				_scrollHandler = (Vector2 v) => { };
				break;
		}
	}

	/* ====== 통합 이벤트 시스템의 콜백 함수 ====== */
	public void OnBeginDrag(PointerEventData pointer)
	{
		_inputPositionHandler(pointer, TouchPhase.Began);
	}

	public void OnDrag(PointerEventData pointer)
	{
		_inputPositionHandler(pointer, TouchPhase.Moved);
	}

	public void OnEndDrag(PointerEventData pointer)
	{
		_inputPositionHandler(pointer, TouchPhase.Ended);
	}

	public void OnScroll(PointerEventData pointer)
	{
		_scrollHandler(pointer.scrollDelta);
	}


	//dragging 위치 및 dragging 상태에 따라 목록 이동
	private void DragPositionHandler(PointerEventData pointer, TouchPhase state)
	{
		switch (state) {
			case TouchPhase.Began:
				break;

			case TouchPhase.Moved:
				_deltaInputPos = GetInputCanvasPosition(pointer.delta);
				// 포인터의 이동 거리만큼 목록을 밀어넣음
				_movementCtrl.SetMovement(_deltaInputPos, true);
				break;

			case TouchPhase.Ended:
				_movementCtrl.SetMovement(_deltaInputPos / Time.deltaTime, false);
				break;
		}
	}


	//캔버스 공간에서 입력 위치를 가져와 이동 방향에 따라 해당 축의 값을 반환합니다.
	private float GetInputCanvasPosition(Vector3 pointerPosition)
	{
		switch (direction) {
			case Direction.Vertical:
				return pointerPosition.y / _parentCanvas.scaleFactor;
			case Direction.Horizontal:
				return pointerPosition.x / _parentCanvas.scaleFactor;
			default:
				return 0.0f;
		}
	}


	/* ====== 이동 함수 ====== */
	// listBoxes의 이동제어
	private void Update()
	{
		if (!_movementCtrl.IsMovementEnded()) {
			var distance = _movementCtrl.GetDistance(Time.deltaTime);
			foreach (ListBox listBox in listBoxes)
				listBox.UpdatePosition(distance);
		}
	}

	// 리스트 상태 확인
	private void LateUpdate()
	{
		FindDeltaDistanceToCenter();
	}

	//중심 위치에 가장 가까운 listBox를 찾아 해당 박스와 중심 위치 사이의 델타 x나 y 위치를 계산
	private void FindDeltaDistanceToCenter()
	{
		float minDeltaPos = Mathf.Infinity;
		float deltaPos = 0.0f;

		switch (direction) {
			case Direction.Vertical:
				foreach (ListBox listBox in listBoxes) {
					// linear mode에서 비활성화된 상자 건너뛰기
					if (!listBox.isActiveAndEnabled)
						continue;

					deltaPos = -listBox.transform.localPosition.y;
					if (Mathf.Abs(deltaPos) < Mathf.Abs(minDeltaPos))
						minDeltaPos = deltaPos;
				}
				break;

			case Direction.Horizontal:
				foreach (ListBox listBox in listBoxes) {
					// linear mode에서 비활성화된 상자 건너뛰기
					if (!listBox.isActiveAndEnabled)
						continue;

					deltaPos = -listBox.transform.localPosition.x;
					if (Mathf.Abs(deltaPos) < Mathf.Abs(minDeltaPos))
						minDeltaPos = deltaPos;
				}
				break;
		}

		_deltaDistanceToCenter = minDeltaPos;
	}

	// unit position의 시간 거리에 대한 목록 이동
	private void SetUnitMove(int unit)
	{
		_movementCtrl.SetMovement(unit * unitPos, false);
	}

	// 모든 목록 상자 1 유닛을 위로 이동
	public void MoveOneUnitUp()
	{
		SetUnitMove(6);
	}

	public void Stop(){
		SetUnitMove(0);
	}

	public void MoveOneUnitnext()
	{
		SetUnitMove(1);
	}
	public void MoveOneUnitprev()
	{
		SetUnitMove(-1);
	}


	// 가운데에 가장 가까운 ListBox를 비교해서 가운데 있는 ListBox의 개체를 가져옴
	public ListBox GetCenteredBox()
	{
		float minPosition = Mathf.Infinity;
		float position;
		ListBox candidateBox = null;

		switch (direction) {
			case Direction.Vertical:
				foreach (ListBox listBox in listBoxes) {
					position = Mathf.Abs(listBox.transform.localPosition.y);
					if (position < minPosition) {
						minPosition = position;
						candidateBox = listBox;
					}
				}
				break;
			case Direction.Horizontal:
				foreach (ListBox listBox in listBoxes) {
					position = Mathf.Abs(listBox.transform.localPosition.x);
					if (position < minPosition) {
						minPosition = position;
						candidateBox = listBox;
					}
				}
				break;
		}

		return candidateBox;
	}

	// 가운데 상자의 ID 내용 가져오기
	public int GetCenteredContentID()
	{
		return GetCenteredBox().GetContentID();
	}

	public void Dragchoice(PointerEventData pointer, TouchPhase state)
	{
		switch (state) {
			case TouchPhase.Began:
				GameObject.Find("choicemask").transform.GetChild(0).gameObject.SetActive(true);
			    GameObject.Find("koreamask").transform.GetChild(0).gameObject.SetActive(false);
				GameObject.Find("chinamask").transform.GetChild(0).gameObject.SetActive(false);
				GameObject.Find("japanmask").transform.GetChild(0).gameObject.SetActive(false);
				break;

			case TouchPhase.Moved:
				_deltaInputPos = GetInputCanvasPosition(pointer.delta);
				GameObject.Find("choicemask").transform.GetChild(0).gameObject.SetActive(true);
				GameObject.Find("koreamask").transform.GetChild(0).gameObject.SetActive(false);
				GameObject.Find("chinamask").transform.GetChild(0).gameObject.SetActive(false);
				GameObject.Find("japanmask").transform.GetChild(0).gameObject.SetActive(false);
				_movementCtrl.SetMovement(_deltaInputPos, true);
				break;

			case TouchPhase.Ended:
				if(_movementCtrl.IsMovementEnded() == true){
					Invoke("ListDone", 2);
					
				}
				_movementCtrl.SetMovement(_deltaInputPos / Time.deltaTime, false);
				break;
		}
	}

	public void ListDone(){
		GameObject.Find("choicemask").transform.GetChild(0).gameObject.SetActive(false);
		GameObject.Find("koreamask").transform.GetChild(0).gameObject.SetActive(true);
		GameObject.Find("chinamask").transform.GetChild(0).gameObject.SetActive(true);
		GameObject.Find("japanmask").transform.GetChild(0).gameObject.SetActive(true);
	}

	

}
