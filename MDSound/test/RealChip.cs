﻿using NScci;
using Nc86ctl;
using System;
using System.Threading;

namespace test
{
    public class RealChip : IDisposable
    {
        private NScci.NScci nScci;
        private Nc86ctl.Nc86ctl nc86ctl;

        #region IDisposable Support

        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Close();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        public RealChip() 
        {
            log.ForcedWrite("RealChip:Ctr:STEP 00(Start)");

            int n = 0;
            try
            {
                nScci = new NScci.NScci();
                n = nScci.NSoundInterfaceManager_.getInterfaceCount();
                if (n == 0)
                {
                    nScci.Dispose();
                    nScci = null;
                    log.ForcedWrite("RealChip:Ctr:Not found SCCI.");
                }
                else
                {
                    log.ForcedWrite(string.Format("RealChip:Ctr:Found SCCI.(Interface count={0})", n));
                    getScciInstances();
                    nScci.NSoundInterfaceManager_.setLevelDisp(false);
                }
            }
            catch
            {
                nScci = null;
            }

            log.ForcedWrite("RealChip:Ctr:STEP 01");
            try
            {
                nc86ctl = new Nc86ctl.Nc86ctl();
                nc86ctl.initialize();
                n = nc86ctl.getNumberOfChip();
                if (n == 0)
                {
                    nc86ctl.deinitialize();
                    nc86ctl = null;
                    log.ForcedWrite("RealChip:Ctr:Not found G.I.M.I.C.");
                }
                else
                {
                    log.ForcedWrite(string.Format("RealChip:Ctr:Found G.I.M.I.C.(Interface count={0})", n));
                    Nc86ctl.NIRealChip nirc = nc86ctl.getChipInterface(0);
                    nirc.reset();
                }
            }
            catch
            {
                nc86ctl = null;
            }
            log.ForcedWrite("RealChip:Ctr:STEP 02(Success)");
        }

        public void Close()
        {
            if (nScci != null)
            {
                try
                {
                    nScci.Dispose();
                }
                catch { }
                nScci = null;
            }
            if (nc86ctl != null)
            {
                try
                {
                    nc86ctl.deinitialize();
                }
                catch { }
                nc86ctl = null;
            }
        }


        public void getScciInstances()
        {
            int ifc = nScci.NSoundInterfaceManager_.getInterfaceCount();

            for (int i = 0; i < ifc; i++)
            {
                NSoundInterface sif = nScci.NSoundInterfaceManager_.getInterface(i);

                int scc = sif.getSoundChipCount();
                for (int j = 0; j < scc; j++)
                {
                    NSoundChip sc = sif.getSoundChip(j);
                    NSCCI_SOUND_CHIP_INFO info = sc.getSoundChipInfo();
                }
            }

        }

        public void setLevelDisp(bool v)
        {
            if (nScci != null) nScci.NSoundInterfaceManager_.setLevelDisp(v);
        }

        //public void Init()
        //{
        //    if (nScci != null)
        //    {
        //        nScci.NSoundInterfaceManager_.init();
        //    }
        //    if (nc86ctl != null)
        //    {
        //        nc86ctl.initialize();
        //    }
        //}

        public void reset()
        {
            if (nScci != null) nScci.NSoundInterfaceManager_.reset();
            if (nc86ctl != null)
            {
                //nc86ctl.initialize();
                int n=nc86ctl.getNumberOfChip();
                for(int i = 0; i < n; i++)
                {
                    NIRealChip rc= nc86ctl.getChipInterface(i);
                    rc.reset();
                }
            }
        }

        public void SendData()
        {
            if (nScci != null) nScci.NSoundInterfaceManager_.sendData();
            if (nc86ctl != null)
            {
                //int n = nc86ctl.getNumberOfChip();
                //for (int i = 0; i < n; i++)
                //{
                //    NIRealChip rc = nc86ctl.getChipInterface(i);
                //    if (rc != null)
                //    {
                //        while ((rc.@in(0x0) & 0x00) != 0)
                //            System.Threading.Thread.Sleep(0);
                //    }
                //}
            }

            Thread.Sleep(1000);
        }

        public void WaitOPNADPCMData(bool isGIMIC)
        {
            if (nScci != null) nScci.NSoundInterfaceManager_.sendData();
            if (nc86ctl != null && isGIMIC)
            {
                //int n = nc86ctl.getNumberOfChip();
                //for (int i = 0; i < n; i++)
                //{
                //    NIRealChip rc = nc86ctl.getChipInterface(i);
                //    if (rc != null)
                //    {
                //        int timeOutCounter;
                //        timeOutCounter = 10000;
                //        while ((rc.@in(0x0) & 0x83) != 0 && timeOutCounter > 0)
                //        {
                //            Thread.Sleep(1);
                //            timeOutCounter--;
                //            //Debug.WriteLine("{0:x}", rc.@in(0x0));
                //        }
                //        timeOutCounter = 10000;
                //        while ((rc.@in(0x100) & 0xbf) != 0 && timeOutCounter > 0)
                //        {
                //            Thread.Sleep(1);
                //            timeOutCounter--;
                //        }
                //    }
                //}
            }

            Thread.Sleep(1000);
        }

