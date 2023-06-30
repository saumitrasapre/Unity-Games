using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float lookSensitivity = 1f;
    [SerializeField] private float aimSensitivity = 1f;
    [SerializeField] private Image crossHair;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform hitTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private Rig aimRig;
    
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private Animator animator;



    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (starterAssetsInputs.aim)
        {
            //Player is aiming
            aimVirtualCamera.gameObject.SetActive(true);
            //Only show crosshair when player is aiming
            crossHair.gameObject.SetActive(true);
            thirdPersonController.SetLookSensitivity(aimSensitivity);
            //Change player pose to the aim pose
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1),1f,Time.deltaTime*10f));
            aimRig.weight = Mathf.Lerp(aimRig.weight, 1f, Time.deltaTime * 20f);

            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                //Raycast hits an object with a collider
                hitTransform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
            }
            else
            {
                //Raycast doesn't hit an object with a collider (e.g. Aiming at the sky)
                hitTransform.position = ray.GetPoint(20f);
                mouseWorldPosition = ray.GetPoint(20f);
            }
            thirdPersonController.SetRotateOnMove(false);
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
           
            if (starterAssetsInputs.shoot)
            {
                Vector3 bulletAimDir = (mouseWorldPosition - bulletSpawnPosition.transform.position).normalized;
                Instantiate(pfBulletProjectile, bulletSpawnPosition.position,Quaternion.LookRotation(bulletAimDir,Vector3.up));
                starterAssetsInputs.shoot = false;
            }
        }
        else
        {
            //Player is not aiming
            thirdPersonController.SetRotateOnMove(true);
            aimVirtualCamera.gameObject.SetActive(false);
            crossHair.gameObject.SetActive(false);
            thirdPersonController.SetLookSensitivity(lookSensitivity);
            if (starterAssetsInputs.shoot)
            {
                starterAssetsInputs.shoot = false;
            }
            //Reset player pose back to idle
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
            aimRig.weight = Mathf.Lerp(aimRig.weight, 0f, Time.deltaTime * 20f);
        }
    }

}
