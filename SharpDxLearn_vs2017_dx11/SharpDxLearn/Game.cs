using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Windows;
using System.Drawing;
using SharpDX.DXGI;
using D3D11 = SharpDX.Direct3D11;

public class Game : IDisposable
{
    RenderForm m_renderForm;

    int m_width = 1280;
    int m_height = 720;

    D3D11.Device m_d3d11Device;
    D3D11.DeviceContext m_d3d11DeviceContext;
    SwapChain m_swapChain;
    D3D11.RenderTargetView m_renderTargetView;

    public Game()
    {
        m_renderForm = new RenderForm("SharpDxLearn");
        m_renderForm.ClientSize = new Size(m_width, m_height);
        m_renderForm.AllowUserResizing = false;

        InitializeDeviceResources();
    }

    void InitializeDeviceResources()
    {
        ModeDescription backBufferDesc = new ModeDescription(m_width, m_height, new Rational(60, 1), Format.R8G8B8A8_UNorm);
        SwapChainDescription swapChainDesc = new SwapChainDescription()
        {
            ModeDescription = backBufferDesc,
            SampleDescription = new SampleDescription(1, 0),
            Usage = Usage.RenderTargetOutput,
            BufferCount = 1,
            OutputHandle = m_renderForm.Handle,
            IsWindowed = true,
        };
        D3D11.Device.CreateWithSwapChain(SharpDX.Direct3D.DriverType.Hardware, D3D11.DeviceCreationFlags.None, swapChainDesc,
            out m_d3d11Device, out m_swapChain);
        m_d3d11DeviceContext = m_d3d11Device.ImmediateContext;

        using (D3D11.Texture2D backBufer = m_swapChain.GetBackBuffer<D3D11.Texture2D>(0))
        {
            m_renderTargetView = new D3D11.RenderTargetView(m_d3d11Device, backBufer);
        }
    }

    void Draw()
    {
        m_d3d11DeviceContext.OutputMerger.SetRenderTargets(m_renderTargetView);
        SharpDX.Mathematics.Interop.RawColor4 color = new SharpDX.Mathematics.Interop.RawColor4(32 / (float)255, 103 / (float)255, 178 / (float)255, 1);
        m_d3d11DeviceContext.ClearRenderTargetView(m_renderTargetView, color);
        m_swapChain.Present(1, PresentFlags.None);
    }

    public void Run()
    {
        RenderLoop.Run(m_renderForm, RenderCallback);
    }

    void RenderCallback()
    {
        Draw();
    }

    public void Dispose()
    {
        m_renderForm.Dispose();
        m_renderTargetView.Dispose();
        m_swapChain.Dispose();
        m_d3d11Device.Dispose();
        m_d3d11DeviceContext.Dispose();
    }
}