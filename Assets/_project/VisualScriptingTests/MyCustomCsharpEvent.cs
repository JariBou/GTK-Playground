using Unity.VisualScripting;

namespace _project.VisualScriptingTests
{
    public class DeconstructTestClass : Unit
    {
        [DoNotSerialize] // No need to serialize ports
        public ValueInput input; // Adding the ValueInput variable for myValueA

        [DoNotSerialize] // No need to serialize ports
        public ValueOutput numberOfTimesTriggered; // Adding the ValueInput variable for myValueB

        private TestClass _data;
    
        protected override void Definition()
        {
            input = ValueInput<TestClass>(nameof(input), null);
        
            numberOfTimesTriggered = ValueOutput("result", _ => _data.numberOfTimesTriggered);
        }
    }

    public static class EventNames
    {
        public static string MyCustomCsharpEvent = "MyCustomCsharpEvent";
    }

    [UnitTitle("On my Custom C# Event")]//The Custom Scripting Event node to receive the Event. Add "On" to the node title as an Event naming convention.
    [UnitCategory("Events\\MyEvents")]//Set the path to find the node in the fuzzy finder as Events > My Events.
    public class MyCustomCsharpEvent : EventUnit<TestClass>
    {
        [DoNotSerialize]// No need to serialize ports.
        public ValueOutput result { get; private set; }// The Event output data to return when the Event is triggered.
        protected override bool register => true;

        // Add an EventHook with the name of the Event to the list of Visual Scripting Events.
        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook(EventNames.MyCustomCsharpEvent);
        }
        protected override void Definition()
        {
            base.Definition();
            // Setting the value on our port.
            result = ValueOutput<TestClass>(nameof(result));
        }
        // Setting the value on our port.
        protected override void AssignArguments(Flow flow, TestClass data)
        {
            flow.SetValue(result, data);
        }
    }
}