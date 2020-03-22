using System.Collections.Generic;

namespace Solis
{
    public class DrawLayer
    {
        public int LayerID { get; set; }
        public bool IsVisible { get; set; }
        public bool ShouldSort { get; set; }
        private List<DrawableComponent> _components;

        public DrawLayer(int layer)
        {
            LayerID = layer;
            IsVisible = true;
            _components = new List<DrawableComponent>();
        }

        public void AddComponent(DrawableComponent component)
        {
            _components.Add(component);
        }

        public void RemoveComponent(DrawableComponent component)
        {
            _components.Remove(component);
        }

        public void Draw()
        {
            foreach (DrawableComponent component in _components)
            {
                component.Draw();
            }
        }
    }
}