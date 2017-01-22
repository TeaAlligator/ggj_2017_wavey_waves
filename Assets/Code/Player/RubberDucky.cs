using System.Collections.Generic;
using Assets.Code.Extensions;
using Assets.Code.Input;
using Assets.Code.Play;
using Assets.Code.Weapons;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code.Player
{
    struct HealthChangedData
    {
        public float OldHealth;
        public float NewHealth;
        public float DeltaHealth;
        public float Percent;
    }

    class RubberDucky : NetworkBehaviour
    {
        [SerializeField] private NetworkIdentity _netId;
        [SerializeField] public Statted Stats;

        [AutoResolve] private GroundRaycaster _groundCast;
        [AutoResolve] private ButtonKnower _buttonKnower;
        [AutoResolve] private DuckInspectCanvasController _inspectDuck;
        
        public List<Weapon> Weapons;
        public Weapon SelectedWeapon;

        protected void Awake()
        {
            Resolver.AutoResolve(this);
        }

        public override void OnStartLocalPlayer()
        {
            _inspectDuck.StartSession(new DuckInfoSession
            {
                Subject = this
            });

            base.OnStartLocalPlayer();
        }

        protected void Update()
        {
            if (isLocalPlayer) HandleInput();
        }

        private void HandleInput()
        {
            if (!_buttonKnower.WasJustADamnedButton() && UnityEngine.Input.GetButton("fire"))
            {
                transform.rotation = Quaternion.Euler(0,
                    AngleMath.SignedAngleBetween(
                        Vector3.forward,
                        _groundCast.GetMouseGroundPosition(UnityEngine.Input.mousePosition),
                        Vector3.up), 0);
            }

            if (!_buttonKnower.WasJustADamnedButton() && UnityEngine.Input.GetButtonUp("fire"))
            {
                CmdShoot();
            }
        }

        [Command]
	    private void CmdShoot()
	    {
	        if (SelectedWeapon == null || !SelectedWeapon.CanActivate()) return;

            SelectedWeapon.Activate(this);
	    }
    }
}
