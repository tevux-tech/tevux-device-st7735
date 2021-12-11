﻿using System.Device.Spi;
using System.Threading;
using Tevux.Device.St7735.Register;

namespace Tevux.Device.St7735;

/// <summary>
/// TODO
/// </summary>
public class PimoroniLcd096 : ST7735 {
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="spiDevice">TODO</param>
    /// <param name="controlPin">TODO</param>
    public PimoroniLcd096(SpiDevice spiDevice, int controlPin)
        : base(spiDevice, controlPin, 80, 160) {
    }

    /// <summary>
    /// TODO
    /// </summary>
    public override void Initialize() {
        SetRegister(Address.SwReset);
        Thread.Sleep(200);

        SetRegister(Address.SleepOut);
        Thread.Sleep(200);

        // Normal mode //Rate = fosc/(1x2+40) * (LINE+2C+2D)
        SetRegister(Address.FrameRateControl1, new byte[] { 0x01, 0x2C, 0x2D });

        // Idle mode //Rate = fosc/(1x2+40) * (LINE+2C+2D)
        SetRegister(Address.FrameRateControl2, new byte[] { 0x01, 0x2C, 0x2D });

        // Partial mode. // Dot inversion mode.// Line inversion mode.
        SetRegister(Address.FrameRateControl3, new byte[] { 0x01, 0x2C, 0x2D, 0x01, 0x2C, 0x2D });

        // <-- No inversion
        SetRegister(Address.InversionControl, 0x07);

        // -4.6V, auto mode.
        SetRegister(Address.PowerControl1, new byte[] { 0xA2, 0x02, 0x84 });

        // Opamp current small, boost frequency.
        SetRegister(Address.PowerControl2, new byte[] { 0x0A, 0x00 });

        // BCLK / 2, Opamp current small & Medium low.
        SetRegister(Address.PowerControl4, new byte[] { 0x8A, 0x2A });

        SetRegister(Address.PowerControl5, new byte[] { 0x8A, 0xEE });

        SetRegister(Address.VcomControl, 0x0E);

        SetRegister(Address.DisplayInversionOn);

        SetRegister(Address.PixelFormat, 0x05); // 65k mode .

        SetRegister(Address.GammaPositiveCorrection, new byte[] { 0x02, 0x1c, 0x07, 0x12, 0x37, 0x32, 0x29, 0x2d, 0x29, 0x25, 0x2e, 0x39, 0x00, 0x01, 0x03, 0x10 });
        SetRegister(Address.GammaNegativeCorrection, new byte[] { 0x03, 0x1d, 0x07, 0x06, 0x2e, 0x2c, 0x29, 0x2d, 0x2e, 0x2e, 0x37, 0x3f, 0x00, 0x00, 0x02, 0x10 });

        SetRegister(Address.NormalModeOn);
        Thread.Sleep(10);

        SetRegister(Address.DisplayOn);
        Thread.Sleep(100);

        SetOrientation(Orientation.Normal);
    }
}