using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Pistol : MonoBehaviour
{
    public Sprite idlePistol;
    public Sprite shotPistol;
    public Sprite trowPistol;
    public Sprite trow2Pistol;
    public Sprite trow3Pistol;
    public Sprite trow4Pistol;
    public Sprite trow5Pistol;
    public float pistolDamage;
    public float pistolRange;
    public AudioClip shotSound;
    public AudioClip reloadSound;
    public AudioClip emptyGunSound;
    public Text ammoText;
    AudioSource source;
    public GameObject quemadura;

    bool isShot;
    bool isReloading;

    public int ammoAmount;
    public int ammoClipSize;
    int ammoLeft;
    int ammoClipLeft;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        ammoLeft = ammoAmount;
        ammoClipLeft = ammoClipSize;
    }

    void Update()
    {
        ammoText.text = ammoClipLeft + " / " + ammoLeft;

        if (Input.GetButtonDown("Fire1") && isReloading == false)
            isShot = true;
        if (Input.GetKeyDown(KeyCode.R) && isReloading == false)
        {
            Reload();
        }
    }


    void FixedUpdate()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (isShot == true && ammoClipLeft > 0 && isReloading == false)
        {
            isShot = false;
            ammoClipLeft--;
            source.PlayOneShot(shotSound);
            StartCoroutine("shot");
            
            
            if (Physics.Raycast(ray, out hit, pistolRange))
            {
                Debug.Log("Wszedlem w kolizje z " + hit.collider.gameObject.name);
                
                
                hit.collider.gameObject.SendMessage("pistolHit", pistolDamage, SendMessageOptions.DontRequireReceiver);
                Instantiate(quemadura, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            }
        }
        else if (isShot == true && ammoClipLeft <= 0 && isReloading == false)
        {
            
            isShot = false;
            Reload();
        }
    }

    void Reload()
    {
        {
            
            int bulletsToReload = ammoClipSize - ammoClipLeft;
            if (ammoLeft >= bulletsToReload)
            {
                StartCoroutine("ReloadWeapon");
                ammoLeft -= bulletsToReload;
                ammoClipLeft = ammoClipSize;
            }
            else if (ammoLeft < bulletsToReload && ammoLeft > 0)
            {
                StartCoroutine("ReloadWeapon");
                ammoClipLeft += ammoLeft;
                ammoLeft = 0;
            }
            else if (ammoLeft <= 0)
            {
                source.PlayOneShot(emptyGunSound);
            }
        }
    }

    IEnumerator ReloadWeapon()
    {
        isReloading = true;
        source.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(1);
        isReloading = false;
    }

    
    IEnumerator shot()
    {
        GetComponent<SpriteRenderer>().sprite = idlePistol;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = shotPistol;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = trowPistol;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = trow2Pistol;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().sprite = trow3Pistol;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = trow4Pistol;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = trow5Pistol;
        yield return new WaitForSeconds(0.001f);
        GetComponent<SpriteRenderer>().sprite = idlePistol;
        yield return new WaitForSeconds(0.001f);


    }
}