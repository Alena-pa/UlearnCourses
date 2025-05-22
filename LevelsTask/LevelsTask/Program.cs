using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection.Emit;

namespace func_rocket
{
    public class LevelsTask
    {
        static readonly Physics standardPhysics = new();

        public static IEnumerable<Level> CreateLevels()
        {
            var start = new Vector(200, 500);
            var target = new Vector(600, 200);
            var rocket = new Rocket(start, Vector.Zero, -0.5 * Math.PI);
            var anomaly = (start + target) / 2;

            yield return CreateZero(rocket, target);
            yield return CreateHeavy(rocket, target);
            yield return CreateUp(rocket);
            yield return CreateWhiteHole(rocket, target);
            yield return CreateBlackHole(rocket, target, anomaly);
            yield return CreateBlackAndWhite(rocket, target, anomaly);
        }

        static Level CreateZero(Rocket rocket, Vector target) =>
        new("Zero", rocket, target, (size, v) => Vector.Zero, standardPhysics);

        static Level CreateHeavy(Rocket rocket, Vector target) =>
        new("Heavy", rocket, target, (size, v) => new Vector(0, 0.9), standardPhysics);

        static Level CreateUp(Rocket rocket) =>
            new("Up", rocket, new Vector(700, 500),
        (size, v) => new Vector(0, -300 / (size.Y - v.Y + 300.0)), standardPhysics);

        static Level CreateWhiteHole(Rocket rocket, Vector target) =>
            new("WhiteHole", rocket, target,
                (size, v) =>
                {
                    var d = (target - v).Length;
                    return (target - v).Normalize() * (-140 * d) / (d * d + 1);
                }, standardPhysics);
        static Level CreateBlackHole(Rocket rocket, Vector target, Vector anomaly) =>
            new("BlackHole", rocket, target,
                (size, v) =>
                {
                    var d = (anomaly - v).Length;
                    return (anomaly - v).Normalize() * (300 * d) / (d * d + 1);
                }, standardPhysics);

        static Level CreateBlackAndWhite(Rocket rocket, Vector target, Vector anomaly)
        {
            Gravity whiteHoleGravity = (size, v) =>
            {
                var d = (target - v).Length;
                return (target - v).Normalize() * (-140 * d) / (d * d + 1);
            };

            Gravity blackHoleGravity = (size, v) =>
            {
                var d = (anomaly - v).Length;
                return (anomaly - v).Normalize() * (300 * d) / (d * d + 1);
            };

            return new Level("BlackAndWhite", rocket, target,
                (size, v) => (whiteHoleGravity(size, v) + blackHoleGravity(size, v)) / 2,
                standardPhysics);
        }
    }
}