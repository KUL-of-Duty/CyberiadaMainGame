using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AssignArmatureData : MonoBehaviour
{
    [SerializeField] Transform armature;
    [SerializeField] TwoBoneIKConstraint leg_l;
    [SerializeField] TwoBoneIKConstraint leg_r;
    [SerializeField] TwoBoneIKConstraint arm_l;
    [SerializeField] TwoBoneIKConstraint arm_r;
    [SerializeField] BoneRenderer boneRenderer;

    public string rootPrefix = "Root/Stomach";
    public void ExecuteAction()
    {/*
        boneRenderer. = armature.GetComponentsInChildren<Transform>();

        leg_l.data.root = armature.Find($"{rootPrefix}/Hip.L");
        leg_l.data.mid = armature.Find($"{rootPrefix}/Hip.L/Leg.L");
        leg_l.data.tip = armature.Find($"{rootPrefix}/Hip.L/Leg.L/Foot.L");

        leg_r.data.root = armature.Find($"{rootPrefix}/Hip.R");
        leg_r.data.mid = armature.Find($"{rootPrefix}/Hip.R/Leg.R");
        leg_r.data.tip = armature.Find($"{rootPrefix}/Hip.R/Leg.R/Foot.R");

        arm_l.data.root = armature.Find($"{rootPrefix}/Chest/Neck/Arm.L");
        arm_l.data.mid = armature.Find($"{rootPrefix}/Chest/Neck/Arm.L/Forearm.L");
        arm_l.data.tip = armature.Find($"{rootPrefix}/Chest/Neck/Arm.L/Forearm.L/Hand.L");

        arm_r.data.root = armature.Find($"{rootPrefix}/Chest/Neck/Arm.R");
        arm_r.data.mid = armature.Find($"{rootPrefix}/Chest/Neck/Arm.R/Forearm.R");
        arm_r.data.tip = armature.Find($"{rootPrefix}/Chest/Neck/Arm.R/Forearm.R/Hand.R");

        */
    }
}
