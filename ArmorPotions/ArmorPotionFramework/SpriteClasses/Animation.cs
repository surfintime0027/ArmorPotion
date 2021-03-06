﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using System;

namespace ArmorPotionFramework.SpriteClasses
{
    public enum AnimationKey { Down = 1, Left = 2, Right = 0, Up = 3 } //Add Jump here

    public class Animation : ICloneable
    {
        #region Field Region

        private Rectangle[] _frames;
        private int _framesPerSecond;
        private TimeSpan _frameLength;
        private TimeSpan _frameTimer;
        private int _currentFrame;
        private int _frameWidth;
        private int _frameHeight;
        private bool _loop;
        private bool _iterated;

        #endregion

        #region Property Region

        public int FramesPerSecond
        {
            get { return _framesPerSecond; }
            set
            {
                if (value < 1)
                    _framesPerSecond = 1;
                else if (value > 60)
                    _framesPerSecond = 60;
                else
                    _framesPerSecond = value;
                _frameLength = TimeSpan.FromSeconds(1 / (double)_framesPerSecond);
            }
        }

        public Rectangle CurrentFrameRect
        {
            get { return _frames[_currentFrame]; }
        }

        public int CurrentFrame
        {
            get { return _currentFrame; }
            set
            {
                _currentFrame = (int)MathHelper.Clamp(value, 0, _frames.Length - 1);
            }
        }

        public int FrameWidth
        {
            get { return _frameWidth; }
        }

        public int FrameHeight
        {
            get { return _frameHeight; }
        }

        public bool Iterated
        {
            get { return this._iterated; }
            set { this._iterated = value; }
        }

        public bool Loop
        {
            get { return this._loop; }
            set { this._loop = value; }
        }

        #endregion

        #region Constructor Region

        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset, bool loop)
        {
            _frames = new Rectangle[frameCount];
            this._frameWidth = frameWidth;
            this._frameHeight = frameHeight;
            this._loop = loop;

            for (int i = 0; i < frameCount; i++)
            {
                _frames[i] = new Rectangle(
                        xOffset + (frameWidth * i),
                        yOffset,
                        frameWidth,
                        frameHeight);
            }
            FramesPerSecond = 5;
            Reset();
        }

        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset) : this(frameCount, frameWidth, frameHeight, xOffset, yOffset, true)
        {
        }

        private Animation(Animation animation)
        {
            this._frames = animation._frames;
            FramesPerSecond = 5;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            _frameTimer += gameTime.ElapsedGameTime;

            if (_frameTimer >= _frameLength && !_iterated)
            {
                _frameTimer = TimeSpan.Zero;
                _currentFrame = (_currentFrame + 1) % _frames.Length;
                if (!_loop && _currentFrame == _frames.Length - 1)
                    _iterated = true;
            }
        }

        public void Reset()
        {
            _currentFrame = 0;
            _iterated = false;
            _frameTimer = TimeSpan.Zero;
        }

        #endregion

        #region Interface Method Region

        public object Clone()
        {
            Animation animationClone = new Animation(this);

            animationClone._frameWidth = this._frameWidth;
            animationClone._frameHeight = this._frameHeight;
            animationClone.FramesPerSecond = this._framesPerSecond;
            animationClone.Loop = this._loop;
            animationClone.Reset();

            return animationClone;
        }

        #endregion
    }
}
