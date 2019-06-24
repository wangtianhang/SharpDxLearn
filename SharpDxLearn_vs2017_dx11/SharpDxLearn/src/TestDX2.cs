using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
//using SharpDX.Windows;
//using System.Drawing;
//using SharpDX.DXGI;
using D3D11 = SharpDX.Direct3D11;
//using SharpDX;
using D3DCompiler = SharpDX.D3DCompiler;
using DXGI = SharpDX.DXGI;


public class TestDX2 : IDisposable
{
    SharpDX.Windows.RenderForm m_renderForm;

    int m_width = 1280;
    int m_height = 720;

    D3D11.Device m_d3d11Device;
    D3D11.DeviceContext m_d3d11DeviceContext;
    DXGI.SwapChain m_swapChain;
    D3D11.RenderTargetView m_renderTargetView;

    //SharpDX.Vector3[] m_vertices = new SharpDX.Vector3[] { new SharpDX.Vector3(-0.5f, 0.5f, 0.0f), new SharpDX.Vector3(0.5f, 0.5f, 0.0f), new SharpDX.Vector3(0.0f, -0.5f, 0.0f) };
    VertexPositionColor[] m_vertices = new VertexPositionColor[]
    {
        new VertexPositionColor(new SharpDX.Vector3(-0.5f, 0.5f, 0.0f), SharpDX.Color.Red),
        new VertexPositionColor(new SharpDX.Vector3(0.5f, 0.5f, 0.0f), SharpDX.Color.Green),
        new VertexPositionColor(new SharpDX.Vector3(0.0f, -0.5f, 0.0f), SharpDX.Color.Blue)
    };
    D3D11.Buffer m_triangleVertexBuffer;
    D3D11.VertexShader m_vertexShader;
    D3D11.PixelShader m_pixelShader;
    D3D11.InputElement[] m_inputElments = new D3D11.InputElement[]
    {
        new D3D11.InputElement("POSITION", 0, DXGI.Format.R32G32B32_Float, 0, 0, D3D11.InputClassification.PerVertexData, 0),
        new D3D11.InputElement("COLOR", 0, DXGI.Format.R32G32B32A32_Float, 12, 0, D3D11.InputClassification.PerVertexData, 0)
    };
    D3DCompiler.ShaderSignature m_inputSignature;
    D3D11.InputLayout m_inputLayout;
    SharpDX.Viewport m_viewPort;

    public TestDX2()
    {
        m_renderForm = new SharpDX.Windows.RenderForm("SharpDxLearn");
        m_renderForm.ClientSize = new System.Drawing.Size(m_width, m_height);
        m_renderForm.AllowUserResizing = false;

        InitializeDeviceResources();

        InitializeShaders();

        InitializeTriangle();
    }

    void InitializeDeviceResources()
    {
        DXGI.ModeDescription backBufferDesc = new DXGI.ModeDescription(m_width, m_height, new DXGI.Rational(60, 1), DXGI.Format.R8G8B8A8_UNorm);
        DXGI.SwapChainDescription swapChainDesc = new DXGI.SwapChainDescription()
        {
            ModeDescription = backBufferDesc,
            SampleDescription = new DXGI.SampleDescription(1, 0),
            Usage = DXGI.Usage.RenderTargetOutput,
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

        m_viewPort = new SharpDX.Viewport(0, 0, m_width, m_height);
        m_d3d11DeviceContext.Rasterizer.SetViewport(m_viewPort);
    }

    void InitializeTriangle()
    {
        m_triangleVertexBuffer = D3D11.Buffer.Create<VertexPositionColor>(m_d3d11Device, D3D11.BindFlags.VertexBuffer, m_vertices);
    }

    void InitializeShaders()
    {
        using (var vertexShaderByteCode = D3DCompiler.ShaderBytecode.CompileFromFile("vertexShader.hlsl", "main", "vs_4_0", D3DCompiler.ShaderFlags.Debug))
        {
            m_inputSignature = D3DCompiler.ShaderSignature.GetInputOutputSignature(vertexShaderByteCode);
            m_vertexShader = new D3D11.VertexShader(m_d3d11Device, vertexShaderByteCode);
        }
        using (var pixelShaderByteCode = D3DCompiler.ShaderBytecode.CompileFromFile("pixelShader.hlsl", "main", "ps_4_0", D3DCompiler.ShaderFlags.Debug))
        {
            m_pixelShader = new D3D11.PixelShader(m_d3d11Device, pixelShaderByteCode);
        }

        m_d3d11DeviceContext.VertexShader.Set(m_vertexShader);
        m_d3d11DeviceContext.PixelShader.Set(m_pixelShader);

        m_d3d11DeviceContext.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;

        m_inputLayout = new D3D11.InputLayout(m_d3d11Device, m_inputSignature, m_inputElments);
        m_d3d11DeviceContext.InputAssembler.InputLayout = m_inputLayout;
    }

    void Draw()
    {
        m_d3d11DeviceContext.OutputMerger.SetRenderTargets(m_renderTargetView);
        SharpDX.Mathematics.Interop.RawColor4 color = new SharpDX.Mathematics.Interop.RawColor4(32 / (float)255, 103 / (float)255, 178 / (float)255, 1);
        m_d3d11DeviceContext.ClearRenderTargetView(m_renderTargetView, color);

        m_d3d11DeviceContext.InputAssembler.SetVertexBuffers(0, new D3D11.VertexBufferBinding(m_triangleVertexBuffer, SharpDX.Utilities.SizeOf<VertexPositionColor>(), 0));
        m_d3d11DeviceContext.Draw(m_vertices.Count(), 0);

        m_swapChain.Present(1, DXGI.PresentFlags.None);
    }

    public void Run()
    {
        SharpDX.Windows.RenderLoop.Run(m_renderForm, RenderCallback);
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

        m_triangleVertexBuffer.Dispose();
        m_vertexShader.Dispose();
        m_pixelShader.Dispose();

        m_inputLayout.Dispose();
        m_inputSignature.Dispose();
    }
}