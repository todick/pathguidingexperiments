using System.Collections.Generic;
using GuidedPathTracerExperiments.Integrators;
using SeeSharp.Integrators;

namespace GuidedPathTracerExperiments;

public class Experiment : SeeSharp.Experiments.Experiment {
    int numSamples = 16;
    int maxTime = int.MaxValue;
    IntegratorSettings settings = new() {
        IncludeDebugVisualizations = true,
        LearnInterval = 1,
        TreeSplitMargin = 400,
    };


    public Experiment(int numSamples, int maxTime = int.MaxValue) {
        this.numSamples = numSamples;
        this.maxTime = maxTime;
    }

    public override List<Method> MakeMethods() => new() {
        //new("PathTracer", new PathTracer() {
        //    TotalSpp = numSamples,
        //    MaximumRenderTimeMs = maxTime,
        //    NumShadowRays = 1,
        //}),
        //new("GuidedPathTracer", new GuidedPathTracer() {
        //    TotalSpp = numSamples,
        //    MaximumRenderTimeMs = maxTime,
        //    NumShadowRays = 1,
        //}),
        new("RootAdaptiveGuidedPathTracer", new RootAdaptiveGuidedPathTracer() {
            TotalSpp = numSamples,
            MaximumRenderTimeMs = maxTime,
            NumShadowRays = 1,
            Settings = settings,
        }),
        new("KullbackLeiblerGuidedPathTracer", new KullbackLeiblerGuidedPathTracer() {
            TotalSpp = numSamples,
            MaximumRenderTimeMs = maxTime,
            NumShadowRays = 1,
            Settings = settings,
        }),
        new("SecondMomentGuidedPathTracer", new SecondMomentGuidedPathTracer() {
            TotalSpp = numSamples,
            MaximumRenderTimeMs = maxTime,
            NumShadowRays = 1,
            Settings = settings,
        })
    };
}