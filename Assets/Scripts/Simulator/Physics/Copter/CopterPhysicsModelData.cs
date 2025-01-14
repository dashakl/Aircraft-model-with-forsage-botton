﻿namespace AircraftSimulator.Physics.Basic
{
    public struct CopterPhysicsModelData
    {
        public float DeadZone;
        public float ControlRate;
        public float Lerp;

        public float MaxTurn;
        public float AileronTurnRate;
        public float ElevatorTurnRate;
        public float RudderTurnRate;
    }
}