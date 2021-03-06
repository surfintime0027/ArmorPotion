﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmorPotionFramework.Game;
using ArmorPotionFramework.EntityClasses;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ArmorPotionFramework.Items;
using ArmorPotionFramework.Input;
using ArmorPotionFramework.CameraSystem;
using ArmorPotionFramework.Projectiles;
using ArmorPotionFramework.TileEngine;
using ArmorPotionFramework.Loading;
using ArmorPotionFramework.EntityClasses.Data;
using ArmorPotionFramework.Data;

namespace ArmorPotionFramework.WorldClasses
{
    public class World
    {
        private Player _player;
        private ArmorPotionsGame _game;
        public Item item;
        private Camera _camera;
        private List<Projectile> _projectiles;
        private List<Projectile> _projectilesToAdd;
        private Map currentDungeon;

        private Factory<Enemy, EnemyData> _enemyFactory;
        public readonly int MaxTileWidth;
        public readonly int MaxTileHeight;

        public World(ArmorPotionsGame game)
        {
            _game = game;
            _projectiles = new List<Projectile>();
            _projectilesToAdd = new List<Projectile>();

            _player = new Player(this, _game.Content.Load<Texture2D>(@"Player\PlayerWalking"), _game.Content.Load<Texture2D>(@"Player\SwordAttack"));
            _player.Position = new Vector2(350, 350);

            Rectangle clientBounds = _game.Window.ClientBounds;
            _camera = new Camera(_game, 0, 0, clientBounds.Width, clientBounds.Height, 1f);
            _game.Components.Add(_camera);

            MaxTileWidth = clientBounds.Width / Tile.Width;
            MaxTileHeight = clientBounds.Height / Tile.Height;
        }

        public ArmorPotionsGame Game
        {
            get
            {
                return _game;
            }
        }

        public Player Player
        {
            get
            {
                return _player;
            }
        }

        public List<Projectile> Projectiles
        {
            get
            {
                return this._projectiles;
            }
        }

        public List<Projectile> ProjectilesToAdd
        {
            get
            {
                return this._projectilesToAdd;
            }
        }

        public Camera Camera
        {
            get
            {
                return _camera;
            }
            set
            {
                _camera = value;
            }
        }

        public Map CurrentDungeon
        {
            get
            {
                return currentDungeon;
            }
            set{
                currentDungeon = value;
            }
        }

        public Factory<Enemy, EnemyData> EnemyFactory
        {
            get
            {
                return _enemyFactory;
            }
            set
            {
                this._enemyFactory = value;
            }
        }

        public void Update(GameTime gameTime)
        {
            Camera.CameraCenter = _player.Position;

            currentDungeon.Update(gameTime);

            UpdateProjectiles(gameTime);
            _player.Update(gameTime);

            if (_player.Position.X > 70 &&
                _player.Position.X < 70 + 256 &&
                _player.Position.Y > 70 &&
                _player.Position.Y < 70 + 256)
            {
                if (item != null) { item.CollectedBy(_player); item = null; };
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawMap(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);
            DrawProjectiles(gameTime, spriteBatch);
        }

        #region Helper Methods

        private void UpdateProjectiles(GameTime gameTime)
        {
            List<Projectile> removedProjectiles = new List<Projectile>();
            foreach (Projectile projectile in _projectiles)
            {
                projectile.Update(gameTime);

                if (!projectile.IsAlive)
                    removedProjectiles.Add(projectile);
            }

            foreach (Projectile projectile in removedProjectiles)
            {
                if (projectile.Source != null) projectile.Source.HasProjectile = false;
                _projectiles.Remove(projectile);
            }

            _projectiles.AddRange(_projectilesToAdd);
            _projectilesToAdd.Clear();
        }

        private void DrawProjectiles(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                _projectiles[i].Draw(gameTime, spriteBatch);
            }
        }

        private void DrawMap(GameTime gameTime, SpriteBatch spritebatch)
        {
            currentDungeon.Draw(gameTime, spritebatch);
        }

        #endregion
    }
}
