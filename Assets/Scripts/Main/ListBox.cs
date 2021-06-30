//스크롤 목록의 기본 구성 요소.
//위치를 제어하고 목록 요소의 내용을 업데이트함
using System;
using UnityEngine;
using UnityEngine.UI;

public class ListBox : MonoBehaviour
{
	// 이 공개 변수들은 ListPositionCtrl.InitializeBoxDependency()에서 초기화됨
	[HideInInspector] public int listBoxID; // `listBoxes`의 순서와 동일함
	[HideInInspector] public ListBox lastListBox;
	[HideInInspector] public ListBox nextListBox;
	private int _contentID;

	private ListPositionCtrl _positionCtrl;
	private BaseListBank _listBank;
	private CurveResolver _positionCurve;
	private CurveResolver _scaleCurve;

	public Action<float> UpdatePosition { private set; get; }

	/* ====== 위치 변수 ====== */
	// 여기서 계산된 위치가 목록의 로컬 공간에 있음
	private float _unitPos; // 상자 사이의 거리
	private float _lowerBoundPos; // 상자의 가장 왼쪽/아래쪽 위치
	private float _upperBoundPos; // box_changeSide(Lower/Upper)BoundPos의 오른쪽/맨 위 위치는 상자를 다른 끝으로 이동할지 여부를 확인하기 위한 경계
	private float _changeSideLowerBoundPos;
	private float _changeSideUpperBoundPos;

	//상자의 정보를 디버그 로그로 출력
	public void ShowBoxInfo()
	{
		Debug.Log("Box ID: " + listBoxID.ToString() +
		          ", Content ID: " + _contentID.ToString() +
		          ", Content: " + _listBank.GetListContent(_contentID));
	}

	//상자의 내용 ID 가져오기
	public int GetContentID()
	{
		return _contentID;
	}

	// 상자를 초기화
	public void Initialize(ListPositionCtrl listPositionCtrl)
	{
		_positionCtrl = listPositionCtrl;
		_listBank = _positionCtrl.listBank;

		switch (_positionCtrl.direction) {
			case ListPositionCtrl.Direction.Vertical:
				UpdatePosition = MoveVertically;
				break;
			case ListPositionCtrl.Direction.Horizontal:
				UpdatePosition = MoveHorizontally;
				break;
		}

		_unitPos = _positionCtrl.unitPos;
		_lowerBoundPos = _positionCtrl.lowerBoundPos;
		_upperBoundPos = _positionCtrl.upperBoundPos;
		_changeSideLowerBoundPos = _lowerBoundPos + _unitPos * 0.5f;
		_changeSideUpperBoundPos = _upperBoundPos - _unitPos * 0.5f;

		_positionCurve = new CurveResolver(
			_positionCtrl.boxPositionCurve,
			_changeSideLowerBoundPos, _changeSideUpperBoundPos);
		_scaleCurve = new CurveResolver(
			_positionCtrl.boxScaleCurve,
			_changeSideLowerBoundPos, _changeSideUpperBoundPos);

		InitialPosition();
		InitialContent();
		AddClickEvent();
	}

	//Button.onClick 클릭된 상자의 내용 ID를 ListPositionCtrl.onBoxClick에 등록된 이벤트 핸들러에 전달
	private void AddClickEvent()
	{
		Button button = transform.GetComponent<Button>();
		if (button != null)
			button.onClick.AddListener(() => _positionCtrl.onBoxClick.Invoke(_contentID));
	}

	//ID에 따라 목록 상자의 로컬 위치를 초기화
	private void InitialPosition()
	{
		int numOfBoxes = _positionCtrl.listBoxes.Length;
		float majorPosition = _unitPos * (listBoxID * -1 + numOfBoxes / 2);
		float passivePosition;

		// 상자 수가 짝수인 경우 1/2 유닛 Pos 다운 위치를 조절
		if ((numOfBoxes & 0x1) == 0) {
			majorPosition = _unitPos * (listBoxID * -1 + numOfBoxes / 2) - _unitPos / 2;
		}

		passivePosition = GetPassivePosition(majorPosition);

		switch (_positionCtrl.direction) {
			case ListPositionCtrl.Direction.Vertical:
				transform.localPosition = new Vector3(
					passivePosition, majorPosition, transform.localPosition.z);
				break;
			case ListPositionCtrl.Direction.Horizontal:
				transform.localPosition = new Vector3(
					majorPosition, passivePosition, transform.localPosition.z);
				break;
		}

		UpdateScale(majorPosition);
	}

