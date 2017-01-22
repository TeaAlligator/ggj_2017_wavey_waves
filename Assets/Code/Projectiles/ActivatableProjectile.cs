using System;
using Assets.Code.Player;
using UnityEngine.Networking;

namespace Assets.Code.Projectiles
{
    class ProjectileActivation
    {
        public ActivatableProjectile Projectile;
        public Action Activate;
    }
    
    class ActivatableProjectile : Projectile
    {
        public override void RegisterWithSender(RubberDucky sender)
        {
            base.RegisterWithSender(sender);

            if (Sender != null)
            {
                Sender.AddActivatable(new ProjectileActivation
                {
                    Activate = CmdActivate,
                    Projectile = this
                });
            }
        }

        [Command]
        protected virtual void CmdActivate()
        {
            Destroy(gameObject);
        }
    }
}
