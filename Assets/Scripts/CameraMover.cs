﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
/// カメラの移動と操作を管理するクラスです。
/// このクラスは、ユーザーがカメラを操作するための入力を処理します。
/// 
/// 操作方法:
/// - WASD: 前後左右の移動
/// - QE: 上昇・降下
/// - 右ドラッグ: カメラの回転
/// - 左ドラッグ: 前後左右の移動
/// - スペース: カメラ操作の有効・無効の切り替え
/// - F: 回転を実行時の状態に初期化する
/// </summary>
public class CameraMover : MonoBehaviour
{
    // カメラの移動量
    [SerializeField, Range(0.1f, 10.0f)]
    private float _positionStep = 2.0f;

    // カメラの回転量
    [SerializeField, Range(0.1f, 10.0f)]
    private float _rotationStep = 0.5f;

    // マウス感度
    [SerializeField, Range(30.0f, 150.0f)]
    private float _mouseSensitive = 90.0f;

    // カメラ操作の有効無効
    private bool _cameraMoveActive = true;

    // カメラのtransform  
    private Transform _camTransform;

    // マウスの始点 
    private Vector3 _startMousePos;

    // カメラ回転の始点情報
    private Vector3 _presentCamRotation;
    private Vector3 _presentCamPos;

    // 初期状態 Rotation
    private Quaternion _initialCamRotation;
    private Vector3 _initialcamTransform;

    // UIメッセージの表示
    private bool _uiMessageActiv;

    void Start()
    {
        _camTransform = this.gameObject.transform;

        // 初期移動の保存
        _initialcamTransform = this.gameObject.transform.position;

        // 初期回転の保存
        _initialCamRotation = this.gameObject.transform.rotation;
    }

    void Update()
    {
        CamControlIsActive(); // カメラ操作の有効無効

        if (_cameraMoveActive)
        {
            ResetCameraTransform(); // カメラリセット
            CameraRotationMouseControl(); // カメラの回転 マウス
            CameraSlideMouseControl(); // カメラの縦横移動 マウス
            CameraPositionKeyControl(); // カメラのローカル移動 キー
        }
    }

    // カメラ操作の有効無効
    public void CamControlIsActive()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _cameraMoveActive = !_cameraMoveActive;

            if (_uiMessageActiv == false)
            {
                StartCoroutine(DisplayUiMessage());
            }
            Debug.Log("CamControl : " + _cameraMoveActive);
        }
    }

    // 回転を初期状態にする
    private void ResetCameraTransform()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            this.gameObject.transform.rotation = _initialCamRotation;
            this.gameObject.transform.position = _initialcamTransform;
            Debug.Log("Cam Rotate : " + _initialCamRotation.ToString());
        }
    }

    // カメラの回転 マウス
    private void CameraRotationMouseControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startMousePos = Input.mousePosition;
            _presentCamRotation.x = _camTransform.transform.eulerAngles.x;
            _presentCamRotation.y = _camTransform.transform.eulerAngles.y;
        }

        if (Input.GetMouseButton(0))
        {
            //(移動開始座標 - マウスの現在座標) / 解像度 で正規化
            float x = (_startMousePos.x - Input.mousePosition.x) / Screen.width;
            float y = (_startMousePos.y - Input.mousePosition.y) / Screen.height;

            // 回転開始角度 ＋ マウスの変化量 * マウス感度
            float eulerX = _presentCamRotation.x + y * _mouseSensitive;
            float eulerY = _presentCamRotation.y + x * _mouseSensitive;

            _camTransform.rotation = Quaternion.Euler(eulerX, eulerY, 0);
        }
    }

    // カメラの移動 マウス
    private void CameraSlideMouseControl()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _startMousePos = Input.mousePosition;
            _presentCamPos = _camTransform.position;
        }

        if (Input.GetMouseButton(1))
        {
            //(移動開始座標 - マウスの現在座標) / 解像度 で正規化
            float x = (_startMousePos.x - Input.mousePosition.x) / Screen.width;
            float y = (_startMousePos.y - Input.mousePosition.y) / Screen.height;

            x = x * _positionStep;
            y = y * _positionStep;

            Vector3 velocity = _camTransform.rotation * new Vector3(x, y, 0);
            velocity = velocity + _presentCamPos;
            _camTransform.position = velocity;
        }
    }

    // カメラのローカル移動 キー
    private void CameraPositionKeyControl()
    {
        Vector3 campos = _camTransform.position;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKey(KeyCode.W)) { _camTransform.Rotate(Vector3.left); }
            if (Input.GetKey(KeyCode.S)) { _camTransform.Rotate(Vector3.right); }
            if (Input.GetKey(KeyCode.A)) { _camTransform.Rotate(Vector3.down); }
            if (Input.GetKey(KeyCode.D)) { _camTransform.Rotate(Vector3.up); }
        }
        else
        {
            if (Input.GetKey(KeyCode.W)) { campos += _camTransform.forward * Time.deltaTime * _positionStep; }
            if (Input.GetKey(KeyCode.S)) { campos -= _camTransform.forward * Time.deltaTime * _positionStep; }
            if (Input.GetKey(KeyCode.A)) { campos -= _camTransform.right * Time.deltaTime * _positionStep; }
            if (Input.GetKey(KeyCode.D)) { campos += _camTransform.right * Time.deltaTime * _positionStep; }
        }
        if (Input.GetKey(KeyCode.E)) { campos += _camTransform.up * Time.deltaTime * _positionStep; }
        if (Input.GetKey(KeyCode.Q)) { campos -= _camTransform.up * Time.deltaTime * _positionStep; }

        if (Input.GetKey(KeyCode.Alpha2)) { transform.Rotate(new Vector3(-1, 0, 0) * Time.deltaTime * _rotationStep); }

        _camTransform.position = campos;
    }

    // UIメッセージの表示
    private IEnumerator DisplayUiMessage()
    {
        _uiMessageActiv = true;
        float time = 0;
        while (time < 2)
        {
            time = time + Time.deltaTime;
            yield return null;
        }
        _uiMessageActiv = false;
    }

    void OnGUI()
    {
        if (_uiMessageActiv == false) { return; }
        GUI.color = Color.black;
        if (_cameraMoveActive == true)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height - 30, 100, 20), "カメラ操作 有効");
        }

        if (_cameraMoveActive == false)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height - 30, 100, 20), "カメラ操作 無効");
        }
    }

}
