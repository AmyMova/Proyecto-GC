using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<Image> weaponUISlots = new List<Image>(6);
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int[] passiveItemLevels = new int[6];
    public List<Image> passiveItemUISlots = new List<Image>(6);

    PlayerStats player;

    void Start() {
        player = GetComponent<PlayerStats>();
    }

    [System.Serializable]
    public class WeaponUpgrade {
        public int weaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade {
        public int passiveItemUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemData;
    }

    [System.Serializable]
    public class UpgradeUI {
        public TextMeshProUGUI upgradeNameDisplay;
        public TextMeshProUGUI upgradeDescriptionDisplay;
        public Image upgradIcon;
        public Button upgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();

    public void AddWeapon(int slotIndex, WeaponController weapon) {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;

        if(GameManager.instance != null && GameManager.instance.choosingUpgrade) {
            GameManager.instance.EndLevelUp();
        }
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem) {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemUISlots[slotIndex].enabled = true;
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade) {
            GameManager.instance.EndLevelUp();
        }
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex) {
        if(weaponSlots.Count > slotIndex) {
            WeaponController weapon = weaponSlots[slotIndex];

            if(!weapon.weaponData.NextLevelPrefab) {
                return;
            }

            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform);
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level;

            weaponUpgradeOptions[upgradeIndex].weaponData = upgradedWeapon.GetComponent<WeaponController>().weaponData;

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade) {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex) {
        if (passiveItemSlots.Count > slotIndex) {
            PassiveItem weapon = passiveItemSlots[slotIndex];

            if (!weapon.passiveItemData.NextLevelPrefab) {
                return;
            }

            GameObject upgradedItem = Instantiate(weapon.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedItem.transform.SetParent(transform);
            AddPassiveItem(slotIndex, upgradedItem.GetComponent<PassiveItem>());
            Destroy(weapon.gameObject);
            passiveItemLevels[slotIndex] = upgradedItem.GetComponent<PassiveItem>().passiveItemData.Level;

            passiveItemUpgradeOptions[upgradeIndex].passiveItemData = upgradedItem.GetComponent<PassiveItem>().passiveItemData;

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade) {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    void ApplyUpgradeOptions() {
        List<WeaponUpgrade> availableWeapon = new List<WeaponUpgrade>(weaponUpgradeOptions);
        List<PassiveItemUpgrade>availableItem = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);

        foreach(var upgradeOption in upgradeUIOptions) {
            if(availableWeapon.Count == 0 && availableItem.Count == 0) {
                return;
            }

            int upgradeType;

            if(availableWeapon.Count == 0) {
                upgradeType = 2;
            } else if(availableItem.Count == 0) {
                upgradeType = 1;
            } else {
                upgradeType = Random.Range(1,3);
            }

            if(upgradeType == 1) {
                WeaponUpgrade chosenWeaponUpgrade = availableWeapon[Random.Range(0, availableWeapon.Count)];
                availableWeapon.Remove(chosenWeaponUpgrade);

                if(chosenWeaponUpgrade != null) {
                    EnableUpgradeUI(upgradeOption);
                    bool newWeapon = false;

                    for(int i = 0; i < weaponSlots.Count;  i++) {
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData) {
                            newWeapon = false;

                            if (!newWeapon) {
                                if(!chosenWeaponUpgrade.weaponData.NextLevelPrefab) {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }

                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.weaponUpgradeIndex));
                                upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                            }
                            break;
                        } else {
                            newWeapon = true;
                        }
                    }

                    if(newWeapon) {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
                    }

                    upgradeOption.upgradIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                }
            } else if(upgradeType == 2) {
                PassiveItemUpgrade chosenPassiveUpgrade = availableItem[Random.Range(0, availableItem.Count)];
                availableItem.Remove(chosenPassiveUpgrade);

                if (chosenPassiveUpgrade != null) {
                    EnableUpgradeUI(upgradeOption);
                    bool newPassive = false;

                    for (int i = 0; i < passiveItemSlots.Count; i++) {
                        if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveUpgrade.passiveItemData) {
                            newPassive = false;

                            if (!newPassive) {
                                if(!chosenPassiveUpgrade.passiveItemData.NextLevelPrefab) {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }

                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, chosenPassiveUpgrade.passiveItemUpgradeIndex));
                                upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenPassiveUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Name;
                            }
                            break;
                        } else {
                            newPassive = true;
                        }
                    }
                        
                    if (newPassive) {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveUpgrade.initialPassiveItem));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveUpgrade.passiveItemData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveUpgrade.passiveItemData.Name;
                    }

                    upgradeOption.upgradIcon.sprite = chosenPassiveUpgrade.passiveItemData.Icon;
                }
            }
        }
    }

    void RemoveUpgradeOptions() {
        foreach(var upgradeOption in upgradeUIOptions) {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgrades() {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI ui) {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI ui) {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }
}
