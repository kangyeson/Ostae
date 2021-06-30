using AnimationCurveExtend;
using System;
using UnityEngine;
using PositionState = ListPositionCtrl.PositionState;

public interface IMovementCtrl
{
	void SetMovement(float baseValue, bool flag);
	bool IsMovementEnded();
	float GetDistance(float deltaTime);
}

/* 자유 이동을 위한 이동 제어
 *
 * 이동 상태는 세 가지
 * - Dragging: 이동 거리가 드래그 거리와 같음
 * - Released: 드래그 후 리스트가 공개되면 이동 거리는 방출 속도 및 속도 인자 곡선에 의해 결정
 * - Aligning: 정렬 옵션이 설정되어 있거나 목록이 끝에 도달한 경우, 선형 모드에서는 이동이 이 상태로 전환되어 목록을 작성, 원하는 위치로 이동
 */
public class FreeMovementCtrl : IMovementCtrl
{
	//자유 이동을 위한 이동
	private readonly VelocityMovement _releasingMovement;
	//목록 정렬을 위한 이동
	private readonly DistanceMovement _aligningMovement;
	//리스트가 Draging중인지
	private bool _isDragging;
	//dragging 거리
	private float _draggingDistance;
	//이동 후 목록을 정렬해야 하는지
	private readonly bool _toAlign;
	//끝을 초과하는 목록 거리
	private float _overGoingDistance;
	//리스트가 얼마나 끝을 초과할 수 있는지
	private readonly float _overGoingDistanceThreshold;
	//목록을 정렬하기 위해 목록을 중지하는 속도 임계값, _alignMiddle'이 참일 때 사용
	private const float _stopVelocityThreshold = 200.0f;
	//목록을 정렬할 거리를 계산하는 함수
	private readonly Func<float> _getAligningDistance;
	//목록 위치의 상태를 가져오는 함수
	private readonly Func<PositionState> _getPositionState;

	public FreeMovementCtrl(AnimationCurve releasingCurve, bool toAlign,
		float overGoingDistanceThreshold,
		Func<float> getAligningDistance, Func<PositionState> getPositionState)
	{
		_releasingMovement = new VelocityMovement(releasingCurve);
		_aligningMovement = new DistanceMovement(
			AnimationCurve.EaseInOut(0.0f, 0.0f, 0.25f, 1.0f));
		_toAlign = toAlign;
		_overGoingDistanceThreshold = overGoingDistanceThreshold;
		_getAligningDistance = getAligningDistance;
		_getPositionState = getPositionState;
	}

	/* 새 이동의 기본 값 설정
	 *
	 * 값 'isDragging'이 참이면 이 값은 드래그 거리임. 그렇지 않은 경우 이 값은 해제 이동의 기본 속도.
	 	is Dragging 목록이 드래그되고 있는지
	 */
	public void SetMovement(float value, bool isDragging)
	{
		if (isDragging) {
			_isDragging = true;
			_draggingDistance = value;

			// dragging을 시작할 때 마지막 이동 종료
			if (!_releasingMovement.IsMovementEnded())
				_releasingMovement.EndMovement();
		} else if (_getPositionState() != PositionState.Middle) {
			_aligningMovement.SetMovement(_getAligningDistance());
		} else {
			_releasingMovement.SetMovement(value);
		}
	}


	public bool IsMovementEnded()
	{
		return !_isDragging &&
		       _aligningMovement.IsMovementEnded() &&
		       _releasingMovement.IsMovementEnded();
	}

	public float GetDistance(float deltaTime)
	{
		var distance = 0.0f;

		if (_isDragging) {
			_isDragging = false;
			distance = _draggingDistance;

			if (IsGoingTooFar(_draggingDistance)) {
				var threshold = _overGoingDistanceThreshold * Mathf.Sign(_overGoingDistance);
				distance -= _overGoingDistance - threshold;
			}
		}
		else if (!_aligningMovement.IsMovementEnded()) {
			distance = _aligningMovement.GetDistance(deltaTime);
		}
		else if (!_releasingMovement.IsMovementEnded()) {
			distance = _releasingMovement.GetDistance(deltaTime);

			if (NeedToAlign(distance)) {
				_releasingMovement.EndMovement();

				_aligningMovement.SetMovement(_getAligningDistance());
				distance = _aligningMovement.GetDistance(deltaTime);
			}
		}

		return distance;
	}


	private bool NeedToAlign(float distance)
	{
		return IsGoingTooFar(distance) ||
		       (_toAlign &&
		        Mathf.Abs(_releasingMovement.lastVelocity) < _stopVelocityThreshold);
	}

	private bool IsGoingTooFar(float distance)
	{
		if (_getPositionState() == PositionState.Middle)
			return false;

		_overGoingDistance = -1 * _getAligningDistance();
		return Mathf.Abs(_overGoingDistance += distance) > _overGoingDistanceThreshold;
	}
}

