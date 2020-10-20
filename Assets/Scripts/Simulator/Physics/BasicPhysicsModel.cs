﻿using UnityEngine;

namespace AircraftSimulator.Physics {
    public class BasicPhysicsModel : PhysicsModel {
        public BasicPhysicsModel(Aircraft aircraft, Vector3 initialVelocity) : base(aircraft, initialVelocity) { }

        protected override void Evaluate(ControlData control, float deltaTime) {
            float influence = 10f;
            float P = Mathf.Lerp(PreviousState.RollRate, control.AileronAngle * influence, 0.9f);
            float Q = Mathf.Lerp(PreviousState.PitchRate, control.ElevatorAngle * influence, 0.9f);
            float R = Mathf.Lerp(PreviousState.YawRate, control.RudderAngle * influence, 0.9f);
            float U = PreviousState.U;
            float V = PreviousState.V;
            float W = PreviousState.W;
            float m = (float)Aircraft.Mass;

            float FxG = 0, FxA = 0, FxT = 0, FyA = 0, FyG = 0, FyT = 0, FzG = 0, FzA = 0, FzT = 0;
            
            //FzG = m * Simulator.GravityConstant; // gravity force
            //FyT = 100;
            //FzA = 90;
            
            CurrentState.U = U + deltaTime * ( R * V - Q * W - FxG / m + FxA / m + FxT / m);
            CurrentState.V = V + deltaTime * (-R * U + P * W + FyG / m + FyA / m + FyT / m);
            CurrentState.W = W + deltaTime * ( Q * U - P * V + FzG / m + FzA / m + FzT / m);

            CurrentState.RollRate = P;
            CurrentState.PitchRate = Q;
            CurrentState.YawRate = R;
        }
    }
}