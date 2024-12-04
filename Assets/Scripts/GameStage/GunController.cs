using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GunController : MonoBehaviour
{
    public static GunController instance;
    [SerializeField] private Animator gunAnim;
    [SerializeField] private Animator amoBtnAnim => GameObject.Find("Amo").GetComponent<Animator>();
    [SerializeField] private Transform gun;
    [SerializeField] private float gunDistance = 1.2f;
    private bool gunFacingRight = true;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject superBulletPrefab;
    [SerializeField] private float bulletSpeed = 15;
    [SerializeField] private int maxBullet = 15;
    private float currentSpeed = 0f;
    private int currentBullet = 0;

    [Header("Super Bullet")]
    public float calmdown = 5f;
    public float timer = 5f;
    public bool superPower = false;

    private void Awake() => instance = this;
    void Start() => ReloadGun();

    void Update()
    {
        if (superPower)
        {
            currentSpeed = bulletSpeed + 0.8f;
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = calmdown;
                superPower = !superPower;
                currentSpeed = bulletSpeed;
            }
        }

        GunStateController();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        gun.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.position = transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(gunDistance, 0, 0);
        if (Input.GetKeyDown(KeyCode.Mouse0) && HaveBullets())
        {
            Shoot(direction);
        }
        if (mousePos.x < gun.position.x && gunFacingRight) GunFlip();
        else if (mousePos.x > gun.position.x && !gunFacingRight) GunFlip();
    }

    private bool IsPointerOverUIObject()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void GunStateController()
    {
        gunAnim.SetBool("SuperBullets", superPower);
    }

    private void GunFlip()
    {
        gunFacingRight = !gunFacingRight;
        gun.localScale = new Vector3(gun.localScale.x, gun.localScale.y * -1, gun.localScale.z);
    }

    private void Shoot(Vector3 direction)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        GameBgmManager.Instance.PlayShootSound();
        currentBullet--;
        if (SceneManager.GetActiveScene().name == "Tutorial") TutorialUI.instance.UpdateAmoInfo(currentBullet, maxBullet);
        else UI.instance.UpdateAmoInfo(currentBullet, maxBullet);
        gunAnim.SetTrigger("Shoot");
        if (!superPower)
        {
            GameObject newBullet = Instantiate(bulletPrefab, gun.position, Quaternion.identity);
            newBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * currentSpeed;
            Destroy(newBullet, 3);
        }
        else
        {
            GameObject newBullet = Instantiate(superBulletPrefab, gun.position, Quaternion.identity);
            newBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * currentSpeed;
            Destroy(newBullet, 3);
        }
    }

    public void ReloadGun()
    {
        currentBullet = maxBullet;
        currentSpeed = bulletSpeed;

        if (SceneManager.GetActiveScene().name == "Tutorial") TutorialUI.instance.UpdateAmoInfo(currentBullet, maxBullet);
        else UI.instance.UpdateAmoInfo(currentBullet, maxBullet);
        Time.timeScale = 1;
        gunAnim.SetBool("HaveBullets", true);

    }
    private bool HaveBullets()
    {
        if (currentBullet <= 0)
        {
            gunAnim.SetBool("HaveBullets", false);
            gunAnim.SetTrigger("OutOfBullets");
            amoBtnAnim.SetTrigger("OutOfBullets");
            return false;
        }
        return true;
    }

}
