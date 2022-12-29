using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovementAnimator _PMS;

    // Start is called before the first frame update
    void Awake()
    {
        _PMS = GetComponentInChildren<PlayerMovementAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_PMS != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _PMS.ChangeAnimatinState(PlayerAnimationStates.T_pose);
                Debug.Log("Tpose");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
                _PMS.ChangeAnimatinState(PlayerAnimationStates.Idle);

            if (Input.GetKeyDown(KeyCode.Alpha3))
                _PMS.ChangeAnimatinState(PlayerAnimationStates.Walk);

            if (Input.GetKeyDown(KeyCode.Alpha4))
                _PMS.ChangeAnimatinState(PlayerAnimationStates.Run);

            if (Input.GetKeyDown(KeyCode.Alpha5))
                _PMS.ChangeAnimatinState(PlayerAnimationStates.Jump_start);

            if (Input.GetKeyDown(KeyCode.Alpha6))
                _PMS.ChangeAnimatinState(PlayerAnimationStates.Jump_inair);

            if (Input.GetKeyDown(KeyCode.Alpha7))
                _PMS.ChangeAnimatinState(PlayerAnimationStates.Jump_end);
        }
        else
        {
            _PMS = GetComponent<PlayerMovementAnimator>();
            Debug.Log("_PMS: " + _PMS);
        }
    }

}
