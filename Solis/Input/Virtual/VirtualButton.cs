using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Solis
{
    public class VirtualButton : VirtualInput
    {
        public abstract class Node : VirtualInputNode
        {
            public abstract bool IsDown { get; }
            public abstract bool IsPressed { get; }
            public abstract bool IsReleased { get; }
        }

        public List<Node> Nodes;
        public float BufferTime;
        public float FirstRepeatTime;
        public float MultiRepeatTime;
        public bool IsRepeating { get; private set; }

        private float _bufferCounter;
        private float _repeatCounter;
        private bool _willRepeat;

        public VirtualButton(float bufferTime)
        {
            Nodes = new List<Node>();
            BufferTime = bufferTime;
        }

        public VirtualButton() : this(0)
        {
        }

        public VirtualButton(float bufferTime, params Node[] nodes)
        {
            Nodes = new List<Node>(nodes);
            BufferTime = bufferTime;
        }

        public VirtualButton(params Node[] nodes) : this(0, nodes)
        {
        }

        public void SetRepeat(float repeatTime)
        {
            SetRepeat(repeatTime, repeatTime);
        }

        public void SetRepeat(float firstRepeatTime, float multiRepeatTime)
        {
            FirstRepeatTime = firstRepeatTime;
            MultiRepeatTime = multiRepeatTime;
            _willRepeat = firstRepeatTime > 0;
            if (!_willRepeat)
                IsRepeating = false;
        }

        public override void Update()
        {
            _bufferCounter -= Time.UnscaledDeltaTime;
            IsRepeating = false;

            var check = false;
            for (var i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].Update();
                if (Nodes[i].IsPressed)
                {
                    _bufferCounter = BufferTime;
                    check = true;
                }
                else if (Nodes[i].IsDown)
                {
                    check = true;
                }
            }

            if (!check)
            {
                _repeatCounter = 0;
                _bufferCounter = 0;
            }
            else if (_willRepeat)
            {
                if (_repeatCounter == 0)
                {
                    _repeatCounter = FirstRepeatTime;
                }
                else
                {
                    _repeatCounter -= Time.UnscaledDeltaTime;
                    if (_repeatCounter <= 0)
                    {
                        IsRepeating = true;
                        _repeatCounter = MultiRepeatTime;
                    }
                }
            }
        }

        public bool IsDown
        {
            get
            {
                foreach (var node in Nodes)
                    if (node.IsDown)
                        return true;

                return false;
            }
        }

        public bool IsPressed
        {
            get
            {
                if (_bufferCounter > 0 || IsRepeating)
                    return true;

                foreach (var node in Nodes)
                    if (node.IsPressed)
                        return true;

                return false;
            }
        }

        public bool IsReleased
        {
            get
            {
                foreach (var node in Nodes)
                    if (node.IsReleased)
                        return true;

                return false;
            }
        }

        public void ConsumeBuffer()
        {
            _bufferCounter = 0;
        }

        public VirtualButton AddKeyboardKey(Keys key)
        {
            Nodes.Add(new KeyboardKey(key));
            return this;
        }

        public VirtualButton AddKeyboardKey(Keys key, Keys modifier)
        {
            Nodes.Add(new KeyboardModifiedKey(key, modifier));
            return this;
        }

        public class KeyboardKey : Node
        {
            public Keys Key;

            public KeyboardKey(Keys key)
            {
                Key = key;
            }

            public override bool IsDown => Input.IsKeyDown(Key);

            public override bool IsPressed => Input.IsKeyPressed(Key);

            public override bool IsReleased => Input.IsKeyReleased(Key);
        }

        public class KeyboardModifiedKey : Node
        {
            public Keys Key;
            public Keys Modifier;

            public KeyboardModifiedKey(Keys key, Keys modifier)
            {
                Key = key;
                Modifier = modifier;
            }

            public override bool IsDown => Input.IsKeyDown(Modifier) && Input.IsKeyDown(Key);

            public override bool IsPressed => Input.IsKeyDown(Modifier) && Input.IsKeyPressed(Key);

            public override bool IsReleased => Input.IsKeyReleased(Key);
        }
    }
}