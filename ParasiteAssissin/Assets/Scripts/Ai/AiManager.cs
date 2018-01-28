using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    static public AiManager instance;

    public ArrayList _aiEntitys;

    public AiSystem _aiSystem;

    public GameObject doctor;

    void Awake()
    {
        // create instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        _aiSystem = this.GetComponent<AiSystem>();
        instance._aiEntitys = new ArrayList();
    }

    void Update()
    {

    }

    static public void AddAiEntity(AiEntity e)
    {
        instance._aiEntitys.Add(e);

        e.OnEndOfPath += instance.OnEndOfPath;
        e.OnErrorInPath += instance.OnErrorInPath;
        e.OnDoneInspection += instance.OnDoneInspection;

        // doctor
        if (e.getEntityInfo().IsDoctor)
        {
            e.OnStartInspection = instance.OnStartInspection;
        }
    }

    static public void SetNewRandomPath(AiEntity e)
    {
        e.SetPath(instance._aiSystem.CalculateRandomPathByTier(e.GetCurrentTile().GetPosition(), e.getEntityInfo().Tier));
    }

    private AiEntity getSickEntityInRange(AiEntity e)
    {
        ArrayList list = new ArrayList();
        for (int i = 0; i < _aiEntitys.Count; i++)
        {
            if (!((AiEntity)_aiEntitys[i]).getEntityInfo().IsDoctor && Vector2.Distance(e.transform.position, ((AiEntity)_aiEntitys[i]).transform.position) < 10)
            {
                list.Add(_aiEntitys[i]);
            }
        }

        return (AiEntity)list[UnityEngine.Random.Range(0, list.Count - 1)];
    }

    private void OnEndOfPath(AiEntity e)
    {
        this.getPath(e);
    }

    private void OnErrorInPath(AiEntity e)
    {
        if (e.GetCurrentTile() != null)
        {
            e.SetPath(_aiSystem.CalculateRandomPathByTier(e.GetCurrentTile().GetPosition(), e.getEntityInfo().Tier));
            return;
        }
        e.SetPath(_aiSystem.CalculateRandomPathByTier(new Vector2(1, 1), e.getEntityInfo().Tier));
    }

    private void OnAtTile(AiEntity e, TileObject t)
    { // doctor
        if (e.getEntityInfo().IsDoctor)
        {
            if (e.getTarget() == null)
            {
                e.setTarget(this.getSickEntityInRange(e));
            }
            else
            {
                this.doctorCanInspect(e, e.getTarget());
            }


            if (e.GetCurrentTile().GetPosition() == e.getTarget().GetCurrentTile().GetPosition())
            {
                return;
            }
            e.SetPath(_aiSystem.CalulatePathTo(e.GetCurrentTile().GetPosition(), e.getTarget().GetCurrentTile().GetPosition()));
        }
    }

    private void getPath(AiEntity e)
    {
        if (e.GetCurrentTile() == null)
        {
            return;
        }

        // for normal calulate path to random tile
        e.SetPath(_aiSystem.CalculateRandomPathByTier(e.GetCurrentTile().GetPosition(), e.getEntityInfo().Tier));
    }

    private void OnStartInspection(AiEntity e){
        if (e.getEntityInfo().IsDoctor)
        {
            if (e.getTarget() == null)
            {
                e.setTarget(this.getSickEntityInRange(e));
            }
            else
            {
                this.doctorCanInspect(e, e.getTarget());
            }

            e.SetPath(_aiSystem.CalulatePathTo(e.GetCurrentTile().GetPosition(), e.getTarget().GetCurrentTile().GetPosition()));
            e.OnAtTile += instance.OnAtTile;
        }
    }

    public void spawnNewDoctor() {
        ArrayList spawnableTiles = AiManager.instance._aiSystem.GetTilesByTier(doctor.GetComponent<AiEntity>().getEntityInfo().Tier);
        int randomTile = Random.Range(0, spawnableTiles.Count);
        TileObject targetPosition = (TileObject)spawnableTiles[randomTile];

        GameObject newEntity = Instantiate(doctor, targetPosition.GetPosition(), Quaternion.identity);
        AiManager.AddAiEntity(newEntity.GetComponent<AiEntity>());
        AiEntity ae = newEntity.GetComponent<AiEntity>();
        ae.SetCurrentTile(targetPosition);
        ae.SetPath(AiManager.instance._aiSystem.CalculateRandomPathByTier(targetPosition.GetPosition(), doctor.GetComponent<AiEntity>().getEntityInfo().Tier));
    }

    private void OnDoneInspection(AiEntity e){ // doctor
        e.SetPath(_aiSystem.CalculateRandomPathByTier(e.GetCurrentTile().GetPosition(), e.getEntityInfo().Tier));

        if (e.getEntityInfo().IsDoctor)
        {
            e.OnAtTile -= instance.OnAtTile;
        }
    }

    private void doctorCanInspect(AiEntity d, AiEntity t)
    { // doctor
        if (Vector2.Distance(d.transform.position, t.transform.position) < 1.5f)
        {
            StartCoroutine(d.WaitForInspection());
            StartCoroutine(t.WaitForInspection());
        }
    }
}
