using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class Npc : MonoBehaviour
{
    [SerializeField] bool showReferences;
    [SerializeField] [ReadOnly] bool modelLoaded;

    [Title("Character")]
    enum Gender { male, female}
    [SerializeField] Gender gender;
    [SerializeField] bool randomMaterial = true;
    [SerializeField] bool randomModel = true;
    [ShowIf("@this.gender == Gender.male")]
    [SerializeField] bool randomBeard;
    [SerializeField] bool randomHat;
    [SerializeField] bool randomHair;

    [ShowIf("@randomMaterial == false && this.gender == Gender.male")]
    [SerializeField] [Range(0, 18)] int maleMaterial;
    [ShowIf("@randomMaterial == false && this.gender == Gender.female")]
    [SerializeField] [Range(0, 7)] int femaleMaterial;

    [ShowIf("@randomModel == false && this.gender == Gender.male")]
    [EnumPaging] [SerializeField] MaleBodyTypes maleBodyType;
    enum MaleBodyTypes { normal, fat, skinny, strong, monk }
    [ShowIf("@randomModel == false && this.gender == Gender.female")]
    [EnumPaging] [SerializeField] FemaleBodyTypes femaleBodyType;
    enum FemaleBodyTypes { skinny, plus }

    [ShowIf("@showReferences == true")] [SerializeField] Material[] maleMaterials;
    [ShowIf("@showReferences == true")] [SerializeField] Material[] femaleMaterials;

    [Title("Work")]
    [EnumToggleButtons]
    enum TypeWork { unEmployed, building, cooking, farming, fishing, gathering, lumberjacking, mining }
    [SerializeField] TypeWork typeWork;

    #region enums
    [ShowIf("@this.typeWork == TypeWork.building")]
    enum TypeBuilding {crouchGround, standingWallV1, standingWallV2, onTableV1, onTableV2, standingSawingV1, standingSawingV2 }
    [SerializeField] TypeBuilding typeBuilding;

    [ShowIf("@this.typeWork == TypeWork.cooking")]
    enum TypeCooking { stirPot, addSeasoning, addDrinks }
    [SerializeField] TypeCooking typeCooking;

    [ShowIf("@this.typeWork == TypeWork.farming")]
    enum TypeFarming {idle, working, plantSeedsV1, plantSeedsV2, pouringWater }
    [SerializeField] TypeFarming typeFarming;

    [ShowIf("@this.typeWork == TypeWork.fishing")]
    enum TypeFishing {fishindIdle }
    [SerializeField] TypeFishing typeFishing;

    [ShowIf("@this.typeWork == TypeWork.gathering")]
    enum TypeGathering {gatherCrouchedV1,  gatherCrouchedV2, gatherStanding }
    [SerializeField] TypeGathering typeGathering;

    [ShowIf("@this.typeWork == TypeWork.lumberjacking")]
    enum TypeLumberjacking {choppingSide, choppingVertical, idle }
    [SerializeField] TypeLumberjacking typeLumberjacking;

    [ShowIf("@this.typeWork == TypeWork.mining")]
    enum TypeMining {miningV1, miningV2, diggingV1, diggingV2 }
    [SerializeField] TypeMining typeMining;

    #endregion

    #region references

    [PropertySpace]

    [Title("Props")]
    [ShowIf("@showReferences == true")] [SerializeField] GameObject hammer;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject farmTool;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject fishingRod;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject axe;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject pickAxe;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject spoon;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject salt;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject wateringCan;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject handSaw;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject sack;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject shovel;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject glass;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject glass2;

    [Title("Bodies")]
    [ShowIf("@showReferences == true")] [SerializeField] GameObject femaleSkinny;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject femalePlus;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject maleMonk;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject maleFat;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject maleNormal;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject maleSkinny;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject maleStrong;

    [Title("Hair/Hats")]
    [ShowIf("@showReferences == true")] [SerializeField] GameObject[] femaleHairs;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject[] maleHairs;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject[] maleBeards;
    [ShowIf("@showReferences == true")] [SerializeField] GameObject[] hats;
    #endregion

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        SetupAnimator();
        LoadModel();
    }

    [PropertySpace]
    [Button]
    void LoadModel()
    {
        if (modelLoaded)
            return;
        SetupObjects();
        SetupModel();
        modelLoaded = true;
    }

    #region editorFunction
    [PropertySpace]
    [Button]
    void ResetModel()
    {
        spoon.SetActive(false);
        salt.SetActive(false);
        wateringCan.SetActive(false);
        handSaw.SetActive(false);
        shovel.SetActive(false);
        hammer.SetActive(false);
        farmTool.SetActive(false);
        fishingRod.SetActive(false);
        axe.SetActive(false);
        pickAxe.SetActive(false);
        sack.SetActive(false);
        glass.SetActive(false);
        glass2.SetActive(false);

        femaleSkinny.SetActive(false);
        femalePlus.SetActive(false);
        maleMonk.SetActive(false);
        maleFat.SetActive(false);
        maleNormal.SetActive(false);
        maleSkinny.SetActive(false);
        maleStrong.SetActive(false);

        foreach (var item in femaleHairs)
        {
            item.SetActive(false);
        }
        foreach (var item in maleHairs)
        {
            item.SetActive(false);
        }
        foreach (var item in maleBeards)
        {
            item.SetActive(false);
        }
        foreach (var item in hats)
        {
            item.SetActive(false);
        }

        modelLoaded = false;
    }
    #endregion

    void SetupObjects()
    {
        switch (typeWork)
        {
            case TypeWork.unEmployed:
                break;
            case TypeWork.building:
                switch (typeBuilding)
                {
                    case TypeBuilding.crouchGround:
                        hammer.SetActive(true);
                        break;
                    case TypeBuilding.standingWallV1:
                        hammer.SetActive(true);
                        break;
                    case TypeBuilding.standingWallV2:
                        hammer.SetActive(true);
                        break;
                    case TypeBuilding.onTableV1:
                        hammer.SetActive(true);
                        break;
                    case TypeBuilding.onTableV2:
                        hammer.SetActive(true);
                        break;
                    case TypeBuilding.standingSawingV1:
                        handSaw.SetActive(true);
                        break;
                    case TypeBuilding.standingSawingV2:
                        handSaw.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case TypeWork.cooking:
                switch (typeCooking)
                {
                    case TypeCooking.stirPot:
                        spoon.SetActive(true);
                        break;
                    case TypeCooking.addSeasoning:
                        salt.SetActive(true);
                        break;
                    case TypeCooking.addDrinks:
                        glass.SetActive(true);
                        glass2.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case TypeWork.farming:
                switch (typeFarming)
                {
                    case TypeFarming.idle:
                        farmTool.SetActive(true);
                        break;
                    case TypeFarming.working:
                        farmTool.SetActive(true);
                        break;
                    case TypeFarming.plantSeedsV1:
                        sack.SetActive(true);
                        break;
                    case TypeFarming.plantSeedsV2:
                        sack.SetActive(true);
                        break;
                    case TypeFarming.pouringWater:
                        wateringCan.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case TypeWork.fishing:
                fishingRod.SetActive(true);
                break;
            case TypeWork.gathering:
                switch (typeGathering)
                {
                    case TypeGathering.gatherCrouchedV1:
                        break;
                    case TypeGathering.gatherCrouchedV2:
                        break;
                    case TypeGathering.gatherStanding:
                        break;
                    default:
                        break;
                }
                break;
            case TypeWork.lumberjacking:
                axe.SetActive(true);
                switch (typeLumberjacking)
                {
                    case TypeLumberjacking.choppingSide:
                        break;
                    case TypeLumberjacking.choppingVertical:
                        break;
                    case TypeLumberjacking.idle:
                        break;
                    default:
                        break;
                }
                break;
            case TypeWork.mining:
                switch (typeMining)
                {
                    case TypeMining.miningV1:
                        pickAxe.SetActive(true);
                        break;
                    case TypeMining.miningV2:
                        pickAxe.SetActive(true);
                        break;
                    case TypeMining.diggingV1:
                        shovel.SetActive(true);
                        break;
                    case TypeMining.diggingV2:
                        shovel.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    void SetupAnimator()
    {
        switch (typeWork)
        {
            case TypeWork.unEmployed:
                anim.SetBool("UnEmployed", true);
                break;
            case TypeWork.building:
                anim.SetBool("Building", true);
                switch (typeBuilding)
                {
                    case TypeBuilding.crouchGround:
                        anim.SetInteger("Type", 1);
                        break;
                    case TypeBuilding.standingWallV1:
                        anim.SetInteger("Type", 2);
                        break;
                    case TypeBuilding.standingWallV2:
                        anim.SetInteger("Type", 3);
                        break;
                    case TypeBuilding.onTableV1:
                        anim.SetInteger("Type", 4);
                        break;
                    case TypeBuilding.onTableV2:
                        anim.SetInteger("Type", 5);
                        break;
                    case TypeBuilding.standingSawingV1:
                        anim.SetInteger("Type", 6);
                        break;
                    case TypeBuilding.standingSawingV2:
                        anim.SetInteger("Type", 7);
                        break;
                    default:
                        break;
                }
                break;
            case TypeWork.cooking:
                anim.SetBool("Cooking", true);
                switch (typeCooking)
                {
                    case TypeCooking.stirPot:
                        anim.SetInteger("Type", 1);
                        break;
                    case TypeCooking.addSeasoning:
                        anim.SetInteger("Type", 2);
                        break;
                    case TypeCooking.addDrinks:
                        anim.SetInteger("Type", 3);
                        break;
                    default:
                        break;
                }
                break;
            case TypeWork.farming:
                anim.SetBool("Farming", true);
                switch (typeFarming)
                {
                    case TypeFarming.idle:
                        anim.SetInteger("Type", 1);
                        break;
                    case TypeFarming.working:
                        anim.SetInteger("Type", 2);
                        break;
                    case TypeFarming.plantSeedsV1:
                        anim.SetInteger("Type", 3);
                        break;
                    case TypeFarming.plantSeedsV2:
                        anim.SetInteger("Type", 4);
                        break;
                    case TypeFarming.pouringWater:
                        anim.SetInteger("Type", 5);
                        break;
                    default:
                        break;
                }
                break;
            case TypeWork.fishing:
                anim.SetBool("Fishing", true);
                break;
            case TypeWork.gathering:
                anim.SetBool("Gathering", true);
                switch (typeGathering)
                {
                    case TypeGathering.gatherCrouchedV1:
                        anim.SetInteger("Type", 1);
                        break;
                    case TypeGathering.gatherCrouchedV2:
                        anim.SetInteger("Type", 2);
                        break;
                    case TypeGathering.gatherStanding:
                        anim.SetInteger("Type", 3);
                        break;
                    default:
                        break;
                }
                break;
            case TypeWork.lumberjacking:
                anim.SetBool("Lumberjacking", true);
                switch (typeLumberjacking)
                {
                    case TypeLumberjacking.choppingSide:
                        anim.SetInteger("Type", 1);
                        break;
                    case TypeLumberjacking.choppingVertical:
                        anim.SetInteger("Type", 2);
                        break;
                    case TypeLumberjacking.idle:
                        anim.SetInteger("Type", 3);
                        break;
                    default:
                        break;
                }
                break;
            case TypeWork.mining:
                anim.SetBool("Mining", true);
                switch (typeMining)
                {
                    case TypeMining.miningV1:
                        anim.SetInteger("Type", 1);
                        break;
                    case TypeMining.miningV2:
                        anim.SetInteger("Type", 2);
                        break;
                    case TypeMining.diggingV1:
                        anim.SetInteger("Type", 3);
                        break;
                    case TypeMining.diggingV2:
                        anim.SetInteger("Type", 4);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    void SetupModel()
    {
        switch(gender)
        {
            case Gender.male:
                if (randomModel)
                {
                    System.Array values = System.Enum.GetValues(typeof(MaleBodyTypes));
                    maleBodyType = (MaleBodyTypes)values.GetValue(Random.Range(0, values.Length));
                }
                GameObject modelRef = GetMaleBody();
                Material m = maleMaterials[maleMaterial];
                if (randomMaterial)
                {
                    m = maleMaterials[Random.Range(0, maleMaterials.Length)];
                }
                if (randomBeard)
                {
                    int randomBeard = Random.Range(0, maleBeards.Length);
                    maleBeards[randomBeard].GetComponent<Renderer>().material = m;
                    maleBeards[randomBeard].SetActive(true);
                }
                if (randomHair)
                {
                    int randomNumber = Random.Range(0, maleHairs.Length);
                    maleHairs[randomNumber].GetComponent<Renderer>().material = m;
                    maleHairs[randomNumber].SetActive(true);
                }
                modelRef.GetComponent<Renderer>().material = m;
                if (randomHat)
                {
                    int randomNumber = Random.Range(0, hats.Length);
                    hats[randomNumber].SetActive(true);
                }
                break;
            case Gender.female:
                if (randomModel)
                {
                    System.Array values = System.Enum.GetValues(typeof(FemaleBodyTypes));
                    femaleBodyType = (FemaleBodyTypes)values.GetValue(Random.Range(0, values.Length));
                }
                GameObject femaleRef = GetFemaleBody();
                Material fm = femaleMaterials[femaleMaterial];
                if (randomMaterial)
                {
                    fm = femaleMaterials[Random.Range(0, femaleMaterials.Length)];
                }
                if (randomHair)
                {
                    int randomNumber = Random.Range(0, hats.Length);
                    femaleHairs[randomNumber].GetComponent<Renderer>().material = fm;
                    femaleHairs[randomNumber].SetActive(true);
                }
                femaleRef.GetComponent<Renderer>().material = fm;
                if (randomHat)
                {
                    int randomNumber = Random.Range(0, hats.Length);
                    hats[randomNumber].SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    GameObject GetFemaleBody()
    {
        switch (femaleBodyType)
        {
            case FemaleBodyTypes.skinny:
                femaleSkinny.SetActive(true);
                return femaleSkinny;
            case FemaleBodyTypes.plus:
                femalePlus.SetActive(true);
                return femalePlus;
            default:
                break;
        }
        return null;
    }

    GameObject GetMaleBody()
    {
        switch (maleBodyType)
        {
            case MaleBodyTypes.normal:
                maleNormal.SetActive(true);
                return maleNormal;
            case MaleBodyTypes.fat:
                maleFat.SetActive(true);
                return maleFat;
            case MaleBodyTypes.skinny:
                maleSkinny.SetActive(true);
                return maleSkinny;
            case MaleBodyTypes.strong:
                maleStrong.SetActive(true);
                return maleStrong;
            case MaleBodyTypes.monk:
                maleMonk.SetActive(true);
                return maleMonk;
            default:
                break;
        }
        return null;
    }

    void Update()
    {
        
    }
}