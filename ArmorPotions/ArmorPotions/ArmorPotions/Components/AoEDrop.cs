﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmorPotionFramework.EntityClasses.Components;
using ArmorPotionFramework.Projectiles;
using ArmorPotionFramework.SpriteClasses;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ArmorPotionFramework.EntityClasses;
using ArmorPotionFramework.TileEngine;

namespace ArmorPotions.Components
{
    public class AoEDrop : IAIComponent
    {
        private int _lifetime;
        private int _waitTime;
        private AreaOfEffectProjectile _projectile;
        private bool _isSetUp = false;

        private int _defaultLifetime;
        private int _defaultWaitTime;
        private ushort _aoeDamage;

        private AnimatedSprite _sprite;
        private EventType _eventType;

        public AoEDrop()
        {
            _defaultLifetime = 3000;
            _defaultWaitTime = 1000;
        }

        public void SetUp(ArmorPotionFramework.EntityClasses.Enemy enemy)
        {
            _lifetime = _defaultLifetime;
            _waitTime = _defaultWaitTime;
            AnimationKey currentAnimation = enemy.CurrentSprite.CurrentAnimation;
            enemy.CurrentSpriteKey = "Charging";
            enemy.CurrentSprite.IsAnimating = true;
            enemy.CurrentSprite.CurrentAnimation = currentAnimation;
        }

        public int LifeTime
        {
            get { return this._defaultLifetime; }
            set { this._defaultLifetime = value; }
        }

        public int Wait
        {
            get { return this._defaultWaitTime; }
            set { this._defaultWaitTime = value; }
        }

        public AnimatedSprite AttackTexture
        {
            get { return this._sprite; }
            set { this._sprite = value.Clone(); }
        }

        public EventType EventType
        {
            get { return this._eventType; }
            set { this._eventType = value; }
        }

        public ushort Damage
        {
            get { return this._aoeDamage; }
            set { this._aoeDamage = value; }
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime, ArmorPotionFramework.EntityClasses.Enemy enemy)
        {
            bool canAttack = WaitTimer(gameTime);
            if (canAttack && !_isSetUp)
            {
                Vector2 newPosition = new Vector2(enemy.Position.X - enemy.CurrentSprite.Width / 2, enemy.Position.Y - enemy.CurrentSprite.Height / 2);

                Animation animation = new Animation(1, 256, 256, 0, 0);
                _projectile = new AreaOfEffectProjectile(enemy.World, null, ProjectileTarget.Player, EventType.LightningEvent, false, newPosition, _defaultLifetime);
                _projectile.AnimatedSprites.Add("Normal", _sprite.Clone());
                _projectile.DamageAmount = _aoeDamage;

                enemy.World.Projectiles.Add(_projectile);
                _lifetime = _defaultLifetime;
                _isSetUp = true;
                enemy.CurrentSpriteKey = "Walking";
            }
            else if(canAttack && _isSetUp)
            {
                _lifetime -= gameTime.ElapsedGameTime.Milliseconds;
                if (_lifetime < 0)
                {
                    _isSetUp = false;
                    enemy.ActionComplete();
                }
            }
        }

        private bool WaitTimer(GameTime gameTime)
        {
            if (_waitTime < 0)
                return true;

            _waitTime -= gameTime.ElapsedGameTime.Milliseconds;
            return false;
        }


        public IAIComponent Clone()
        {
            AoEDrop newAOEDropComponent = new AoEDrop();
            
            newAOEDropComponent.EventType = this._eventType;
            newAOEDropComponent.LifeTime = this._defaultLifetime;
            newAOEDropComponent.AttackTexture = this._sprite; //The set property calls clone already
            newAOEDropComponent.Damage = this._aoeDamage;
            newAOEDropComponent.Wait = this._defaultWaitTime;

            return newAOEDropComponent;
        }
    }
}
