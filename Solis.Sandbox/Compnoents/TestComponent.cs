using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solis.Sandbox.Compnoents
{
    public class TestComponent : Component, IUpdatable
    {
        public string Name;

        public TestComponent(string name)
        {
            Name = name;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            Console.WriteLine("Component {0} has been added to {1}", Name, ParentEntity.Name);
        }

        public void Update()
        {
            Console.WriteLine("{0} is attached to {1}", Name, ParentEntity.Id);
        }
    }
}
