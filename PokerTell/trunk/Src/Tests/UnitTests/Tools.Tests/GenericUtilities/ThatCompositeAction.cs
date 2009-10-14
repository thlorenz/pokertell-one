namespace Tools.Tests.GenericUtilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Moq;

    using NUnit.Framework;

    using Tools.GenericUtilities;

    public class ThatCompositeAction
    {
        CompositeAction<int> _actions;

        StubBuilder _stub;

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _actions = new CompositeAction<int>();
        }

        [Test]
        public void Execute_NoActionsRegistered_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _actions.Execute(_stub.Some<int>()));
        }

        [Test]
        public void Register_Null_Throws()
        {
            const Action<int> nullAction = null;
            Assert.Throws<ArgumentNullException>(() => _actions.RegisterAction(nullAction));
        }

        [Test]
        public void Register_Action_RegistersAnAction()
        {
            Action<int> action = i => Console.WriteLine("Sample Action: {0}", i);
            _actions.RegisterAction(action);

            Assert.That(_actions.RegisteredActions.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Register_Action_RegistersThatAction()
        {
            Action<int> action = i => Console.WriteLine("Sample Action: {0}", i);
            _actions.RegisterAction(action);

            Assert.That(_actions.RegisteredActions.Last(), Is.EqualTo(action));
        }

        [Test]
        public void Unregister_ActionWasNotAdded_DoesNotThrow()
        {
            Action<int> action = i => Console.WriteLine("Sample Action: {0}", i);
            Assert.DoesNotThrow(() => _actions.UnregisterAction(action));
        }

        [Test]
        public void Unregister_ActionWasAdded_RemovesIt()
        {
            Action<int> action = i => Console.WriteLine("Sample Action: {0}", i);

            _actions
                .RegisterAction(action)
                .UnregisterAction(action);

            Assert.That(_actions.RegisteredActions.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_OneActionRegistered_ExecutesIt()
        {
            bool actionExecuted = false;

            Action<int> action = i => actionExecuted = true;

            _actions
                .RegisterAction(action)
                .Execute(_stub.Some<int>());

            Assert.That(actionExecuted, Is.True);
        }

        [Test]
        public void Execute_TwoActionsRegistered_ExecutesBoth()
        {
            bool actionExecuted1 = false;
            bool actionExecuted2 = false;

            Action<int> action1 = i => actionExecuted1 = true;
            Action<int> action2 = i => actionExecuted2 = true;

            _actions
                .RegisterAction(action1)
                .RegisterAction(action2)
                .Execute(_stub.Some<int>());

            var bothExecuted = actionExecuted1 && actionExecuted2;
            Assert.That(bothExecuted, Is.True);
        }

        [Test]
        public void Register_OneActionIsAlreadyRegistered_RegistersAsLast()
        {
            Action<int> action1 = i => Console.WriteLine("First");
            Action<int> action2 = i => Console.WriteLine("Second");

            _actions
                .RegisterAction(action1)
                .RegisterAction(action2);

            Assert.That(_actions.RegisteredActions.Last(), Is.EqualTo(action2));
        }

        [Test]
        public void Register_SameActionAlreadyRegistered_RemovesItAndRegistersItAgain()
        {
            Action<int> action1 = i => Console.WriteLine("First");

            _actions
                .RegisterAction(action1)
                .RegisterAction(action1);

            Assert.That(_actions.RegisteredActions.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Register_AsFirstIsTrueOneActionsAlreadyRegistered_RegistersAsFirst()
        {
            Action<int> action1 = i => Console.WriteLine("First");
            Action<int> action2 = i => Console.WriteLine("Second");

            _actions
                .RegisterAction(action1)
                .RegisterAction(action2, true);

            Assert.That(_actions.RegisteredActions.First(), Is.EqualTo(action2));
        }


        [Test]
        public void Execute_Action1ChangesPassedValueArgument_DoesNotChangeArgumentPassedToAction2()
        {
            var passedToAction2 = 0;

            Action<int> action1 = i => i++;
            Action<int> action2 = i => passedToAction2 = i;

            _actions
                .RegisterAction(action1)
                .RegisterAction(action2)
                .Execute(0);

            Assert.That(passedToAction2, Is.EqualTo(0));
        }

        [Test]
        public void Execute_Action1ChangesPassedReferenceArgument_DoesChangeArgumentPassedToAction2()
        {
            var refActions = new CompositeAction<ReferenceType>();

            const int originalIdentity = 0;
            const int newIdentity = 1;
            
            ReferenceType passedToAction2 = null;
           
            Action<ReferenceType> action1 = refArg => refArg.Identity = newIdentity;
            Action<ReferenceType> action2 = refArg => passedToAction2 = refArg;

            refActions
                .RegisterAction(action1)
                .RegisterAction(action2)
                .Execute(new ReferenceType(originalIdentity));

            Assert.That(passedToAction2.Identity, Is.EqualTo(newIdentity));
        }

        [Test]
        public void Execute_OneActionRegistered_ExecutesItWithPassedArgument()
        {
            const int arg = 1;
            int passedArg = 0;
            Action<int> action = i => passedArg = i;

            _actions
                .RegisterAction(action)
                .Execute(arg);

            Assert.That(passedArg, Is.EqualTo(arg));
        }

        [Test]
        public void Register_ActionCollection_RegistersAllActionsInCollection()
        {
            Action<int> action1 = i => Console.WriteLine("First");
            Action<int> action2 = i => Console.WriteLine("Second");

            IList<Action<int>> actionCollection = new List<Action<int>>()
                {
                    action1,
                    action2
                };

            _actions
                .RegisterActions(actionCollection);

            Assert.That(_actions.RegisteredActions.Count(), Is.EqualTo(2));
        }
    }

    internal class ReferenceType
    {
        public int Identity { get; set; }

        public ReferenceType(int identity)
        {
            Identity = identity;
        }
    }
}
