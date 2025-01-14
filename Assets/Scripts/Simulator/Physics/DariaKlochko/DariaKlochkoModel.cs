﻿using UnityEngine;

namespace AircraftSimulator.Physics.DariaKlochko {
    public class DariaKlochkoModel : PhysicsModel {
        private DariaKlochkoModelData _data;
        public DariaKlochkoModel(Aircraft aircraft, Vector3 initialVelocity, DariaKlochkoModelData data) : base(
            aircraft, initialVelocity) {
            _data = data;
        }

         protected override void PerformStep(ControlData control, float deltaTime) {
            var rRate = control.AileronAngle - _data.DeadZone;
            var controlR = Mathf.Abs(control.AileronAngle) > _data.DeadZone
                ? Mathf.Abs(rRate) * Mathf.Sign(rRate) * _data.ControlRate
                : 0f;
            var pRate = control.ElevatorAngle - _data.DeadZone;
            var controlP = Mathf.Abs(control.ElevatorAngle) > _data.DeadZone
                ? Mathf.Abs(pRate) * Mathf.Sign(pRate) * _data.ControlRate
                : 0f;
            var yRate = control.RudderAngle - _data.DeadZone;
            var controlY = Mathf.Abs(control.RudderAngle) > _data.DeadZone
                ? Mathf.Abs(yRate) * Mathf.Sign(yRate) * _data.ControlRate
                : 0f;

            float P = Mathf.Lerp(PreviousState.RollRate, controlR, _data.Lerp);
            float Q = Mathf.Lerp(PreviousState.PitchRate, controlP, _data.Lerp);
            float R = Mathf.Lerp(PreviousState.YawRate, controlY, _data.Lerp);

            float U = PreviousState.U;
            float V = PreviousState.V;
            float W = PreviousState.W;
            float m = (float)Aircraft.Mass;

            // engine control
            float totalPower = 0;
            foreach (var component in Aircraft.Components)
            {
                if (component is Engine engine)
                {
                    engine.CurrentPower = engine.MaxPower * control.Power;
                    totalPower += (float)engine.CurrentPower;
                }
            }

            var isForsage = _data.Forsage.IsActive;
            if (isForsage) totalPower *= 5;
            // evaluate current state
            // this is not physics!!!
            CurrentState.U = 0;
            CurrentState.V = totalPower;
            CurrentState.W += deltaTime * Simulator.GravityConstant * 0.1f;
            CurrentState.RollRate = P;
            CurrentState.PitchRate = Q;
            CurrentState.YawRate = R;
        }
    }
}