using Godot;

namespace Game.Utilities
{
    public partial class State : Node
    {
        [Export] public Node StateOwner;

        public virtual void EnterState()
        {
            
        }

        public virtual void ExitState()
        {
            
        }
    }
}