        public RSoundChip SearchOPNA()
        {
            if (nScci != null)
            {
                int iCount = nScci.NSoundInterfaceManager_.getInterfaceCount();
                for (int i = 0; i < iCount; i++)
                {
                    NSoundInterface iIntfc = nScci.NSoundInterfaceManager_.getInterface(i);
                    NSCCI_INTERFACE_INFO iInfo = nScci.NSoundInterfaceManager_.getInterfaceInfo(i);
                    int sCount = iIntfc.getSoundChipCount();
                    for (int s = 0; s < sCount; s++)
                    {
                        NSoundChip sc = iIntfc.getSoundChip(s);
                        int t = sc.getSoundChipType();
                        if (t != 1)
                        {
                            continue;
                        }

                        string ChipName = sc.getSoundChipInfo().cSoundChipName;
                        string InterfaceName = iInfo.cInterfaceName;
                        RScciSoundChip rsc = new RScciSoundChip(0, i, s);
                        rsc.scci = nScci;

                        return rsc;
                    }
                }
            }

            if (nc86ctl != null)
            {
                int iCount = nc86ctl.getNumberOfChip();
                for (int i = 0; i < iCount; i++)
                {
                    NIRealChip rc = nc86ctl.getChipInterface(i);
                    NIGimic2 gm = rc.QueryInterface();
                    ChipType cct = gm.getModuleType();

                    if (cct != ChipType.CHIP_YM2608 && cct != ChipType.CHIP_YMF288)
                    {
                        continue;
                    }

                    int o = -1;
                    string seri = gm.getModuleInfo().Serial;
                    if (!int.TryParse(seri, out o)) o = -1;
                    string ChipName = gm.getModuleInfo().Devname;
                    string InterfaceName = gm.getMBInfo().Devname;

                    RC86ctlSoundChip rsc = new RC86ctlSoundChip(-1, i, o);
                    rsc.c86ctl = nc86ctl;
                    return rsc;
                }
            }

            return null;

        }

    }

    public class RSoundChip
    {
        protected int SoundLocation;
        protected int BusID;
        protected int SoundChip;

        public uint dClock = 3579545;

        public RSoundChip(int soundLocation,int busID,int soundChip)
        {
            SoundLocation = soundLocation;
            BusID = busID;
            SoundChip = soundChip;
        }

        virtual public void init()
        {
            throw new NotImplementedException();
        }

        virtual public void setRegister(int adr, int dat)
        {
            throw new NotImplementedException();
        }

        virtual public int getRegister(int adr)
        {
            throw new NotImplementedException();
        }

        virtual public bool isBufferEmpty()
        {
            throw new NotImplementedException();
        }

        virtual public uint SetMasterClock(uint mClock)
        {
            throw new NotImplementedException();
        }

        virtual public void setSSGVolume(byte vol)
        {
            throw new NotImplementedException();
        }

    }

    public class RScciSoundChip : RSoundChip
    {
        public NScci.NScci scci = null;
        private NSoundChip realChip = null;

        public RScciSoundChip(int soundLocation, int busID, int soundChip) : base(soundLocation, busID, soundChip)
        {
        }

        override public void init()
        {
            NSoundInterface nsif = scci.NSoundInterfaceManager_.getInterface(BusID);
            NSoundChip nsc = nsif.getSoundChip(SoundChip);
            realChip = nsc;
            dClock = (uint)nsc.getSoundChipClock();

            //chipの種類ごとに初期化コマンドを送りたい場合
            switch (nsc.getSoundChipType())
            {
                case (int)EnmRealChipType.YM2608:
                    //setRegister(0x2d, 00);
                    //setRegister(0x29, 82);
                    //setRegister(0x07, 38);
                    break;
            }
        }

        override public void setRegister(int adr, int dat)
        {
            realChip.setRegister(adr, dat);
        }

        override public int getRegister(int adr)
        {
            return realChip.getRegister(adr);
        }

        override public bool isBufferEmpty()
        {
            return realChip.isBufferEmpty();
        }

        /// <summary>
        /// マスタークロックの設定
        /// </summary>
        /// <param name="mClock">設定したい値</param>
        /// <returns>実際設定された値</returns>
        override public uint SetMasterClock(uint mClock)
        {
            //SCCIはクロックの変更不可

            return (uint)realChip.getSoundChipClock();
        }

        override public void setSSGVolume(byte vol)
        {
            //SCCIはSSG音量の変更不可
        }

    }

    public class RC86ctlSoundChip : RSoundChip
    {
        public Nc86ctl.Nc86ctl c86ctl = null;
        public Nc86ctl.NIRealChip realChip = null;
        public Nc86ctl.ChipType chiptype = ChipType.CHIP_UNKNOWN;

        public RC86ctlSoundChip(int soundLocation, int busID, int soundChip) : base(soundLocation, busID, soundChip)
        {
        }

        override public void init()
        {
            NIRealChip rc = c86ctl.getChipInterface(BusID);
            rc.reset();
            realChip = rc;
            NIGimic2 gm = rc.QueryInterface();
            dClock = gm.getPLLClock();
            chiptype = gm.getModuleType();
            if (chiptype == ChipType.CHIP_YM2608)
            {
                //setRegister(0x2d, 00);
                //setRegister(0x29, 82);
                //setRegister(0x07, 38);
            }
        }

        override public void setRegister(int adr, int dat)
        {
            realChip.@out((ushort)adr, (byte)dat);
        }

        override public int getRegister(int adr)
        {
            return realChip.@in((ushort)adr);
        }

        override public bool isBufferEmpty()
        {
            return true;
        }

        /// <summary>
        /// マスタークロックの設定
        /// </summary>
        /// <param name="mClock">設定したい値</param>
        /// <returns>実際設定された値</returns>
        override public uint SetMasterClock(uint mClock)
        {
            NIGimic2 gm = realChip.QueryInterface();
            uint nowClock= gm.getPLLClock();
            if (nowClock != mClock)
            {
                gm.setPLLClock(mClock);
            }

            return gm.getPLLClock();
        }

        override public void setSSGVolume(byte vol)
        {
            NIGimic2 gm = realChip.QueryInterface();
            gm.setSSGVolume(vol);
        }

    }


}
