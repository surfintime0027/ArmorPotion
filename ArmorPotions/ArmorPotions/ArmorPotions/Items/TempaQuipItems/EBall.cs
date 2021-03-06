﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmorPotionFramework.Items;
using Microsoft.Xna.Framework.Graphics;
using ArmorPotionFramework.SpriteClasses;
using ArmorPotionFramework.Projectiles;
using Microsoft.Xna.Framework;
using ArmorPotionFramework.TileEngine;

namespace ArmorPotions.Items.TempaQuipItems
{
    public class EBall : TempaQuip
    {
        private AnimatedSprite _secondaryProjectileSprite;
        private float _throwDistance;
        private float _spreadAngle;
        private float _revolutions;
        private float _projectilesPerIteration;
        private float _projectileDistance;
        private EventType _eventType;

        private Vector2 _aoeDestination;
        private int _aoeLife;

        public EBall(Texture2D icon, String name, AnimatedSprite sprite, int wait, AnimatedSprite secondaryProjectileSprite, float throwDistance, float projectileDistance, int spreadAngle, float revolutions, float projectilesPerIteration, String eventType, float destinationX, float destinationY, int aoeLife)
            : base(icon, name, sprite, wait)
        {
            _secondaryProjectileSprite = secondaryProjectileSprite;

            this._allowMulti = false;
            this._hasProjectile = false;

            _throwDistance = throwDistance;
            _projectileDistance = projectileDistance;
            _spreadAngle = MathHelper.ToRadians(spreadAngle);
            _revolutions = revolutions;
            _projectilesPerIteration = projectilesPerIteration;
            _eventType = (EventType)Enum.Parse(typeof(EventType), eventType);

            _aoeDestination = new Vector2(destinationX, destinationY);
            _aoeLife = aoeLife;
        }

        public override void OnEquip(ArmorPotionFramework.EntityClasses.Player equippedBy)
        {
        }

        public override void OnActivate(Microsoft.Xna.Framework.GameTime gameTime, ArmorPotionFramework.EntityClasses.Player activatedBy)
        {
            if (!_hasProjectile)
            {
                _currentWaitTime = _maxWaitTime;

                ThrowProjectile projectile = new ThrowProjectile(
                    activatedBy.World, 
                    this,
                    ProjectileTarget.Enemy,
                    _eventType,
                    false,
                    this.CenterEntity(activatedBy), 
                    _throwDistance, 
                    MathHelper.ToRadians((int)activatedBy.CurrentSprite.CurrentAnimation * 90), 
                    _projectileDistance, 
                    _spreadAngle, 
                    _revolutions, 
                    _projectilesPerIteration,
                    true,
                    _aoeDestination,
                    _aoeLife);

                projectile.DamageAmount = 5;

                if (_eventType == EventType.LightningEvent)
                    projectile.DamageAmount = 25;
                else if (_eventType == EventType.IceEvent)
                    projectile.DamageAmount = 3;

                projectile.AnimatedSprites.Add("Normal", AnimatedSprite);
                projectile.AnimatedSprites.Add("Projectile", _secondaryProjectileSprite);

                activatedBy.World.Projectiles.Add(projectile);

                _hasProjectile = true;
            }
        }

        public override void OnUnEquip(ArmorPotionFramework.EntityClasses.Player removedBy)
        {
        }
    }
}
