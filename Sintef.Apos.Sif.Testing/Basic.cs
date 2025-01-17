namespace Sintef.Apos.Sif.Testing
{
    public class Basic
    {
        [Fact]
        public void VerifyModelVersion()
        {
            Assert.Equal("18", Definition.Version);
        }

        [Fact]
        public void BuildSaveLoadCompare()
        {
            var builder = new Builder();

            var sif = builder.SIFs.Append("SIF-00ABC23");

            var subsystem = sif.Subsystems.Append();

            var initator = subsystem.CreateInputDevice();
            var logicSolver = subsystem.CreateLogicSolver();
            var finalElement = subsystem.CreateFinalElement();

            var initiatorGroup = initator.Groups.Append();
            var logicSolverGroup = logicSolver.Groups.Append();
            var finalElementGroup = finalElement.Groups.Append();

            initiatorGroup.ComponentVoter.K.Value = "2";
            var initiatorComponent1 = initiatorGroup.Components.Append("TT-1001");
            initiatorComponent1.SIL.Value = "SIL2";
            initiatorComponent1.ProofTestInterval.Value = "3000";

            var initiatorComponent2 = initiatorGroup.Components.Append("TT-1002");
            initiatorComponent2.SIL.Value = "SIL2";
            initiatorComponent2.ProofTestInterval.Value = "6000";

            var logicSolverComponent = logicSolverGroup.Components.Append("C01");
            logicSolverComponent.SIL.Value = "SIL1";

            var finalElementComponent = finalElementGroup.Components.Append("ESV-3023");
            finalElementComponent.SIL.Value = "SIL1";

            var sifIsValid = builder.Validate();
            Assert.True(sifIsValid);

            const string filename = "SIF-00ABC23.aml";

            builder.SaveToFile(filename);

            var builder2 = new Builder();
            builder2.LoadFromFile(filename);

            var sif2IsValid = builder2.Validate();
            Assert.True(sif2IsValid);

            var sif2 = builder2.SIFs.Single();


            Assert.Equal("SIF-00ABC23", sif2.SIFID.Value);

            var subsystem2 = sif2.Subsystems.Single();
            var initiator2 = subsystem2.InputDevice;
            var logicSolver2 = subsystem2.LogicSolver;
            var finalElement2 = subsystem2.FinalElement;

            var initiator2Group = initiator2.Groups.Single();
            var logicSolver2Group = logicSolver2.Groups.Single();
            var finalElement2Group = finalElement2.Groups.Single();

            Assert.Equal(2, initiator2Group.Components.Count());
            Assert.Single(logicSolver2Group.Components);
            Assert.Single(finalElement2Group.Components);

            Assert.Equal("2", initiator2Group.ComponentVoter.K.Value);

            var initiator2Component1 = initiator2Group.Components.Single(x => x.Name.Value == "TT-1001");
            Assert.Equal("SIL2", initiator2Component1.SIL.Value);
            Assert.Equal("3000", initiator2Component1.ProofTestInterval.Value);

            var initiator2Component2 = initiator2Group.Components.Single(x => x.Name.Value == "TT-1002");
            Assert.Equal("SIL2", initiator2Component2.SIL.Value);
            Assert.Equal("6000", initiator2Component2.ProofTestInterval.Value);


            var logicSolver2Component = logicSolver2Group.Components.Single(x => x.Name.Value == "C01");
            Assert.Equal("SIL1", logicSolver2Component.SIL.Value);

            var finalElement2Component = finalElement2Group.Components.Single(x => x.Name.Value == "ESV-3023");
            Assert.Equal("SIL1", finalElementComponent.SIL.Value);


            Assert.True(sif.IsSameAs(sif2));
        }
    }
}