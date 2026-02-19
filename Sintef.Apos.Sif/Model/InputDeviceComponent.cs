namespace Sintef.Apos.Sif.Model
{
    public class InputDeviceComponent : SISComponent
    {
        public UnitOfMeasure UnitOfMeasure { get; protected set; }
        public RangeMax RangeMax { get; protected set; }
        public RangeMin RangeMin { get; protected set; }
        public Accuracy Accuracy { get; protected set; }
        public Boolean AlarmResetAfterShutdownIsRequired { get; protected set; }
        public String AlarmDescription { get; protected set; }
        public Boolean MeasurementComparisonIsRequired { get; protected set; }

        public new const string RefBaseSystemUnitPath = "SIS Unit Classes/InputDeviceComponent";

        public InputDeviceComponent(Group parent, string name) : base(parent, name, 7)
        {
        }
    }
}
