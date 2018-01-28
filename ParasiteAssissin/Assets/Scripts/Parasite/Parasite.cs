using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Parasite : MonoBehaviour
{
    public float range;
    [SerializeField]
    private GameObject currentTarget;
    [SerializeField]
    private float jumpSpeed;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private float minZoom;
    [SerializeField]
    private float maxZoom;
    [SerializeField]
    private float zoomOutSpeed;
    [SerializeField]
    private float shakePower;

    public float controlSpeed;
    public float resistance;
    public int qteDecreaser;

    bool enemyControl;


    SpriteRenderer sprRenderer;

    LineRenderer line;

    bool attacking = false;

    Enemy stuckOn;

    Rigidbody2D curRidgidBody;
    bool qteFailed = false;

    GameObject RangeCircle;

    GameObject arrow;
    SpriteRenderer arrowRenderer;

    // Use this for initialization

    [SerializeField]
    private Enemy target;

    void Start()
    {
        enemyControl = false;

        RangeCircle = transform.GetChild(0).gameObject;
        RangeCircle.transform.localScale = Vector3.one * range * 2;
        sprRenderer = GetComponent<SpriteRenderer>();

        arrow = transform.GetChild(1).gameObject;
        arrow.SetActive(false);
        arrowRenderer = arrow.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Smooth camera movement
        Vector3 point = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 delta = transform.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        Vector3 smoothMovement = Vector3.SmoothDamp(Camera.main.transform.position, destination, ref velocity, dampTime);
        smoothMovement = new Vector3(smoothMovement.x, smoothMovement.y, Camera.main.transform.position.z);
        Camera.main.transform.position = smoothMovement;
        // Shake camera
        /*if (attacking) {
            Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + Random.value * Time.deltaTime * shakePower, Camera.main.transform.position.y + Random.value * Time.deltaTime * shakePower, Camera.main.transform.position.z);
        }*/
        arrow.SetActive(stuckOn != null);
        if (target == null)
            target = FindObjectOfType<Enemy>();
        //arrowRenderer.color = new Color(1, 1, 1, 1f / Vector2.Distance(transform.position, target.transform.position));
        //print(arrowRenderer.color);
        if (stuckOn != null)
        {
            float angle = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            arrow.transform.localEulerAngles = new Vector3(0, 0, angle - 90f);
        }
        if (Quicktime.isDeath())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (attacking) return;
        // Zoom the camera out if the parasite is not attacking 
        Camera.main.orthographicSize += ((Mathf.Lerp(maxZoom, minZoom, Camera.main.orthographicSize / minZoom)) - Camera.main.orthographicSize) * zoomOutSpeed * Time.deltaTime;
        // Movement target
        if (stuckOn != null)
        {
            curRidgidBody.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        // Get mousePosition in world space
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10.0f;
        Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 _v = v;
        // Get the direction towards the mouse
        Vector3 direction = transform.position - _v;
        // Draw ray
        Debug.DrawRay(transform.position, direction.normalized * -1 * range, Color.green);
        // If left mouse button is down
        if (Input.GetMouseButtonDown(0))
        {
            print("Casting");
            // Shoot a ray towards mouse
            RaycastHit2D rayhit = Physics2D.Raycast(transform.position, direction * -1, range);

            if (rayhit.collider != null)
            {
                print(rayhit.collider.gameObject);
                // Get enemy script
                Enemy e = rayhit.collider.gameObject.GetComponent<Enemy>();
                if (e != null)
                    // If the mouse is over that enemy set it as our current target
                    if (e.mouseOver)
                    {
                        currentTarget = rayhit.collider.gameObject;
                        StartCoroutine(attack(transform.position, e, e.ai.getEntityInfo().Tier + 2));
                    }
            }
        }
        RangeCircle.transform.localScale = Vector3.one * range * 2;
    }

    IEnumerator attack(Vector3 startPos, Enemy e, int dif)
    {
        if (stuckOn != null)
        {
            stuckOn.GetComponent<AiEntity>().enabled = true;
            AiManager.SetNewRandomPath(stuckOn.GetComponent<AiEntity>());
            curRidgidBody.bodyType = RigidbodyType2D.Kinematic;
            curRidgidBody = null;
            stuckOn.gameObject.layer = LayerMask.NameToLayer("Default");
            sprRenderer.enabled = true;
            transform.parent = null;
        }

        for (int i = 0; i < AiManager.instance._aiEntitys.Count; i++)
        {
            print(AiManager.instance._aiEntitys[i].GetType());
            ((AiEntity)AiManager.instance._aiEntitys[i]).isWaiting = true;
        }

        attacking = true;
        float t = 0;
        while ((t += Time.deltaTime) < 0.8f)
        {
            transform.position = Vector3.Lerp(startPos, e.transform.position, t);
            Camera.main.orthographicSize = Mathf.Lerp(minZoom, maxZoom, (minZoom - maxZoom) * t);
            // Shake camera
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + Random.value * Time.deltaTime * shakePower, Camera.main.transform.position.y + Random.value * Time.deltaTime * shakePower, Camera.main.transform.position.z);
            yield return new WaitForEndOfFrame();
        }
        transform.position = Vector3.Lerp(startPos, e.transform.position, 0.8f);

        for (int i = 0; i < dif; i++)
        {
            yield return qte(dif);
            transform.position = Vector3.Lerp(startPos, e.transform.position, 0.8f + (0.2f / dif) * i);
            // Shake camera
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + Random.value * Time.deltaTime * shakePower, Camera.main.transform.position.y + Random.value * Time.deltaTime * shakePower, Camera.main.transform.position.z);
            if (qteFailed)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        stuckOn = e;

        if (!stuckOn.visited)
        {
            stuckOn.visited = true;
            PlayerLevelManager.instance.addExp(25 * stuckOn.ai.getEntityInfo().Tier * stuckOn.ai.getEntityInfo().Tier);
        }

        foreach (TileObject tObj in AiManager.instance._aiSystem.GetTilesByTierHigher(e.ai.getEntityInfo().Tier))
        {
            BoxCollider2D bc2D = tObj.GetComponent<BoxCollider2D>();
            if (bc2D != null)
                bc2D.enabled = true;
        }
        foreach (TileObject tObj in AiManager.instance._aiSystem.GetTilesByTier(e.ai.getEntityInfo().Tier))
        {
            BoxCollider2D bc2D = tObj.GetComponent<BoxCollider2D>();
            if (bc2D != null)
                bc2D.enabled = false;
        }

        curRidgidBody = e.GetComponent<Rigidbody2D>();
        curRidgidBody.bodyType = RigidbodyType2D.Dynamic;
        stuckOn.GetComponent<AiEntity>().enabled = false;
        transform.parent = stuckOn.transform;
        sprRenderer.enabled = false;
        transform.localPosition = Vector3.zero;
        stuckOn.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        attacking = false;
        Quicktime.DisableQuicktime();
        foreach (AiEntity aiE in AiManager.instance._aiEntitys)
        {
            aiE.isWaiting = false;
        }
    }

    IEnumerator qte(int dif)
    {
        float t = 0;
        Quicktime.EnableQuicktime(dif, 1);
        yield return new WaitForSecondsRealtime(0.2f);
        while ((t += Time.deltaTime) < Quicktime.instance.maxTime && !qteFailed)
        {

            if (Quicktime.GetQuicktimeSucces(t))
            {
                Quicktime.DisableQuicktime();
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        if (!Quicktime.GetQuicktimeSucces(t))
            qteFailed = true;
        Quicktime.DisableQuicktime();
    }
}
