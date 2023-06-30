using System.Collections.Generic;
using System.Numerics;
using SimpleImageIO;

namespace GuidedPathTracerExperiments.ProbabilityTrees {

    public class GuidingProbabilityPathSegment {
        public bool UseForLearning;
        public Vector3 Position; 
        public float BsdfPdf, GuidePdf, MisWeight, SamplePdf;
        public RgbColor ScatteredContribution, ScatteringWeight, DirectContribution, BsdfCosine;
    }

    public class GuidingProbabilityPathSegmentStorage {
        List<GuidingProbabilityPathSegment> segments = new();
        public GuidingProbabilityPathSegment LastSegment { get; set; }

        public GuidingProbabilityPathSegment NextSegment() {
            GuidingProbabilityPathSegment segment = new();
            segments.Add(segment);
            LastSegment = segment;
            return segment;
        }

        public void Clear() {
            LastSegment = null;
            segments.Clear();
        }

        public void EvaluatePath(GuidingProbabilityTree tree) {
            for (int i = segments.Count - 2; i >= 0; i--) {
                var segment = segments[i];

                if(segment.UseForLearning && segment.GuidePdf != 0 && segment.BsdfPdf != 0) {
                    RgbColor throughput = new(1.0f);
                    RgbColor contrib = new(0.0f);

                    for (int j = i+1; j < segments.Count; j++) {
                        var nextSegment = segments[j];

                        contrib += throughput * nextSegment.ScatteredContribution;
                        
                        if(j == i+1) contrib += throughput * nextSegment.DirectContribution;
                        else contrib += throughput * nextSegment.MisWeight * nextSegment.DirectContribution;

                        throughput = throughput * nextSegment.ScatteringWeight;
                    }

                    if (contrib.R > 0.0f || contrib.G > 0.0f || contrib.B > 0.0f) {
                        tree.AddSampleData(
                            segment.Position, 
                            segment.GuidePdf,
                            segment.BsdfPdf, 
                            segment.SamplePdf,
                            contrib * segment.BsdfCosine
                        );
                    }
                }
            }
            Clear();
        }
    }
}