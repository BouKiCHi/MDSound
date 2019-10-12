﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDSound.ZM_1
{
    public class Pcm
    {
        private byte _PCMMode = 0;
        public byte PCMMode
        {
            get { return _PCMMode; }
            set { _PCMMode = value; }
        }

        private uint _PlayAddress = 0;
        public uint PlayAddress
        {
            get { return _PlayAddress; }
            set { _PlayAddress = value; }
        }

        private uint _StopAddress = 0;
        public uint StopAddress
        {
            get { return _StopAddress; }
            set { _StopAddress = value; }
        }

        private uint _LoopAddress = 0;
        public uint LoopAddress
        {
            get { return _LoopAddress; }
            set { _LoopAddress = value; }
        }

        private ushort _KeyOffAddress = 0;
        public ushort KeyOffAddress
        {
            get { return _KeyOffAddress; }
            set { _KeyOffAddress = value; }
        }

        private byte _PCMConfig = 0;
        public byte PCMConfig
        {
            get { return _PCMConfig; }
            set { _PCMConfig = value; }
        }

        private long _LoopFeedBack = 0;
        public long LoopFeedBack
        {
            get { return _LoopFeedBack; }
            set { _LoopFeedBack = value; }
        }

        private byte _LeftVolume = 0;
        public byte LeftVolume
        {
            get { return _LeftVolume; }
            set { _LeftVolume = value; }
        }

        private byte _RightVolume = 0;
        public byte RightVolume
        {
            get { return _RightVolume; }
            set { _RightVolume = value; }
        }

        private byte _LPFFilter = 0;
        public byte LPFFilter
        {
            get { return _LPFFilter; }
            set { _LPFFilter = value; }
        }

        private byte _HPFFilter = 0;
        public byte HPFFilter
        {
            get { return _HPFFilter; }
            set { _HPFFilter = value; }
        }

        private byte _EffectConfiguration = 0;
        public byte EffectConfiguration
        {
            get { return _EffectConfiguration; }
            set { _EffectConfiguration = value; }
        }

        public void Write(byte adress, byte data)
        {
            switch (adress)
            {
                case 0x00:
                    PCMMode = data;
                    break;
                case 0x15:
                    PCMConfig = data;
                    break;
                case 0x1e:
                    LeftVolume = data;
                    break;
                case 0x1f:
                    RightVolume = data;
                    break;
                case 0x20:
                    LPFFilter = data;
                    break;
                case 0x21:
                    HPFFilter = data;
                    break;
                case 0x22:
                    EffectConfiguration = data;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("アドレス指定が異常です");
            }
        }

        public void Write(byte adress, ushort data)
        {
            switch (adress)
            {
                case 0x0d:
                    KeyOffAddress = data;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("アドレス指定が異常です");
            }
        }

        public void Write(byte adress, uint data)
        {
            switch (adress)
            {
                case 0x01:
                    PlayAddress = data;
                    break;
                case 0x05:
                    StopAddress = data;
                    break;
                case 0x09:
                    LoopAddress = data;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("アドレス指定が異常です");
            }
        }

        public void Write(byte adress, long data)
        {
            switch (adress)
            {
                case 0x16:
                    LoopFeedBack = data;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("アドレス指定が異常です");
            }
        }
    }

}