public class UnitMovementCtrl : IMovementCtrl
{

	private readonly DistanceMovement _unitMovement;

	private readonly DistanceMovement _bouncingMovement;

	private readonly float _bouncingDeltaPos;

	private readonly Func<float> _getAligningDistance;

	private readonly Func<PositionState> _getPositionState;


	public UnitMovementCtrl(AnimationCurve movementCurve, float bouncingDeltaPos,
		Func<float> getAligningDistance, Func<PositionState> getPositionState)
	 {
		var bouncingCurve = new AnimationCurve(
			new Keyframe(0.0f, 0.0f, 0.0f, 5.0f),
			new Keyframe(0.125f, 1.0f, 0.0f, 0.0f),
			new Keyframe(0.25f, 0.0f, -5.0f, 0.0f));

		_unitMovement = new DistanceMovement(movementCurve);
		_bouncingMovement = new DistanceMovement(bouncingCurve);
		_bouncingDeltaPos = bouncingDeltaPos;
		_getAligningDistance = getAligningDistance;
		_getPositionState = getPositionState;
	 }

	public void SetMovement(float distanceAdded, bool flag)
	{
		if (!_bouncingMovement.IsMovementEnded())
			return;

		var state = _getPositionState();
		var movingDirection = Mathf.Sign(distanceAdded);

		if ((state == PositionState.Top && movingDirection < 0) ||
		    (state == PositionState.Bottom && movingDirection > 0)) {
			_bouncingMovement.SetMovement(movingDirection * _bouncingDeltaPos);
		} else {
			distanceAdded += _unitMovement.distanceRemaining;
			_unitMovement.SetMovement(distanceAdded);
		}
	}

	public bool IsMovementEnded()
	{
		return _bouncingMovement.IsMovementEnded() &&
		       _unitMovement.IsMovementEnded();
	}

	public float GetDistance(float deltaTime)
	{
		var distance = 0.0f;

		if (!_bouncingMovement.IsMovementEnded()) {
			distance = _bouncingMovement.GetDistance(deltaTime);
		} else {
			distance = _unitMovement.GetDistance(deltaTime);

			if (NeedToAlign(distance)) {
				_unitMovement.EndMovement();

				_bouncingMovement.SetMovement(-1 * _getAligningDistance());
				_bouncingMovement.GetDistance(0.125f);
				distance = _bouncingMovement.GetDistance(deltaTime);
			}
		}

		return distance;
	}

	private bool NeedToAlign(float deltaDistance)
	{
		if (_getPositionState() == PositionState.Middle)
			return false;

		return Mathf.Abs(_getAligningDistance() * -1 + deltaDistance) > _bouncingDeltaPos ||
		        _unitMovement.IsMovementEnded();
	}
}

internal class VelocityMovement
{
	private readonly DeltaTimeCurve _velocityFactorCurve;
	private float _baseVelocity;
	public float lastVelocity { get; private set; }

	public VelocityMovement(AnimationCurve factorCurve)
	{
		_velocityFactorCurve = new DeltaTimeCurve(factorCurve);
	}

	public void SetMovement(float baseVelocity)
	{
		_velocityFactorCurve.Reset();
		_baseVelocity = baseVelocity;
		lastVelocity = _velocityFactorCurve.CurrentEvaluate() * _baseVelocity;
	}
	public bool IsMovementEnded()
	{
		return _velocityFactorCurve.IsTimeOut();
	}
	public void EndMovement()
	{
		_velocityFactorCurve.Evaluate(_velocityFactorCurve.timeTotal);
	}
	public float GetDistance(float deltaTime)
	{
		lastVelocity = _velocityFactorCurve.Evaluate(deltaTime) * _baseVelocity;
		return lastVelocity * deltaTime;
	}
}

internal class DistanceMovement
{
	private readonly DeltaTimeCurve _distanceFactorCurve;
	private float _distanceTotal;
	private float _lastDistance;
	public float distanceRemaining
	{
		get { return _distanceTotal - _lastDistance; }
	}
	public DistanceMovement(AnimationCurve factorCurve)
	{
		_distanceFactorCurve = new DeltaTimeCurve(factorCurve);
	}
	public void SetMovement(float totalDistance)
	{
		_distanceFactorCurve.Reset();
		_distanceTotal = totalDistance;
		_lastDistance = 0.0f;
	}

	public bool IsMovementEnded()
	{
		return _distanceFactorCurve.IsTimeOut();
	}

	// 시간 초과로 강제로 이동 종료
	public void EndMovement()
	{
		_distanceFactorCurve.Evaluate(_distanceFactorCurve.timeTotal);
		_lastDistance = _distanceTotal;
	}

	public float GetDistance(float deltaTime)
	{
		var nextDistance = _distanceTotal * _distanceFactorCurve.Evaluate(deltaTime);
		var deltaDistance = nextDistance - _lastDistance;

		_lastDistance = nextDistance;
		return deltaDistance;
	}
}
