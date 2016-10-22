﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDSound
{
    public class ym2610 : Instrument
    {
        private fmgen.OPNB[] chip = new fmgen.OPNB[2];
        private const uint DefaultYM2610ClockValue = 8000000;

        public override void Reset(byte ChipID)
        {
            if (chip[ChipID] == null) return;
            chip[ChipID].Reset();
        }

        public override uint Start(byte ChipID, uint clock)
        {
            chip[ChipID] = new fmgen.OPNB();
            chip[ChipID].Init(DefaultYM2610ClockValue, clock);

            return clock;
        }

        public uint Start(byte ChipID, uint clock, uint FMClockValue, params object[] option)
        {
            chip[ChipID] = new fmgen.OPNB();
            chip[ChipID].Init(FMClockValue, clock,false, new byte[0x20ffff], 0x20ffff, new byte[0xffff], 0xffff);

            return clock;
        }

        public override void Stop(byte ChipID)
        {
            chip[ChipID] = null;
        }

        public override void Update(byte ChipID, int[][] outputs, int samples)
        {
            if (chip[ChipID] == null) return;
            int[] buffer = new int[2];
            buffer[0] = 0;
            buffer[1] = 0;
            chip[ChipID].Mix(buffer, 1);
            for (int i = 0; i < 1; i++)
            {
                outputs[0][i] = buffer[i * 2 + 0];
                outputs[1][i] = buffer[i * 2 + 1];
                //Console.Write("[{0:d8}] : [{1:d8}] [{2}]\r\n", outputs[0][i], outputs[1][i],i);
            }
        }

        public int YM2610_Write(byte ChipID, uint adr, byte data)
        {
            if (chip[ChipID] == null) return 0;
            chip[ChipID].SetReg(adr, data);
            return 0;
        }

        public void YM2610_setAdpcmA(byte ChipID, byte[] _adpcma, int _adpcma_size)
        {
            if (chip[ChipID] == null) return;
            chip[ChipID].setAdpcmA(_adpcma, _adpcma_size);
        }

        public void YM2610_setAdpcmB(byte ChipID, byte[] _adpcmb, int _adpcmb_size)
        {
            if (chip[ChipID] == null) return;
            chip[ChipID].setAdpcmB(_adpcmb, _adpcmb_size);
        }


    }
}
