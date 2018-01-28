﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiEntity : MonoBehaviour {

    [SerializeField]
    private Entity _entityInfo;

    private Queue<TileObject> _path;
    private TileObject _currentTile;

    private AiEntity target;
    private bool isInspecting;

    public bool isWaiting;

    public Action<AiEntity, TileObject> OnWaitForNextTile;
    public Action<AiEntity, TileObject> OnAtTile;
    public Action<AiEntity> OnEndOfPath;

    public Action<AiEntity> OnStartInspection;
    public Action<AiEntity> OnDoneInspection;
    public Action<AiEntity> OnErrorInPath;

    private void Start() {
        _path = new Queue<TileObject>();

        AiManager.AddAiEntity(this);

        if (_entityInfo.IsDoctor) {
            StartCoroutine(doctorSearch());
        }

        this.GetComponent<SpriteRenderer>().color = _entityInfo.color;
	}
	
	void Update () {
        this.move();
        this.visualize();
	}

    public void move() {
        if (isInspecting || isWaiting) {
            return;
        }

        if (_path.Count <= 0 || _currentTile == null){
            if (OnErrorInPath != null)
                OnErrorInPath(this);
            return;
        }

        this.transform.position = Vector2.MoveTowards(transform.position, _path.Peek().GetPosition(), _entityInfo.Speed * Time.deltaTime);
        
        if ((Vector2)this.transform.position == _path.Peek().GetPosition()) {
            _path.Dequeue();

            if (_path.Count <= 0){
                if(OnEndOfPath != null)
                    OnEndOfPath(this);
            }else {
                _currentTile = _path.Peek();
                this.transform.localEulerAngles = new Vector3(0,0,(Mathf.Atan2(_currentTile.transform.position.y - transform.position.y, _currentTile.transform.position.x - transform.position.x) * Mathf.Rad2Deg) - 90f);
                if(OnAtTile != null)
                    OnAtTile(this, _currentTile);
            }
        }
    }

    private void visualize() {
        TileObject[] list = _path.ToArray();
        for (int i = 0; i < list.Length; i++){
            if (i >= list.Length - 1) {
                break;
            }
            Debug.DrawLine(list[i].GetPosition(), list[i+1].GetPosition(), Color.red);
        }
    }

    public void SetPath(Queue<TileObject> q) {
        if (q.Count > 0) {
            _currentTile = q.Peek();
        }
        _path = q;
    }

    public TileObject GetCurrentTile() {
        return _currentTile;
    }

    public void SetCurrentTile(TileObject t){
        _currentTile = t;
    }

    public Entity getEntityInfo() {
        return _entityInfo;
    }

    public AiEntity getTarget() {
        return target;
    }

    public void setTarget(AiEntity t) {
        target = t;
    }

    public IEnumerator WaitForInspection() {
        print("wait");
        isInspecting = true;
        yield return new WaitForSeconds(5.0f);
        isInspecting = false;
        if (OnDoneInspection != null) {
            OnDoneInspection(this);
        }
        this.target = null;
        if (_entityInfo.IsDoctor) {
            if (UnityEngine.Random.Range(0, 100) < 40) {
                AiManager.instance.spawnNewDoctor();
            }
            StartCoroutine(doctorSearch());
        }
    }

    private IEnumerator doctorSearch(){
        yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(10, 15));
        print("done");
        if (OnStartInspection != null){
            OnStartInspection(this);
        }
    }
}
