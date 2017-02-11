using System.Collections.Generic;
using System.Globalization;
using Assets.Code.Audio;
using Assets.Code.Extensions;
using Assets.Code.Input;
using Assets.Code.Play;
using Assets.Code.Projectiles;
using Assets.Code.Timing;
using Assets.Code.Weapons;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code.Player
{
    class RubberDucky : NetworkBehaviour
    {
        [SerializeField] private NetworkIdentity _netId;
        [SerializeField] public Statted Stats;
        [SerializeField] public Transform FacingTransform;
        [SerializeField] public Transform WeaponParent;
        [SerializeField] private DuckWorldspaceInfoCanvasController _worldspaceInfo;

        [AutoResolve] private GroundRaycaster _groundCast;
        [AutoResolve] private BetterInput _betterInput;
        [AutoResolve] private DuckInspectCanvasController _inspectDuck;
        [AutoResolve] private AudioPooler _audioPooler;
        [AutoResolve] private Delayer _delayer;
        
        public List<Weapon> Weapons;
        public List<AudioClip> HurtSounds;
        public List<AudioClip> AttackSounds;
        public List<AudioClip> BoomSounds;
        public List<AudioClip> MoveSounds;

        [SyncVar] private int _selectedWeaponIndex = -1;
        public Weapon SelectedWeapon;
        private Weapon _targetSwitchWeapon;

        private SubscribedEventToken _onHealthChanged;
        private SubscribedEventToken _onWeaponSwitchedFrom;

        private Queue<ProjectileActivation> _activationQueue;

        protected void Awake()
        {
            Resolver.AutoResolve(this);

            _onHealthChanged = Stats.OnHealthChanged.Subscribe(OnHealthChanged);
            _activationQueue = new Queue<ProjectileActivation>();
            
            _worldspaceInfo.StartSession(new DuckInfoSession
                {
                    Subject = this
                });
        }

        public override void OnStartClient()
        {
            //Debug.Log("boop");
            if (!isLocalPlayer)
            {
                //Debug.Log("boop 2");
                HandleSwitchWeapon(_selectedWeaponIndex);
            }

            base.OnStartClient();
        }

        public override void OnStartLocalPlayer()
        {
            _inspectDuck.StartSession(new DuckInfoSession
            {
                Subject = this
            });

            base.OnStartLocalPlayer();
            
            _audioPooler.PlaySound(new PooledAudioRequest
            {
                Sound = HurtSounds.Count > 0 ? MoveSounds.GetRandomItem() : null,
                Target = transform.position
            });
        }

        private void OnHealthChanged(HealthChangedData data)
        {
            _audioPooler.PlaySound(new PooledAudioRequest
            {
                Sound = HurtSounds.Count > 0 ? HurtSounds.GetRandomItem() : null,
                Target = transform.position
            });
        }

        protected void Update()
        {
            if (isLocalPlayer && _betterInput.IsMouseInWindow()) HandleInput();
        }

        private void HandleInput()
        {
            FacingTransform.rotation = Quaternion.Euler(0,
                AngleMath.SignedAngleBetween(
                    Vector3.forward,
                    _groundCast.GetMouseGroundPosition(UnityEngine.Input.mousePosition) -
                    transform.position,
                    Vector3.up), 0);

            if (_betterInput.BasicDecencyMet &&
                UnityEngine.Input.GetButtonUp("fire") && SelectedWeapon != null)
            {
                _audioPooler.PlaySound(new PooledAudioRequest
                {
                    Sound = HurtSounds.Count > 0 ? AttackSounds.GetRandomItem() : null,
                    Target = transform.position
                });

                CmdShoot(SelectedWeapon.WeaponOrigin.position, SelectedWeapon.WeaponOrigin.rotation);
            }

            if (_betterInput.BasicDecencyMet
                && UnityEngine.Input.GetButtonUp("activate"))
            {
                _audioPooler.PlaySound(new PooledAudioRequest
                {
                    Sound = HurtSounds.Count > 0 ? BoomSounds.GetRandomItem() : null,
                    Target = transform.position
                });

                CmdActivate();
            }

            // switching weapons
            if (_betterInput.BasicDecencyMet)
            {
                for (var i = 0; i < 10; i++)
                    if (i < Weapons.Count)
                        if (UnityEngine.Input.GetButtonDown(string.Format("equip_{0}", i.ToString(CultureInfo.InvariantCulture))))
                            NetSwitchWeapon(i);

                if (UnityEngine.Input.GetButtonDown("equip_null"))
                    NetSwitchWeapon();
            }
        }
    
        public void AddActivatable(ProjectileActivation act)
        {
            _activationQueue.Enqueue(act);
        }
        
        private void HandleSwitchWeapon(int weaponIndex)
        {
            // bail if our index is out of range
            if (weaponIndex >= Weapons.Count) return;

            var targetWeapon = weaponIndex > -1 ? Weapons[weaponIndex] : null;

            // we don't switch if the weapons are the same
            if (targetWeapon == SelectedWeapon)
            {
                if (_onWeaponSwitchedFrom != null)
                {
                    _onWeaponSwitchedFrom.Cancel();
                    _onWeaponSwitchedFrom = null;
                }

                if (SelectedWeapon != null)
                    SelectedWeapon.Switcher.SwitchTo();

                return;
            }

            if (_onWeaponSwitchedFrom != null)
            {
                // if we're already switching weapons
                // and haven't put our old weapon away yet
                // then we can just change the targets
                
                _targetSwitchWeapon = targetWeapon;
                return;
            }

            // if we currently have a weapon (not the target one)
            // we gotta switch from it
            if (SelectedWeapon != null && SelectedWeapon != targetWeapon)
            {
                _targetSwitchWeapon = targetWeapon;

                _onWeaponSwitchedFrom = SelectedWeapon.Switcher.OnSwitchedFromFinished.Subscribe(OnWeaponSwitchedFromFinished);
                SelectedWeapon.Switcher.SwitchFrom();
            }

            // but if we did not have a weapon (or had the target one)
            // then we can immediately switch to our new one
            else if (targetWeapon != null)
            {
                SelectedWeapon = targetWeapon;
                SelectedWeapon.Switcher.SwitchTo();
            }
        }

        public void NetSwitchWeapon(int weaponIndex = -1)
        {
            _selectedWeaponIndex = weaponIndex;
            CmdSwitchWeapon(weaponIndex);
        }

        [ClientRpc]
        private void RpcSwitchWeapon(int weaponIndex)
        {
            HandleSwitchWeapon(weaponIndex);
        }

        [Command]
        private void CmdSwitchWeapon(int weaponIndex)
        {
            RpcSwitchWeapon(weaponIndex);
        }

        private void OnWeaponSwitchedFromFinished()
        {
            // clean up our subscription
            _onWeaponSwitchedFrom.Cancel();
            _onWeaponSwitchedFrom = null;
            
            SelectedWeapon = _targetSwitchWeapon;

            // it's possible we switched to no weapon
            // gotta handle that
            if (_targetSwitchWeapon == null) return;
            
            // otherwise switch to our new weapon
            SelectedWeapon.Switcher.SwitchTo();
        }

        [Command]
	    private void CmdShoot(Vector3 position, Quaternion rotation)
	    {
	        if (SelectedWeapon == null || !SelectedWeapon.CanActivate()) return;

            SelectedWeapon.Activate(this, position, rotation);
            RpcShoot();
	    }

        [ClientRpc]
        private void RpcShoot()
        {
            SelectedWeapon.CurrentAmmo--;
            SelectedWeapon.OnAmmoCountChanged.Fire(SelectedWeapon.CurrentAmmo);
        }

        [Command]
        private void CmdActivate()
        {
            ProjectileActivation activation = null;

            while (_activationQueue.Count > 0)
            {
                activation = _activationQueue.Dequeue();

                if (activation != null) break;
            }

            if (activation == null) return;
            activation.Activate();
        }
    }

    struct HealthChangedData
    {
        public float OldHealth;
        public float NewHealth;
        public float DeltaHealth;
        public float Percent;
    }
}
