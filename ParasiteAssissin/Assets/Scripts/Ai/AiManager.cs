using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour {
    static public AiManager instance;
    
    public ArrayList _aiEntitys;
    
    public AiSystem _aiSystem;

	void Awake () {
        // create instance
        if (instance == null){
            instance = this;
        }else {
            Destroy(this);
            return;
        }

        _aiSystem = this.GetComponent<AiSystem>();
        instance._aiEntitys = new ArrayList();
    }
	
	void Update () {
		
	}

    static public void AddAiEntity(AiEntity e) {
        instance._aiEntitys.Add(e);

        e.OnEndOfPath += instance.OnEndOfPath;
        e.OnErrorInPath += instance.OnErrorInPath;
        e.OnDoneInspection += instance.OnDoneInspection;

        if (e.getEntityInfo().IsDoctor) {
            e.OnAtTile += instance.OnAtTile;
        }
    }

    private AiEntity getSickEntityInRange(AiEntity e) {
        ArrayList list = new ArrayList();
        for (int i = 0; i < _aiEntitys.Count; i++){
            if (Vector2.Distance(e.transform.position, ((AiEntity)_aiEntitys[i]).transform.position) < 10) {
                list.Add(_aiEntitys[i]);
            }
        }

        return (AiEntity)list[UnityEngine.Random.Range(0, list.Count - 1)];
    }

    private void OnEndOfPath(AiEntity e) {
        this.getPath(e);
    }

    static public void SetNewRandomPath(AiEntity e) {
        e.SetPath(instance._aiSystem.CalculateRandomPathByTier(e.GetCurrentTile().GetPosition(), e.getEntityInfo().Tier));
    }

    private void OnErrorInPath(AiEntity e){
        e.SetPath(_aiSystem.CalculateRandomPathByTier(new Vector2(1, 1), e.getEntityInfo().Tier));
    }

    private void OnAtTile(AiEntity e, TileObject t) {
        if (e.getEntityInfo().IsDoctor) {
            if (e.getTarget() == null) {
                e.setTarget(this.getSickEntityInRange(e));
            }

            e.SetPath(_aiSystem.CalulatePathTo(e.GetCurrentTile().GetPosition(), e.getTarget().GetCurrentTile().GetPosition()));
        }
    }

    private void getPath(AiEntity e) {
        if (e.GetCurrentTile() == null) {
            return;
        }

        // for docter calculate path to sick entity
        if (e.getEntityInfo().IsDoctor) {
            if (e.getTarget() == null) {
                e.setTarget(this.getSickEntityInRange(e));
            }

            e.SetPath(_aiSystem.CalulatePathTo(e.GetCurrentTile().GetPosition(), e.getTarget().GetCurrentTile().GetPosition()));
            return;
        }

        // for normal calulate path to random tiler
        e.SetPath(_aiSystem.CalculateRandomPathByTier(e.GetCurrentTile().GetPosition(), e.getEntityInfo().Tier));
    }

    private void OnDoneInspection(AiEntity e, TileObject t) {
        this.getPath(e);
    }

    private void doctorCanInspect(AiEntity d, AiEntity t) {
        if (Vector2.Distance(d.transform.position, t.transform.position) < 1.2f) {
            d.WaitForInspection();
            t.WaitForInspection();
        }
    }

}
