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

    private bool isWaiting;

    public Action<AiEntity, TileObject> OnWaitForNextTile;
    public Action<AiEntity, TileObject> OnAtTile;
    public Action<AiEntity> OnEndOfPath;

    public Action<AiEntity, TileObject> OnDoneInspection;
    public Action<AiEntity> OnErrorInPath;

    private void Start() {
        _path = new Queue<TileObject>();

        AiManager.AddAiEntity(this);

        this.GetComponent<SpriteRenderer>().color = _entityInfo.color;
	}
	
	void Update () {
        this.move();
        this.visualize();
	}

    public void move() {
        if (isInspecting) {
            return;
        }

        if (_path.Count <= 0 || _currentTile == null){
            OnErrorInPath(this);
            return;
        }

        Debug.Log(_currentTile);

        if (_currentTile.heading == null) {
            _currentTile.heading = this;
        } else if (_currentTile.heading != this) {
            OnWaitForNextTile(this, _currentTile);
            return;
        }

        this.transform.position = Vector2.MoveTowards(transform.position, _path.Peek().GetPosition(), _entityInfo.Speed * Time.deltaTime);
        
        if ((Vector2)this.transform.position == _path.Peek().GetPosition()) {
            _currentTile.heading = null;
            
            
            _path.Dequeue();

            if (_path.Count <= 0){
                OnEndOfPath(this);
            }else {
                _currentTile = _path.Peek();
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
        q.Dequeue();
        _currentTile = q.Peek();
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
        OnDoneInspection(this, _currentTile);
    }
}