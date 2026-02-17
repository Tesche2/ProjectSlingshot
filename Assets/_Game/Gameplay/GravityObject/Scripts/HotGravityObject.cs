using Unity.AppUI.UI;
using UnityEngine;

public class HotGravityObject : GravityObject
{
    [SerializeField] private float _temperature = 100f;

    private HeatHandler _playerHeat;

    private static int _numberOfInfluencingHeat = 0;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(_playerHeat != null )
            _playerHeat.HeatUp(_temperature / sqrDistToPlayer);
    }

    protected override void OnTriggerEnter2D(Collider2D playerCollider)
    {
        base.OnTriggerEnter2D(playerCollider);

        // Get heat handler reference
        _playerHeat = playerCollider.gameObject.GetComponent<HeatHandler>();

        // Add this to the heat field count
        _numberOfInfluencingHeat += 1;
        _playerHeat.isInHeatField = true;
    }

    protected override void OnTriggerExit2D(Collider2D playerCollider)
    {
        base.OnTriggerExit2D(playerCollider);

        // Remove this from the heat field count
        _numberOfInfluencingHeat -= 1;
        if(_numberOfInfluencingHeat == 0)
        {
            // Let player cool down if there are no more heating fields around
            _playerHeat.isInHeatField = false;
        }

        _playerHeat = null;
    }
}