	/* 상자를 수직으로 이동하고 최종 위치와 크기를 조정
	 * 이 기능은 수직 모드의 업데이트 위치
	 * @param delta The moving distance
	 */
	private void MoveVertically(float delta)
	{
		bool needToUpdateToLastContent = false;
		bool needToUpdateToNextContent = false;
		float majorPosition = GetMajorPosition(transform.localPosition.y + delta,
			ref needToUpdateToLastContent, ref needToUpdateToNextContent);
		float passivePosition = GetPassivePosition(majorPosition);

		transform.localPosition = new Vector3(
			passivePosition, majorPosition, transform.localPosition.z);
		UpdateScale(majorPosition);

		if (needToUpdateToLastContent)
			UpdateToLastContent();
		else if (needToUpdateToNextContent)
			UpdateToNextContent();
	}

	/* 상자를 수평으로 이동하고 최종 위치와 크기를 조정
	 * 이 기능은 수평 모드의 업데이트 위치
	 * @param delta The moving distance
	 */
	private void MoveHorizontally(float delta)
	{
		bool needToUpdateToLastContent = false;
		bool needToUpdateToNextContent = false;
		float majorPosition = GetMajorPosition(transform.localPosition.x + delta,
			ref needToUpdateToLastContent, ref needToUpdateToNextContent);
		float passivePosition = GetPassivePosition(majorPosition);

		transform.localPosition = new Vector3(
			majorPosition, passivePosition, transform.localPosition.z);
		UpdateScale(majorPosition);

		if (needToUpdateToLastContent)
			UpdateToLastContent();
		else if (needToUpdateToNextContent)
			UpdateToNextContent();
	}

	/* 요청된 위치에 따라 majon 위치를 가져옴
	 * 상자가 경계를 초과할 경우 전달된 플래그 중 하나가 내용을 업데이트해야 함을 나타내도록 설정됨
	 * @param positionValue The requested position
	 * @param needToUpdateToLastContent Is it need to update to the last content?
	 * @param needToUpdateToNextContent Is it need to update to the next content?
	 * @return The decided major position
	 */
	private float GetMajorPosition(float positionValue,
		ref bool needToUpdateToLastContent, ref bool needToUpdateToNextContent)
	{
		float beyondPos = 0.0f;
		float majorPos = positionValue;

		if (positionValue < _changeSideLowerBoundPos) {
			beyondPos = positionValue - _lowerBoundPos;
			majorPos = _upperBoundPos - _unitPos + beyondPos;
			needToUpdateToLastContent = true;
		} else if (positionValue > _changeSideUpperBoundPos) {
			beyondPos = positionValue - _upperBoundPos;
			majorPos = _lowerBoundPos + _unitPos + beyondPos;
			needToUpdateToNextContent = true;
		}

		return majorPos;
	}

	//major 위치에 따라 수동 위치를 파악
	private float GetPassivePosition(float majorPosition)
	{
		float passivePosFactor = _positionCurve.Evaluate(majorPosition);
		return _upperBoundPos * passivePosFactor;
	}

	// major 위치에 따라 listBox 크기 조정
	private void UpdateScale(float majorPosition)
	{
		float scaleValue = _scaleCurve.Evaluate(majorPosition);
		transform.localScale = new Vector3(scaleValue, scaleValue, transform.localScale.z);
	}

