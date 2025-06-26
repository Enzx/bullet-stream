using System;
using System.Collections.Generic;

namespace BulletSteam.GameFramework.Collections
{
    public abstract class Pipe<TInput>
    {
        private Action _action;
        protected readonly TInput Agent;

        public TInput GetAgent() => Agent;

        protected Pipe(TInput agent)
        {
            this.Agent = agent;
        }

        public void SetNext(Action handle)
        {
            _action = handle;
        }

        public void Handle()
        {
            OnHandle();
            _action.Invoke();
        }

        protected abstract void OnHandle();
    }

    public class PipelineBuilder<Tinput>
    {
        private readonly List<Type> _pipeTypes;
        private readonly Pipe<Tinput> _mainPipe;

        public PipelineBuilder(Pipe<Tinput> pipe)
        {
            _pipeTypes = new List<Type>(10);
            _mainPipe = pipe;
        }

        public PipelineBuilder<Tinput> Add<TPipe>() where TPipe : Pipe<Tinput>
        {
            _pipeTypes.Add(typeof(TPipe));

            return this;
        }

        public Pipe<Tinput> Build() => Create();

        private Pipe<Tinput> Create(int index = 0)
        {
            if (_mainPipe == null) throw new NullReferenceException("Can not create pipe with nor root pipe");

            Pipe<Tinput> current = _mainPipe;
            Tinput agent = _mainPipe.GetAgent();
            for (int i = 0; i < _pipeTypes.Count; i++)
            {
                Pipe<Tinput> child = Activator.CreateInstance(_pipeTypes[i], args: agent) as Pipe<Tinput>;
                if (child == null) throw new NullReferenceException($"Can not create pipe with type: {_pipeTypes[i]}");
                current?.SetNext(child.Handle);
                current = child;
            }
            current.SetNext(() => { });
            
            return _mainPipe;
        }
    }
}