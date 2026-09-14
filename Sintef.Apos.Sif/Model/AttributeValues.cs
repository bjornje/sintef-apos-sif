namespace Sintef.Apos.Sif.Model
{
    public static class AttributeValues
    {
        public const string Version = "34";
    }

    public static class v_comparison
    {
        public const string greater_than = "greater than";
        public const string less_than_or_equal = "less than or equal";
        public const string less_than = "less than";
        public const string greater_than_or_equal = "greater than or equal";
        public const string equal = "equal";
    }

    public static class v_detectedOrUndetected
    {
        public const string undetected = "undetected";
        public const string detected = "detected";
    }

    public static class v_failPass
    {
        public const string passed = "passed";
        public const string failed = "failed";
    }

    public static class v_failureTypes
    {
        public const string dangerous_detected_DD = "dangerous detected (DD)";
        public const string dangerous_undetected_DU = "dangerous undetected (DU)";
        public const string spurious = "spurious";
        public const string safe_undetected_SU = "safe undetected (SU)";
        public const string safe_detected_SD = "safe detected (SD)";
        public const string non_critical_NONC = "non-critical (NONC)";
        public const string not_applicable_NA = "not applicable (NA)";
        public const string safe = "safe";
        public const string dangerous = "dangerous";
        public const string degraded = "degraded";
    }

    public static class v_independentOrCCF
    {
        public const string independent_failure = "independent failure";
        public const string common_cause_failure = "common cause failure";
    }

    public static class v_systematicOrRHF
    {
        public const string random_hardware_failure = "random hardware failure";
        public const string systematic_failure = "systematic failure";
    }

    public static class v_assetIntegrityLevel
    {
        public const string AIL_1 = "AIL 1";
        public const string AIL_2 = "AIL 2";
        public const string AIL_3 = "AIL 3";
        public const string AIL_4 = "AIL 4";
        public const string no_AIL = "no AIL";
    }

    public static class v_environmentalIntegrityLevel
    {
        public const string EIL_1 = "EIL 1";
        public const string EIL_2 = "EIL 2";
        public const string EIL_3 = "EIL 3";
        public const string EIL_4 = "EIL 4";
        public const string no_EIL = "no EIL";
    }

    public static class v_modeOfOperation
    {
        public const string low_demand = "low demand";
        public const string high_demand = "high demand";
        public const string continuous = "continuous";
    }

    public static class v_SIFType
    {
        public const string global = "global";
        public const string local = "local";
    }

    public static class v_SILLevel
    {
        public const string SIL_1 = "SIL 1";
        public const string SIL_2 = "SIL 2";
        public const string SIL_3 = "SIL 3";
        public const string SIL_4 = "SIL 4";
        public const string SIL_0 = "SIL 0";
    }

    public static class v_subsystem
    {
        public const string input_device_subsystem = "input device subsystem";
        public const string logic_solver_subsystem = "logic solver subsystem";
        public const string final_element_subsystem = "final element subsystem";
        public const string composite_subsystem = "composite subsystem";
    }

    public static class v_alarmType
    {
        public const string high_high = "high high";
        public const string high = "high";
        public const string low_low_low = "low low low";
        public const string low_low = "low low";
        public const string high_high_high = "high high high";
        public const string low = "low";
        public const string other = "other";
    }

    public static class v_application
    {
        public const string emergency_shutdown_ESD = "emergency shutdown (ESD)";
        public const string process_shutdown_PSD = "process shutdown (PSD)";
        public const string fire_and_gas_FandG = "fire and gas (FandG)";
        public const string heating_ventilation_and_air_conditioning_HVAC = "heating, ventilation, and air conditioning (HVAC)";
        public const string emergency_depressurisation_EDP___blowdown = "emergency depressurisation (EDP) - blowdown";
        public const string process_control_system_PCS = "process control system (PCS)";
        public const string high_integrity_pressure_protection_system_HIPPS = "high integrity pressure protection system (HIPPS)";
        public const string pressure_protection_system_PPS = "pressure protection system (PPS)";
        public const string equipment_protection = "equipment protection";
        public const string offloading = "offloading";
        public const string monitoring = "monitoring";
        public const string combined_ESD_and_PSD = "combined ESD and PSD";
        public const string combined_ESD_PSD_and_PCS = "combined ESD, PSD and PCS";
        public const string combined_PSD_and_PCS = "combined PSD and PCS";
        public const string combined_FandG_and_HVAC = "combined FandG and HVAC";
    }

    public static class v_deviceType_ValveTopside
    {
        public const string BallastValve = "BallastValve";
        public const string BlowdownValve = "BlowdownValve";
        public const string DelugeValve = "DelugeValve";
        public const string FastOpeningValve_inFlare = "FastOpeningValve_inFlare";
        public const string FireWaterMonitorValve = "FireWaterMonitorValve";
        public const string FoamValve = "FoamValve";
        public const string GaseousAgentValve = "GaseousAgentValve";
        public const string HPU_BleedOffValve = "HPU_BleedOffValve";
        public const string OffloadingIsolationValve = "OffloadingIsolationValve";
        public const string PipelineIsolationValve = "PipelineIsolationValve";
        public const string PressureReliefValve = "PressureReliefValve";
        public const string ProcessIsolationValve = "ProcessIsolationValve";
        public const string RiserIsolationValve = "RiserIsolationValve";
        public const string WaterMistValve = "WaterMistValve";
        public const string XmasTree_ChemicalInjectionValve_Topside = "XmasTree_ChemicalInjectionValve_Topside";
        public const string XmasTreeMasterValve_Topside = "XmasTreeMasterValve_Topside";
        public const string XmasTree_WingValve_Topside = "XmasTree_WingValve_Topside";
    }

    public static class v_energizeSource
    {
        public const string electrical = "electrical";
        public const string pneumatic = "pneumatic";
        public const string hydraulic = "hydraulic";
    }

    public static class v_externalExposure
    {
        public const string low = "low";
        public const string logic_solver__outdoor_cabinet = "logic solver: outdoor cabinet";
        public const string moderate = "moderate";
        public const string severe = "severe";
        public const string logic_solver__indoor_cabinet = "logic solver: indoor cabinet";
    }

    public static class v_failurePhilosophy
    {
        public const string fail_safe = "fail-safe";
        public const string fail_as_is = "fail as-is";
    }

    public static class v_fluidSeverity
    {
        public const string medium_moderate_service = "medium/moderate service";
        public const string dirty_severe_service = "dirty/severe service";
        public const string clean_benign_service = "clean/benign service";
    }

    public static class v_resetAfterShutdown_FinalElement
    {
        public const string reset_in_logic_solver = "reset in logic solver";
        public const string reset_in_field = "reset in field";
    }

    public static class v_SCLevel
    {
        public const string SC_1 = "SC 1";
        public const string SC_2 = "SC 2";
        public const string SC_3 = "SC 3";
        public const string SC_4 = "SC 4";
    }

    public static class v_standbyRedundancyType
    {
        public const string hot = "hot";
        public const string cold = "cold";
    }

    public static class v_tripEnergyMode
    {
        public const string de_energize_to_trip = "de-energize to trip";
        public const string energize_to_trip = "energize to trip";
    }

    public static class v_typeAB
    {
        public const string type_A = "type A";
        public const string type_B = "type B";
    }

    public static class v_boolean
    {
        public const string true_ = "true";
        public const string false_ = "false";
    }

}
