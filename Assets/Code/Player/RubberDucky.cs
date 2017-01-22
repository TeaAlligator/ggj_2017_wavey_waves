using System.Collections.Generic;
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
        [SerializeField] private readonly float _maximumHealth = 100f;

        [AutoResolve] private GroundRaycaster _groundCast;
        [AutoResolve] private ButtonKnower _buttonKnower;
        [AutoResolve] private DuckInspectCanvasController _inspectDuck;

        private float _health;
        public float Health
        {
            get { return _health; }
            set
            {
                var oldValue = _health;

                _health = value;
                HealthPercent = value / _maximumHealth;

                OnHealthChanged.Fire(new HealthChangedData
                {
                    OldHealth = oldValue,
                    NewHealth = value,
                    DeltaHealth = value - oldValue,
                    Percent = HealthPercent
                });
            }
        }
        public float HealthPercent { get; private set; }
        public SubscribedEvent<HealthChangedData> OnHealthChanged;
        
        public List<Weapon> Weapons;
        public Weapon SelectedWeapon;

        protected void Awake()
        {
            Resolver.AutoResolve(this);

            _health = _maximumHealth;
            HealthPercent = 1.0f;

            OnHealthChanged = new SubscribedEvent<HealthChangedData>();
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
                transform.rotation.SetLookRotation(transform.position -
                    _groundCast.GetMouseGroundPosition(UnityEngine.Input.mousePosition),
                    Vector3.up);
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
