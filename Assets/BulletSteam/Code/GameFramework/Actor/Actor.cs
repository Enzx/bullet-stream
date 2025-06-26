using BulletSteam.GameFramework.Actor.Behaviours;
using BulletSteam.GameFramework.Actor.DataModel;
using BulletSteam.GameFramework.Actor.View;
using BulletSteam.GameFramework.Collections;
using BulletSteam.GameFramework.DataModel;
using BulletSteam.GameFramework.Graph;
using BulletSteam.GameFramework.Messaging;

namespace BulletSteam.GameFramework.Actor
{
    public class Actor : IBehaviour
    {
        
        private StateMachine<Actor> _stateMachine;
        public readonly ServiceLocator<IBehaviour> Behaviors;
        public readonly Events Events;

        public readonly ActorView View;
        public readonly IWorld World;


        public Actor(ActorData entityData, ActorView entityView, IWorld world)
        {
            View = entityView;
            World = world;
            Events = new Events();
            Behaviors = new ServiceLocator<IBehaviour>(entityData.Behaviours.Count);
            Builder builder = new(this, Behaviors);
            for (int index = 0; index < entityData.Behaviours.Count; index++)
            {
                IData bd = entityData.Behaviours[index];
                bd.Accept(builder);
            }

            Behaviors.Build();
        }
        
        public void SetLogicGraph(StateMachine<Actor> logicGraph)
        {
            _stateMachine = logicGraph;
        }

        public void Update()
        {
            
            _stateMachine.DeltaTime = World.DeltaTime;
            Behaviors.ForEach( b =>
            {
                Behaviour behaviour = (Behaviour) b;  
                behaviour.Update(World.DeltaTime);
            });
            _stateMachine.Execute();
        }
    }
}