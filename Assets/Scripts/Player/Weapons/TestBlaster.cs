using UnityEngine;

[CreateAssetMenu(fileName = "New TestBlaster", menuName = "Weapons/TestBlaster")]
public class TestBlaster : Weapon
{
    public override void Attack()
    {
        Debug.Log("TestBlaster fired!");
    }
}
