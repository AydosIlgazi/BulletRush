using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgrades : MonoBehaviour
{
    int bulletDamage;
    float bulletSpeed;
    int bulletPenetration;
    float fireRate;
    int damageButtonClicked;
    int speedButtonClicked;
    int penetrationButtonClicked;
    int rateButtonClicked;
    float sliderIncrease;

    GameManager gameManager;
    [SerializeField] GameObject spendableText=default;
    [SerializeField] GameObject nextLevelButton = default;
    [SerializeField] GameObject revertButton = default;
    [SerializeField] GameObject restartButton = default;
    [SerializeField] Slider bulletDamageSlider = default;
    [SerializeField] Slider bulletSpeedSlider = default;
    [SerializeField] Slider bulletPenetrationSlider = default;
    [SerializeField] Slider fireRateSlider = default;

    public int SpendablePoints { get; set; }

    void Awake()
    {
        bulletDamage = 10;
        fireRate = -0.1f;
        bulletPenetration = 5;
        bulletSpeed = 0.3f;
        sliderIncrease = 0.2f;
    }

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        spendableText.GetComponent<TextMeshProUGUI>().text = "Spandable Points: " + SpendablePoints;
    }

    void OnEnable()
    {
        damageButtonClicked=0;
        speedButtonClicked=0;
        penetrationButtonClicked=0;
        rateButtonClicked=0;
    }

    public void IncreaseBulletDamage(Slider slider)
    {
        if (SpendablePoints > 0)
        {
            SpendablePoints--;
            damageButtonClicked++;
            gameManager.BulletDamage += bulletDamage;
            slider.value += sliderIncrease;
        }
    }

    public void IncreaseBulletSpeed(Slider slider)
    {
        if (SpendablePoints > 0)
        {
            SpendablePoints--;
            speedButtonClicked++;
            gameManager.BulletSpeed += bulletSpeed;
            slider.value += sliderIncrease;
        }
    }

    public void IncreaseBulletPenetration(Slider slider)
    {
        if (SpendablePoints > 0)
        {
            SpendablePoints--;
            penetrationButtonClicked++;
            gameManager.BulletPenetration += bulletPenetration;
            slider.value += sliderIncrease;
        }
    }

    public void IncreaseFireRate(Slider slider)
    {
        if (SpendablePoints > 0)
        {
            SpendablePoints--;
            rateButtonClicked++;
            gameManager.FireRate += fireRate;
            slider.value += sliderIncrease;
        }
    }

    public void DecreaseBulletDamage(Slider slider)
    {
        if (damageButtonClicked > 0)
        {
            SpendablePoints++;
            damageButtonClicked--;
            gameManager.BulletDamage -= bulletDamage;
            slider.value -= sliderIncrease;

        }
    }

    public void DecreaseBulletSpeed(Slider slider)
    {
        if (speedButtonClicked > 0)
        {
            SpendablePoints++;
            speedButtonClicked--;
            gameManager.BulletSpeed -= bulletSpeed;
            slider.value -= sliderIncrease;


        }
    }

    public void DecreaseBulletPenetration(Slider slider)
    {
        if (penetrationButtonClicked > 0)
        {
            SpendablePoints++;
            penetrationButtonClicked--;
            gameManager.BulletPenetration -= bulletPenetration;
            slider.value -= sliderIncrease;

        }
    }

    public void DecreaseFireRate(Slider slider)
    {
        if (rateButtonClicked > 0)
        {
            SpendablePoints++;
            rateButtonClicked--;
            gameManager.FireRate -= fireRate;
            slider.value -= sliderIncrease;

        }
    }

    public void HideStartLevel()
    {
        nextLevelButton.SetActive(false);
    }

    public void ShowStartLevel()
    {
        nextLevelButton.SetActive(true);

    }

    public void HideRevert()
    {
        revertButton.SetActive(false);
    }

    public void ShowRevert()
    {
        revertButton.SetActive(true);
    }

    public void HideRestart()
    {
        restartButton.SetActive(false);
    }

    public void ShowRestart()
    {
        restartButton.SetActive(true);
    }

    public void Revert()
    {
        int damageButtonClickedTemp = damageButtonClicked;
        int speedButtonClickedTemp = speedButtonClicked;
        int penetrationButtonClickedTemp = penetrationButtonClicked;
        int rateButtonClickedTemp = rateButtonClicked;

        for (int i = 0; i < damageButtonClickedTemp; i++)
            DecreaseBulletDamage(bulletDamageSlider);

        for (int i = 0; i < speedButtonClickedTemp; i++)
            DecreaseBulletSpeed(bulletSpeedSlider);

        for (int i = 0; i < penetrationButtonClickedTemp; i++)
            DecreaseBulletPenetration(bulletPenetrationSlider);

        for (int i = 0; i < rateButtonClickedTemp; i++)
            DecreaseFireRate(fireRateSlider);

    }
}
