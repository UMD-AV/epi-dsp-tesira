﻿using PepperDash.Essentials.Core;

namespace Tesira_DSP_EPI.Bridge.JoinMaps {
    /// <summary>
    /// Meter Joinmap for Advanced Bridge - Meant for holistic DSP Object
    /// </summary>
    public class TesiraSwitcherJoinMapAdvanced : JoinMapBaseAdvanced
    {
        [JoinName("Index")] public JoinDataComplete Index =
            new JoinDataComplete(new JoinData {JoinNumber = 150, JoinSpan = 1},
                new JoinMetadata
                {
                    Description = "Source Selector Index Set and Feedback",
                    JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                    JoinType = eJoinType.Analog
                });

        [JoinName("Label")] public JoinDataComplete Label =
            new JoinDataComplete(new JoinData {JoinNumber = 150, JoinSpan = 1},
                new JoinMetadata
                {
                    Description = "PSource Selector Label",
                    JoinCapabilities = eJoinCapabilities.ToSIMPL,
                    JoinType = eJoinType.Serial
                });

        public TesiraSwitcherJoinMapAdvanced(uint joinStart)
            : base(joinStart, typeof (TesiraSwitcherJoinMapAdvanced))
        {
        }

    }

    /// <summary>
    /// Meter Joinmap for Advanced Bridge - Meant for bridging the Meter as a standalone device
    /// </summary>
    public class TesiraSwitcherJoinMapAdvancedStandalone : JoinMapBaseAdvanced
    {
        [JoinName("Index")] public JoinDataComplete Index =
            new JoinDataComplete(new JoinData {JoinNumber = 1, JoinSpan = 1},
                new JoinMetadata
                {
                    Description = "Source Selector Index Set and Feedback",
                    JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                    JoinType = eJoinType.Analog
                });

        [JoinName("Label")] public JoinDataComplete Label =
            new JoinDataComplete(new JoinData {JoinNumber = 1, JoinSpan = 1},
                new JoinMetadata
                {
                    Description = "Source Selector Label",
                    JoinCapabilities = eJoinCapabilities.ToSIMPL,
                    JoinType = eJoinType.Serial
                });
        [JoinName("SourcesLabelXSig")]
        public JoinDataComplete SourcesLabelXSig =
            new JoinDataComplete(new JoinData { JoinNumber = 2, JoinSpan = 1 },
            new JoinMetadata
            {
                Description = "SourcesLabelXSig",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Serial
            });


        public TesiraSwitcherJoinMapAdvancedStandalone(uint joinStart)
            : base(joinStart, typeof (TesiraSwitcherJoinMapAdvancedStandalone))
        {
        }

    }

}