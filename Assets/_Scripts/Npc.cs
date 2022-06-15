using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

[System.Serializable]
public struct bodyInfo
{
    public Npc.ModelType modelType;
    public GameObject model;
    [Space]
    public Transform rightHand;
    public Transform leftHand;
    [Space]
    public bool hasHair;
    [ShowIf("@this.hasHair == true")]
    public GameObject[] hair;
}

[System.Serializable]
public struct toolObject
{
    public bool leftHanded;
    public GameObject tool;
}

public class Npc : Actor
{
    [Space]
    [SerializeField] bool showReferences;
    [SerializeField] [ReadOnly] bool modelLoaded;
    [SerializeField] [ReadOnly] GameObject activeModel;

    [Title("Character")]
    [SerializeField] bool randomModel = true;

    [ShowIf("@this.randomModel == false")]
    [EnumPaging] public ModelType modelType;

    public enum ModelType { blackSmith, butcher, farmer1, farmer2, farmer3, guard1, guard2, guard3, king, laberor, monk, noble1, noble2, noble3, noble4, trader1, trader2 }

    [Title("Work")]
    [EnumToggleButtons]
    enum TypeWork { unemployed, building, cooking, farming, fishing, gathering, lumberjacking, mining }
    [SerializeField] TypeWork typeWork;

    #region enums


    [ShowIf("@this.typeWork == TypeWork.unemployed")]
    enum TypeIdle { standing, sittingBench, sittingGround}
    [SerializeField] TypeIdle typeIdle;

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

    [Title("Bodies")]
    [ShowIf("@showReferences == true")] [SerializeField] List<bodyInfo> bodyTypes = new List<bodyInfo>();

    [Title("Props")]
    [ShowIf("@showReferences == true")] [SerializeField] Transform propsDefaultParent;
    [ShowIf("@showReferences == true")] [SerializeField] toolObject[] props;
    [Space]
    [Title("Gizmo")]
    [ShowIf("@showReferences == true")] [SerializeField] bool drawGizmo = true;
    [ShowIf("@showReferences == true")] [SerializeField] Mesh gizmoMesh;
    [ShowIf("@showReferences == true")] [SerializeField] Vector3 gizmoOffset;
    [ShowIf("@showReferences == true")] [SerializeField] Vector3 gizmoSize;

    // hammer           0;
    // farmTool         1;
    // fishingRod       2;
    // axe              3;
    // pickAxe          4;
    // spoon            5;
    // salt             6;
    // wateringCan      7;
    // handSaw          8;
    // sack             9;
    // shovel           10;
    // glass            11;
    // glass2           12;
    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        LoadModel();
        SetupAnimator(activeModel);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (dead || invincible)
            return;
        health -= damage;
        if (health < 1)
        {
            dead = true;
            anim.SetTrigger("Dead");
        }
        else
        {
            anim.Play("Damaged");
        }
    }

    [PropertySpace]
    [Button]
    void LoadModel()
    {
        if (modelLoaded)
            return;

        activeModel = SetupModel();
        modelLoaded = true;
    }

    #region editorFunction
    [PropertySpace]
    [Button]
    void ResetModel()
    {
        foreach (var body in bodyTypes)
        {
            body.model.SetActive(false);
            if (body.hasHair)
            {
                foreach (var hair in body.hair)
                {
                    hair.SetActive(false);
                }
            }
        }

        foreach (var p in props)
        {
            p.tool.SetActive(false);
            p.tool.transform.parent = propsDefaultParent;
        }

        modelLoaded = false;
        activeModel = null;
    }
    #endregion

    void SetupObjects(bodyInfo hands)
    {
        foreach (var p in props)
        {
            if (p.leftHanded)
            {
                p.tool.transform.parent = hands.leftHand.transform;
            }
            else
            {
                p.tool.transform.parent = hands.rightHand.transform;
            }
        }

        switch (typeWork)
        {
            case TypeWork.unemployed:
                break;
            case TypeWork.building:
                switch (typeBuilding)
                {
                    case TypeBuilding.crouchGround:
                        props[0].tool.SetActive(true);
                        break;
                    case TypeBuilding.standingWallV1:
                        props[0].tool.SetActive(true);
                        break;
                    case TypeBuilding.standingWallV2:
                        props[0].tool.SetActive(true);
                        break;
                    case TypeBuilding.onTableV1:
                        props[0].tool.SetActive(true);
                        break;
                    case TypeBuilding.onTableV2:
                        props[0].tool.SetActive(true);
                        break;
                    case TypeBuilding.standingSawingV1:
                        props[8].tool.SetActive(true);
                        break;
                    case TypeBuilding.standingSawingV2:
                        props[8].tool.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case TypeWork.cooking:
                switch (typeCooking)
                {
                    case TypeCooking.stirPot:
                        props[5].tool.SetActive(true);
                        break;
                    case TypeCooking.addSeasoning:
                        props[6].tool.SetActive(true);
                        break;
                    case TypeCooking.addDrinks:
                        props[11].tool.SetActive(true);
                        props[12].tool.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case TypeWork.farming:
                switch (typeFarming)
                {
                    case TypeFarming.idle:
                        props[1].tool.SetActive(true);
                        break;
                    case TypeFarming.working:
                        props[1].tool.SetActive(true);
                        break;
                    case TypeFarming.plantSeedsV1:
                        props[9].tool.SetActive(true);
                        break;
                    case TypeFarming.plantSeedsV2:
                        props[9].tool.SetActive(true);
                        break;
                    case TypeFarming.pouringWater:
                        props[7].tool.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case TypeWork.fishing:
                props[2].tool.SetActive(true);
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
                props[3].tool.SetActive(true);
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
                        props[4].tool.SetActive(true);
                        break;
                    case TypeMining.miningV2:
                        props[4].tool.SetActive(true);
                        break;
                    case TypeMining.diggingV1:
                        props[10].tool.SetActive(true);
                        break;
                    case TypeMining.diggingV2:
                        props[10].tool.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    void SetupAnimator(GameObject model)
    {
        anim = model.GetComponent<Animator>();

        if (anim == null)
            return;

        switch (typeWork)
        {
            case TypeWork.unemployed:
                anim.SetBool("UnEmployed", true);
                switch (typeIdle)
                {
                    case TypeIdle.standing:
                        anim.SetInteger("Type", 1);
                        break;
                    case TypeIdle.sittingBench:
                        anim.SetInteger("Type", 2);
                        break;
                    case TypeIdle.sittingGround:
                        anim.SetInteger("Type", 3);
                        break;
                    default:
                        break;
                }
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

    GameObject SetupModel()
    {
        if (randomModel)
        {
            System.Array values = System.Enum.GetValues(typeof(ModelType));
            modelType = (ModelType)values.GetValue(Random.Range(0, values.Length));
        }
        return GetCharacterModel();
    }

    GameObject GetCharacterModel()
    {
        foreach (var body in bodyTypes)
        {
            if (body.modelType == modelType)
            {
                body.model.SetActive(true);

                if (body.hasHair)
                {
                    body.hair[Random.Range(0, body.hair.Length)].SetActive(true);
                }

                SetupObjects(body);

                return body.model;
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (modelLoaded == false)
        {
            if (drawGizmo)
            {
                Gizmos.DrawWireMesh(gizmoMesh, transform.position + gizmoOffset, transform.rotation, gizmoSize);
            }
        }
    }
}