	// ListBox의 내용을 초기화
	private void InitialContent()
	{
		// 가운데 상자의 내용 ID 가져오기
		_contentID = _positionCtrl.centeredContentID;

		// 최초 순서에 따라 contentID 조정 
		_contentID += listBoxID - _positionCtrl.listBoxes.Length / 2;

		// 선형 모드에서 필요한 경우 상자를 비활성화
		if (_positionCtrl.listType == ListPositionCtrl.ListType.Linear) {
			// 목록의 맨 위에 있는 상자를 비활성화하여 내용물의 끝에 항목을 고정
			if (_contentID < 0) {
				_positionCtrl.numOfUpperDisabledBoxes += 1;
				gameObject.SetActive(false);
			}
			// 목록의 하단에서 반복되는 항목을 저장할 상자를 비활성화
			else if (_contentID >= _listBank.GetListLength()) {
				_positionCtrl.numOfLowerDisabledBoxes += 1;
				gameObject.SetActive(false);
			}
		}

		// content id를 반올림
		while (_contentID < 0)
			_contentID += _listBank.GetListLength();
		_contentID = _contentID % _listBank.GetListLength();

		//UpdateDisplayContent();
	}

	// ListBox에서 표시 내용을 업데이트
	// private void UpdateDisplayContent()
	// {
	// 	// contentID에 따라 내용 업데이트
	// 	content.text = _listBank.GetListContent(_contentID);
	// }

	// 내용을 다음 ListBox의 마지막 내용으로 업데이트
	private void UpdateToLastContent()
	{
		_contentID = nextListBox.GetContentID() - 1;
		_contentID = (_contentID < 0) ? _listBank.GetListLength() - 1 : _contentID;

		if (_positionCtrl.listType == ListPositionCtrl.ListType.Linear) {
			if (_contentID == _listBank.GetListLength() - 1 ||
			    !nextListBox.isActiveAndEnabled) {
				// 상자가 다른 쪽에서 비활성화된 경우 다른 쪽의 counter를 줄임
				if (!isActiveAndEnabled)
					--_positionCtrl.numOfLowerDisabledBoxes;

				// 선형 모드에서는 다른 끝의 content를 표시하지 않음
				gameObject.SetActive(false);
				++_positionCtrl.numOfUpperDisabledBoxes;
			} else if (!isActiveAndEnabled) {
				// 다음 상자가 활성화된 경우 다른 끝의 비활성화된 상자가 다시 활성화됨
				gameObject.SetActive(true);
				--_positionCtrl.numOfLowerDisabledBoxes;
			}
		}

		//UpdateDisplayContent();
	}

	// 마지막 ListBox의 다음 컨텐츠로 컨텐츠 업데이트
	private void UpdateToNextContent()
	{
		_contentID = lastListBox.GetContentID() + 1;
		_contentID = (_contentID == _listBank.GetListLength()) ? 0 : _contentID;

		if (_positionCtrl.listType == ListPositionCtrl.ListType.Linear) {
			if (_contentID == 0 || !lastListBox.isActiveAndEnabled) {
				if (!isActiveAndEnabled)
					--_positionCtrl.numOfUpperDisabledBoxes;

				// 선형 모드에서는 다른 끝의 내용을 표시하지 않음
				gameObject.SetActive(false);
				++_positionCtrl.numOfLowerDisabledBoxes;
			} else if (!isActiveAndEnabled) {
				gameObject.SetActive(true);
				--_positionCtrl.numOfUpperDisabledBoxes;
			}
		}

		//UpdateDisplayContent();
	}

	//최종 값을 평가하기 위해 AnimationCurve에 적합하도록 사용자 정의 범위를 변환하는 클래스
	private class CurveResolver
	{
		private AnimationCurve _curve;
		private float _maxValue;
		private float _minValue;

		public CurveResolver(AnimationCurve curve, float minValue, float maxValue)
		{
			_curve = curve;
			_minValue = minValue;
			_maxValue = maxValue;
		}

		//입력 값을 [minValue, maxValue] 사이의 보간 값으로 변환하고 결과를 곡선에 전달하여 최종 값을 가져옴
		public float Evaluate(float value)
		{
			float lerpValue = Mathf.InverseLerp(_minValue, _maxValue, value);
			return _curve.Evaluate(lerpValue);
		}
	}
}
