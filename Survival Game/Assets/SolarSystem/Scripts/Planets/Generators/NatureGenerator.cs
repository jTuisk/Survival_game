using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.SolarSystem.Planet.Settings;

namespace Game.SolarSystem.Planet.Generators
{
    public class NatureGenerator
    {
        ForestGenerator forestGenerator;

        public NatureGenerator()
        {
            forestGenerator = new ForestGenerator();
        }

        public void UpdateSettings(NatureSettings natureSettings = null, int seed = 0, ShapeGenerator shapeGenerator = null)
        {
            forestGenerator.UpdateSettings(natureSettings, seed, shapeGenerator);
        }

        public void GenerateNature()
        {
            forestGenerator.GenerateForest();
        }
    }
}