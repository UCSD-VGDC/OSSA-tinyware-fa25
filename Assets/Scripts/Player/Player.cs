using UnityEngine;

public class Player : MonoBehaviour
{
    public Weapon currentWeapon;

    [SerializeField] private GameObject visuals;
    
    private void OnEnable()
    {
        InputController.OnButtonLeftPressedEvent += HandleLeftButton;
        InputController.OnButtonRightPressedEvent += HandleRightButton;
    }

    private void OnDisable()
    {
        InputController.OnButtonLeftPressedEvent -= HandleLeftButton;
        InputController.OnButtonRightPressedEvent -= HandleRightButton;
    }

    private void HandleLeftButton(InputController controller)
    {
        visuals.transform.localScale = new Vector3(-1, 1, 1);
        currentWeapon.Attack();
    }

    private void HandleRightButton(InputController controller)
    {
        visuals.transform.localScale = new Vector3(1, 1, 1);
        currentWeapon.Attack();
    }
}
