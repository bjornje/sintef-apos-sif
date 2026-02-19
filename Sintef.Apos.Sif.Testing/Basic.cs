namespace Sintef.Apos.Sif.Testing
{
    public class Basic
    {
        [Fact]
        public void VerifyModelVersion()
        {
            Assert.Equal("22", Definition.Version);
        }

        [Fact]
        public void BuildSaveLoadCompare()
        {
            var builder = new Builder();

            var sif = builder.SIFs.Append("SIF-00ABC23");

            var initator = sif.Subsystems.AppendInputDevice();
            initator.VoteBetweenGroups(1, 1);

            var logicSolver = sif.Subsystems.AppendLogicSolver();
            logicSolver.VoteBetweenGroups(1, 1);

            var finalElement = sif.Subsystems.AppendFinalElement();
            finalElement.VoteBetweenGroups(1, 1);

            Assert.Equal(3, sif.Subsystems.Count());

            var initiatorGroup = initator.Groups.Append();
            initiatorGroup.VoteWithinGroup(1, 1);

            var logicSolverGroup = logicSolver.Groups.Append();
            logicSolverGroup.VoteWithinGroup(1, 1);

            var finalElementGroup = finalElement.Groups.Append();
            finalElementGroup.VoteWithinGroup(1, 1);

            initiatorGroup.VoteWithinGroup(2, 2);

            var initiatorComponent1 = initiatorGroup.Components.Append("TT-1001");
            initiatorComponent1.ProofTestIntervalSILCompliance.StringValue = "3000";
            initiatorComponent1.ProofTestCoverage.StringValue = "25.7";


            var initiatorComponent2 = initiatorGroup.Components.Append("TT-1002");
            initiatorComponent2.ProofTestIntervalSILCompliance.StringValue = "6000";
            initiatorComponent2.ProofTestCoverage.StringValue = "15.8";



            var logicSolverComponent = logicSolverGroup.Components.Append("C01");
            logicSolverComponent.ProofTestIntervalSILCompliance.StringValue = "7000";


            var finalElementComponent = finalElementGroup.Components.Append("ESV-3023");
            finalElementComponent.ProofTestIntervalSILCompliance.StringValue = "2000";

            Assert.Equal(2, initiatorGroup.MInVotingMooN.Value);
            Assert.Equal(2, initiatorGroup.NumberOfDevicesWithinGroup.Value);
            Assert.Equal(3000.0, initiatorComponent1.ProofTestIntervalSILCompliance.Value);
            Assert.Equal(25.7, initiatorComponent1.ProofTestCoverage.Value);
            Assert.Equal(6000.0, initiatorComponent2.ProofTestIntervalSILCompliance.Value);
            Assert.Equal(15.8, initiatorComponent2.ProofTestCoverage.Value);
            Assert.Equal(7000.0, logicSolverComponent.ProofTestIntervalSILCompliance.Value);
            Assert.Equal(2000.0, finalElementComponent.ProofTestIntervalSILCompliance.Value);


            var sifIsValid = builder.Validate();
            Assert.Empty(builder.Errors);
            Assert.True(sifIsValid);

            const string filename = "SIF-00ABC23.aml";

            builder.SaveToFile(filename);

            var builder2 = new Builder();
            builder2.LoadFromFile(filename);

            var sif2IsValid = builder2.Validate();
            Assert.Empty(builder2.Errors);
            Assert.True(sif2IsValid);

            var sif2 = builder2.SIFs.Single();
            Assert.Equal(3, sif2.Subsystems.Count());


            Assert.Equal("SIF-00ABC23", sif2.SIFID.Value);

            var initiator2 = sif2.InputDevice;
            var logicSolver2 = sif2.LogicSolver;
            var finalElement2 = sif2.FinalElement;

            Assert.Equal("1", initiator2.MInVotingMooN.StringValue);
            Assert.Equal("1", initiator2.NumberOfGroups.StringValue);

            Assert.Equal("1", logicSolver2.MInVotingMooN.StringValue);
            Assert.Equal("1", logicSolver2.NumberOfGroups.StringValue);

            Assert.Equal("1", finalElement2.MInVotingMooN.StringValue);
            Assert.Equal("1", finalElement2.NumberOfGroups.StringValue);

            var initiator2Group = initiator2.Groups.Single();
            var logicSolver2Group = logicSolver2.Groups.Single();
            var finalElement2Group = finalElement2.Groups.Single();

            Assert.Equal(2, initiator2Group.Components.Count());
            Assert.Single(logicSolver2Group.Components);
            Assert.Single(finalElement2Group.Components);

            Assert.Equal("2", initiator2Group.MInVotingMooN.StringValue);
            Assert.Equal("2", initiator2Group.NumberOfDevicesWithinGroup.StringValue);

            var initiator2Component1 = initiator2Group.Components.Single(x => x.Name.StringValue == "TT-1001");
            Assert.Equal("3000", initiator2Component1.ProofTestIntervalSILCompliance.StringValue);
            Assert.Equal("25.7", initiator2Component1.ProofTestCoverage.StringValue);

            var initiator2Component2 = initiator2Group.Components.Single(x => x.Name.StringValue == "TT-1002");
            Assert.Equal("6000", initiator2Component2.ProofTestIntervalSILCompliance.StringValue);
            Assert.Equal("15.8", initiator2Component2.ProofTestCoverage.StringValue);


            var logicSolver2Component = logicSolver2Group.Components.Single(x => x.Name.StringValue == "C01");
            Assert.Equal("7000", logicSolver2Component.ProofTestIntervalSILCompliance.StringValue);

            var finalElement2Component = finalElement2Group.Components.Single(x => x.Name.StringValue == "ESV-3023");
            Assert.Equal("2000", finalElement2Component.ProofTestIntervalSILCompliance.StringValue);

            Assert.Equal(2, initiator2Group.MInVotingMooN.Value);
            Assert.Equal(2, initiator2Group.NumberOfDevicesWithinGroup.Value);
            Assert.Equal(3000.0, initiator2Component1.ProofTestIntervalSILCompliance.Value);
            Assert.Equal(25.7, initiator2Component1.ProofTestCoverage.Value);
            Assert.Equal(6000.0, initiator2Component2.ProofTestIntervalSILCompliance.Value);
            Assert.Equal(15.8, initiator2Component2.ProofTestCoverage.Value);
            Assert.Equal(7000.0, logicSolver2Component.ProofTestIntervalSILCompliance.Value);
            Assert.Equal(2000.0, finalElement2Component.ProofTestIntervalSILCompliance.Value);

            Assert.True(sif.IsSameAs(sif2));
        }
        [Fact]
        public void BuildSaveToLoadFromStreamCompare()
        {
            var builder = new Builder();

            var sif = builder.SIFs.Append("SIF-00ABC23");

            var initator = sif.Subsystems.AppendInputDevice();
            initator.VoteBetweenGroups(1, 1);

            var logicSolver = sif.Subsystems.AppendLogicSolver();
            logicSolver.VoteBetweenGroups(1, 1);

            var finalElement = sif.Subsystems.AppendFinalElement();
            finalElement.VoteBetweenGroups(1, 1);

            Assert.Equal(3, sif.Subsystems.Count());

            var initiatorGroup = initator.Groups.Append();
            initiatorGroup.VoteWithinGroup(1, 1);

            var logicSolverGroup = logicSolver.Groups.Append();
            logicSolverGroup.VoteWithinGroup(1, 1);

            var finalElementGroup = finalElement.Groups.Append();
            finalElementGroup.VoteWithinGroup(1, 1);

            initiatorGroup.VoteWithinGroup(2, 2);

            var initiatorComponent1 = initiatorGroup.Components.Append("TT-1001");
            initiatorComponent1.ProofTestIntervalSILCompliance.StringValue = "3000";
            initiatorComponent1.ProofTestCoverage.StringValue = "25.7";


            var initiatorComponent2 = initiatorGroup.Components.Append("TT-1002");
            initiatorComponent2.ProofTestIntervalSILCompliance.StringValue = "6000";
            initiatorComponent2.ProofTestCoverage.StringValue = "15.8";



            var logicSolverComponent = logicSolverGroup.Components.Append("C01");
            logicSolverComponent.ProofTestIntervalSILCompliance.StringValue = "7000";


            var finalElementComponent = finalElementGroup.Components.Append("ESV-3023");
            finalElementComponent.ProofTestIntervalSILCompliance.StringValue = "2000";

            Assert.Equal(2, initiatorGroup.MInVotingMooN.Value);
            Assert.Equal(2, initiatorGroup.NumberOfDevicesWithinGroup.Value);
            Assert.Equal(3000.0, initiatorComponent1.ProofTestIntervalSILCompliance.Value);
            Assert.Equal(25.7, initiatorComponent1.ProofTestCoverage.Value);
            Assert.Equal(6000.0, initiatorComponent2.ProofTestIntervalSILCompliance.Value);
            Assert.Equal(15.8, initiatorComponent2.ProofTestCoverage.Value);
            Assert.Equal(7000.0, logicSolverComponent.ProofTestIntervalSILCompliance.Value);
            Assert.Equal(2000.0, finalElementComponent.ProofTestIntervalSILCompliance.Value);


            var sifIsValid = builder.Validate();
            Assert.Empty(builder.Errors);
            Assert.True(sifIsValid);

            using var outputStream = new MemoryStream();
            builder.SaveToStream(outputStream);

            var builder2 = new Builder();
            builder2.LoadFromStream(outputStream);

            var sif2IsValid = builder2.Validate();
            Assert.Empty(builder2.Errors);
            Assert.True(sif2IsValid);

            var sif2 = builder2.SIFs.Single();
            Assert.Equal(3, sif2.Subsystems.Count());


            Assert.Equal("SIF-00ABC23", sif2.SIFID.Value);

            var initiator2 = sif2.InputDevice;
            var logicSolver2 = sif2.LogicSolver;
            var finalElement2 = sif2.FinalElement;

            Assert.Equal("1", initiator2.MInVotingMooN.StringValue);
            Assert.Equal("1", initiator2.NumberOfGroups.StringValue);

            Assert.Equal("1", logicSolver2.MInVotingMooN.StringValue);
            Assert.Equal("1", logicSolver2.NumberOfGroups.StringValue);

            Assert.Equal("1", finalElement2.MInVotingMooN.StringValue);
            Assert.Equal("1", finalElement2.NumberOfGroups.StringValue);

            var initiator2Group = initiator2.Groups.Single();
            var logicSolver2Group = logicSolver2.Groups.Single();
            var finalElement2Group = finalElement2.Groups.Single();

            Assert.Equal(2, initiator2Group.Components.Count());
            Assert.Single(logicSolver2Group.Components);
            Assert.Single(finalElement2Group.Components);

            Assert.Equal("2", initiator2Group.MInVotingMooN.StringValue);
            Assert.Equal("2", initiator2Group.NumberOfDevicesWithinGroup.StringValue);

            var initiator2Component1 = initiator2Group.Components.Single(x => x.Name.StringValue == "TT-1001");
            Assert.Equal("3000", initiator2Component1.ProofTestIntervalSILCompliance.StringValue);
            Assert.Equal("25.7", initiator2Component1.ProofTestCoverage.StringValue);

            var initiator2Component2 = initiator2Group.Components.Single(x => x.Name.StringValue == "TT-1002");
            Assert.Equal("6000", initiator2Component2.ProofTestIntervalSILCompliance.StringValue);
            Assert.Equal("15.8", initiator2Component2.ProofTestCoverage.StringValue);


            var logicSolver2Component = logicSolver2Group.Components.Single(x => x.Name.StringValue == "C01");
            Assert.Equal("7000", logicSolver2Component.ProofTestIntervalSILCompliance.StringValue);

            var finalElement2Component = finalElement2Group.Components.Single(x => x.Name.StringValue == "ESV-3023");
            Assert.Equal("2000", finalElement2Component.ProofTestIntervalSILCompliance.StringValue);

            Assert.Equal(2, initiator2Group.MInVotingMooN.Value);
            Assert.Equal(2, initiator2Group.NumberOfDevicesWithinGroup.Value);
            Assert.Equal(3000.0, initiator2Component1.ProofTestIntervalSILCompliance.Value);
            Assert.Equal(25.7, initiator2Component1.ProofTestCoverage.Value);
            Assert.Equal(6000.0, initiator2Component2.ProofTestIntervalSILCompliance.Value);
            Assert.Equal(15.8, initiator2Component2.ProofTestCoverage.Value);
            Assert.Equal(7000.0, logicSolver2Component.ProofTestIntervalSILCompliance.Value);
            Assert.Equal(2000.0, finalElement2Component.ProofTestIntervalSILCompliance.Value);

            Assert.True(sif.IsSameAs(sif2));
        }
    }
}