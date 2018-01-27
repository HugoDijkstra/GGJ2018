using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Parasite : MonoBehaviour {
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

    LineRenderer line;

    bool attacking = false;

    Enemy stuckOn;

    bool qteFailed = false;

    GameObject RangeCircle;
    // Use this for initialization
    void Start () {
        RangeCircle = transform.GetChild (0).gameObject;
        RangeCircle.transform.localScale = Vector3.one * range * 2;
    }

    // Update is called once per frame
    void Update () {
        // Smooth camera movement
        Vector3 point = Camera.main.WorldToViewportPoint (transform.position);
        Vector3 delta = transform.position - Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        Vector3 smoothMovement = Vector3.SmoothDamp (Camera.main.transform.position, destination, ref velocity, dampTime);
        smoothMovement = new Vector3 (smoothMovement.x, smoothMovement.y, Camera.main.transform.position.z);
        Camera.main.transform.position = smoothMovement;
        // Shake camera
        /*if (attacking) {
            Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + Random.value * Time.deltaTime * shakePower, Camera.main.transform.position.y + Random.value * Time.deltaTime * shakePower, Camera.main.transform.position.z);
        }*/

        if (Quicktime.isDeath ()) {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
        }
        if (attacking) return;
        // Zoom the camera out if the parasite is not attacking
        Camera.main.orthographicSize += ((Mathf.Lerp (maxZoom, minZoom, Camera.main.orthographicSize / minZoom)) - Camera.main.orthographicSize) * zoomOutSpeed * Time.deltaTime;
        // Movement target
        if (stuckOn != null) {
            transform.position = stuckOn.transform.position;
            stuckOn.transform.position += new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")) * Time.deltaTime;
        }
        // Get mousePosition in world space
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10.0f;
        Vector2 v = Camera.main.ScreenToWorldPoint (mousePosition);
        Vector3 _v = v;
        // Get the direction towards the mouse
        Vector3 direction = transform.localPosition - _v;
        // Draw ray
        Debug.DrawRay (transform.localPosition, direction.normalized * -1 * range, Color.green);
        // If left mouse button is down
        if (Input.GetMouseButtonDown (0)) {
            // Shoot a ray towards mouse
            RaycastHit2D rayhit = Physics2D.Raycast (transform.localPosition, direction * -1, range);

            if (rayhit.collider != null) {
                // Get enemy script
                Enemy e = rayhit.collider.gameObject.GetComponent<Enemy> ();
                if (e != null)
                    // If the mouse is over that enemy set it as our current target
                    if (e.mouseOver) {
                        currentTarget = rayhit.collider.gameObject;
                        StartCoroutine (attack (transform.position, e, 2));
                    }
            }
        }
        RangeCircle.transform.localScale = Vector3.one * range * 2;
    }

    IEnumerator attack (Vector3 startPos, Enemy e, int dif) {
        if(stuckOn != null)
        {
            stuckOn.gameObject.layer = LayerMask.NameToLayer("Default");
        }

        foreach (AiEntity aiE in AiManager.instance._aiEntitys)
        {
            aiE.isWaiting = true;
        }
        attacking = true;
        float t = 0;
        while ((t += Time.deltaTime) < 0.8f) {
            transform.position = Vector3.Lerp (startPos, e.transform.position, t);
            Camera.main.orthographicSize = Mathf.Lerp (minZoom, maxZoom, (minZoom - maxZoom) * t);
            // Shake camera
            Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + Random.value * Time.deltaTime * shakePower, Camera.main.transform.position.y + Random.value * Time.deltaTime * shakePower, Camera.main.transform.position.z);
            yield return new WaitForEndOfFrame ();
        }
        transform.position = Vector3.Lerp (startPos, e.transform.position, 0.8f);

        for (int i = 0; i < dif; i++) {
            yield return qte (dif);
            transform.position = Vector3.Lerp (startPos, e.transform.position, 0.8f + (0.2f / dif) * i);
            // Shake camera
            Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + Random.value * Time.deltaTime * shakePower, Camera.main.transform.position.y + Random.value * Time.deltaTime * shakePower, Camera.main.transform.position.z);
            if (qteFailed) {
                SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
            }
        }
        stuckOn = e;
        stuckOn.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        attacking = false;
        Quicktime.DisableQuicktime ();
        foreach (AiEntity aiE in AiManager.instance._aiEntitys)
        {
            aiE.isWaiting = false;
        }
    }

    IEnumerator qte (int dif) {
        float t = 0;
        Quicktime.EnableQuicktime (dif, 1);
        yield return new WaitForEndOfFrame ();
        while ((t += Time.deltaTime) < Quicktime.instance.maxTime && !qteFailed) {

            if (Quicktime.GetQuicktimeSucces (t)) {
                Quicktime.DisableQuicktime ();
                print ("Done: " + t);
                break;
            }
            yield return new WaitForEndOfFrame ();
        }
        if (!Quicktime.GetQuicktimeSucces (t))
            qteFailed = true;
        Quicktime.DisableQuicktime ();
    }
}
