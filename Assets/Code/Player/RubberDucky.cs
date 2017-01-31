using System.Collections.Generic;
using Assets.Code.Audio;
using Assets.Code.Extensions;
using Assets.Code.Input;
using Assets.Code.Play;
using Assets.Code.Projectiles;
using Assets.Code.Weapons;
using UnityEngine;
using UnityEngine.Assertions.Must;
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
        [AutoResolve] private AudioPooler _audioPooler;
        
        public List<Weapon> Weapons;
        public Weapon SelectedWeapon;
        public List<AudioClip> HurtSounds;
        public List<AudioClip> AttackSounds;
        public List<AudioClip> BoomSounds;
        public List<AudioClip> MoveSounds;

        private SubscribedEventToken _onHealthChanged;

        private Queue<ProjectileActivation> _activationQueue;

        protected void Awake()
        {
            Resolver.AutoResolve(this);

            _onHealthChanged = Stats.OnHealthChanged.Subscribe(OnHealthChanged);
            _activationQueue = new Queue<ProjectileActivation>();
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
            if (isLocalPlayer) HandleInput();
        }

        private void HandleInput()
		{
			transform.rotation = Quaternion.Euler(0,
				AngleMath.SignedAngleBetween(
					Vector3.forward,
					_groundCast.GetMouseGroundPosition(UnityEngine.Input.mousePosition) -
                    transform.position,
					Vector3.up), 0);

            if (!_buttonKnower.WasJustADamnedButton() && UnityEngine.Input.GetButtonUp("fire"))
            {
                _audioPooler.PlaySound(new PooledAudioRequest
                {
                    Sound = HurtSounds.Count > 0 ? AttackSounds.GetRandomItem() : null,
                    Target = transform.position
                });

                CmdShoot();
            }

            if (!_buttonKnower.WasJustADamnedButton() && UnityEngine.Input.GetButtonUp("activate"))
            {
                _audioPooler.PlaySound(new PooledAudioRequest
                {
                    Sound = HurtSounds.Count > 0 ? BoomSounds.GetRandomItem() : null,
                    Target = transform.position
                });

                CmdActivate();
            }
        }

        public void AddActivatable(ProjectileActivation act)
        {
            _activationQueue.Enqueue(act);
        }

        [Command]
	    private void CmdShoot()
	    {
	        if (SelectedWeapon == null || !SelectedWeapon.CanActivate()) return;

            SelectedWeapon.Activate(this);
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
}
